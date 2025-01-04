using DarkSky.Core.Factories;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Graph;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Lists
{
    /*
	 * Get users added to a list
	 * Using this API https://docs.bsky.app/docs/api/app-bsky-graph-get-list
	 */
    public class ListUsersCursorSource : AbstractCursorSource<ProfileViewModel>
    {
        private ListViewModel List;
        public ListUsersCursorSource(ListViewModel list) : base()
        {
            this.List = list;
        }

        protected override async Task OnGetMoreItemsAsync(int limit = 50)
        {
            GetListOutput list = (await atProtoService.ATProtocolClient.Graph.GetListAsync(List.ListView.Uri, limit, Cursor)).AsT0;
            Cursor = list.Cursor;
            foreach (var item in list.Items)
            {
                if (item.Subject is not null)
                    Add(ProfileFactory.Create(item.Subject));
            }
        }
    }
}
