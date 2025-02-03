using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels.Feeds;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Factories
{
	public class FeedFactory
	{
		public async static Task<IFeedViewModel> CreateAsync(SavedFeed feed)
		{
			var proto = ServiceContainer.Services.GetService<ATProtoService>()!.ATProtocolClient;
			if (feed.TypeValue == "feed")
				return new FeedViewModel(((await proto.Feed.GetFeedGeneratorAsync(new ATUri(feed.Value))).AsT0).View);
			else if (feed.TypeValue == "list")
				return new ListViewModel(((await proto.Graph.GetListAsync(new ATUri(feed.Value))).AsT0).List);
			else if (feed.TypeValue == "timeline")
				return new Timeline();
			else
				throw new Exception($"FeedFactory SavedFeed CreateAsync could not find feed for TypeValue: {feed.TypeValue}");
		}
	}
}
