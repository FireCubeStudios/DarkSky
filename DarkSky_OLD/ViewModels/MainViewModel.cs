using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Services;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.ViewModels
{
	public partial class MainViewModel : ObservableObject
	{
		private ATProtoService atProtoService;
		private INavigationService navigationService;

		[ObservableProperty]
		private FeedProfile currentProfile;

		public MainViewModel(ATProtoService atProtoService, INavigationService navigationService)
		{
			this.atProtoService = atProtoService;
			this.navigationService = navigationService;
			if (atProtoService.Session is null)
				navigationService.NavigateTo<LoginViewModel>();
			else
				setup();
		}

		private async void setup()
		{
			var profiles = await atProtoService.ATProtocolClient.Actor.GetProfileAsync(atProtoService.Session.Did);
			CurrentProfile = profiles.AsT0;
			try
			{
				// follow firecube.bsky.social so users can get app updates TEMPORARY
				var cube = (await atProtoService.ATProtocolClient.Actor.GetProfileAsync(ATIdentifier.Create("did:plc:y4pmm7ixx6u5gd7rtxe4rnpn"))).AsT0;
				if (cube.Viewer.Following is null)
				{
					var x = await atProtoService.ATProtocolClient.Repo.CreateFollowAsync(atProtoService.Session.Did);
				}
			}
			catch (Exception e)
			{

			}
		}
	}
}
