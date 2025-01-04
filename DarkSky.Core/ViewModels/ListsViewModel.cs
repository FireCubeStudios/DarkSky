using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Cursors.Lists;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using DarkSky.Core.Services.Interfaces;
using FishyFlip.Models;
using System;

namespace DarkSky.Core.ViewModels
{
    public partial class ListsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ProfileListsCursorSource listsSource;

        private ATProtoService atProtoService;
        private IAccountService accountService;
        public ListsViewModel(ATProtoService atProtoService, IAccountService accountService)
        {
            this.atProtoService = atProtoService;
            this.accountService = accountService;
            WeakReferenceMessenger.Default.Register<AuthenticationSessionMessage>(this, (r, m) =>
            {
                Setup(m.Value);
            });

            if (atProtoService.ATProtocolClient.Session is not null)
                Setup(atProtoService.ATProtocolClient.Session);
        }

        private async void Setup(Session session)
        {
            try
            {
                var currentProfile = await accountService.GetCurrentProfileAsync();
                ListsSource = new ProfileListsCursorSource(currentProfile);
            }
            catch (Exception e)
            {
                WeakReferenceMessenger.Default.Send(new ErrorMessage(e));
            }
        }
    }
}
