using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DarkSky.Core.Cursors.Feeds
{
    /*
	 * An interface for loading FeedViewPost items using a Cursor with refresh and incremental loading support
	 * https://docs.bsky.app/docs/tutorials/viewing-feeds
	 */
    public interface IFeedCursorSource<T> : ICursorSource, INotifyPropertyChanged
    {
        public new ObservableCollection<T> Items { get; }
    }
}
