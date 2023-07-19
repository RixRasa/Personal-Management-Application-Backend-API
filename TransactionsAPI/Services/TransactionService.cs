using AutoMapper;
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


        //OVDE JE DEO ZA B3 USLOV**************************************************************************************************
        public async Task<bool> InsertCategory(Category c) {
            var categoryExist = await CheckIfCategoryExist(c.Code);
            if (categoryExist != null) {
                return await _repository.UpdateCategory(c.Code, c);
            }
            else {
                return await _repository.CreateCategory(_mapper.Map<CategoryEntity>(c));
            }
        }

        private async Task<CategoryEntity> CheckIfCategoryExist(string code) {
            var category = await _repository.GetCategoryByCodeId(code);
            return category;
        }


        //OVDE JE DEO ZA B2 USLOV**************************************************************************************************
        public async Task<PageSortedList<Transaction>> GetTransactions(TransactionKind? transaction_kind, DateTime? start_date, DateTime? end_date, int page, int pageSize, SortOrder sortOrder, string? sortBy) {
            var transactions = await _repository.GetTransactions(transaction_kind, start_date, end_date, page, pageSize, sortOrder, sortBy);
            return _mapper.Map<PageSortedList<Transaction>>(transactions);
        }


        //OVO JE DEO ZA B1 USLOV****************************************************************************************************
        public async Task<bool> InsertTransactions(Transaction t) {
            var checkIfTransactionExist = await CheckIfTransactionExist(t.Id);
            if (!checkIfTransactionExist) {
                return await _repository.CreateProduct(_mapper.Map<TransactionEntity>(t));
            }
            return false;

        }

        private async Task<bool> CheckIfTransactionExist(string Id) {
            var product = await _repository.GetTransactionById(Id);
            if (product == null) return false;
            else return true;

        }

       
    }
}
