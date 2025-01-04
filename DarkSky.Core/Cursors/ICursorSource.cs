using System.Collections;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors
{
    /*
	 * An interface for loading ATProto items using a Cursor with refresh and incremental loading support
	 * Items ObservableCollection<T> needs to be defined seperately
	 * https://docs.bsky.app/docs/tutorials/viewing-feeds
	 */
    public interface ICursorSource : INotifyPropertyChanged
    {
        bool IsLoading { get; set; }
        IEnumerable Items { get; }

        Task RefreshAsync();
        Task GetMoreItemsAsync(int limit = 50);
        void Clear();
    }
}
