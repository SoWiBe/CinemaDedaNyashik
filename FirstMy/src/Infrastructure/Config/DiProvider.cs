using System.Reflection;
using FirstMy.Bot.Models;
using FirstMy.Bot.Services.Users;
using FirstMy.src.Bot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FirstMy.Infrastructure.Config;

public static class DiProvider
{
    public static ServiceProvider Init()
    {
        var currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
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
        services.Configure<ApiSettings>(config.GetSection("Api"));
        services.AddSingleton(config);
        services.AddLogging();
        services.AddTransient<CinemaBot>();
        services.AddScoped<IUsersService, UsersService>();

        return services.BuildServiceProvider();
    }
}