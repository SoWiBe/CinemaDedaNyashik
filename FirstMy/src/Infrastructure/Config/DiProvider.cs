using System.Reflection;
using FirstMy.Bot;
using FirstMy.Bot.Handlers;
using FirstMy.Bot.Models;
using FirstMy.Bot.Services.MediaService;
using FirstMy.Bot.Services.Users;
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
        services.AddTransient<HttpClient>();
        services.AddTransient<CinemaBotHandler>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IMediaContentService, MediaContentService>();

        return services.BuildServiceProvider();
    }
}