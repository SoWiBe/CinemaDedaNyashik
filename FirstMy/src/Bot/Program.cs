using FirstMy.Infrastructure.Config;
using FirstMy.src.Bot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var provider = DiProvider.Init();
var logger = provider.GetService<Logger<Program>>();
var bot = provider.GetService<CinemaBot>();

try
{
    await bot!.StartAsync();
    Console.ReadLine();
}
catch (Exception exception)
{
    logger!.LogError($"Error after start: {exception}");
}
