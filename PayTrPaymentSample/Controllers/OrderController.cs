using Microsoft.AspNetCore.Mvc;

namespace PayTrPaymentSample.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Generate random order
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var userId = 1; // currentUserId
            var order = await _orderService.GenerateRandomOrder(userId);

            return View(order);
        }

    }
}
