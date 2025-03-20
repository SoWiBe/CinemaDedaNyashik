using System.Text;
using FirstMy.Bot.Extensions;
using FirstMy.Bot.Models;
using FirstMy.Bot.Models.MediaContent;
using FirstMy.Bot.Models.User;
using FirstMy.Bot.Services.MediaService;
using FirstMy.Bot.Services.Users;
using FirstMy.Shared.Constants;
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
                await SendStart(botClient, message);
                break;
            case CommandConstants.Add:
                await SendMediaContent(botClient, message.Chat.Id);
                userState.CurrentState = BotStateType.WaitingForText;
                break;
            case CommandConstants.List:
                await SendListContent(botClient, message);
                break;
            case CommandConstants.Random:
                await RandomMediaContent(botClient, message);
                break;
            case CommandConstants.RandomAll:
                await RandomAllMediaContent(botClient, message);
                break;
            default:
                await EchoMessage(botClient, message);
                break;
        }
    }

    private async Task RandomAllMediaContent(ITelegramBotClient botClient, Message message)
    {
        try
        {
            var result = await _mediaContentService.GetRandom();
            await botClient.SendMessage(message.Chat.Id, $"Выбор пал на: {result?.Title ?? string.Empty}");
        }
        catch (ApiRequestException ex)
        {
            await botClient.SendMessage(message.Chat.Id, ex.Message);
        }
    }

    private async Task RandomMediaContent(ITelegramBotClient botClient, Message message)
    {
        try
        {
            var result = await _mediaContentService.GetMyRandom(message.From.Id);
            await botClient.SendMessage(message.Chat.Id, $"Выбор пал на: {result?.Title ?? string.Empty}");
        }
        catch (ApiRequestException ex)
        {
            await botClient.SendMessage(message.Chat.Id, ex.Message);
        }
    }

    private async Task SendListContent(ITelegramBotClient botClient, Message message)
    {
        try
        {
           var result = await _mediaContentService.GetMyList(message.From.Id);
           await botClient.SendMessage(message.Chat.Id, result.ToMessageFormat());
        }
        catch (ApiRequestException ex)
        {
            await botClient.SendMessage(message.Chat.Id, ex.Message);
        }
    }

    private async Task SendStart(ITelegramBotClient botClient, Message message)
    {
        var user = message.From;
        
        try
        {
            var userResponse = await _usersService.GetUserAsync(user.Id);
            if (userResponse is null)
            {
                await _usersService.CreateUserAsync(new UserRequest
                {
                    Username = user?.Username,
                    FirstName = user?.FirstName,
                    LastName = user.LastName,
                    LastInteraction = DateTime.UtcNow,
                    TelegramUserId = user.Id

                });
            }
            
            var commands = await botClient.GetMyCommands();
            var welcomeMessage = @$"
                Привет! Я твой персональный кинопомощник 🎬

                С помощью меня ты можешь:
                • Добавлять любимые фильмы и сериалы в свой список
                • Получать рекомендации случайного фильма по настроению
                • Вести учет просмотренного (еще не готово)
                
                Основные команды:
                {ToMessageFormat(commands)}

                Давай начнем с первого фильма / сериала ! Напиши /add и название фильма 😊";
            
            await botClient.SendMessage(message.Chat.Id, welcomeMessage);
        }
        catch (ApiRequestException ex)
        {
            await botClient.SendMessage(message.Chat.Id, ex.Message);
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
            string message;
            var response = await _mediaContentService.CreateContent(new MediaContentRequest
            {
                Title = userInput,
                UserId = userId
            });
            
            if (response)
                message = "Контент успешно добавлен!";
            else
                message = "Контент по какой-то причине не добавлен(((";
            
            await telegramBotClient.SendMessage(chatId, message);
        }
        catch (ApiRequestException ex)
        {
            await telegramBotClient.SendMessage(chatId, ex.Message);
        }
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
    
    private static string ToMessageFormat(IEnumerable<BotCommand> items)
    {
        var sb = new StringBuilder();
        
        for (var i = 0; i < items.Count(); i++)
        {
            sb.AppendFormat($"/{items.ElementAt(i).Command}; ");
        }

        return sb.ToString().TrimEnd();
    }
}