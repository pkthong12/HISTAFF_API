using API.All.DbContexts;
using RegisterServicesWithReflection.Services.Base;
using System.Reflection;

namespace API.All.Services
{
    [ScopedRegistration]
    public class LanguageService: ILanguageService
    {
        private readonly FullDbContext _fullDbContext;
        public LanguageService(FullDbContext fullDbContext)
        {
            _fullDbContext = fullDbContext;
        }
        
        public async Task<string?> Translate(string key, string lang)
        {
            SYS_LANGUAGE? record = await _fullDbContext.SysLanguages.SingleOrDefaultAsync(x => x.KEY == key);
            if (record != null)
            {
                PropertyInfo? property = record.GetType().GetProperty(lang);
                var result = property?.GetValue(record);
                return (string?)result;
            } else
            {
                return null;
            }
        }
    }
}
