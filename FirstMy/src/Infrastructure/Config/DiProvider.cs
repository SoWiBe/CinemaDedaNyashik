using System.Reflection;
using FirstMy.src.Bot;
using FirstMy.src.Bot.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FirstMy.src.Infrastructure.Config;

public static class DiProvider
{
    public static ServiceProvider Init()
    {
        string currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var basePath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(currentDir)));
        
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", false, true)
            .Build();
        
        var services = new ServiceCollection();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole();
            loggingBuilder.SetMinimumLevel(LogLevel.Information);
        });

        services.Configure<BotSettings>(config.GetSection("BotSettings"));
        services.AddSingleton(config);
        services.AddLogging();
        services.AddTransient<CinemaBot>();

        return services.BuildServiceProvider();
    }
}