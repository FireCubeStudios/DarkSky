using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using DarkSky.Core.Services.Interfaces;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Models;
using System;

namespace DarkSky.Core.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private ATProtoService atProtoService;
        private IAccountService accountService;
        private INavigationService navigationService;

        [ObservableProperty]
        private ProfileViewModel currentProfile;

        public MainViewModel(IAccountService accountService, ATProtoService atProtoService, INavigationService navigationService)
        {
            this.accountService = accountService;
            this.atProtoService = atProtoService;
            this.navigationService = navigationService;
            WeakReferenceMessenger.Default.Register<AuthenticationSessionMessage>(this, (r, m) =>
            {
                setup(m.Value);
            });

            if (atProtoService.ATProtocolClient.Session is not null)
                setup(atProtoService.ATProtocolClient.Session);
        }

        private async void setup(Session session)
        {
            try
            {
                CurrentProfile = await accountService.GetCurrentProfileAsync();
                // follow firecube.bsky.social so users can get app updates TEMPORARY
                // move to OOBE and notify user about follow with a one-timeprompt instead
                /*var cube = (await atProtoService.ATProtocolClient.Actor.GetProfileAsync(ATIdentifier.Create("did:plc:y4pmm7ixx6u5gd7rtxe4rnpn"))).AsT0;
				if (cube.Viewer.Following is null)
				{
					var x = await atProtoService.ATProtocolClient.CreateFollowAsync(cube.Did);
				}*/
            }
            catch (Exception e)
            {
                WeakReferenceMessenger.Default.Send(new ErrorMessage(e));
            }
        }
    }
}
