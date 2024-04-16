using Microsoft.Extensions.Options;

namespace API.All.DbContexts
{
    public class TmpDbContext: DbContext
    {

        private readonly AppSettings _appSettings;

        public TmpDbContext(IOptions<AppSettings> options) {
            _appSettings = options.Value;
        }

        public DbSet<SYS_MUTATION_LOG> SysMutationLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            if (_appSettings.DbType == EnumDBType.MSSQL)
            {
                if (!_appSettings.ConnectionStrings.CoreDb.Contains("Initial Catalog"))
                {
                    throw new Exception("The ConnectionString is not for MS SQL");
                }
                else
                {
                    optionsBuilder.UseSqlServer(_appSettings.ConnectionStrings.CoreDb, o => o.UseCompatibilityLevel(120));

                }
            }
            else if (_appSettings.DbType == EnumDBType.ORACLE)
            {
                if (!_appSettings.ConnectionStrings.CoreDb.Contains("DESCRIPTION"))
                {
                    throw new Exception("The ConnectionString is not for ORACLE");
                }
                else
                {
                    optionsBuilder.UseOracle(_appSettings.ConnectionStrings.CoreDb);
                }
            }

        }
    }
}
