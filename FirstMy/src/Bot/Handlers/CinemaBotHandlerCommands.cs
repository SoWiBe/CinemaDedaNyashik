using Serilog;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using FirstMy.Bot.Models;
using FirstMy.Bot.Models.MediaContent;
using FirstMy.Bot.Models.User;
using FirstMy.Shared.Constants;
using FirstMy.Shared.Constants.Emoji;
using FirstMy.Shared.Constants.Event;
using FirstMy.Shared.Constants.User;
using FirstMy.Shared.Enums;

namespace FirstMy.Bot.Handlers;

public partial class CinemaBotHandler
{
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
            Log.Error($"{nameof(RandomAllMediaContent)} : {ex.Message}");
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
            Log.Error($"{nameof(RandomMediaContent)} : {ex.Message}");
        }
    }

    private async Task GetListContent(ITelegramBotClient botClient, long userId, long chatId)
    {
        try
        {
           var mediaContents = await _mediaContentService.GetMyList(userId);
           if (mediaContents is null)
           {
               await botClient.SendMessage(chatId, ListConstants.EmptyList);
               return;;
           }

           var keyboard = new List<List<InlineKeyboardButton>>();
           foreach (var content in mediaContents)
           {
               SetCallbacksForItem(content.Id, ContentActionType.Update, out var updateKey);
               SetCallbacksForItem(content.Id, ContentActionType.Delete, out var deleteKey);
               
               keyboard.Add(new List<InlineKeyboardButton>
               {
                   new()
                   {
                       Text = $"{ContentEmojiConstants.MediaContentEmoji} {content.Title ?? string.Empty}",
                       CallbackData = $"{content.Id}"
                   },
                   new()
                   {
                       Text = content.Status == MediaContentStatus.Success ? 
                                               ActionsEmojiConstants.SuccessEmoji : 
                                               ActionsEmojiConstants.WaitingEmoji,
                       CallbackData = updateKey
                   },
                   new()
                   {
                       Text = ActionsEmojiConstants.DeleteEmoji,
                       CallbackData = deleteKey
                   }
               });
           }

           var replyMarkup = new InlineKeyboardMarkup(keyboard);
           
           await botClient.SendMessage(
               chatId: chatId,
               text: UserConstants.YourList,
               replyMarkup: replyMarkup);
        }
        catch (ApiRequestException ex)
        {
            await botClient.SendMessage(chatId, ex.Message);
            Log.Error($"{nameof(GetListContent)} : {ex.Message}");
        }
    }

    private void SetCallbacksForItem(long itemId, ContentActionType actionType, out string key)
    {
        key = string.Empty;
        
        switch (actionType)
        {
            case ContentActionType.Update:
                key = $"{EventConstants.Update}{itemId}";
                if (_mediaContentUpdateCallbacks.TryGetValue(key, out _)) return;
                _mediaContentUpdateCallbacks[key] = itemId;
                break;
            case ContentActionType.Delete:
                key = $"{EventConstants.Delete}{itemId}";
                if (_mediaContentDeleteCallbacks.TryGetValue(key, out _)) return;
                _mediaContentDeleteCallbacks[key] = itemId;
                break;
            default:
                Log.Error($"{nameof(SetCallbacksForItem)} : {itemId} : {key}");
                break;
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
            Log.Error($"{nameof(SendStart)} : {ex.Message}");
        }
    }

    private async Task SendMediaContent(ITelegramBotClient botClient, long chatId)
    {
        var mediaContentText = "Пожалуйста введите название для вашего контента: ";
        await botClient.SendMessage(chatId, mediaContentText);
    }
    
    private async Task ProcessCreateContentUserInput(long chatId, ITelegramBotClient telegramBotClient, long userId, string userInput)
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
            Log.Error($"{nameof(ProcessCreateContentUserInput)} : {ex.Message}");
        }
    }

    private async Task EchoMessage(ITelegramBotClient botClient, Message message)
    {
        await botClient.SendMessage(
            message.Chat.Id,
            $"Ты сказал(-а): {message.Text}"
        );
    }

    private async Task ClearMediaContent(ITelegramBotClient botClient, Message message, BotState userState)
    {
        try
        {
            var response = "Ваш контент не удален по какой-то причине, попробуйте позже.";

            var result = await _mediaContentService.ClearMediaContent(message.From.Id);
            if (result)
            {
                response = "Ваш контент успешно очищен!";
                userState.CurrentState = BotStateType.WaitingForText;
            }
            
            await botClient.SendMessage(message.Chat.Id, response);
        }
        catch (ApiRequestException ex)
        {
            await botClient.SendMessage(message.Chat.Id, ex.Message);
            Log.Error($"{nameof(GetListContent)} : {ex.Message}");
        }
    }

    private async Task RemoveAtMediaContent(ITelegramBotClient botClient, Message message)
    {
        var elems = new List<string> { "Content", "Test 2", "datatatat 3" };

        var keyboard = new List<List<InlineKeyboardButton>>();
        for (var index = 0; index < elems.Count; index++)
        {
            var elem = elems[index];
            keyboard.Add(new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = elem,
                    CallbackData = $"{index}"
                },
                new()
                {
                    Text = ActionsEmojiConstants.DeleteEmoji,
                    CallbackData = $"delete_{index}"
                },
            });
        }

        var replyMarkup = new InlineKeyboardMarkup(keyboard);
            
        await botClient.SendMessage(
            chatId: message.Chat.Id,
            text: "Выберите действие:",
            replyMarkup: replyMarkup);
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