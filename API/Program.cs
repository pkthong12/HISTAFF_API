using API;
using CoreDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using RegisterServicesWithReflection.Extensions;
using API.All.SYSTEM.CoreDAL.EntityFrameworkCore;
using API.All.SYSTEM.Common.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CORE.DataContract;
using API.All.DbContexts;
using API.Socket;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileProviders;
using System.Net;
using Swashbuckle.AspNetCore.SwaggerGen;
using API.All.Logger.TextLogger;
using Hangfire;
using API.Main;
using Hangfire.Dashboard;
using API.All.Logger;
using Hangfire.Oracle.Core;
using CORE.StaticConstant;
using System.IdentityModel.Tokens.Jwt;
using API.GrpcServices;
using CORE.Middleware;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

builder.Logging.ClearProviders();
builder.Logging.AddTextLogger(options =>
{
    config.GetSection("Logging").GetSection("Text").GetSection("Options").Bind(options);
});

// Add services to the container.

services.Configure<AppSettings>(config.GetSection("AppSettings"));
var _appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();


if (_appSettings?.DbType == EnumDBType.MSSQL)
{
    services.AddHangfire(x => x.UseSqlServerStorage(_appSettings?.ConnectionStrings.CoreDb));
}
else if (_appSettings?.DbType == EnumDBType.ORACLE)
{
    services.AddHangfire(x => x.UseStorage(new OracleStorage(_appSettings?.ConnectionStrings.CoreDb)));
}

services.AddHangfireServer();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/hubs/signal")))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

#region DbContexts
//services.AddDbContext<PatternDbContext>();
services.AddDbContext<FullDbContext>();
services.AddDbContext<TestDbContext>();
services.AddDbContext<CoreDbContext>();
services.AddDbContext<ProfileDbContext>();
services.AddDbContext<RefreshTokenContext>();
services.AddDbContext<AttendanceDbContext>();
#endregion DbContexts

#region: STANDARD-DI
services.RegisterServices(builder.Configuration);
#endregion: STANDARD-DI

#region: NONE-STANDARD-DI
// To set NameIdentifier property of each Hub connection
services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
#endregion: NONE-STANDARD-DI

services.AddRouting(o => o.LowercaseQueryStrings = true);
//services.AddAuthorization();

services.AddIdentity<SysUser, IdentityRole>(opts =>
{
    opts.Password.RequiredUniqueChars = 0;
    opts.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<CoreDbContext>()
.AddDefaultTokenProviders();


services.AddCors(options =>
{
    /* Latter, in Production, we need to add specific policy */
    options.AddPolicy("Development",
          builder =>
              builder
                .AllowAnyHeader()
                .WithExposedHeaders("X-Message-Code", "Content-Type")
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true)
          );
});

services.AddSignalR();

services.AddControllers(options =>
{
    //ModelValidation
    options.Filters.Add<ModelValidationFilterAttribute>();

    // RequestResponseLogger
    options.Filters.Add(new RequestResponseLoggerActionFilter());
    options.Filters.Add(new RequestResponseLoggerErrorFilter());

}).ConfigureApiBehaviorOptions(options =>
{
    //Disable The Default
    options.SuppressModelStateInvalidFilter = true;
})
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new SnackToCamelCaseContractResolver();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// services.AddSwaggerGenNewtonsoftSupport();
services.AddEndpointsApiExplorer();

#region AddGrpc
services.AddGrpcReflection();
services.AddGrpc().AddJsonTranscoding();
services.AddGrpcSwagger();
#endregion AddGrpc

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("system", new OpenApiInfo { Title = "Histaff VNS - SYSTEM Swagger UI", Version = "v5472" });
    c.SwaggerDoc("om", new OpenApiInfo { Title = "Histaff VNS - OM Swagger UI", Version = "v5472" });
    c.SwaggerDoc("profile", new OpenApiInfo { Title = "Histaff VNS - PROFILE Swagger UI", Version = "v5472" });
    c.SwaggerDoc("attendance", new OpenApiInfo { Title = "Histaff VNS - ATTENDANCE Swagger UI", Version = "v5472" });
    c.SwaggerDoc("payroll", new OpenApiInfo { Title = "Histaff VNS - PAYROLL Swagger UI", Version = "v5472" });
    c.SwaggerDoc("insurance", new OpenApiInfo { Title = "Histaff VNS - INSURANCE Swagger UI", Version = "v5472" });
    c.SwaggerDoc("training", new OpenApiInfo { Title = "Histaff VNS - TRAINING Swagger UI", Version = "v5472" });
    c.SwaggerDoc("developer", new OpenApiInfo { Title = "Histaff VNS - DEVELOPER TESTING Swagger UI", Version = "v5472" });
    c.SwaggerDoc("recruitment", new OpenApiInfo { Title = "Histaff VNS - RECRUITMENT Swagger UI", Version = "v5472" });
    c.SwaggerDoc("portal", new OpenApiInfo { Title = "Histaff VNS - PORTAL Swagger UI", Version = "v5472" });
    c.SwaggerDoc("gRPC", new OpenApiInfo { Title = "gRPC using .NET 7", Version = "v1" });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);

    /* Swagger Grouping */
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        if (api.ActionDescriptor.DisplayName == "HTTP: GET /")
        {
            return new[] { "gRPC" };
        }

        if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        if ((api.RelativePath ?? "").StartsWith("api/grpc/"))
        {
            return new[] { "gRPC" };
        }

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });

    c.DocInclusionPredicate((name, api) =>
    {

        try
        {
            if (name == "gRPC")
            {
                return (api.GroupName == null);
            }

            if (api.GroupName == null)
            {
                return false;
            };
            if (api.GroupName.Length > (4 + name.Length))
            {

                if (api.GroupName.ToLower().Substring(4, name.Length) == name.ToLower())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch (SwaggerGeneratorException ex)
        {
            return false;
        }
    });
});
//builder.Services.AddScoped<ILogService, LogService>();
var app = builder.Build();

/* Latter, in Production, we need to use specific policy */
app.UseCors("Development");

app.UseMiddleware<JwtMiddleware>();

app.UseWhen(
    predicate: context => context.Request.Query.ContainsKey("enableTimeZoneConversion") &&
                            context.Request.Query["enableTimeZoneConversion"] == "true" &&
                            context.Request.Headers.ContainsKey("Content-Type") &&
                            context.Request.Headers.ContentType == "application/json",
    configuration: builder => builder.UseMiddleware<RequestResponseTimeZoneConverter>()
);

app.UseWhen(
    predicate: context => context.Request.Query.ContainsKey("computingWithLocalTimeZone") &&
                            context.Request.Query["computingWithLocalTimeZone"] == "true" &&
                            context.Request.Headers.ContainsKey("Content-Type") &&
                            context.Request.Headers.ContentType == "application/json",
    configuration: builder => builder.UseMiddleware<RequestTakenAsIfItIsInLocalTimeZone>()
);

app.UseWhen(
predicate: context =>
{
    var sid = context.Request.Sid(_appSettings!);
    if (
        !context.Request.Path.StartsWithSegments("/api") ||
        context.Request.Method != "POST" ||
        context.Response.StatusCode != 200 ||
        context.Request.Path.StartsWithSegments("/hubs/signal") ||
        context.Request.Path.StartsWithSegments("/jobs") ||
        context.Request.Path.StartsWithSegments("/assets") ||
        sid == ""
    ) return false;
    return true;
},
configuration: builder =>
{
    builder.UseMiddleware<RequestResponseLoggerMiddleware>();
});

app.UseWhen(
predicate: context =>
{
    if (context.Request.Path.StartsWithSegments("/api"))
    {
        try
        {
            var jwtToken = context.Request.Headers.Authorization[0]!.ToString().Split("Bearer ")[1];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var isMobile = token?.Claims.SingleOrDefault(x => x.Type == "appType")!.Value == AppType.MOBILE;
            return isMobile;
        }
        catch
        {
            return false;
        }
    }
    else
    {
        return false;
    }
},
configuration: builder =>
{
    builder.UseMiddleware<MessageCodeTranslationMiddleware>();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/system/swagger.json", "SYSTEM API");
        c.SwaggerEndpoint("/swagger/om/swagger.json", "OM API");
        c.SwaggerEndpoint("/swagger/profile/swagger.json", "PROFILE API");
        c.SwaggerEndpoint("/swagger/attendance/swagger.json", "ATTENDANCE API");
        c.SwaggerEndpoint("/swagger/payroll/swagger.json", "PAYROLL API");
        c.SwaggerEndpoint("/swagger/insurance/swagger.json", "INSURANCE API");
        c.SwaggerEndpoint("/swagger/training/swagger.json", "TRAINING API");
        c.SwaggerEndpoint("/swagger/developer/swagger.json", "DEVELOPER TESTING API");
        c.SwaggerEndpoint("/swagger/portal/swagger.json", "PORTAL");
        c.SwaggerEndpoint("/swagger/recruitment/swagger.json", "RECRUITMENT");
        c.SwaggerEndpoint("/swagger/gRPC/swagger.json", "gRPC using .NET7");
        c.InjectStylesheet("/custom.css");
    });
}

/* Using Hangfire */
app.UseHangfireDashboard("/jobs", new DashboardOptions()
{
    Authorization = new[] { new HangFireAuthorizationFilter() },
    //IsReadOnlyFunc = (DashboardContext context) => true
    IsReadOnlyFunc = (DashboardContext context) => false
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "static")),
    RequestPath = "/static"
});

/* START: OnPrepareResponse will be hit only when static files stored inside wwwroot */
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (ctx.Context.Request.Path.StartsWithSegments("/avatars"))
        {
            var authorization = ctx.Context.Request.HttpContext.Request.Headers["Authorization"];
            Console.WriteLine(authorization);
            // respond HTTP 401 Unauthorized with empty body.
            ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            ctx.Context.Response.ContentLength = 0;
            ctx.Context.Response.Body = Stream.Null;
        }
    }
});
/* END: OnPrepareResponse will be hit only when static files stored inside wwwroot */

// Configure the HTTP request pipeline.
app.MapGrpcService<SysFunctionService>();
app.MapGrpcService<SysGroupService>();
app.MapGrpcReflectionService();

app.UseAuthorization();

app.MapControllers();

app.MapHub<SignalHub>("/hubs/signal");

app.Run();