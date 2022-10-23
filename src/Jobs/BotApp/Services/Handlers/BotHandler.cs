using System.Collections.Concurrent;
using BotApp.Interfaces;
using Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace BotApp.Services.Handlers;

public partial class BotHandler : IBotHandler
{
    private static ConcurrentDictionary<long, ReplyTypes> _chatReplies =
        new ConcurrentDictionary<long, ReplyTypes>();

    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<BotHandler> _logger;
    private readonly IUserService _userService;
    private readonly IFilterService _filterService;
    private readonly ICityService _cityService;

    public BotHandler(
        ITelegramBotClient botClient,
        ILogger<BotHandler> logger,
        IUserService userService,
        IFilterService filterService,
        ICityService cityService)
    {
        _botClient = botClient;
        _logger = logger;
        _userService = userService;
        _filterService = filterService;
        _cityService = cityService;
    }

    public Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        return update switch
        {
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            _ => Task.CompletedTask,
        };
    }

    public Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogError(errorMessage);

        return Task.CompletedTask;
    }

    private Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is not { } messageText)
        {
            return Task.CompletedTask;
        }

        if (message.Chat is not { } chat)
        {
            return Task.CompletedTask;
        }

        return messageText switch
        {
            Commands.Start => Start(message, cancellationToken),
            Commands.Filter => Filter(message, cancellationToken),
            Commands.Stop => Stop(message, cancellationToken),
            _ => _chatReplies.TryGetValue(chat.Id, out var replyTypes) ?
                 replyTypes switch
                 {
                     ReplyTypes.Price => SetPrice(message, messageText, cancellationToken),
                     _ => Task.CompletedTask,
                 } :
                 Task.CompletedTask,
        };
    }

    private Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message is not { } message)
        {
            return Task.CompletedTask;
        }

        return callbackQuery.Data switch
        {
            Commands.SetFilterCity => SetFilterCity(message, cancellationToken),
            Commands.SetFilterPrice => SetFilterPrice(callbackQuery, cancellationToken),
            Commands.ReloadFilter => ReloadFilter(message, cancellationToken),
            Commands.SetFilterCityTbilisi => SetCity(message, "Tbilisi", cancellationToken),
            Commands.SetFilterCityBatumi => SetCity(message, "Batumi", cancellationToken),
            _ => Task.CompletedTask,
        };
    }
}
