namespace ChalitaLearning.Model
{
    public class OrderDto
    {
        public string OrderId { get; set; } = "";
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public string CustomerName { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }
}
