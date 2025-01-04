using DarkSky.Core.Services.Interfaces;

namespace DarkSky.Core.Tests.Stubs
{
    internal class StubKeyService : IKeyService
    {
        private Dictionary<string, object> Storage = new Dictionary<string, object>();

        public T Get<T>(string key) => (T)Storage.GetValueOrDefault(key) ?? throw new Exception("Not found");

        public void Set<T>(string key, T value) => Storage.Add(key, value);
    }
}