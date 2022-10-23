using Interfaces.Models;
using NotifyJob.Interfaces;
using Polly;
using Polly.Retry;
using Telegram.Bot;

namespace NotifyJob.Services;

public class NotificationService : INotificationService
{
    private static readonly AsyncRetryPolicy _retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(new[]
            {
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(20),
                    TimeSpan.FromSeconds(40),
            });

    private readonly ILogger<INotificationService> _logger;
    private readonly ITelegramBotClient _botClient;

    public NotificationService(
        ITelegramBotClient botClient,
        ILogger<INotificationService> logger)
    {
        _logger = logger;
        _botClient = botClient;
    }

    public async Task NotifyUser(long chatId, FlatDTO[] flats)
    {
        _logger.LogInformation($"Notify chat {chatId} flats length: {flats.Length}");
        var message = string.Join(Environment.NewLine, flats.Select(x => $"{x.Price}${Environment.NewLine}https://www.myhome.ge/ru/pr/{x.ProductId}"));
        await _retryPolicy.ExecuteAndCaptureAsync(async () => await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: message,
            disableWebPagePreview: false));
    }
}
