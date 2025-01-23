using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Feeds
{
    public class FeedCursorSource : AbstractFeedCursorSource
    {
        private string FeedUri;
        public FeedCursorSource(string feed) : base()
        {
            FeedUri = feed;
        }


        protected override async Task OnGetMoreItemsAsync(int limit = 20)
        {
            GetFeedOutput timeLine = (await atProtoService.ATProtocolClient.Feed.GetFeedAsync(new ATUri(FeedUri), limit, Cursor)).AsT0;
            Cursor = timeLine.Cursor;
            foreach (var item in timeLine.Feed)
                AddPost(item);
        }
    }
}
