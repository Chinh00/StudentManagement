
using System.Text.Json;
using StackExchange.Redis;

namespace SidecarService.Services;

public class RedisService : IRedisService
{
    private readonly Lazy<ConnectionMultiplexer> _lazy;

    public RedisService(IConfiguration configuration)
    {
        _lazy = new Lazy<ConnectionMultiplexer>(() =>
            ConnectionMultiplexer.Connect(
                $"localhost:6379"));

    }

    private ConnectionMultiplexer ConnectionMultiplexer => _lazy.Value;

    private readonly SemaphoreSlim _connectionLock = new(1, 1);


    private IDatabase Database
    {
        get
        {
            _connectionLock.Wait();

            try
            {
                return ConnectionMultiplexer.GetDatabase();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connectionLock.Release();
            }
        }
    }


    public async Task<T> StringSetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var data = JsonSerializer.Serialize(value);

        await Database.StringSetAsync(key, data, expiry);
        return value;
    }

    public async Task<T> StringGetAsync<T>(string key)
    {
        return JsonSerializer.Deserialize<T>(Database.StringGet(key));
    }

    public Task<T> StringExitAsync<T>(string key)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveAsync(string key)
    {
        return (await Database.StringGetDeleteAsync(key)).HasValue;
    }

    public async Task HashSetAsync<T>(string key, T value)
    {
    }

    public Task<T> HashGetAsync<T>(string key)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HashExitAsync(string key)
    {
        throw new NotImplementedException();
    }
}