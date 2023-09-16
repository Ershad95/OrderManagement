using System.Net;
using System.Text.Json;
using Application.Features.Order.AddOrder;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WebHost;
using WebHost.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AddOrderCommand).Assembly));

builder.Services.CustomServiceRegistration();
builder.Services.Serilog(builder.Configuration);
builder.Services.MassTransit();
builder.Services.Swagger();
builder.Services.Jwt(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<CheckTokenMiddleware>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConfigurationOptions = new ConfigurationOptions()
    {
        EndPoints = { builder.Configuration["Redis"] },
        AllowAdmin = true
    };
});
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.ApiExceptionHandling();
app.UseMiddleware<CheckTokenMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.UseRouting();
app.UseAuthorization();

app.Run();