using FishyFlip.Lexicon.App.Bsky.Notification;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors
{
    public class NotificationsCursorSource : AbstractCursorSource<Notification>
    {
        public NotificationsCursorSource() : base() { }

        protected override async Task OnGetMoreItemsAsync(int limit = 50)
        {
            ListNotificationsOutput? notifications = (await atProtoService.ATProtocolClient.Notification.ListNotificationsAsync(null, limit, false, Cursor)).AsT0;
            if(notifications is not null)
            {
				Cursor = notifications!.Cursor;
				foreach (var item in notifications!.Notifications)
				{
					Add(item);
				}
			}
        }
    }
}
