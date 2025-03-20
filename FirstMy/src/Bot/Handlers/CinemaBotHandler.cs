using FirstMy.Bot.Models;
using FirstMy.Bot.Services.MediaService;
using FirstMy.Bot.Services.Users;
using FirstMy.Shared.Constants;
using FirstMy.Shared.Constants.Error;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace FirstMy.Bot.Handlers;

public partial class CinemaBotHandler : IUpdateHandler
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
        if (update.Message is not { } message)
        {
            Log.Error(BotErrorConstants.FailedToDeconstruct);
            return;
        }

        if (message.Text is null)
        {
            Log.Error(BotErrorConstants.MissingText);
            return;
        }

        if (message.From is null)
        {
            Log.Error(BotErrorConstants.MissingSender);
            return;
        }
        
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
                if (await IsTextAreNotCommand(botClient, message.Text) && IsTextAreNotEmpty(message.Text))
                {
                    await ProcessUserInput(chatId, botClient, message.From.Id, message.Text);
                    userState.CurrentState = BotStateType.WaitingForCommand;
                }
                break;
        }
        
        Log.Information($"Received a {message.Text} in chat {message.Chat.Id}.");
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

        Log.Error(errorMessage);
        return Task.CompletedTask;
    }
    
    private async Task<bool> IsTextAreNotCommand(ITelegramBotClient botClient, string userInput)
    {
        var commands = await botClient.GetMyCommands();
        return commands.All(cmd => cmd.Command != userInput);
    }
    
    private bool IsTextAreNotEmpty(string userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput)) return false;
        return true;
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
                await GetListContent(botClient, message);
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
}