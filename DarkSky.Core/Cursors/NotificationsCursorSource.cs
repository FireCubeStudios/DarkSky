using FishyFlip.Lexicon.App.Bsky.Notification;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors
{
    public class NotificationsCursorSource : AbstractCursorSource<Notification>
    {
        public NotificationsCursorSource() : base() { }

        protected override async Task OnGetMoreItemsAsync(int limit = 50)
        {
            ListNotificationsOutput notifications = (await atProtoService.ATProtocolClient.Notification.ListNotificationsAsync(limit, false, Cursor)).AsT0;
            Cursor = notifications.Cursor;
            foreach (var item in notifications.Notifications)
            {
                Add(item);
            }
        }
    }
}
