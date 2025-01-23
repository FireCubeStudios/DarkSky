using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Factories;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Lexicon.App.Bsky.Unspecced;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Search
{
	/*
	 * Search for feeds using the UNSPECCED API
	 * app.bsky.unspecced.getPopularFeedGenerators
	 */
	public class SearchFeedsCursorSource : AbstractCursorSource<FeedViewModel>
	{
		private string Query;
		public SearchFeedsCursorSource(string query) : base()
		{
			Query = query;
		}

		protected override async Task OnGetMoreItemsAsync(int limit = 50)
		{
			try
			{
				GetPopularFeedGeneratorsOutput ? feeds = (await atProtoService.ATProtocolClient.Unspecced.GetPopularFeedGeneratorsAsync(limit, Cursor, Query)).AsT0;
				Cursor = feeds?.Cursor!;
				foreach (var item in feeds?.Feeds!)
					Add(new FeedViewModel(item));
			}
			catch (Exception e)
			{
				WeakReferenceMessenger.Default.Send(new ErrorMessage(e));
			}
		}
	}
}
