using CommunityToolkit.Mvvm.Messaging.Messages;
using System;

namespace DarkSky.Core.Messages
{
    /*
	 * New navigation system for the secondary panel
	 * The secondary panel is now persistent, it can be navigated to by using a Messenger to specify when to navigate
	 * T is a ViewModel and object is a payload
	 * The MainPage will listen for this, it will also have a dictionary to map ViewModels to Pages
	 * Current defined ones are:
	 * PostViewModel -> PostPage
	 * 
	 * This usage should close secondary panel page
	 * new SecondaryNavigation(null)
	 */
    public record SecondaryNavigation(Type ViewModel, object? payload = null);
    public class SecondaryNavigationMessage : ValueChangedMessage<SecondaryNavigation>
    {
        public SecondaryNavigationMessage(SecondaryNavigation Navigation) : base(Navigation)
        {
        }
    }
}
