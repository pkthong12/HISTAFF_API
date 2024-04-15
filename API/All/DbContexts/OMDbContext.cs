// using API.Entities;
using API.Entities.OM;
using Microsoft.Extensions.Options;

namespace API.All.DbContexts;
 public class OMDbContext : DbContextBase {
    public OMDbContext(IConfiguration config, DbContextOptions<OMDbContext> options, IHttpContextAccessor accessor, IOptions<AppSettings> appSettings)
            : base(config, options, accessor, appSettings) {}
    
    public DbSet<OM_ORGANIZATION> OMOrganizations { get; set; }
    public DbSet<OM_JOB> OM_Jobs { get; set; }

    protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>()
            .HavePrecision(18, 9);
    }
}