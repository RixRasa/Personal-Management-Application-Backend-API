using Microsoft.EntityFrameworkCore;
using TransactionsAPI.Database.Entities;
using TransactionsAPI.Models;


namespace TransactionsAPI.Database.Repositories {
    public class TransactionRepository : ITransactionRepository {

        TransDbContext _dbContext;

        public TransactionRepository(TransDbContext dbContext) {
            _dbContext = dbContext;
        }

        //OVO OVDE JE ZA B4 USLOV******************************************************************************************
        public async Task<bool> CategorizeTransaction(string id, string idCategory) {
            var category = await _dbContext.Categories.SingleAsync(x => x.Code.Equals(idCategory));
            var transaction = await _dbContext.Transactions.SingleAsync(x => x.Id.Equals(id));

            transaction.Category = category;
            transaction.CategoryId = category.Code;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        //OVO OVDE JE ZA B3 USLOV*******************************************************************************************
        public async Task<CategoryEntity> GetCategoryByCodeId(string codeId) {
            return await _dbContext.Categories.FirstOrDefaultAsync(t => t.Code.Equals(codeId));
        }
        public async Task<bool> CreateCategory(CategoryEntity categoryEntity) {
            _dbContext.Categories.Add(categoryEntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateCategory(string codeId, Category c) {
            var category = await _dbContext.Categories.SingleAsync(x => x.Code.Equals(codeId));
            category.Name = c.Name;
            category.Parent_code = c.Parent_code;
            await _dbContext.SaveChangesAsync();
            return true;
        }


        //OVO OVDE JE ZA B1 USLOV********************************************************************************************
        public async Task<bool> CreateProduct(TransactionEntity transactionEntity) {
            _dbContext.Transactions.Add(transactionEntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<TransactionEntity> GetTransactionById(string Id) {
            return await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id.Equals(Id));
        }


        //OVO OVDE JE ZA B2 USLOV*********************************************************************************************
        public async Task<PageSortedList<TransactionEntity>> GetTransactions(TransactionKind? transaction_kind, DateTime? start_date, DateTime? end_date, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc, string? sortBy = null) {

            var query = _dbContext.Transactions.AsQueryable();

            if (transaction_kind != null) {
                query.Where(x => x.Kind.Equals(transaction_kind));
            }
            if (start_date != null) {
                query.Where(x => (DateTime.Compare(x.Date, (DateTime)start_date) == 0 || DateTime.Compare(x.Date, (DateTime)start_date) == 1));
            }
            if (end_date != null) {
                query.Where(x => (DateTime.Compare(x.Date, (DateTime)end_date) == 0 || DateTime.Compare(x.Date, (DateTime)end_date) == -1));
            }

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling((totalCount * 1.0) / pageSize);

            if (!String.IsNullOrEmpty(sortBy)) {
                switch (sortBy) {
                    case "Id":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "Beneficiary_name":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Beneficiary_name) : query.OrderByDescending(x => x.Beneficiary_name);
                        break;
                    case "amount":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Amount) : query.OrderByDescending(x => x.Amount);
                        break;
                    case "description":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
                        break;
                }
            }
            else {
                query = query.OrderBy(x => x.Id);
            }

            

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var transactions = await query.ToListAsync();

            return new PageSortedList<TransactionEntity> {
                TotalPages = totalPages,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder,
                Items = transactions
            };
        }
    }
}
