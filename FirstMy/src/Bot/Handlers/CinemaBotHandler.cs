using FirstMy.Bot.Models;
using FirstMy.Bot.Models.MediaContent;
using FirstMy.Bot.Models.User;
using FirstMy.Bot.Services.Core;
using FirstMy.Bot.Services.MediaService;
using FirstMy.Bot.Services.Users;
using FirstMy.Shared.Constants;
using Microsoft.AspNetCore.Http.Timeouts;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace FirstMy.Bot.Handlers;

public class CinemaBotHandler : IUpdateHandler
{
    private readonly IUsersService _usersService;
    private readonly IMediaContentService _mediaContentService;
    
    private readonly Dictionary<long, BotState> _userStates = new();

    public CinemaBotHandler(IUsersService userService, IMediaContentService mediaContentService)
    {
        _usersService = userService;
        _mediaContentService = mediaContentService;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message) return;
        if (message.Text is null ) return;
        
        var chatId = message.Chat.Id;
        
        if (!_userStates.TryGetValue(chatId, out var userState))
        {
            userState = new BotState { ChatId = chatId };
            _userStates[chatId] = userState;
        }

        switch (userState.CurrentState)
        {
            case BotStateType.WaitingForCommand:
                await WaitingCommands(botClient, message, userState);
                break;

            case BotStateType.WaitingForText:
                if (!string.IsNullOrWhiteSpace(message.Text))
                {
                    await ProcessUserInput(chatId, botClient, message.From.Id, message.Text);
                    userState.CurrentState = BotStateType.WaitingForCommand;
                }
                break;
        }

        
        
        Console.WriteLine($"Received a {message.Text} in chat {message.Chat.Id}.");
    }

    private async Task WaitingCommands(ITelegramBotClient botClient, Message message, BotState userState)
    {
        switch (message.Text!.ToLower())
        {
            case CommandConstants.Info:
                await SendSecret(botClient, message);
                break;
            case CommandConstants.Add:
                await SendMediaContent(botClient, message.Chat.Id);
                userState.CurrentState = BotStateType.WaitingForText;
                break;
            case CommandConstants.List:
                await SendListContent(botClient, message);
                break;
            default:
                await EchoMessage(botClient, message);
                break;
        }
    }

    private async Task SendListContent(ITelegramBotClient botClient, Message message)
    {
        try
        {
           var result = await _mediaContentService.GetMyList(message.From.Id);
           await botClient.SendMessage(message.Chat.Id, result.FirstOrDefault()?.Title);
        }
        catch (ApiRequestException ex)
        {
            await botClient.SendMessage(message.Chat.Id, ex.Message);
            return;
        }
    }

    private async Task SendSecret(ITelegramBotClient botClient, Message message)
    {
        var user = message.From;

        try
        {
            await _usersService.GetUserAsync(user.Id);
            await botClient.SendMessage(message.Chat.Id, "Привет! Я твой бот. Доступные команды: /info, /add, /list, /secret");
        }
        catch (ApiRequestException ex)
        {
            await _usersService.CreateUserAsync(new UserRequest
            {
                Username = user?.Username,
                FirstName = user?.FirstName,
                LastName = user.LastName,
                LastInteraction = DateTime.UtcNow,
                TelegramUserId = user.Id

            });
            await botClient.SendMessage(message.Chat.Id, "Привет! Я твой бот. Доступные команды: /info, /add, /list, /secret");
        }
    }

    private async Task SendMediaContent(ITelegramBotClient botClient, long chatId)
    {
        var mediaContentText = "Пожалуйста введите название для вашего контента: ";
        await botClient.SendMessage(chatId, mediaContentText);
    }
    
    private async Task ProcessUserInput(long chatId, ITelegramBotClient telegramBotClient, long userId, string userInput)
    {
        try
        {
            await _mediaContentService.CreateContent(new MediaContentRequest
            {
                Title = userInput,
                UserId = userId
            });
        }
        catch (ApiRequestException ex)
        {
            await telegramBotClient.SendMessage(chatId, ex.Message);
            return;
        }

        var successText = "Контент успешно добавлен!";
        await telegramBotClient.SendMessage(chatId, successText);
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