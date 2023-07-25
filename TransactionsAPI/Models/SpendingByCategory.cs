using System.Text.Json.Serialization;

namespace TransactionsAPI.Models {
    public class SpendingByCategory {

        [JsonPropertyName("catcode")]
        public string Catcode { get; set; }
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }

    }
}
