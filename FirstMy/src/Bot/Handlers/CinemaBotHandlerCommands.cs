using System.Text;
using FirstMy.Bot.Extensions;
using FirstMy.Bot.Models.MediaContent;
using FirstMy.Bot.Models.User;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

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

    private async Task GetListContent(ITelegramBotClient botClient, Message message)
    {
        try
        {
           var result = await _mediaContentService.GetMyList(message.From.Id);
           await botClient.SendMessage(message.Chat.Id, result.ToMessageFormat());
        }
        catch (ApiRequestException ex)
        {
            await botClient.SendMessage(message.Chat.Id, ex.Message);
            Log.Error($"{nameof(GetListContent)} : {ex.Message}");
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
            Log.Error($"{nameof(ProcessUserInput)} : {ex.Message}");
        }
    }

    private async Task EchoMessage(ITelegramBotClient botClient, Message message)
    {
        await botClient.SendMessage(
            message.Chat.Id,
            $"Ты сказал(-а): {message.Text}"
        );
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