namespace DarkSky.Core.Services.Interfaces
{
    public interface IKeyService
    {
        public T Get<T>(string key);

        public void Set<T>(string key, T value);
    }
}
