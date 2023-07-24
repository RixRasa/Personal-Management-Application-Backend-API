using AutoMapper;
using TransactionsAPI.Commands;
using TransactionsAPI.Database.Entities;
using TransactionsAPI.Database.Repositories;
using TransactionsAPI.Models;

namespace TransactionsAPI.Services {
    public class TransactionService : ITransactionService {

        ITransactionRepository _repository;
        IMapper _mapper;

        public TransactionService(ITransactionRepository repository, IMapper mapper) {
            _repository = repository;
            _mapper = mapper;
        }


        //*************** B1 ****************************************************************************************************
        public async Task<bool> InsertTransactions(Transaction t) {
            var checkIfTransactionExist = await CheckIfTransactionExist(t.Id);
            if (!checkIfTransactionExist) {
                return await _repository.CreateTransaction(_mapper.Map<TransactionEntity>(t));
            }
            return false;

        }

        private async Task<bool> CheckIfTransactionExist(string Id) {
            var transaction = await _repository.GetTransactionById(Id);
            if (transaction == null) return false;
            else return true;

        }


        //*************** B2 **************************************************************************************************
        public async Task<PageSortedList<Transaction>> GetTransactions(List<TransactionKind>? listOfKinds, DateTime? start_date, DateTime? end_date, int page, int pageSize, SortOrder sortOrder, string? sortBy) {
            var transactions = await _repository.GetTransactions(listOfKinds, start_date, end_date, page, pageSize, sortOrder, sortBy);
            return _mapper.Map<PageSortedList<Transaction>>(transactions);
        }


        //*************** B3 **************************************************************************************************
        public async Task<bool> InsertCategory(Category c) {

            //Checking if there is a Category with Code = c.ParentCode
            if (!c.Parent_code.Equals("")) {
                var checkIfParentCategoryExist = await CheckIfCategoryExist(c.Parent_code);
                if (checkIfParentCategoryExist == false) return false;
            }
            

            //Checking if there is already a Category with same Code = c.Code
            var checkIfCategoryExist = await CheckIfCategoryExist(c.Code);
            if (checkIfCategoryExist) {
                return await _repository.UpdateCategory(c);
            }
            else {
                return await _repository.CreateCategory(_mapper.Map<CategoryEntity>(c));
            }
        }

        private async Task<bool> CheckIfCategoryExist(string code) {
            var category = await _repository.GetCategoryByCodeId(code);
            if (category == null) return false;
            else return true;
        }


        //*************** B4 *******************************************************************************************************
        public async Task<bool> CategorizeTransaction(string id, string idCategory) {
            var transaction = await _repository.GetTransactionById(id);
            var category = await _repository.GetCategoryByCodeId(idCategory);
            if(transaction == null || category == null) return false;
            else {
                return await _repository.CategorizeTransaction(transaction, category);
            }
            
        }


        //*************** B5 ********************************************************************************************************
        public async Task<List<SpendingByCategory>> GetAnaliytics(string? catcode, DateTime? startDate, DateTime? endDate, DirectionKind? directionKind) {
            return await _repository.GetAnalytics(catcode, startDate, endDate, directionKind);
        }


        //*************** B6 ********************************************************************************************************
        public async Task<bool> SplitTransaction(Splits[] splits, string id) {

            //Check if transaction exist
            var transaction = await _repository.GetTransactionById(id);
            if (transaction == null) return false;

            //Check if all categories exist and if combined amount is the same as amount of choosen transaction
            double amount = 0.0;
            for(int i = 0; i < splits.Length; i++) {
                var category = await _repository.GetCategoryByCodeId(splits[i].catcode);
                if(category == null) return false;
                amount += splits[i].amount;
            }
            if (transaction.Amount != amount) return false;

            //Perfrom the split
            return await _repository.SplitTheTransaction(transaction, splits);
        }


        //*************** B7 ********************************************************************************************************
        public async Task<List<Category>> GetCategories(string? parentCode) {
            if(parentCode != null) {
                //We will first check if there is a Category with code = parentCode
                var category = await _repository.GetCategoryByCodeId(parentCode);
                if (category == null) return null;
                else return _mapper.Map<List<Category>>(await _repository.GetChildCategories(parentCode));
            }
            else {
                return _mapper.Map<List<Category>>(await _repository.GetRootCategories());
            }
            
        }
    }
}
