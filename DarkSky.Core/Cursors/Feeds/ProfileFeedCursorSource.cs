using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Feeds
{
    /*
	 * Load posts from a Profile
	 */
    public class ProfileFeedCursorSource : AbstractFeedCursorSource
    {
        private string Filter;
        private ProfileViewModel Profile;
        public ProfileFeedCursorSource(ProfileViewModel profile, string filter) : base()
        {
            Filter = filter;
            Profile = profile;
        }

        protected override async Task OnGetMoreItemsAsync(int limit = 50)
        {
            GetAuthorFeedOutput timeLine = (await atProtoService.ATProtocolClient.Feed.GetAuthorFeedAsync(Profile.Handle, limit, Cursor, Filter, false)).AsT0;
            Cursor = timeLine.Cursor;

            // Add pinned post first if it exists
            if (((ObservableCollection<PostViewModel>)Items).Count == 0 && Profile.PinnedPost is not null)
                Add(Profile.PinnedPost);

            foreach (var item in timeLine.Feed)
            {
                // Ignore pinned posts if there are any
                if (Profile.PinnedPost is null || item.Post.Cid != Profile.PinnedPost.Cid)
                    AddPost(item);
            }
        }
    }
}
