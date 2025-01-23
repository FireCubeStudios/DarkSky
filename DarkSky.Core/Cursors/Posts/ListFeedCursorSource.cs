using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Feeds
{
    /*
	 * Get posts from a List
	 * https://docs.bsky.app/docs/api/app-bsky-feed-get-list-feed
	 */
    public class ListFeedCursorSource : AbstractFeedCursorSource
    {
        private string ListUri;
        public ListFeedCursorSource(string list) : base()
        {
            ListUri = list;
        }

        protected override async Task OnGetMoreItemsAsync(int limit = 20)
        {
            GetListFeedOutput list = (await atProtoService.ATProtocolClient.Feed.GetListFeedAsync(new ATUri(ListUri), limit, Cursor)).AsT0;
            Cursor = list.Cursor;
            foreach (var item in list.Feed)
                AddPost(item);
        }
    }
}
