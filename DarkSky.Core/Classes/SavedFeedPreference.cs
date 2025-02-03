using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Cursors;
using DarkSky.Core.Cursors.Feeds;
using DarkSky.Core.Factories;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels.Feeds;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Classes
{
	/*
	 * Represents the user's saved feeds, lists and timeline from SavedFeedsPrefV2
	 * Retrieved from this API https://docs.bsky.app/docs/api/app-bsky-actor-get-preferences
	 */
	public partial class SavedFeedPreference : ObservableObject
	{
		[ObservableProperty]
		private IFeedViewModel feed;
		public bool IsPinned { get; set; }
		public bool IsTimeline { get; set; } = false; // If true then the item should not be clickable in Feed Page

		public SavedFeedPreference(SavedFeed feed)
		{
			IsPinned = feed.Pinned;
			IsTimeline = feed.TypeValue == "timeline";
			setup(feed);
		}

		private async void setup(SavedFeed feed)
		{
			try 
			{
				Feed = await FeedFactory.CreateAsync(feed);
			}
            catch (Exception e)
            {
                WeakReferenceMessenger.Default.Send(new ErrorMessage(e));
            }
		}
	}
}
