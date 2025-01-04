using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using FishyFlip.Models;

namespace DarkSky.Core.Tests.Stubs
{
    /*
	 * Stub class to listen to messages such as AuthenticationSessionMessage
	 */
    public class StubMessengerListener
    {
        public Session? session = null;
        public StubMessengerListener()
        {
            WeakReferenceMessenger.Default.Register<AuthenticationSessionMessage>(this, (r, m) =>
            {
                session = m.Value;
            });
        }
    }
}
