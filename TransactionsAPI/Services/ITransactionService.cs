using TransactionsAPI.Models;

namespace TransactionsAPI.Services {
    public interface ITransactionService {

        //OVO JE DEO ZA B3 USLOV*********************************************************************************************
        Task<bool> InsertCategory(Category c);

        //OVO OVDE JE ZA B2 USLOV********************************************************************************************
        Task<PageSortedList<Transaction>> GetTransactions(TransactionKind? transaction_kind, DateTime? start_date, DateTime? end_date, int page, int pageSize, SortOrder sortOrder , string? sortBy );


        //OVO OVDE JE ZA B1 USLOV********************************************************************************************
        Task<bool> InsertTransactions(Transaction t);


        //OVO OVDE JE ZA B4 USLOV********************************************************************************************
        Task<bool> CategorizeTransaction(string id, string idCategory);
    }
}
