namespace OrdersAPI
{
    public class Orders
    {
        public int OrdersId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public int ProductId { get; set; }
    }
}