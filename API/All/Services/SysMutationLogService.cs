using API.All.DbContexts;
using API.Main;
using CORE.DTO;
using Microsoft.Extensions.Options;
using RegisterServicesWithReflection.Services.Base;

namespace API.All.Services
{
    [ScopedRegistration]
    public class SysMutationLogService : ISysMutationLogService
    {
        AppSettings _appSettings;
        private readonly FullDbContext _fullDbContext;
        public SysMutationLogService(FullDbContext fullDbContext, IOptions<AppSettings> options)
        {
            _fullDbContext = fullDbContext;
            _appSettings = options.Value;
        }

        public async Task<FormatedResponse> Write(SysMutationLogBeforeAfterRequest request, string sid)
        {

            string[] _befores = ToArray(request.Before);
            string[] _afters = ToArray(request.After);

            var now = DateTime.UtcNow;
            SYS_MUTATION_LOG model = new()
            {
                SYS_FUNCTION_CODE = request.SysFunctionCode,
                SYS_ACTION_CODE = request.SysActionCode,

                BEFORE = _befores[0],
                BEFORE1 = _befores[1],
                BEFORE2 = _befores[2],
                BEFORE3 = _befores[3],

                AFTER = _afters[0],
                AFTER1 = _afters[1],
                AFTER2 = _afters[2],
                AFTER3 = _afters[3],

                USERNAME = request.Username,
                CREATED_DATE = now,
                CREATED_BY = request.Username,
                UPDATED_DATE = now,
                UPDATED_BY = request.Username
            };

            await _fullDbContext.SysMutationLogs.AddAsync(model);
            _fullDbContext.SaveChanges();

            // clear out out-dated recored
            var outdated = _fullDbContext.SysMutationLogs.Where(x => x.CREATED_DATE!.Value.AddDays(_appSettings.RequestResponseLogger.DaysToKeep) < DateTime.UtcNow);

            _fullDbContext.SysMutationLogs.RemoveRange(outdated);
            _fullDbContext.SaveChanges();

            return new() { InnerBody = true };
        }

        private static string[] ToArray(string originalString)
        {
            // ORACLE ALLOWS MAXIMUM SIZE 2000 OF NVARCHAR2 TYPE
            int chunkSize = 2000;
            int numberOfChunks = (int)Math.Ceiling((double)originalString.Length / chunkSize);

            string[] stringArray = new string[4];

            for (int i = 0; i < numberOfChunks; i++)
            {
                int startIndex = i * chunkSize;
                int length = Math.Min(chunkSize, originalString.Length - startIndex);
                stringArray[i] = originalString.Substring(startIndex, length);
            }

            return stringArray;
        }

    }
}