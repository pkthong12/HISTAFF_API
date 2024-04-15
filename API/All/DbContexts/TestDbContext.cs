using API.Entities;
using Microsoft.Extensions.Options;

namespace API.All.SYSTEM.CoreDAL.EntityFrameworkCore
{
    public class TestDbContext: DbContext
    {
        private readonly AppSettings _appSettings;
        public TestDbContext(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_appSettings.ConnectionStrings.CoreDb);
        }

        public DbSet<SYS_USER> SysUsers { get; set; }

    }
}
