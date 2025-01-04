using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Services;
using DarkSky.Core.Services.Interfaces;

namespace DarkSky.Core.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private ATProtoService atProtoService;
        private INavigationService navigationService;
        private ICredentialService credentialService;
        public SettingsViewModel(ATProtoService atProtoService, INavigationService navigationService, ICredentialService credentialService)
        {
            this.atProtoService = atProtoService;
            this.navigationService = navigationService;
            this.credentialService = credentialService;
        }
    }
}
