using App.Metrics.Counter;
using Interfaces;
using Interfaces.Models;
using NotifyJob.Interfaces;
using OpenTelemetry.Metrics;
using RabbitMQ.Contracts;
using RabbitMQ.Interfaces;

namespace NotifyJob.Services;

public class FlatConsumer : IRabbitMQConsumer<NewFlatContract>
{
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;
    private readonly NotifyMetricService _notifyMetricService;

    public FlatConsumer(
            IUserService userService,
            INotificationService notificationService,
            NotifyMetricService notifyMetricService)
    {
        _userService = userService;
        _notificationService = notificationService;
        _notifyMetricService = notifyMetricService;
    }

    public async Task Process(NewFlatContract message)
    {
        _notifyMetricService.NewFlatCounter.Add(message.Flats.Count, tag: new KeyValuePair<string, object?>(nameof(message.City), message.City.Name));
        var usersFlats = await GetUsersForNotify(message).ToListAsync();
        var usersGroupedFlats = usersFlats.GroupBy(x => x.user.ChatId, x => x.flat).ToArray();

        foreach (var item in usersGroupedFlats)
        {
            await _notificationService.NotifyUser(item.Key, item.ToArray());
        }
    }

    public async IAsyncEnumerable<(UserDTO user, FlatDTO flat)> GetUsersForNotify(NewFlatContract message)
    {
        var cityName = message.City.Name;

        foreach (var flatDTO in message.Flats)
        {
            var userDTOs = await _userService.GetByFilter(cityName, flatDTO.Price);

            foreach (var userDTO in userDTOs)
            {
                yield return (userDTO, flatDTO);
            }
        }
    }
}
