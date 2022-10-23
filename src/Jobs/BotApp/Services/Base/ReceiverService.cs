using BotApp.Interfaces;
using BotApp.Services.Handlers;
using Telegram.Bot;

namespace BotApp.Services.Base;

public class ReceiverService : ReceiverServiceBase<BotHandler>
{
    public ReceiverService(
        ITelegramBotClient botClient,
        IBotHandler updateHandler,
        ILogger<ReceiverServiceBase<BotHandler>> logger)
        : base(botClient, updateHandler, logger)
    {
    }
}
