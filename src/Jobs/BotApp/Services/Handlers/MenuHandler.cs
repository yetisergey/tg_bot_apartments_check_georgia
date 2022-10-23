using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotApp.Services.Handlers;

public partial class BotHandler
{
    private async Task<Message> Start(Message message, CancellationToken cancellationToken)
    {
        if (message.From is not { } user)
        {
            const string notFoundedText = @$"Пользователь не найден! Укажите UserName {Commands.Start}";

            return await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: notFoundedText,
                cancellationToken: cancellationToken);
        }

        if (!(await _userService.Any(user.Username!, cancellationToken)))
        {
            await _userService.Add(user.Username!, message.Chat.Id, cancellationToken);
        }

        const string helloText = @$"Добро пожаловать!
Настройте фильтр уведомлений о новых квартирах в Грузии, с помощью команды: {Commands.Filter}";

        return await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: helloText,
            cancellationToken: cancellationToken);
    }

    private async Task Filter(Message message, CancellationToken cancellationToken)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Город", Commands.SetFilterCity),
                InlineKeyboardButton.WithCallbackData("Цена", Commands.SetFilterPrice),
            });

        if (message.From is not { } user)
        {
            return;
        }

        var filterDTO = await _filterService.GetByUserName(user.Username!, cancellationToken);

        if (filterDTO == null)
        {
            return;
        }

        var text = $@"Апартаменты в аренду.

Город: {filterDTO.CityName}
Цена: {filterDTO.DownPrice.ToString() + " - " + filterDTO.UpPrice.ToString()}";

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task<Message> Stop(Message message, CancellationToken cancellationToken)
    {
        var replyKeyboardMarkup = new ReplyKeyboardRemove();

        const string text = "Бот остановлен и не будет присылать тебе уведомления!";

        return await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
