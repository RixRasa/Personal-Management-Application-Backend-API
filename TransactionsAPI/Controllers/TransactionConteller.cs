using Microsoft.AspNetCore.Mvc;
using TransactionsAPI.Models;
using TransactionsAPI.Services;
using TransactionsAPI.Commands;
using Microsoft.AspNetCore.Cors;

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
        public async Task<IActionResult> GetTransactions([FromQuery] string? transactionKindQuery = null, [FromQuery] DateTime? start_date = null, [FromQuery] DateTime? end_date = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] SortOrder sortOrder = SortOrder.Asc, [FromQuery] string? sortBy = null) {

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
                                Beneficiary_name = Benef_name,
                                Date = date,
                                Amount = amount,
                                Direction = directions,
                                Description = description,
                                Currency = currency,
                                Mcc = mcc,
                                Kind = kind
                            });
                            if (inserted == false) return BadRequest("Transaction Already Exist");
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
                            
                            if(inserted == false) return BadRequest("Couldnt import category");
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
            if (categorised == false) return BadRequest("Category or Transaction You picked doesnt exist");
            else return Ok("Categorisation completed"); 
        }


        //METHOD CONNECTED TO ANALYZING SPENDINGS *************************************************************************************************************
        [HttpGet("spending-analytics")]
        public async Task<IActionResult> GetSpendingAnalytics([FromQuery] string? catcode = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null, [FromQuery] DirectionKind? directionKind = null) {
            List<SpendingByCategory> list = await _transactionService.GetAnaliytics(catcode, startDate, endDate, directionKind);
            return Ok(list);
        }


        //METHOD CONNECTED TO SPLITING A TRANSACTION IN SMALLER TRANSACTIONS***********************************************************************************
        [HttpPost("transactions/{id}/split")]
        public async Task<IActionResult> SplitTransaction([FromBody] SplitTransactionCommand splits, [FromRoute] string id) {
            var splited = await _transactionService.SplitTransaction(splits.Splits, id);
            if (splited == false) return BadRequest("Could not be splited");
            return Ok("Split is done");
        }


        //METHOD CONNECTED TO GETTING ALL ROOT CATEGORIES OR CHILD-CATEGORIES OF ROOT CATEGORY
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories([FromQuery] string? parentCode = null) {
            List<Category> list = await _transactionService.GetCategories(parentCode);
            return Ok(list);
        }

    }
}