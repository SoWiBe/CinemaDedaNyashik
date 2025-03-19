<<<<<<< HEAD
﻿using FirstMy.Shared.Constants;
using Microsoft.Extensions.Logging;
=======
﻿using FirstMy.src.Shared.Constants;
>>>>>>> main
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FirstMy.Bot.Handlers;

public class UpdateHandler : IUpdateHandler
{

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message) return;
        if (message.Text is null ) return;
        if (message is not { Type: MessageType.Text } ) return;

        switch (message.Text.ToLower())
        {
            case CommandConstants.Info:
                await SendInfoMessage(botClient, message.Chat.Id);
                break;
            case CommandConstants.Add:
                await SendMediaContent(botClient, message.Chat.Id);
                break;
            case CommandConstants.Secret:
                await SendSecret(botClient, message.Chat.Id);
                break;
            default:
                await EchoMessage(botClient, message);
                break;
        }
        
        Console.WriteLine($"Received a {message.Text} in chat {message.Chat.Id}.");
    }

    private async Task SendSecret(ITelegramBotClient botClient, long chatId)
    {
        var text = "💖 💖 💖 НАААЯ, ВЫЗДОРАВЛИВАААЙ!!! 💖 💖 💖";
        await botClient.SendMessage(chatId, text);
    }

    private async Task SendMediaContent(ITelegramBotClient botClient, long chatId)
    {
        var welcomeText = "ДОБАВИЛ КОНТЕНТ!";
        await botClient.SendMessage(chatId, welcomeText);
    }

    private async Task SendInfoMessage(ITelegramBotClient botClient, long chatId)
    {
        var welcomeText = "Привет! Я твой бот. Доступные команды: /info, /add, /list, /secret";
        //TODO: get user request, if is not exist - create
        await botClient.SendMessage(chatId, welcomeText);
    }
    
    private async Task EchoMessage(ITelegramBotClient botClient, Message message)
    {
        await botClient.SendMessage(
            message.Chat.Id,
            $"Ты сказал(-а): {message.Text}"
        );
    }
    
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}