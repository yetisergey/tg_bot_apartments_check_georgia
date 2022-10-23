namespace BotApp.Services.Handlers;

/*
 * Команды бота
 * start - Приветствие
 * stop - Остановить бота
 * filter - Открыть фильтр
 */
public static class Commands
{
    public const string Start = "/start";
    public const string Stop = "/stop";
    public const string Filter = "/filter";
    public const string SetFilterCity = "/setfiltercity";
    public const string SetFilterCityTbilisi = "/setfiltercitytbilisi";
    public const string SetFilterCityBatumi = "/setfiltercitybatumi";
    public const string SetFilterPrice = "/setfilterprice";
    public const string SetPrice = "/setprice";
    public const string ReloadFilter = "/reloadfilter";
}
