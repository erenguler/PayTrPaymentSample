using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayTrPaymentSample.Models;
using PayTrPaymentSample.Services;

namespace PayTrPaymentSample.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PayTrPaymentService _payTrPaymentService;
        private readonly ILogger<PaymentController> _logger;
        private readonly IWebHostEnvironment _webEnvironment;
        private readonly OrderService _orderService;
        private readonly UserService _userService;

        public PaymentController(
              PayTrPaymentService payTrPaymentService
            , ILogger<PaymentController> logger
            , IWebHostEnvironment webEnvironment
            , OrderService orderService
            , UserService userService
        )
        {
            _payTrPaymentService = payTrPaymentService;
            _logger = logger;
            _webEnvironment = webEnvironment;
            _orderService = orderService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(int orderId)
        {
            var order = await _orderService.GetOrder(orderId);

            var model = new PaymentVM
            {
                OrderId = order.Id,
                OrderPrice = order.CalculateOrder().ToPrice()
            };

            return View(model);
        }

        public async Task<IActionResult> GetPaymentIFrameUrl(int orderId, string userIp)
        {
            var iframeUrl = string.Empty;

            var user = await _userService.GetCurrentUser();
            var order = await _orderService.GetOrder(orderId);

            var orderPrice = order.CalculateOrder().ToPrice();

            var ipAddress = userIp;
            var okURL = string.Empty;
            var failURL = string.Empty;
            var currency = "TL";

#if DEBUG
            okURL = $"https://localhost:7094/payment/success/{orderId}";
            failURL = $"https://localhost:7094/payment/error/{orderId}";
#else
            okURL = $"https://website.net/payment/success/{orderId}";
            failURL = $"https://website.net/payment/error/{orderId}";
#endif

            var iframeModel = new PayTrPaymentRequestModel
            {
                UserIp = ipAddress,
                MerchantOid = orderId.ToString(),
                Email = user.Email,
                PaymentAmount = orderPrice.ToString(),
                Currency = currency,
                NoInstallment = 1,
                MaxInstallment = 0,
                UserName = $"{user.FirstName} {user.LastName}",
                UserAddress = $"{user.Address}",
                UserPhone = user.PhoneNumber,
                MerchantOkUrl = okURL,
                MerchantFailUrl = failURL,
                TestMode = 1, // TODO: canlıya alınca 0 yap
                DebugOn = 1,
                TimeoutLimit = 30,
                Lang = "tr",
                UserBasket = order.OrderDetails.ToDictionary(str => str.Product.Name, str => str.Quantity)
            };

            iframeUrl = await _payTrPaymentService.GetIFrameUrl(iframeModel);

            return Ok(new { iframeUrl = iframeUrl });
        }

        /// <summary>
        /// PayTR buraya istek atıyor. kullanıcı görmeden arkaplanda ödeme bilgisini bize ulaştırıyor
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Payment notification", Request.Form);

            // gelen ödeme bildiriminden verileri çek.
            string? hash = Request.Form["hash"];
            string? merchant_oid = Request.Form["merchant_oid"];
            string? status = Request.Form["status"];
            string? total_amount = Request.Form["total_amount"];
            string? payment_type = Request.Form["payment_type"];
            string? payment_amount = Request.Form["payment_amount"];
            string? currency = Request.Form["currency"];
            string? merchant_id = Request.Form["merchant_id"];
            string? test_mode = Request.Form["test_mode"];
            string? failed_reason_code = null;
            string? failed_reason_msg = null;

            // başarısız ödemeler için sebep bilgilerini al
            if (!string.IsNullOrEmpty(status) && status != "success")
            {
                failed_reason_code = Request.Form["failed_reason_code"];
                failed_reason_msg = Request.Form["failed_reason_msg"];
            }

            // ve modelle
            var response = new PayTrPaymentResponseModel
            {
                Currency = currency,
                FailedReasonCode = failed_reason_code,
                FailedReasonMsg = failed_reason_msg,
                Hash = hash,
                MerchantOid = merchant_oid,
                PaymentAmount = payment_amount,
                PaymentType = payment_type,
                Status = status,
                TestMode = test_mode,
                TotalAmount = total_amount
            };

            // gelen isteğin PayTR'den olup olmadığını sorgula
            var requestControl = _payTrPaymentService.ValidatePayTrRequest(response);
            if (!requestControl)
            {
                _logger.LogWarning("Payment notification request received from outside PayTR.", response);

                return BadRequest("Please dont try it :) Your location, ip address and useragent was reported");
            }

            // siparişi getir
            var orderId = Convert.ToInt32(response.MerchantOid);

            // ödeme başarılı olduysa
            if (response.Status == "success")
            {
                await _orderService.SetOrderSuccess(orderId);
            }
            else // ödeme başarısız
            {
                await _orderService.SetOrderFailed(orderId);
            }

            return Ok("OK"); // bildirim alındı
        }

        /// <summary>
        /// PayTR ödemeden sonra KULLANICIYI buraya yönlendiriyor. sadece ödeme sonucunu görsün diye
        /// </summary>
        /// <param name="orderId">hangi sipariş için bu sonucu görüyoruz</param>
        [HttpGet("payment/success/{orderId}")]
        public async Task<IActionResult> Success(string orderId)
        {
            return View();
        }

        /// <summary>
        /// PayTR ödemeden sonra KULLANICIYI buraya yönlendiriyor. sadece ödeme sonucunu görsün diye
        /// </summary>
        /// <param name="orderId">hangi sipariş için bu sonucu görüyoruz</param>
        [HttpGet("payment/error/{orderId}")]
        public async Task<IActionResult> Error(string orderId)
        {
            return View();
        }
    }
}
