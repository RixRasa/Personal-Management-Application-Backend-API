using System.Text.Json.Serialization;

namespace TransactionsAPI.Models {
    public class PageSortedList <T>{
        [JsonPropertyName("total-count")]
        public int TotalCount { get; set; }
        [JsonPropertyName("page-size")]
        public int PageSize { get; set; }
        [JsonPropertyName("page")]
        public int Page { get; set; }
        [JsonPropertyName("total-pages")]
        public int TotalPages { get; set; }
        [JsonPropertyName("sort-order")]
        public SortOrder SortOrder { get; set; }
        [JsonPropertyName("sort-by")]
        public string SortBy { get; set; }
        [JsonPropertyName("items")]
        public List<T> Items { get; set; }
        
    }
}
