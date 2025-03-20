using System.Text;
using FirstMy.Bot.Models.MediaContent;

namespace FirstMy.Bot.Extensions;

public static class MessageExtensions
{
    public static string ToMessageFormat(this IEnumerable<MediaContentResponse> items)
    {
        var sb = new StringBuilder();
        
        for (var i = 0; i < items.Count(); i++)
        {
            sb.AppendFormat($"{i + 1}: {items.ElementAt(i).Title}\n");
        }

        return sb.ToString().TrimEnd();
    }
}