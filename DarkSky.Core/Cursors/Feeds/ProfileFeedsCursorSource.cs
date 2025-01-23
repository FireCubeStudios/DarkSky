using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Graph;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Feeds
{
	/*
	 * Get feeds created by a profile
	 * https://docs.bsky.app/docs/api/app-bsky-feed-get-actor-feeds
	 */
	public class ProfileFeedsCursorSource : AbstractCursorSource<FeedViewModel>
	{
		private ProfileViewModel Profile;
		public ProfileFeedsCursorSource(ProfileViewModel profile) : base()
		{
			Profile = profile;
		}

		protected override async Task OnGetMoreItemsAsync(int limit = 50)
		{
			GetActorFeedsOutput lists = (await atProtoService.ATProtocolClient.GetActorFeedsAsync(Profile.Handle, limit, Cursor)).AsT0;
			Cursor = lists.Cursor;

			foreach (var item in lists.Feeds)
				Add(new FeedViewModel(item));
		}
	}
}
