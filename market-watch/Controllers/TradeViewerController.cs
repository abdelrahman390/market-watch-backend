using market_watch.Data;
using market_watch.Models;
using market_watch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.Design;
using System.Data;
using System.Security.Claims;

namespace market_watch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MarketWatchTrainingContext _dbContext;
        private readonly AuditLogsService _AuditLogsService;

        public TradeController(IConfiguration configuration, MarketWatchTrainingContext dbContext, AuditLogsService auditLogsService)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _AuditLogsService = auditLogsService;
        }

        [Authorize]
        [HttpGet("getTradesAdo/{companyId}")]
        public IActionResult getResultsAdo(int companyId)
        {
            //Console.WriteLine("TradeController was called!");
            //Console.WriteLine($"Company ID: {companyId}");

            string connectionString =
                _configuration.GetConnectionString("DefaultConnection")!;

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userIpAdress = HttpContext.Connection.RemoteIpAddress?.ToString();

                using SqlConnection connection =
                    new SqlConnection(connectionString);

                using SqlCommand command =
                    new SqlCommand("usp_SearchTrades", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                    "@CompanyId",
                    SqlDbType.Int
                ).Value = companyId;

                connection.Open();

                Console.WriteLine("Database connected successfully!");

                using SqlDataReader reader =
                    command.ExecuteReader();

                List<Trade> trades = new List<Trade>();

                while (reader.Read())
                {

                    Trade tempTrade = new Trade
                    {
                        TradeId = Convert.ToInt32(reader["TradeId"]),
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Price = Convert.ToDecimal(reader["Price"]),
                        TotalValue = Convert.ToDecimal(reader["TotalValue"])
                    };

                    trades.Add(tempTrade);
                }

                _AuditLogsService.Log(int.Parse(userIdClaim.Value), "getTradesAdo/{companyId}", "Trades", DateTime.Now, userIpAdress);

                return Ok(trades);
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
        [HttpGet("getTrades")]
        public IActionResult getResultsEntity(int companyId)
        {

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userIpAdress = HttpContext.Connection.RemoteIpAddress?.ToString();

                List<Trade> trades = _dbContext.Trades.Where(t => t.CompanyId == companyId).ToList();

                _AuditLogsService.Log(int.Parse(userIdClaim.Value), "getTrades", "Trades", DateTime.Now, userIpAdress);

                return Ok(trades);

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

