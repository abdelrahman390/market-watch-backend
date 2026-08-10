using market_watch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using market_watch.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.Design;

namespace market_watch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MarketWatchTrainingContext _dbContext;
        public TradeController(IConfiguration configuration, MarketWatchTrainingContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpGet("getTradesAdo/{companyId}")]
        public IActionResult getResultsAdo(int companyId)
        {
            Console.WriteLine("TradeController was called!");
            Console.WriteLine($"Company ID: {companyId}");

            string connectionString =
                _configuration.GetConnectionString("DefaultConnection")!;

            try
            {
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

                    //Console.WriteLine(
                    //    $"TradeId: {reader["TradeId"]}"
                    //);

                    //Console.WriteLine(
                    //    $"OrderId: {reader["OrderId"]}"
                    //);

                    //Console.WriteLine(
                    //    $"Quantity: {reader["Quantity"]}"
                    //);

                    //Console.WriteLine(
                    //    $"Price: {reader["Price"]}"
                    //);

                    //Console.WriteLine(
                    //    $"TotalValue: {reader["TotalValue"]}"
                    //);

                    //Console.WriteLine("------------------------");
                }

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

        [HttpGet("getTrades")]
        public IActionResult getResultsEntity(int companyId)
        {

            try
            {
                List<Trade> trades = _dbContext.Trades.Where(t => t.CompanyId == companyId).ToList();

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

