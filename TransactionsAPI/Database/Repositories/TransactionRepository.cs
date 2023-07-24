using Microsoft.EntityFrameworkCore;
using TransactionsAPI.Commands;
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
            _dbContext.Update(transaction);
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
        public async Task<bool> CreateTransaction(TransactionEntity transactionEntity) {
            _dbContext.Transactions.Add(transactionEntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<TransactionEntity> GetTransactionById(string Id) {
            
            return await _dbContext.Transactions.Include(x => x.SplitTransactions).FirstOrDefaultAsync(t => t.Id.Equals(Id));
        }


        //OVO OVDE JE ZA B2 USLOV*********************************************************************************************
        public async Task<PageSortedList<TransactionEntity>> GetTransactions(List<TransactionKind>? listOfKinds, DateTime? start_date, DateTime? end_date, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc, string? sortBy = null) {

            var query = _dbContext.Transactions.AsQueryable();

            if (listOfKinds.Count != 0) {
                query = query.Where(x => listOfKinds.Contains(x.Kind));
            }
            
            if (start_date != null) {
                query = query.Where(x => x.Date >= start_date);
            }
            if (end_date != null) {
                query = query.Where(x => x.Date <= end_date);
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
                query = query.OrderByDescending(x => x.Date).ThenBy(x => x.CategoryId);
            }

            

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            query = query.Include(x => x.Category);
            query = query.Include(x => x.SplitTransactions);

            var transactions = await query.ToListAsync();

           /* for (int i = 0; i < transactions.Count; i++) {
                if (transactions[i].CategoryId != null) {
                    var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Code.Equals(transactions[i].CategoryId));
                    transactions[i].Category = category;
                }


                //var listOfSplits = await _dbContext.SplitsOfTransaction.AsQueryable().Where(x => x.TransactionId.Equals(transactions[i].Id)).ToListAsync();
                //transactions[i].SplitTransactions = listOfSplits;
            }*/

           
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


        //OVO OVDE JE ZA B5 USLOV****************************************************************************************
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        public async Task<List<SpendingByCategory>> GetAnalytics(string? catcode, DateTime? startDate, DateTime? endDate, DirectionKind? directionKind) {

            //Case when we gather informations about catcode category and their children categories
            if (catcode != null) {
                
                /*var query1 = await (from trans in _dbContext.Transactions
                                    join category in _dbContext.Categories
                                    on trans.CategoryId equals category.Code

                                    where (category.Code == catcode || category.Parent_code == catcode)
                                    where (directionKind == null || (trans.Direction == directionKind))
                                    where ((startDate == null || (trans.Date >= startDate)) &&
                                    (endDate == null || (trans.Date <= endDate)))

                                    group trans by new { trans.CategoryId} into g
                                    select new SpendingByCategory{
                                        catcode = g.Key.CategoryId,
                                        count = g.Count(x => x.Id != null),
                                        amount = g.Sum(x => x.Amount)
                                    }).ToListAsync();
                return query1;*/

                var queryC = _dbContext.Categories.AsQueryable();
                var queryT = _dbContext.Transactions.AsQueryable();

                var finalList = await queryT.Join(queryC,
                            queryT => queryT.CategoryId,
                            queryC => queryC.Code,
                            (queryT, queryC) => new {
                                Id = queryT.Id,                             
                                Amount = queryT.Amount,
                                CatCode = queryT.CategoryId,
                                ParentCode = queryC.Parent_code,
                                Direction = queryT.Direction,
                                Date = queryT.Date
                            }).Where(x => x.ParentCode.Equals(catcode) || x.CatCode.Equals(catcode))
                            .Where(x => directionKind == null || (x.Direction == directionKind))
                            .Where(x => (startDate == null || (x.Date >= startDate)) && (endDate == null || (x.Date <= endDate)))
                            .GroupBy(x => x.CatCode)
                            .Select(x => new SpendingByCategory {
                                catcode = x.First().CatCode,
                                count = x.Count(),
                                amount = x.Sum(c => c.Amount)
                            }).ToListAsync();

                return finalList;
            }

            //Case when we gather informations about root categories
            else {
                List<SpendingByCategory> listOfSpendings = new List<SpendingByCategory>();

                var query = _dbContext.Categories.AsQueryable();
                query = query.Where(x => x.Parent_code.Equals(""));
                List<CategoryEntity> listOfRoots = await query.ToListAsync();

                for (int i = 0; i < listOfRoots.Count; i++) {
                    string rootCode = listOfRoots[i].Code;
                    List<CategoryEntity> listOfChildrenAndRoot = await _dbContext.Categories.AsQueryable().Where(x => x.Parent_code.Equals(rootCode) || x.Code.Equals(rootCode)).ToListAsync();

                    List<TransactionEntity> listOfTransactions = await _dbContext.Transactions.AsQueryable()
                        .Where(x => listOfChildrenAndRoot.Contains(x.Category))
                        .Where(x => directionKind == null || x.Direction == directionKind)
                        .Where(x => (startDate == null || x.Date >= startDate) && (endDate == null || x.Date <= endDate)).ToListAsync();


                    SpendingByCategory s = new SpendingByCategory(); s.amount = 0.0; s.count = 0; s.catcode = rootCode;
                    for(int j = 0; j < listOfTransactions.Count; j++) {
                        s.amount += listOfTransactions[j].Amount;
                        s.count++;
                    }

                    if(s.count > 0) listOfSpendings.Add(s);
                }
                return listOfSpendings;
            }
        }
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.


        //OVO OVDE JE ZA B6 USLOV********************************************************************************************************
        public async Task<bool> SplitTheTransaction(TransactionEntity transaction, Splits[] splits) {

            var listOfAlreadyExistingSplits = await _dbContext.SplitsOfTransaction.AsQueryable().Where(x => x.TransactionId.Equals(transaction.Id)).ToListAsync();

            for (int i = 0; i < listOfAlreadyExistingSplits.Count; i++) { 
                _dbContext.SplitsOfTransaction.Remove(listOfAlreadyExistingSplits[i]);
            
            }
            await _dbContext.SaveChangesAsync();

            //Persisting splits into database
            for (int i = 0; i < splits.Length; i++) {
                
                SplitTransactionEntity split = new SplitTransactionEntity() { amount = splits[i].amount, catcode = splits[i].catcode ,TransactionId = transaction.Id };
                transaction.SplitTransactions.Add(split);         
            }
            await _dbContext.SaveChangesAsync();
            /*
            //Adding splits to transaction
            var listOfSplits = await _dbContext.SplitsOfTransaction.AsQueryable().Where(x => x.TransactionId.Equals(transaction.Id)).ToListAsync();
            for (int i = 0; i < listOfSplits.Count; i++) {
                transaction.SplitTransactions.Add(listOfSplits[i]);
                await _dbContext.SaveChangesAsync();

            }*/

            return true;
        }
    }
}
