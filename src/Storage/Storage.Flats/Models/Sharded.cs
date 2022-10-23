namespace Storage.Flats.Models;

public interface ISharded
{
    string GetShardKey();
}
