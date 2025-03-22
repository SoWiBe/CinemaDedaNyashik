using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Telegram.Bot;

using FirstMy.Bot;
using FirstMy.Bot.Handlers;
using FirstMy.Bot.Models;
using FirstMy.Infrastructure.Config;

var provider = DiProvider.Init();

var handler = provider.GetService<CinemaBotHandler>();

var config = new ConfigurationBuilder()
    .AddUserSecrets<BotSettings>()
    .Build();

var token = config.GetSection("BotSettings:Token").Get<string>();

try
{
    var telegramBotClient = new TelegramBotClient(token ?? string.Empty);
    var bot = new CinemaBot(telegramBotClient);
    if (handler != null) await bot.StartAsync(handler);
    Console.ReadLine();
}
catch (Exception ex)
{
    Log.Error(ex.ToString());
}
