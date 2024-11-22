namespace SidecarService.Services;

public interface IRedisService
{
    
    public Task<T> StringSetAsync<T>(string key, T value, TimeSpan? expiry = null);
    public Task<T> StringGetAsync<T>(string key);
    public Task<T> StringExitAsync<T>(string key);
    public Task<bool> RemoveAsync(string key);
    
    public Task HashSetAsync<T>(string key, T value);
    public Task<T> HashGetAsync<T>(string key);
    public Task<bool> HashExitAsync(string key);
}