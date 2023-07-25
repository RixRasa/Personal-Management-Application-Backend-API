using System.Text.Json.Serialization;

namespace TransactionsAPI.Models {
    public class Transaction {

        public Transaction() {

        }

        public Transaction(string _TransactionId, string _BeneficiaryName, DateTime _date, DirectionKind _direction, double _amount, string _description, string _currency, string _mcc, TransactionKind _kind) { 
            Id = _TransactionId;
            BeneficiaryName = _BeneficiaryName;
            Date = _date;
            Direction = _direction;
            Amount = _amount;
            Description = _description; 
            Currency = _currency;
            Mcc = _mcc;
            Kind = _kind;
            SplitTransactions = new List<SplitTransaction>();
        }

        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("beneficiary-name")]
        public string BeneficiaryName { get; set; } //Promeniti u camelCase
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("direction")]
        public DirectionKind Direction { get; set; }
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; } //OVO MOZE DA BUDE ENUM
        [JsonPropertyName("mcc")]
        public string Mcc { get; set; }
        [JsonPropertyName("kind")]
        public TransactionKind Kind { get; set; }
        [JsonPropertyName("catcode")]
        public string? CategoryId { get; set; }
        [JsonPropertyName("category")]
        public Category? Category { get; set; }
        [JsonPropertyName("splits")]
        public ICollection<SplitTransaction> SplitTransactions { get; set; }


    }
}
