using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Graph;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Lists
{
    /*
	 * Get the lists of a profile
	 * https://docs.bsky.app/docs/api/app-bsky-graph-get-lists
	 */
    public class ProfileListsCursorSource : AbstractCursorSource<ListViewModel>
    {
        private ProfileViewModel Profile;
        public ProfileListsCursorSource(ProfileViewModel profile) : base()
        {
            Profile = profile;
        }

        protected override async Task OnGetMoreItemsAsync(int limit = 50)
        {
            GetListsOutput lists = (await atProtoService.ATProtocolClient.Graph.GetListsAsync(Profile.Handle, limit, Cursor)).AsT0;
            Cursor = lists.Cursor;

            foreach (var item in lists.Lists)
                Add(new ListViewModel(item));
        }
    }
}
