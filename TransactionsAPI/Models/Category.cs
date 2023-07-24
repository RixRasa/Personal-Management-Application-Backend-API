namespace TransactionsAPI.Models {
    public class Category {
        public string Code { get; set; }
        public string? Parent_code { get; set; }
        public string Name { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        //Ovde moze da ide lista transakcija sa ovom kategorijom
    }
}
