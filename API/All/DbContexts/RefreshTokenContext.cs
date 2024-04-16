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
            optionsBuilder.UseSqlServer(_appSettings.ConnectionStrings.CoreDb);
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
