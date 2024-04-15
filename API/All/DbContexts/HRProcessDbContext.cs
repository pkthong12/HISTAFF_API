using API.Entities;
using Microsoft.Extensions.Options;

namespace API.All.DbContexts
{
    public class HRProcessDbContext : DbContextBase
    {

        public HRProcessDbContext(IConfiguration config, DbContextOptions<HRProcessDbContext> options, IHttpContextAccessor accessor, IOptions<AppSettings> appSettings)
            : base(config, options, accessor, appSettings)
        {
        }

        public DbSet<SE_HR_PROCESS_TYPE> SeHrProcessType { get; set; }
        public DbSet<SE_HR_PROCESS> SeHrProcess { get; set; }
        public DbSet<SE_HR_PROCESS_DATA_MODEL> SeHrProcessDataModel { get; set; }
        public DbSet<SE_HR_PROCESS_INSTANCE> SeHrProcessInstance { get; set; }
        public DbSet<SE_HR_PROCESS_NODE> SeHrProcessNode { get; set; }

        protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 9);
        }


        /*
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }
        */

    }
}
