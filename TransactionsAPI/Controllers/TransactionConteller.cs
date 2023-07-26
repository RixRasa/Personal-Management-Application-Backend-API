using Microsoft.AspNetCore.Mvc;
using TransactionsAPI.Models;
using TransactionsAPI.Services;
using TransactionsAPI.Commands;
using Microsoft.AspNetCore.Cors;
using System.Net;
using TransactionsAPI.Error;

namespace TransactionsAPI.Controllers {
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    [Route("v1/api")]
    public class TransactionControler : ControllerBase {

        ITransactionService _transactionService;
        private readonly ILogger<TransactionControler> _logger;

        public TransactionControler(ILogger<TransactionControler> logger, ITransactionService transactionService) {
            _logger = logger;
            _transactionService = transactionService;
        }


        //METHOD CONNECTED TO RETRIEVING DATA OF TRANSACTIONS **********************************************************************************************
        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery (Name = "transaction-kind")] string? transactionKindQuery = null, [FromQuery (Name = "start-date")] DateTime? start_date = null, [FromQuery(Name = "end-date")] DateTime? end_date = null, [FromQuery ] int page = 1, [FromQuery(Name = "page-size")] int pageSize = 10, [FromQuery(Name = "sort-order")] SortOrder sortOrder = SortOrder.Asc, [FromQuery(Name = "sort-by")] string? sortBy = null) {

            //Parsing transactionKinds string
            List<TransactionKind> listOfKinds = new List<TransactionKind>();
            if(transactionKindQuery != null) {
                string[] transactionKinds = transactionKindQuery.Split(',');
                foreach(string s in transactionKinds) {
                    TransactionKind tKind =  new TransactionKind();
                    Enum.TryParse<TransactionKind>(s, out tKind);
                    listOfKinds.Add(tKind);
                }
            }
            
            var transactions = await _transactionService.GetTransactions(listOfKinds, start_date, end_date, page, pageSize, sortOrder, sortBy);
            return Ok(transactions);
        }


        //METHOD CONNECTED TO IMPORTING TRANSACTIONS *******************************************************************************************************
        [HttpPost("transactions/import")]
        public async Task<IActionResult> ImportAllTransactions([FromForm] IFormFile csvFile) {

            using (var reader = new StreamReader(csvFile.OpenReadStream())) {

                int count = 0;//this is used so we can avoid first row
                while (reader.EndOfStream == false) {
                    var content = reader.ReadLine();
                    var cells = content.Split(',').ToList();
                    if (RowHasData(cells)) {

                        if (count != 0) {
                            string Id = cells[0];

                            string Benef_name = cells[1];

                            string[] date_parts = cells[2].Split('/');
                            DateTime date = new DateTime(int.Parse(date_parts[2]), int.Parse(date_parts[0]), int.Parse(date_parts[1]));

                            DirectionKind directions;
                            Enum.TryParse<DirectionKind>(cells[3], out directions);

                            double amount = parseStringToDouble(cells[4]);

                            string description = cells[5];

                            string currency = cells[6]; //min 3 , max 3 length

                            string mcc = cells[7];

                            TransactionKind kind;
                            Enum.TryParse<TransactionKind>(cells[8], out kind);


                            var inserted = await _transactionService.InsertTransactions(new Transaction() {
                                Id = Id,
                                BeneficiaryName = Benef_name,
                                Date = date,
                                Amount = amount,
                                Direction = directions,
                                Description = description,
                                Currency = currency,
                                Mcc = mcc,
                                Kind = kind
                            });
                            if (inserted == false) {
                                CustomError error = new CustomError() {
                                    problem = "transaction-already-exist",
                                    message = "transaction already exist",
                                    details = "You cannot import transaction with same ID twice"
                                };
                                return new ObjectResult(error) { StatusCode = 440 };
                            }
                        }
                        count = 1;
                    }
                }
            }
            return Ok("Insert Completed");
        }
        //Function used for parsing "amount" values
        static double parseStringToDouble(string s) {
            s = s.Trim('"');

            int index = s.IndexOf(',');
            while (index != -1) {
                s = s.Remove(index, 1);
                index = s.IndexOf(',');
            }

            return double.Parse(s);
        }

        static bool RowHasData(List<string> cells) {
            return cells.Any(x => x.Length > 0);
        }


        //METHOD CONNECTED TO IMPORTING CATEGORIESSS*******************************************************************************************************
        //MOZE DA SE DODA DA SE PROVERI ZA PARENT_CODE DA LI UOPSTE POSTOJI KATEGORIJA KOJA IMA TAJ CODE TJ DA LI PARENT KATEGORIJE POSTOJI
        [HttpPost("categories/import")]
        public async Task<IActionResult> ImportAllCategories([FromForm] IFormFile csvFile) {
            using (var reader = new StreamReader(csvFile.OpenReadStream())) {

                int count = 0;
                while (reader.EndOfStream == false) {
                    var content = reader.ReadLine();
                    var cells = content.Split(',').ToList();
                    if (RowHasData(cells)) {

                        if(count != 0) {
                            string code = cells[0];

                            string parent_code = cells[1];

                            string name = cells[2];

                            Category category = new Category {
                                Code = code,
                                Parent_code = parent_code,
                                Name = name,
                            };

                            var inserted = await _transactionService.InsertCategory(category);
                            
                            if(inserted == false) {
                                CustomError error = new CustomError() {
                                    problem = "parentcode-of-category-does-not-exist",
                                    message = "parentcode of category does not exist",
                                    details = "If you are importing category which is not root category, there must already be category with same code as parent-code of " + "category that you are importing"
                                };
                                return new ObjectResult(error) { StatusCode = 440 };
                                
                            }
                        }
                        count = 1;
                    }
                }
            }
            return Ok("Insert Completed");
            
        }


        //METHOD CONNECTED TO CATEGORIZING TRANSACTIONS*******************************************************************************************************
        [HttpPost("transactions/{id}/categorize")]
        public async Task<IActionResult> GiveCategoryToTransaction([FromRoute] string id, [FromBody] CategorizeTransactionCommand command) {
            var categorised = await _transactionService.CategorizeTransaction(id, command.catcode);
            if (categorised == false) {
                CustomError error = new CustomError() {
                    problem = "transaction-or-category-dont-exist",
                    message = "transaction or category dont exist",
                    details = "When you are categorizing a transaction both category and transaction must already be in a database"
                };
                return new ObjectResult(error) { StatusCode = 440 };
            }
            else return Ok("Categorisation completed"); 
        }


        //METHOD CONNECTED TO ANALYZING SPENDINGS *************************************************************************************************************
        [HttpGet("spending-analytics")]
        public async Task<IActionResult> GetSpendingAnalytics([FromQuery(Name = "catcode")] string? catcode = null, [FromQuery(Name = "start-date")] DateTime? startDate = null, [FromQuery(Name = "end-date")] DateTime? endDate = null, [FromQuery(Name = "direction")] DirectionKind? directionKind = null) {
            List<SpendingByCategory> list = await _transactionService.GetAnaliytics(catcode, startDate, endDate, directionKind);
            return Ok(list);
        }


        //METHOD CONNECTED TO SPLITING A TRANSACTION IN SMALLER TRANSACTIONS **********************************************************************************
        [HttpPost("transactions/{id}/split")]
        public async Task<IActionResult> SplitTransaction([FromBody] SplitTransactionCommand splits, [FromRoute] string id) {
            var splited = await _transactionService.SplitTransaction(splits.Splits, id);
            if (splited == -1) {
                CustomError error = new CustomError() {
                    problem = "transaction-does-not-exist",
                    message = "transaction does not exist",
                    details = "Transaction you are splitting needs to exist already in a database"
                };
                return new ObjectResult(error) { StatusCode = 440 };
            }
            if(splited == -2) {
                CustomError error = new CustomError() {
                    problem = "category-does-not-exist",
                    message = "category does not exist",
                    details = "Categories you are using while splitting need to exist already in a database"
                };
                return new ObjectResult(error) { StatusCode = 440 };
            }
            if(splited == -3) {
                CustomError error = new CustomError() {
                    problem = "Amount-of-splits-are-not-same-as-transaction-amount",
                    message = "Amount of splits are not same as transaction amount",
                    details = "Amount of splits needs to be same as amount of transaction that you are splitting"
                };
                return new ObjectResult(error) { StatusCode = 440 };
            }
            return Ok("Split is done");
        }


        //METHOD CONNECTED TO GETTING ALL ROOT CATEGORIES OR CHILD-CATEGORIES OF ROOT CATEGORY
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories([FromQuery(Name = "parent-id")] string? parentCode = null) {
            List<Category> list = await _transactionService.GetCategories(parentCode);
            return Ok(list);
        }

        //INTEGRACIJA******************************************************************************************************************************************
        [HttpGet("categoriess")]
        public async Task<IActionResult> GetCategoriess() {
            List<Category> list = await _transactionService.GetCategoriess();
            return Ok(list);
        }


        //AUTO-CATEGORIZATION ***********************************************************************************************************************************
        [HttpPost("transaction/auto-categorize")]
        public async Task<IActionResult> AutoCategorise() {
            List<Rule> listOfRules = new List<Rule>();

            using (var sr = new StreamReader("rules.txt")) {

                while (sr.EndOfStream == false) {
                    var content = sr.ReadLine();
                    var partsOfRule = content.Split(',').ToList();

                    if (RowHasData(partsOfRule)) {

                        string ruleNumber = partsOfRule[0];
                        string title = partsOfRule[1];
                        string catcode = partsOfRule[2];
                        string predicate = partsOfRule[3];

                        Rule r = new Rule() {
                            RuleNumber = ruleNumber,
                            Title = title,
                            Catcode = catcode,
                            Predicate = predicate,
                        };

                        listOfRules.Add(r);
                    }
                }
            }

            await _transactionService.AutoCategorize(listOfRules);
            return Ok();
        }
    }
}