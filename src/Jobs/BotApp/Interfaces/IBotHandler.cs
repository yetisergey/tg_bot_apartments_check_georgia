using Telegram.Bot.Types;

namespace BotApp.Interfaces;

public interface IBotHandler
{
    Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);

    Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken);
}
