namespace supershop.SalesRagister
{
    /// <summary>One payment tender of a sale (e.g. Cash 500, Card 300).</summary>
    public class Tender
    {
        public string Method;
        public decimal Amount;
        public Tender(string method, decimal amount) { Method = method; Amount = amount; }
    }
}
