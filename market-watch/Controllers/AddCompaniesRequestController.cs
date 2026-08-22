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
    public class AddCompaniesRequestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MarketWatchTrainingContext _dbContext;
        private readonly AuditLogsService _AuditLogsService;

        public AddCompaniesRequestController(IConfiguration configuration, MarketWatchTrainingContext dbContext, AuditLogsService AuditLogsService)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _AuditLogsService = AuditLogsService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("approveCompanyRequist")]
        public IActionResult approveCompanyRequist(int RequistId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = User.FindFirst(ClaimTypes.Role);
            var userIpAdress = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (userIdClaim == null || userRoleClaim == null || userIpAdress == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            AddCompaniesRequest currRequist = _dbContext.AddCompaniesRequests.FirstOrDefault(r => r.RequistId == RequistId);

            int ManagerId = int.Parse(userIdClaim.Value);

            Company newCompany = new Company
            {
                Symbol = currRequist.Symbol,
                CompanyName = currRequist.CompanyName,
                SectorId = currRequist.SectorId,
                MarketId = currRequist.MarketId,
                FaceValue = currRequist.FaceValue,
                ListedDate = currRequist.ListedDate,
                IsActive = currRequist.IsActive
            };

            currRequist.Status = "Approved";
            currRequist.MangerId = ManagerId;

            _dbContext.Companies.Add(newCompany);
            _dbContext.SaveChanges();

            _AuditLogsService.Log(int.Parse(userIdClaim.Value), "approveCompanyRequist", "Companies", DateTime.Now, userIpAdress);

            return Ok(newCompany.CompanyName);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("rejectCompanyRequist")]
        public IActionResult rejectCompanyRequist(int RequistId, string Reason)
        {
            AddCompaniesRequest currRequist = _dbContext.AddCompaniesRequests.FirstOrDefault(r => r.RequistId == RequistId);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userIpAdress = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            if (currRequist == null)
            {
                return BadRequest("Their is No Requist with this id.");
            }

            int ManagerId = int.Parse(userIdClaim.Value);

            currRequist.Status = "Rejected";
            currRequist.MangerId = ManagerId;
            currRequist.Reason = Reason;

            _dbContext.SaveChanges();

            _AuditLogsService.Log(int.Parse(userIdClaim.Value), "rejectCompanyRequist", "AddCompaniesRequests", DateTime.Now, userIpAdress);

            return Ok($"Company {currRequist.CompanyName} has been rejected.");
        }

    }
}
