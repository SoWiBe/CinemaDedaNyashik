using FirstMy.Bot;
using FirstMy.Bot.Handlers;
using FirstMy.Infrastructure.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var provider = DiProvider.Init();
var logger = provider.GetService<Logger<Program>>();
var bot = provider.GetService<CinemaBot>();
var handler = provider.GetService<CinemaBotHandler>();

try
{
    await bot!.StartAsync(handler);
    Console.ReadLine();
}
catch (Exception exception)
{
    logger!.LogError($"Error after start: {exception}");
}
