namespace TransactionsAPI.Database.Entities {
    public class CategoryEntity {
        public string Code { get; set; }
        public string? Parent_code { get; set; }
        public string Name  { get; set; }

    }
}
