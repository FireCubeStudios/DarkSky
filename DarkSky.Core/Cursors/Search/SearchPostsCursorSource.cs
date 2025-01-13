using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Factories;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Graph;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Search
{
	public partial class SearchPostsCursorSource : AbstractCursorSource<PostViewModel>
	{
		private string Query;
		private string Sort;
		public SearchPostsCursorSource(string query, string sort) : base()
		{
			Query = query;
			Sort = sort;
		}

		protected override async Task OnGetMoreItemsAsync(int limit = 50)
		{
			SearchPostsOutput? posts = (await atProtoService.ATProtocolClient.SearchPostsAsync(Query, Sort, limit:limit, cursor:Cursor)).AsT0;
			Cursor = posts?.Cursor!;
			foreach (var item in posts?.Posts!)
				Add(new PostViewModel(item));
		}
	}
}
