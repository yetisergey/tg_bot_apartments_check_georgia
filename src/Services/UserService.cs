using Interfaces;
using Interfaces.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Bot;
using Storage.Bot.Models;

namespace Services;

public class UserService : IUserService
{
    private readonly ICityService _cityService;
    private readonly UserRepository _userRepository;
    private readonly FilterRepository _filterRepository;

    public UserService(
        UserRepository userRepository,
        FilterRepository filterRepository,
        ICityService cityService)
    {
        _filterRepository = filterRepository;
        _cityService = cityService;
        _userRepository = userRepository;
    }

    public async Task<List<UserDTO>> GetByFilter(string city, decimal price)
    {
        var users = await _userRepository.GetByFilter(city, price);
        return users.Select(x => new UserDTO
        {
            Id = x.Id,
            ChatId = x.ChatId,
            Nickname = x.Nickname,
        }).ToList();
    }

    public Task<bool> Any(string userName, CancellationToken cancellationToken)
    {
        return _userRepository.AnyUserNameAsync(userName, cancellationToken);
    }

    public async Task Add(string userName, long chatId, CancellationToken cancellationToken)
    {
        var city = await _cityService.GetDefaultUserCity(cancellationToken);

        if (city == null)
        {
            return;
        }

        // TODO: unit of work
        var user = _userRepository.Add(new User
        {
            Nickname = userName,
            ChatId = chatId,
        });
        await _userRepository.SaveAsync();

        _filterRepository.Add(new Filter
        {
            User = user.Entity,
            DownPrice = 400,
            UpPrice = 600,
            CityId = city.Id,
        });
        await _userRepository.SaveAsync();
    }
}
