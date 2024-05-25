using MassTransit;
using Microsoft.OpenApi.Models;
using ServiceCollector.Abstractions;
using WebHost.Middleware;

namespace WebHost;

public class ServiceRegistration : IServiceDiscovery
{
    public void AddServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Order Api"
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
        });
        
        serviceCollection.AddMassTransit(configurator =>
        {
            configurator.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });

            configurator.AddConsumer(typeof(Infrastructure.Services.EventConsumer.OrderCreated));
            configurator.AddConsumer(typeof(Infrastructure.Services.EventConsumer.OrderDeleted));
            configurator.AddConsumer(typeof(Infrastructure.Services.EventConsumer.OrderUpdated));
        });
        
        serviceCollection.AddTransient<CheckTokenMiddleware>();

    }
}