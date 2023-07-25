using System.Text.Json.Serialization;

namespace TransactionsAPI.Models {
    public class Category {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("parent-code")]
        public string? Parent_code { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

     
    }
}
