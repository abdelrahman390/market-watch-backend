using market_watch.Data;
using market_watch.Models;
using market_watch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace market_watch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarketController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MarketWatchTrainingContext _dbContext;
        private readonly AuditLogsService _AuditLogsService;

        public MarketController(IConfiguration configuration, MarketWatchTrainingContext dbContext, AuditLogsService AuditLogsService)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _AuditLogsService = AuditLogsService;
        }

        [Authorize]
        [HttpGet("getMarkets")]
        public IActionResult getResultsEntity()
        {

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userRoleClaim = User.FindFirst(ClaimTypes.Role);
                var userIpAdress = HttpContext.Connection.RemoteIpAddress?.ToString();


                List<Market> sectors = _dbContext.Markets.ToList();

                _AuditLogsService.Log(int.Parse(userIdClaim.Value), "getMarkets", "Markets", DateTime.Now, userIpAdress);

                return Ok(sectors);

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
