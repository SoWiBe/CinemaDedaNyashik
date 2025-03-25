using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Telegram.Bot;

using FirstMy.Bot;
using FirstMy.Bot.Handlers;
using FirstMy.Bot.Models;
using FirstMy.Infrastructure.Config;
using Microsoft.Extensions.Configuration;

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
    await Task.Delay(-1);
}
catch (Exception ex)
{
    Log.Error(ex.ToString());
}
