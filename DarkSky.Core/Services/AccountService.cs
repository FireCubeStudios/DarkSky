using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using DarkSky.Core.Services.Interfaces;
using DarkSky.Core.ViewModels.Temporary;
using System.Threading.Tasks;

namespace DarkSky.Core.Services
{
    /*
	 * Service to get and manage current authenticated user
	 */
    public class AccountService : IAccountService
    {
        private ProfileViewModel currentProfile;

        private ATProtoService atProtoService;
        public AccountService(ATProtoService atProtoService)
        {
            this.atProtoService = atProtoService;
            WeakReferenceMessenger.Default.Register<AuthenticationSessionMessage>(this, (r, m) =>
            {
                currentProfile = null;
            });
        }

        public async Task<ProfileViewModel> GetCurrentProfileAsync()
        {
            if (currentProfile is null)
            {
                var profile = await atProtoService.ATProtocolClient.Actor.GetProfileAsync(atProtoService.ATProtocolClient.Session.Did);
                currentProfile = new ProfileViewModel(profile.AsT0);
            }
            return currentProfile;
        }
    }
}
