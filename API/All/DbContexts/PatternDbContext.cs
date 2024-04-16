using Microsoft.Extensions.Options;

namespace API.All.DbContexts
{
    public class PatternDbContext : FullDbContext
    {
        public PatternDbContext(
            IConfiguration config,
            DbContextOptions<FullDbContext> options,
            IHttpContextAccessor accessor,
            IOptions<AppSettings> appSettings
            ) : base(config, options, accessor, appSettings) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            if (_appSettings.PatternDbType == EnumDBType.MSSQL)
            {
                optionsBuilder.UseSqlServer(_appSettings.ConnectionStrings.PatternDb);
            }
            else if (_appSettings.DbType == EnumDBType.ORACLE)
            {
                optionsBuilder.UseOracle(_appSettings.ConnectionStrings.PatternDb);
            }

        }

    }
}
