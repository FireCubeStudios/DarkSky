using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Services;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.ViewModels
{
	public partial class HomeFeedViewModel : ObservableObject
	{
		[ObservableProperty]
		private FeedViewPost[] timelineFeed;

		private ATProtoService atProtoService;
		public HomeFeedViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			Setup();
		}

		private async void Setup()
		{
			TimelineFeed = (await atProtoService.ATProtocolClient.Feed.GetTimelineAsync()).AsT0.Feed;
		}
	}
}
