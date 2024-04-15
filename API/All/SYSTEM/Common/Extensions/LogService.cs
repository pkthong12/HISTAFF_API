using API.All.DbContexts;
namespace Common.Extensions;
public interface ILogService{
     Task<bool> Log( ApiAuditLog logObj );    
}
public class LogService : ILogService
{
 
    private readonly CoreDbContext _dbContext;
    public LogService(CoreDbContext dbContext)
    {
        {
            _dbContext = dbContext;
 
        }
    }
   
    async Task<bool> ILogService.Log(ApiAuditLog logObj)
    {
            var logEntry = new ApiAuditLog
            {
                ControllerName = logObj.ControllerName,
                Status = logObj.Status,
                RequestData = logObj.RequestData,
                TraceId = logObj.TraceId,
                CREATED_BY= logObj.CREATED_BY,
                CREATED_DATE = logObj.CREATED_DATE,
                ResponseBody = logObj.ResponseBody,
                Method = logObj.Method,
            };
            _dbContext.ApiAuditLogs.Add(logEntry);
            await _dbContext.SaveChangesAsync();
        return  true;
    }
}
 