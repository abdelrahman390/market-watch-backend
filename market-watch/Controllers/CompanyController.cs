using market_watch.Data;
using market_watch.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.Design;
using System.Data;
using System.Security.Claims;
using market_watch.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Net;

namespace market_watch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MarketWatchTrainingContext _dbContext;
        private readonly AuditLogsService _AuditLogsService;
        public CompanyController(IConfiguration configuration, MarketWatchTrainingContext dbContext, AuditLogsService AuditLogsService)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _AuditLogsService = AuditLogsService;
        }

        [Authorize]
        [HttpGet("getCompanies")]
        public IActionResult getCompanies()
        {

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userRoleClaim = User.FindFirst(ClaimTypes.Role);
                var userIpAdress = HttpContext.Connection.RemoteIpAddress?.ToString();

                if (userIdClaim == null || userRoleClaim == null || userIpAdress == null)
                {
                    return Unauthorized("Missing data in the token.");
                }

                List<Company> Companies = _dbContext.Companies.ToList();

                 _AuditLogsService.Log(int.Parse(userIdClaim.Value), "getCompanies", "Companies", DateTime.Now, userIpAdress);

                //Console.WriteLine($"Test from AuditLogs second: {testVal}");

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

        // Status: Pending, Approved, Rejected
        [Authorize]
        [HttpPost("addCompany")]
        public IActionResult addCompany(string? Symbol, string? CompanyName, int SectorId, int MarketId, decimal? FaceValue, DateOnly ListedDate, bool IsActive) {

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userRoleClaim = User.FindFirst(ClaimTypes.Role);
                var userIpAdress = HttpContext.Connection.RemoteIpAddress?.ToString();

                if (userIdClaim == null || userRoleClaim == null || userIpAdress == null)
                {
                    return Unauthorized("User ID not found in token.");
                }

                int userId = int.Parse(userIdClaim.Value);
                string userRole = userRoleClaim.Value;

                bool UniqSymbol = !_dbContext.Companies.Any(c => c.Symbol == Symbol);
                bool anyEmpty = !((string.IsNullOrEmpty(Symbol)) && string.IsNullOrEmpty(CompanyName) && FaceValue > 0);
                bool validDate = ListedDate <= DateOnly.FromDateTime(DateTime.Today);
                bool validSector = _dbContext.Sectors.Any(s => s.SectorId == SectorId);
                bool validMarket = _dbContext.Markets.Any(m => m.MarketId == MarketId);

                Console.WriteLine($"UniqSymbol: {UniqSymbol}");
                Console.WriteLine($"anyEmpty: {anyEmpty}");
                Console.WriteLine($"validDate: {validDate}");
                Console.WriteLine($"validSector: {validSector}");
                Console.WriteLine($"validMarket: {validMarket}");


                if (UniqSymbol && anyEmpty && validDate && validSector && validMarket)
                {

                    if(userRole == "Admin")
                    {
                        Company newCompany = new Company
                        {
                            Symbol = Symbol,
                            CompanyName = CompanyName,
                            SectorId = SectorId,
                            MarketId = MarketId,
                            FaceValue = FaceValue,
                            ListedDate = ListedDate,
                            IsActive = IsActive
                        };

                        _AuditLogsService.Log(int.Parse(userIdClaim.Value), "addCompany", "Companies", DateTime.Now, userIpAdress);

                        _dbContext.Companies.Add(newCompany);
                    } else {
                        AddCompaniesRequest newRequist = new AddCompaniesRequest
                        {
                            UserId = userId,
                            Symbol = string.IsNullOrEmpty(Symbol) ? string.Empty : Symbol,
                            CompanyName = CompanyName,
                            SectorId = SectorId,
                            MarketId = MarketId,
                            FaceValue = FaceValue,
                            ListedDate = ListedDate,
                            IsActive = IsActive,
                            Status = "Pending"
                        };

                        _AuditLogsService.Log(int.Parse(userIdClaim.Value), "addCompany", "AddCompaniesRequests", DateTime.Now, userIpAdress);
                        _dbContext.AddCompaniesRequests.Add(newRequist);    
                    }

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

        [Authorize]
        [HttpGet("searchForCompany")]
        public IActionResult searchForCompany(string? Symbol, string? CompanyName)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userRoleClaim = User.FindFirst(ClaimTypes.Role);
                var userIpAdress = HttpContext.Connection.RemoteIpAddress?.ToString();

                if (string.IsNullOrEmpty(Symbol) ||
                    string.IsNullOrEmpty(CompanyName))
                {
                    return BadRequest("Symbol and CompanyName are required.");
                }

                Company? company = _dbContext.Companies
                    .FirstOrDefault(c =>
                        c.CompanyName == CompanyName &&
                        c.Symbol == Symbol);

                if (company == null)
                {
                    return NotFound("Company not found.");
                }

                _AuditLogsService.Log(int.Parse(userIdClaim.Value), "searchForCompany", "Companies", DateTime.Now, userIpAdress);

                return Ok(company);
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
