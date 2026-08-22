using market_watch.Data;
using market_watch.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;


namespace market_watch.Services
{
    public class AuditLogsService
    {

        private readonly MarketWatchTrainingContext _dbContext;

        //public AuditLogsService()
        //{
        //}

        public AuditLogsService(MarketWatchTrainingContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Log(int UserId, string ActionName, string TableName, DateTime ActionDate, string IpAddress)
        {
            AuditLog newLog = new AuditLog
            {
                UserId = UserId,
                ActionName = ActionName,
                TableName = TableName,
                ActionDate = ActionDate,
                Ipaddress = $"{IpAddress}"
            };

            _dbContext.AuditLogs.Add(newLog);
            _dbContext.SaveChanges();

            //Console.WriteLine("Test from AuditLogs");
        }
    }
}
