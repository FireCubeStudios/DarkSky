using DarkSky.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Tests.Stubs
{
	internal class StubKeyService : IKeyService
	{
		private Dictionary<string, object> Storage = new Dictionary<string, object>();

		public T Get<T>(string key) => (T)Storage.GetValueOrDefault(key) ?? throw new Exception("Not found");

		public void Set<T>(string key, T value) => Storage.Add(key, value);
	}
}