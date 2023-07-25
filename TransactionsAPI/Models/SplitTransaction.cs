using System.Text.Json.Serialization;

namespace TransactionsAPI.Models {
    public class SplitTransaction {

        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("catcode")]
        public string Catcode { get; set; }
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        //public Transaction? Transaction { get; set; }
        public string? TransactionId { get; set; }
    }
}
