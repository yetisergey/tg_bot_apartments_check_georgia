using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotApp.Services.Handlers;

public partial class BotHandler
{
    private async Task SetFilterCity(Message message, CancellationToken cancellationToken)
    {
        var cities = await _cityService.Get(cancellationToken);

        var inlineKeyboard = new InlineKeyboardMarkup(cities
            .Select(x => InlineKeyboardButton.WithCallbackData(x.Name, Commands.SetFilterCity + x.Name.ToLower()))
            .ToArray());

        await _botClient.EditMessageReplyMarkupAsync(
            chatId: message!.Chat.Id,
            messageId: message.MessageId,
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task SetCity(Message message, string city, CancellationToken cancellationToken)
    {
        if (message.Chat is not { } chat)
        {
            return;
        }

        await _filterService.UpdateUserCity(chat.Username!, city, cancellationToken);
        await ReloadFilter(message, cancellationToken);
    }

    private async Task SetFilterPrice(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var chatId = callbackQuery.Message!.Chat.Id;
        _chatReplies.TryAdd(chatId, ReplyTypes.Price);

        var text = $@"Напишите диапазон цен в долларах:
100-200";

        await _botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: text,
            cancellationToken: cancellationToken);

        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            cancellationToken: cancellationToken);
    }

    private async Task SetPrice(Message message, string price, CancellationToken cancellationToken)
    {
        if (message.Chat is not { } chat)
        {
            return;
        }

        var regexPriceRange = new Regex(@"\d+-\d+");
        var match = regexPriceRange.Match(price);
        if (!match.Success)
        {
            await Filter(message, cancellationToken);
            return;
        }

        var matchArr = match.Value.Split("-");
        if (matchArr.Length != 2)
        {
            await Filter(message, cancellationToken);
            return;
        }

        if (!decimal.TryParse(matchArr[0], out var first) ||
            !decimal.TryParse(matchArr[1], out var second))
        {
            await Filter(message, cancellationToken);
            return;
        }

        var downPrice = Math.Min(first, second);
        var upPrice = Math.Max(first, second);

        await _filterService.UpdateUserPrice(chat.Username!, downPrice, upPrice, cancellationToken);

        _chatReplies.Remove(chat.Id, out _);
        await Filter(message, cancellationToken);
    }

    private async Task ReloadFilter(Message message, CancellationToken cancellationToken)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Город", Commands.SetFilterCity),
                InlineKeyboardButton.WithCallbackData("Цена", Commands.SetFilterPrice),
            });

        if (message.Chat is not { } chat)
        {
            return;
        }

        var filterDTO = await _filterService.GetByUserName(chat.Username!, cancellationToken);

        if (filterDTO == null)
        {
            return;
        }

        var text = $@"Апартаменты в аренду.

Город: {filterDTO.CityName}
Цена: {filterDTO.DownPrice.ToString() + " - " + filterDTO.UpPrice.ToString()}";

        await _botClient.EditMessageTextAsync(
            chatId: message!.Chat.Id,
            messageId: message.MessageId,
            replyMarkup: inlineKeyboard,
            text: text,
            cancellationToken: cancellationToken);
    }
}
