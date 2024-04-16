using API.Entities;
using Microsoft.Extensions.Options;

namespace API.All.DbContexts
{
    public class RefreshTokenContext : DbContext
    {
        public DbSet<SYS_USER> SysUsers { get; set; }
        public DbSet<SYS_REFRESH_TOKEN> SysRefreshTokens { get; set; }

        private readonly AppSettings _appSettings;

        public RefreshTokenContext(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (_appSettings.DbType == EnumDBType.MSSQL)
            {
                optionsBuilder.UseSqlServer(_appSettings.ConnectionStrings.CoreDb);
            }
            else if (_appSettings.DbType == EnumDBType.ORACLE)
            {
                optionsBuilder.UseOracle(_appSettings.ConnectionStrings.CoreDb);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 9);
        }
    }
}
