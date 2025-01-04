using DarkSky.Core.Services.Interfaces;
using Windows.Storage;

namespace DarkSky.Services
{
    /*
	 * Store individual key-value pair app settings
	 */
    public class KeyService : IKeyService
    {
        private static ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;

        public T Get<T>(string key) => (T)Settings.Values[key];

        public void Set<T>(string key, T value) => Settings.Values[key] = value;
    }
}
