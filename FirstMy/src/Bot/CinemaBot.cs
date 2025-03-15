using FirstMy.src.Bot.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FirstMy.src.Bot;

public class CinemaBot : BotBase
{
    private readonly ILogger<CinemaBot> _logger;

    public CinemaBot(IConfiguration configuration, ILogger<CinemaBot> logger) : base(configuration, logger)
    {
        _logger = logger;
    }
}