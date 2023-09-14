using System.Text;
using Application.Features.AddOrder;
using Application.Repository;
using Application.Services;
using Infrastructure;
using Infrastructure.EventConsumer;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using WebHost;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.CustomServiceRegistration();
builder.Services.AddControllers();
builder.Services.AddSingleton<Serilog.ILogger>(x => new LoggerConfiguration().WriteTo.MSSqlServer(
    builder.Configuration["Serilog:ConnectionString"],
    builder.Configuration["Serilog:TableName"],
    autoCreateSqlTable: true).CreateLogger());

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AddOrderCommand).Assembly));

builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });
    
    configurator.AddConsumer(typeof(OrderCreated));
    configurator.AddConsumer(typeof(OrderDeleted));
    configurator.AddConsumer(typeof(OrderUpdated));
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        RequireExpirationTime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
    };
});


builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();