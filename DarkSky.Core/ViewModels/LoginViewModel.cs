using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DarkSky.Core.Classes;
using DarkSky.Core.Services;
using DarkSky.Core.Services.Interfaces;
using System.Threading.Tasks;

namespace DarkSky.Core.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string userName;
        [ObservableProperty]
        private string password;

        private ATProtoService atProtoService;
        private INavigationService navigationService;
        private ICredentialService credentialService;
        public LoginViewModel(ATProtoService atProtoService, INavigationService navigationService, ICredentialService credentialService)
        {
            this.atProtoService = atProtoService;
            this.navigationService = navigationService;
            this.credentialService = credentialService;
        }

        [RelayCommand]
        private async Task login()
        {
            await atProtoService.LoginAsync(UserName, Password);
            if (atProtoService.ATProtocolClient.Session is not null)
            {
                credentialService.SaveCredential(new Credential(atProtoService.ATProtocolClient.Session.Handle.Handle, Password, atProtoService.ATProtocolClient.Session.RefreshJwt));
                navigationService.NavigateTo<MainViewModel>();
            }
        }
    }
}
