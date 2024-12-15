using DarkSky.Core.Classes;
using DarkSky.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace DarkSky.Services
{
	public class CredentialService : ICredentialService
	{
		public static PasswordVault vault = new PasswordVault();

		// ASSUMES ONLY ONE CREDENTIAL STORED
		public Credential GetCredential()
		{
			var credentialList = vault.RetrieveAll();
			var credential = credentialList[0];
			credential.RetrievePassword();
			return new Credential(credential.UserName, credential.Password, credential.Resource);
		}

		public int Count()
		{
			var credentialList = vault.RetrieveAll();
			return credentialList.Count;
		}

		public void SaveCredential(Credential credential)
		{
			// Store the credentials with token as the resource, username as the user name, and password as the password
			var WinCredential = new PasswordCredential(credential.token, credential.username, credential.password);

			// Add the credential to the vault
			vault.Add(WinCredential);
		}

		public void RemoveCredentials()
		{
			// Retrieve all credentials stored in the vault
			var credentialList = vault.RetrieveAll();

			// Delete each credential
			foreach (var credential in credentialList)
			{
				vault.Remove(credential);
			}
		}
	}
}
