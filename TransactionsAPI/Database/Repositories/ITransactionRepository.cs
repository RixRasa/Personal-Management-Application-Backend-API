using TransactionsAPI.Commands;
using TransactionsAPI.Database.Entities;
using TransactionsAPI.Models;

namespace TransactionsAPI.Database.Repositories {
    public interface ITransactionRepository {


        //**************** B1 *********************************************************************************************
        Task<TransactionEntity> GetTransactionById(string Id);
        Task<bool> CreateTransaction(TransactionEntity transactionEntity);


        //**************** B2 ********************************************************************************************
        Task<PageSortedList<TransactionEntity>> GetTransactions(List<TransactionKind>? listOfKinds, DateTime? start_date, DateTime? end_date, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc, string? sortBy = null);


        //**************** B3 *********************************************************************************************
        Task<CategoryEntity> GetCategoryByCodeId(string codeId);
        Task<bool> CreateCategory(CategoryEntity categoryEntity);
        Task<bool> UpdateCategory(Category c);


        //**************** B4 **********************************************************************************************
        Task<bool> CategorizeTransaction(TransactionEntity transaction, CategoryEntity category);


        //**************** B5 **********************************************************************************************
        Task<List<SpendingByCategory>> GetAnalytics(string? catcode, DateTime? startDate, DateTime? endDate, DirectionKind? directionKind);


        //**************** B6 **********************************************************************************************
        Task<bool> SplitTheTransaction(TransactionEntity transaction, Splits[] splits);


        //**************** B7 **********************************************************************************************
        Task<List<CategoryEntity>> GetChildCategories(string parentCode);
        Task<List<CategoryEntity>> GetRootCategories();
    }
}
