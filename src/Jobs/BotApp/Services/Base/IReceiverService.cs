namespace BotApp.Services.Base;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken);
}
