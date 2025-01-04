using DarkSky.Core.Classes;

namespace DarkSky.Core.Services.Interfaces
{
    public interface ICredentialService
    {
        public Credential GetCredential();
        public void SaveCredential(Credential credential);
        public void RemoveCredentials();
        int Count();
    }
}
