using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Services.Interfaces
{
	public interface IKeyService
	{
		public object Get(string key);

		public void Set(string key, object value);
	}
}
