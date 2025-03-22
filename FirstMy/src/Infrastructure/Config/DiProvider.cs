using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

using FirstMy.Bot;
using FirstMy.Bot.Handlers;
using FirstMy.Bot.Models;
using FirstMy.Bot.Services.MediaService;
using FirstMy.Bot.Services.Users;

namespace FirstMy.Infrastructure.Config;

public static class DiProvider
{
    public static ServiceProvider Init()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<BotSettings>()
            .AddUserSecrets<ApiSettings>()
            .Build();
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();
        
        var services = new ServiceCollection();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole();
            loggingBuilder.SetMinimumLevel(LogLevel.Information);
        });
        
        services.AddSingleton(config);
        services.AddLogging();
        services.AddTransient<CinemaBot>();
        services.AddTransient<BotSettings>();
        services.AddTransient<ApiSettings>();
        services.AddTransient<HttpClient>();
        services.AddTransient<CinemaBotHandler>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IMediaContentService, MediaContentService>();

        return services.BuildServiceProvider();
    }
}