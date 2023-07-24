using TransactionsAPI.Models;

namespace TransactionsAPI.Database.Entities {
    public class TransactionEntity {

        public TransactionEntity() {

        }
        public TransactionEntity(string _TransactionId, string _Beneficiary_name, DateTime _date, DirectionKind _direction, double _amount, string _description, string _currency, string _mcc, TransactionKind _kind) {
            Id = _TransactionId;
            Beneficiary_name = _Beneficiary_name;
            Date = _date;
            Direction = _direction;
            Amount = _amount;
            Description = _description;
            Currency = _currency;
            Mcc = _mcc;
            Kind = _kind;
            SplitTransactions = new List<SplitTransactionEntity>();
        }

        public string Id { get; set; }
        public string Beneficiary_name { get; set; }
        public DateTime Date { get; set; }
        public DirectionKind Direction { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; } //OVO MOZE DA BUDE ENUM
        public string Mcc { get; set; }
        public TransactionKind Kind { get; set; }
        public string? CategoryId { get; set; }
        public CategoryEntity? Category { get; set; }
        public ICollection<SplitTransactionEntity> SplitTransactions { get; set; }

    }
}
