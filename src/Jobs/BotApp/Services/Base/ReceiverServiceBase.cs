using BotApp.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace BotApp.Services.Base;

public abstract class ReceiverServiceBase<TUpdateHandler> : IReceiverService
     where TUpdateHandler : IBotHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IBotHandler _updateHandlers;
    private readonly ILogger<ReceiverServiceBase<TUpdateHandler>> _logger;

    public ReceiverServiceBase(
        ITelegramBotClient botClient,
        IBotHandler updateHandler,
        ILogger<ReceiverServiceBase<TUpdateHandler>> logger)
    {
        _botClient = botClient;
        _updateHandlers = updateHandler;
        _logger = logger;
    }

    public async Task ReceiveAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = Array.Empty<UpdateType>(),
            ThrowPendingUpdates = true,
        };

        var me = await _botClient.GetMeAsync(stoppingToken);
        _logger.LogInformation("Start receiving updates for {BotName}", me.Username ?? "My Awesome Bot");

        await _botClient.ReceiveAsync(
            updateHandler: (client, update, ct) => _updateHandlers.HandleUpdateAsync(update, ct),
            pollingErrorHandler: (client, ex, ct) => _updateHandlers.HandleErrorAsync(ex, ct),
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken);
    }
}
