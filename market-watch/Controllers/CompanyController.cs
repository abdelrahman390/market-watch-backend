using market_watch.Data;
using Microsoft.AspNetCore.Mvc;
using market_watch.Models;
using System.Data;
using System.ComponentModel.Design;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace market_watch.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly MarketWatchTrainingContext _dbContext;
        public CompanyController(IConfiguration configuration, MarketWatchTrainingContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpGet("getComanies")]
        public IActionResult getComanies()
        {

            try
            {
                List<Companies> Companies = _dbContext.Companies.ToList();

                return Ok(Companies);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(
                    500,
                    $"Error: {ex.Message}"
                );
            }
        }

        [HttpPost("addCompany")]
        public IActionResult addCompany(string? Symbol, string? CompanyName, decimal? FaceValue, DateOnly ListedDate)
        {

            try
            {
                bool UniqSymbol = !_dbContext.Companies.Any(c => c.Symbol == Symbol);
                bool anyEmpty = !((string.IsNullOrEmpty(Symbol)) && string.IsNullOrEmpty(CompanyName) && FaceValue > 0);
                bool validDate = ListedDate <= DateOnly.FromDateTime(DateTime.Today);

                Console.WriteLine($"UniqSymbol: {UniqSymbol}");
                Console.WriteLine($"anyEmpty: {anyEmpty}");
                Console.WriteLine($"validDate: {validDate}");

                if(UniqSymbol && anyEmpty && validDate)
                {
                    Companies newCompany = new Companies
                    {
                        Symbol = string.IsNullOrEmpty(Symbol) ? string.Empty : Symbol,
                        CompanyName = CompanyName,
                        FaceValue = FaceValue,
                        ListedDate = ListedDate
                    };

                    _dbContext.Companies.Add(newCompany);
                    _dbContext.SaveChanges();

                    return Ok($"{CompanyName} is now added");
                } else
                {
                    return StatusCode(
                        500,
                        "Data is in correct"
                    );
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(
                    500,
                    $"Error: {ex.Message}"
                );
            }
        }
    }
}
