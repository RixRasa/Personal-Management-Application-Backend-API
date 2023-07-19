using TransactionsAPI.Database.Entities;
using TransactionsAPI.Models;

namespace TransactionsAPI.Database.Repositories {
    public interface ITransactionRepository {


        //OVO JE DEO ZA B3 USLOV*********************************************************************************************
        Task<CategoryEntity> GetCategoryByCodeId(string codeId);
        Task<bool> CreateCategory(CategoryEntity categoryEntity);
        Task<bool> UpdateCategory(string codeId, Category c);


        //OVO OVDE JE ZA B2 USLOV********************************************************************************************
        Task<PageSortedList<TransactionEntity>> GetTransactions(TransactionKind? transaction_kind, DateTime? start_date, DateTime? end_date, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc, string? sortBy = null);


        //OVO OVDE JE ZA B1 USLOV*********************************************************************************************
        Task<TransactionEntity> GetTransactionById(string Id);
        Task<bool> CreateProduct(TransactionEntity transactionEntity);
        
    }
}
