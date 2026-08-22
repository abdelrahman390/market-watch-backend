using market_watch.Data;
using market_watch.Models;
using market_watch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace market_watch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MarketWatchTrainingContext _dbContext;
        private readonly JwtService _jwtService;
        private readonly AuditLogsService _AuditLogsService;

        public AuditLogController(IConfiguration configuration, MarketWatchTrainingContext dbContext, AuditLogsService AuditLogsService)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _AuditLogsService = AuditLogsService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAuditLogs")]
        public IActionResult getAuditLogs()
        {
            List<AuditLog> res = _dbContext.AuditLogs.ToList();

            return Ok(res);
        }
    }
}
