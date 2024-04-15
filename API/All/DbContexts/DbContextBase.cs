using Microsoft.Extensions.Options;
using API.All.SYSTEM.Common.StaticClasses;
using CORE.KeylessEntity;

namespace API.All.DbContexts
{
    /*
     * DbContextBase used to track logs
    */
    public class DbContextBase : DbContext
    {

        /* 
        * Start: Keyless
        * In addition to regular entity types, an EF Core model can contain keyless entity types, 
        * which can be used to carry out database queries against data that doesn't contain key values.
        */

        public DbSet<UniqueIndex> UniqueIndexes { get; set; }

        /* End Keyless*/


        public long EmpId { get; set; }
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        public string CurrentUserId { get; set; }
        public static bool isMigration;

        public readonly IConfiguration _config;
        public AppSettings _appSettings;

        public DbContextBase(IConfiguration config, DbContextOptions options, IHttpContextAccessor accessor, IOptions<AppSettings> appSettings) : base(options)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _appSettings = appSettings.Value;

            isMigration = false;
            UserName = "";
            CurrentUserId = "";

            try
            {
                if (accessor.HttpContext != null)
                {
                    var authorization = accessor.HttpContext.Request.Headers.ContainsKey("Authorization");
                    if (authorization)
                    {
                        string? token = accessor.HttpContext.Request.Headers.Authorization[0]?.Substring("Bearer ".Length);

                        if (token != null)
                        {
                            var tokenInfo = JwtHelper.GetTokenInfo(token);
                            if (tokenInfo != null)
                            {
                                CurrentUserId = tokenInfo.Sid;
                                UserName = tokenInfo.Typ;
                                IsAdmin = tokenInfo.IsAdmin == "True";
                                var res = tokenInfo.Iat;
                                if (res != null && res != "")
                                {
                                    EmpId = int.Parse(res);
                                }
                            }
                        }

                    }
                }
            }
            catch
            {
                throw;
            }
        }

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
                    optionsBuilder.UseSqlServer(_appSettings.ConnectionStrings.CoreDb);

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