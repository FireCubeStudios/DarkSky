using DarkSky.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
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
