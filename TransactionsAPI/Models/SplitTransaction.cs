namespace TransactionsAPI.Models {
    public class SplitTransaction {

        public int Id { get; set; }
        public string catcode { get; set; }
        public double amount { get; set; }
        public Transaction? Transaction { get; set; }
        public string? TransactionId { get; set; }
    }
}
