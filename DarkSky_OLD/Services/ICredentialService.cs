using DarkSky.Core.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Services
{
	public interface ICredentialService
	{
		public Credential GetCredential();
		public void SaveCredential(Credential credential);
		public void RemoveCredentials();
		int Count();
	}
}
