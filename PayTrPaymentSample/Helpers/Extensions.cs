using PayTrPaymentSample.Data.Entities;

namespace PayTrPaymentSample
{
    public static class Extensions
    {
        public static float ToPrice(this float number)
        {
            return (float)Math.Round(number, 2);
        }

        public static decimal ToPrice(this decimal number)
        {
            return (decimal)Math.Round(number, 2);
        }

        public static double ToPrice(this double number)
        {
            return (double)Math.Round(number, 2);
        }

        public static decimal ToPrice(this int number)
        {
            return (decimal)Math.Round(Convert.ToDecimal(number), 2);
        }

        public static decimal CalculateOrder(this Order order)
        {
            decimal sum = 0;

            foreach (var orderDetail in order.OrderDetails)
            {
                sum += orderDetail.Product.Price * orderDetail.Quantity;
            }

            return sum;
        }

    }
}
