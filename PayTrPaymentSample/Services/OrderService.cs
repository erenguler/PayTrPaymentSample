using Microsoft.EntityFrameworkCore;
using PayTrPaymentSample.Data.Entities;

namespace PayTrPaymentSample
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrder(int orderId)
        {
            var order = await _context.Orders.Include(x => x.OrderDetails).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Id == orderId);

            return order;
        }

        public async Task<Order> GenerateRandomOrder(int userId)
        {
            var random = new Random();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var products = await _context.Products.ToListAsync();

            // Generate random order
            var order = new Order
            {
                Id = random.Next(1, int.MaxValue),
                Status = OrderStatus.WaitingForPayment,
                UserId = user.Id,
            };

            int randomProductCount = random.Next(1, products.Count + 1);
            for (int i = 0; i < randomProductCount; i++)
            {
                var product = products[i];
                order.OrderDetails.Add(new OrderDetail { ProductId = product.Id, Product = product, Quantity = random.Next(1, 10) });
            }

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task SetOrderSuccess(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            order.Status = OrderStatus.Success;
            _context.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task SetOrderFailed(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            order.Status = OrderStatus.Failed;
            _context.Update(order);
            await _context.SaveChangesAsync();
        }

    }
}
