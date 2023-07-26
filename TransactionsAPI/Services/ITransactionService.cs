using Microsoft.AspNetCore.Mvc;
using TransactionsAPI.Commands;
using TransactionsAPI.Models;

namespace TransactionsAPI.Services {
    public interface ITransactionService {


        //*************** B1 ********************************************************************************************
        Task<bool> InsertTransactions(Transaction t);


        //*************** B2 ********************************************************************************************
        Task<PageSortedList<Transaction>> GetTransactions(List<TransactionKind>? listOfKinds, DateTime? start_date, DateTime? end_date, int page, int pageSize, SortOrder sortOrder, string? sortBy);


        //*************** B3 *********************************************************************************************
        Task<bool> InsertCategory(Category c);


        //*************** B4 ********************************************************************************************
        Task<bool> CategorizeTransaction(string id, string idCategory);


        //*************** B5 ********************************************************************************************
        Task<List<SpendingByCategory>> GetAnaliytics(string? catcode, DateTime? startDate, DateTime? endDate, DirectionKind? directionKind);


        //*************** B6 ********************************************************************************************
        Task<int> SplitTransaction(Splits[] splits, string id);


        //*************** B7 ********************************************************************************************
        Task <List<Category>> GetCategories(string? parentCode);


        //*************** A2 ********************************************************************************************
        Task<bool> AutoCategorize(List<Rule> listOfRules);

        //*************** INTEGRACIJA ***********************************************************************************
        Task<List<Category>> GetCategoriess();
    }
}
