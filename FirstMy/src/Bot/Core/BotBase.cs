using Serilog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

using FirstMy.Shared.Constants;

namespace FirstMy.Bot.Core;

public abstract class BotBase
{
    protected BotBase(TelegramBotClient botClient)
    {
        BotClient = botClient;
    }

    private TelegramBotClient BotClient { get; }
    

    public async Task StartAsync(IUpdateHandler updateHandler)
    {
        try
        {
            Log.Information(StatusConstants.InitBot);

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            
            var cts = new CancellationTokenSource();
            
            BotClient.StartReceiving(
                receiverOptions: receiverOptions,
                updateHandler: updateHandler,
                cancellationToken: cts.Token);

            var me = await BotClient.GetMe(cancellationToken: cts.Token);

            Log.Information($"Бот запущен и готов к работе {me.Username}");
        }
        catch (Exception ex)
        {
            Log.Error($"Ошибка при запуске бота {ex}");
            throw;
        }
    }
}