using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using BillCollector.Infrastructure.DbContexts;
using BillCollector.Infrastructure.UnitOfWork;
using BillCollector.Application;
using Serilog;
using Serilog.Events;
using BillCollector.Infrastructure.Logger;
using BillCollector.Infrastructure.HttpClient;
using BillCollector.Infrastructure.Repository;
using BillCollector.Infrastructure;
using BillCollector.Infrastructure.Cache;
using Pomelo.EntityFrameworkCore.MySql.Internal;
using BillCollector.Application.Interface;
using BillCollector.Application.Services;
using BillCollector.CbaProxy;


namespace BillCollector.API
{
    public static class ServiceRegistration
    {
        public static WebApplicationBuilder RegisterCustomServices(this WebApplicationBuilder builder, IConfiguration Configuration)
        {
            #region Database related registration
            // Register MySQL context
            //builder.Services.AddDbContext<BillCollectorDbContext>(options =>
            //{
            //    options.UseMySql(
            //        Configuration.GetConnectionString("BillCollector"),
            //        ServerVersion.AutoDetect(Configuration.GetConnectionString("BillCollector")),
            //        mysqlOptions =>
            //        {
            //            mysqlOptions.CommandTimeout(1000);
            //            mysqlOptions.EnableRetryOnFailure(
            //                maxRetryCount: 5,
            //                maxRetryDelay: TimeSpan.FromSeconds(30),
            //                errorNumbersToAdd: null
            //            );
            //        }
            //    )
            //    .EnableDetailedErrors()
            //    .EnableSensitiveDataLogging();
            //}, ServiceLifetime.Singleton);

            //Register Postgres context
            builder.Services.AddDbContext<BillCollectorDbContext>(options =>
            {
                options.UseNpgsql(
                    Configuration.GetConnectionString("BillCollector"),
                    postgresOptions =>
                    {
                        postgresOptions.CommandTimeout(1000);
                        postgresOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null
                        );
                    }
                )
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();
            }, ServiceLifetime.Singleton);

            // Register MongoDB context
            builder.Services.AddSingleton(sp =>
            {
                var connectionString = Configuration.GetConnectionString("MongoContext");
                var databaseName = Configuration.GetSection("ConnectionStrings").GetSection("MongoDbSettings").GetSection("DatabaseName").Value;
                return new MongoDbContext(connectionString, databaseName);
            });
            // Register mongoDB repository
            builder.Services.AddTransient(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));

            // Register DapperContext
            builder.Services.AddSingleton(sp =>
            {
                var connectionString = Configuration.GetConnectionString("BillCollector");
                return new DapperContext(connectionString);
            });
            
            // Register the generic DapperRepository
            builder.Services.AddTransient(typeof(IDapperRepository), typeof(DapperRepository));
            #endregion
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.Configure<AppKeys>(Configuration.GetSection("AppKeys"));

            //Let's maintain order of A-Z so it's easier finding things around
            builder.Services.AddMemoryCache();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddSingleton(typeof(ICacheManager<,>), typeof(CacheManager<,>));
           // builder.Services.AddTransient<IClientService, ClientService>();
            builder.Services.AddTransient(typeof(ILog<>), typeof(LogService<>));
            builder.Services.AddTransient<IRestSharpClient, RestSharpClient>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            #region CBA related registration
            builder.Services.AddScoped<IProductProxy, ProductProxy>();
            builder.Services.AddScoped<CBAProxy>();
            #endregion

            builder.Services.AddDistributedMemoryCache();

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.File("Serilogs\\AppLogs_.log",
                                    outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {ClassName} {MethodName} {Message} {NewLine} {Exception}",
                                    rollingInterval: RollingInterval.Day,
                                    rollOnFileSizeLimit: true,
                                    fileSizeLimitBytes: 10000);
            });


            return builder;

        }
    }
}
