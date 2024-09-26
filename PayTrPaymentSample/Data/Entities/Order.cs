namespace PayTrPaymentSample.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public OrderStatus Status { get; set; }
    }
}
