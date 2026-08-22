using market_watch.Data;
using market_watch.Models;
using market_watch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;


namespace market_watch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SectorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MarketWatchTrainingContext _dbContext;
        private readonly AuditLogsService _AuditLogsService;

        public SectorController(IConfiguration configuration, MarketWatchTrainingContext dbContext, AuditLogsService AuditLogsService)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _AuditLogsService = AuditLogsService;
        }

        [Authorize]
        [HttpGet("getSectors")]
        public IActionResult getResultsEntity()
        {

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userIpAdress = HttpContext.Connection.RemoteIpAddress?.ToString();

                List<Sector> sectors = _dbContext.Sectors.ToList();

                _AuditLogsService.Log(int.Parse(userIdClaim.Value), "getSectors", "Sectors", DateTime.Now, userIpAdress);

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
