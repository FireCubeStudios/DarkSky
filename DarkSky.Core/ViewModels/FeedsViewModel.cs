using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Classes;
using DarkSky.Core.Cursors.Feeds;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DarkSky.Core.ViewModels
{
	public class FeedsViewModel : ObservableObject
	{
		public ObservableCollection<SavedFeedPreference> Feeds = new();

		private ATProtoService atProtoService;
		public FeedsViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			WeakReferenceMessenger.Default.Register<AuthenticationSessionMessage>(this, (r, m) =>
			{
				Setup(m.Value);
			});

			if (atProtoService.ATProtocolClient.Session is not null)
				Setup(atProtoService.ATProtocolClient.Session);
		}

		// todo fix
		private async void Setup(Session session)
		{
			try
			{
				Feeds.Clear();
				var x = await atProtoService.ATProtocolClient.Actor.GetPreferencesAsync();
				var preferences = x.AsT0;
				foreach (var p in preferences.Preferences)
				{
					if (p.Type == "app.bsky.actor.defs#savedFeedsPrefV2")
					{
						SavedFeedsPrefV2 feeds = p as SavedFeedsPrefV2;
						foreach (var item in feeds.Items)
							Feeds.Add(new SavedFeedPreference(item));
					}
				}
			}
			catch (Exception e)
			{
				WeakReferenceMessenger.Default.Send(new ErrorMessage(e));
			}
		}
	}
}
