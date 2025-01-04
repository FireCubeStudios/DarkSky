using CommunityToolkit.Mvvm.Messaging.Messages;
using FishyFlip.Models;

namespace DarkSky.Core.Messages
{
    /*
	 * This message is sent when a new authentication is complete
	 * Most likely by the loginservice if a new user logs in
	 * This will be recieved by any listener who would want to reset data or load the new authenticated data
	 * TO-DO: Multi user support?
	 */
    public class AuthenticationSessionMessage : ValueChangedMessage<Session>
    {
        public AuthenticationSessionMessage(Session session) : base(session)
        {
        }
    }
}
