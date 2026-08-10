namespace market_watch.Models.custom
{
    public class Trade
    {
        public int TradeId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalValue { get; set; }
    }
}