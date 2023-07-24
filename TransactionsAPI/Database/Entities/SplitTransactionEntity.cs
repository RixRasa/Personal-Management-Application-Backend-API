using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionsAPI.Database.Entities {
    public class SplitTransactionEntity {

       
        public int Id { get; set; }
        public string catcode { get; set; }
        public double amount { get; set; }
       // public TransactionEntity? Transaction { get; set; }
        public string? TransactionId { get; set; }
    }
}
