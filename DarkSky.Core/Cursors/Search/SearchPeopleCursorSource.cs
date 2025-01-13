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
	public class SearchPeopleCursorSource : AbstractCursorSource<ProfileViewModel>
	{
		private string Query;
		public SearchPeopleCursorSource(string query) : base()
		{
			Query = query;
		}

		protected override async Task OnGetMoreItemsAsync(int limit = 50)
		{
			SearchActorsOutput? people = (await atProtoService.ATProtocolClient.SearchActorsAsync(Query, limit, Cursor)).AsT0;
			Cursor = people?.Cursor!;
			foreach (var item in people?.Actors!)
				Add(ProfileFactory.Create(item));
		}
	}
}
