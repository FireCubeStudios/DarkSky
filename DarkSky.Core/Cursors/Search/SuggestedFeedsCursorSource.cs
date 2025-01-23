using DarkSky.Core.Factories;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Lexicon.App.Bsky.Feed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Search
{
	public class SuggestedFeedsCursorSource : AbstractCursorSource<FeedViewModel>
	{
		protected override async Task OnGetMoreItemsAsync(int limit = 50)
		{
			GetSuggestedFeedsOutput? feeds = (await atProtoService.ATProtocolClient.GetSuggestedFeedsAsync(limit, Cursor)).AsT0;
			Cursor = feeds?.Cursor!;
			foreach (var item in feeds?.Feeds!)
				Add(new FeedViewModel(item));
		}
	}
}
