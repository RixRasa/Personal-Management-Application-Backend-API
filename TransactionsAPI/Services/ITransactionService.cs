using TransactionsAPI.Commands;
using TransactionsAPI.Models;

namespace TransactionsAPI.Services {
    public interface ITransactionService {

        //OVO JE DEO ZA B3 USLOV*********************************************************************************************
        Task<bool> InsertCategory(Category c);

        //OVO OVDE JE ZA B2 USLOV********************************************************************************************
        Task<PageSortedList<Transaction>> GetTransactions(List<TransactionKind>? listOfKinds, DateTime? start_date, DateTime? end_date, int page, int pageSize, SortOrder sortOrder , string? sortBy );


        //OVO OVDE JE ZA B1 USLOV********************************************************************************************
        Task<bool> InsertTransactions(Transaction t);


        //OVO OVDE JE ZA B4 USLOV********************************************************************************************
        Task<bool> CategorizeTransaction(string id, string idCategory);


        //OVO OVDE JE ZA B5 USLOV********************************************************************************************
        Task<List<SpendingByCategory>> GetAnaliytics(string? catcode, DateTime? startDate, DateTime? endDate, DirectionKind? directionKind);


        //OVO OVDE JE ZA B6 USLOV********************************************************************************************
        Task<bool> SplitTransaction(Splits[] splits, string id);
    }
}
