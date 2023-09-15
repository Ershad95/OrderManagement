using Serilog;

namespace WebHost.Extensions;

public static class SerilogExtension
{
    public static void Serilog(this IServiceCollection serviceCollection, ConfigurationManager configurationManager)
    {
        Log.Logger = new LoggerConfiguration().WriteTo.MSSqlServer(
            autoCreateSqlTable: true,
            connectionString: configurationManager["Serilog:ConnectionStrings:LogDatabase"],
            tableName: configurationManager["Serilog:TableName"],
            schemaName: configurationManager["Serilog:SchemaName"]).CreateLogger();
        Log.CloseAndFlush();
        
        serviceCollection.AddSerilog(Log.Logger);
    }
}