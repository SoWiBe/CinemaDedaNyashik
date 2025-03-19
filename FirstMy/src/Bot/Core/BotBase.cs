using FirstMy.Bot.Handlers;
using FirstMy.Bot.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace FirstMy.src.Bot.Core;

public abstract class BotBase
{
    protected ILogger Logger;
    private TelegramBotClient BotClient { get; set; }
    private BotSettings BotSettings { get; set; }

    protected BotBase(IConfiguration configuration, ILogger logger)
    {
        Logger = logger;
        BotSettings = configuration.GetSection("BotSettings").Get<BotSettings>()!;
        BotClient = new TelegramBotClient(BotSettings!.Token);
    }
    
    public async Task StartAsync()
    {
        try
        {
            Logger.LogInformation("Инициализация бота ...");

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            
            var cts = new CancellationTokenSource();
            
            BotClient.StartReceiving<UpdateHandler>(receiverOptions: receiverOptions,
                                                    cancellationToken: cts.Token);

            var me = await BotClient.GetMe( cancellationToken: cts.Token);

            Logger.LogInformation($"Бот запущен и готов к работе {me.Username}");
        }
        catch (Exception ex)
        {
            Logger.LogError($"Ошибка при запуске бота {ex}");
            throw;
        }
    }
}