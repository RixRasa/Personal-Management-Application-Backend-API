using TransactionsAPI.Commands;
using TransactionsAPI.Database.Entities;
using TransactionsAPI.Models;

namespace TransactionsAPI.Database.Repositories {
    public interface ITransactionRepository {


        //OVO JE DEO ZA B3 USLOV*********************************************************************************************
        Task<CategoryEntity> GetCategoryByCodeId(string codeId);
        Task<bool> CreateCategory(CategoryEntity categoryEntity);
        Task<bool> UpdateCategory(string codeId, Category c);


        //OVO OVDE JE ZA B2 USLOV********************************************************************************************
        Task<PageSortedList<TransactionEntity>> GetTransactions(List<TransactionKind>? listOfKinds, DateTime? start_date, DateTime? end_date, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc, string? sortBy = null);


        //OVO OVDE JE ZA B1 USLOV*********************************************************************************************
        Task<TransactionEntity> GetTransactionById(string Id);
        Task<bool> CreateTransaction(TransactionEntity transactionEntity);


        //OVO OVDE JE ZA B4 USLOV**********************************************************************************************
        Task<bool> CategorizeTransaction(string id, string idCategory);


        //OVO OVDE JE ZA B5 USLOV**********************************************************************************************
        Task<List<SpendingByCategory>> GetAnalytics(string? catcode, DateTime? startDate, DateTime? endDate, DirectionKind? directionKind);


        //OVO OVDE JE ZA B6 USLOV**********************************************************************************************
        Task<bool> SplitTheTransaction(TransactionEntity transaction, Splits[] splits);


    }
}
