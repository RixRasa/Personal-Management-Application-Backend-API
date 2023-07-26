using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TransactionsAPI.Error {
    public class CustomError {
        public string problem { get; set; }
        public string message { get; set; }
        public string details { get; set; }
    }
}
