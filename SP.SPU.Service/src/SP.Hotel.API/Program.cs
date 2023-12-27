using Autofac.Extensions.DependencyInjection;
using Coravel;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using NLog.Web;
using SP.SPU.API.Application.IntegrationEvents.EventHandling;
using SP.SPU.API.Application.IntegrationEvents.PublishEvents;
using SP.SPU.API.Application.Queries;
using SP.SPU.Domian.AggregatesModel.HotelAggregate;
using SP.SPU.Infrastructure;
using SP.SPU.Infrastructure.ElasticSearch;
using SP.SPU.Infrastructure.Repositories;
using SP.SPU.Infrastructure.SeedWork;
using SPCorePackage.Extensions;
using SPCorePackage.Kafka.Interface;
using System.Reflection;

namespace SP.SPU.API;

public class Program
{
    private static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        try
        {
            await RegisterServices(builder);
            var app = builder.Build();
            await ConfigurePipeline(app);
            app.Run();
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
        }

    }
    private static async Task RegisterServices(WebApplicationBuilder builder)
    {
        IConfiguration configuration = builder.Configuration;
        builder.Host
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .UseNLog();

        var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        var profileAssemblies = assemblyNames.Select(assenbly => Assembly.Load(assenbly)).ToList();
        profileAssemblies.Add(Assembly.GetExecutingAssembly());
        builder.Services.AddAutoMapper(profileAssemblies);

        builder.Services.AddScheduler();

        builder.Services.AddHttpContextAccessor();

        var mySqlConnectionStr = configuration.GetSection("MySqlConnection:Main").Value;
        builder.Services.AddDbContext<DataContext>(options =>
           options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr))
        );

        builder.Services.AddScoped<IUnitOfWorkDapper>((context) =>
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            return new UnitOfWork(new MySqlConnection(configuration.GetSection("MySqlConnection:Main").Value));
        });

        builder.Services.AddControllers();
        builder.Services.AddScoped<IHotelRepository, HotelRepository>();
        builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();

        builder.Services.AddMemoryCache();

        // kafka 
        var kafkaConnectString = configuration.GetSection("KafkaConnectString").Value;
        builder.Services.AddKafkaBus(kafkaConnectString);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = Assembly.GetEntryAssembly().GetName().Name, Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
        });

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }

    private static async Task ConfigurePipeline(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PXBox.Installment.API v1"));
        }

        if (app.Environment.IsEnvironment("sit"))
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/7031/swagger/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer>
                        {
                            new() {Url = "https://api-sit.box.pxec.com.tw"}
                        };
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/7031/swagger/v1/swagger.json", "PXBox.Installment.API v1");
                c.RoutePrefix = "7031/swagger";
            });
        }
        /*
        app.UseMiddleware<RequestIdMiddleware>();
        app.UseMiddleware<APILogMiddleware>();*/

        app.UseCors(builder => builder
            .SetIsOriginAllowed(host => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();
        });

        var eventBus = app.Services.GetRequiredService<IEventBus>();

        eventBus.Subscribe<CreateHotelEvent, CreateHotelEventHandler>(CreateHotelEvent.EventName);

        app.Services.UseScheduler(scheduler =>
        {
        });
    }
}
