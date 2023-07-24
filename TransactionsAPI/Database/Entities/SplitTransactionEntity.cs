using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionsAPI.Database.Entities {
    public class SplitTransactionEntity {

       
        public int Id { get; set; }
        public string Catcode { get; set; }
        public double Amount { get; set; }
       // public TransactionEntity? Transaction { get; set; }
        public string? TransactionId { get; set; }
    }
}
