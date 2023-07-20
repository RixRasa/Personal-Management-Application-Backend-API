namespace TransactionsAPI.Commands {
    public class SplitTransactionCommand {


        public Splits[] Splits { get; set; }
    }

    public class Splits {
        public string catcode { get; set; }
        public double amount { get; set; }
    }
}
