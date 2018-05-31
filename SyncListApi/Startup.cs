using System;
using System.IO;
using System.Xml.XPath;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using SyncList.CommonLibrary.Filters;
using SyncList.CommonLibrary.Middlewares;
using SyncList.SyncListApi.CachingManagement.Implementations;
using SyncList.SyncListApi.CachingManagement.Interfaces;
using SyncList.SyncListApi.Data;
using SyncList.SyncListApi.Data.Repositories.Implementations;
using SyncList.SyncListApi.Data.Repositories.Interfaces;

namespace SyncList.SyncListApi
{
    public class Startup
    {

        private string ApiName => "SyncList";
        private readonly IHostingEnvironment _hostingEnv;
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            _hostingEnv = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            
            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("SqlLite")));

            
            var sp = services.BuildServiceProvider();
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "SyncListApi:";
            });
            
            services.AddSingleton<IRedisDatabase>(s =>
            {
                var options = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"));
                options.ResolveDns = true;
                var redis = ConnectionMultiplexer.Connect(options);
                
                var redisLogger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<RedisDatabase>();
                return new RedisDatabase(redis.GetDatabase(), redisLogger, $"{ApiName}:");
            });
            
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IListsRepository, ListsRepository>();
            services.AddScoped<IItemsRepository, ItemsRepository>();
            services.AddScoped<IItemsListRelationsRepository, ItemsListRelationsRepository>();
            services.AddScoped<IItemsInListCacheManager, ItemsInListCacheManager>();
            
            
            
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });;
            
            
            services.AddSwaggerGen(options => { SetupSwagger(options, ApiName, _hostingEnv); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();
            
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", ApiName));
        }
        
        /// <summary>
        /// Setup swagger
        /// </summary>
        /// <param name="options"></param>
        /// <param name="apiName"></param>
        /// <param name="hostingEnv"></param>
        private static void SetupSwagger(SwaggerGenOptions options, string apiName, IHostingEnvironment hostingEnv)
        {
            options.SwaggerDoc("v1", new Info
            {
                Title = apiName,
                Description = $"{apiName} (ASP.NET Core 2.0)",
                Version = "2.0.BUILD_NUMBER"
            });

            options.DescribeAllEnumsAsStrings();

            var comments =
                new XPathDocument($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{hostingEnv.ApplicationName}.xml");
            options.OperationFilter<XmlCommentsOperationFilter>(comments);

            options.AddSecurityDefinition("Bearer", new ApiKeyScheme()
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = "header",
                Type = "apiKey"
            });

            options.IgnoreObsoleteActions();
            options.OperationFilter<AuthResponsesOperationFilter>();
        }
    }
}