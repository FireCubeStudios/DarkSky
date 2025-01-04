using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels.Temporary;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.UserControls.Embeds
{
    [INotifyPropertyChanged]
    public sealed partial class QuoteEmbed : UserControl
    {
        [ObservableProperty]
        private PostViewModel post;
        public QuoteEmbed()
        {
            this.InitializeComponent();
        }

        public void setpost(PostViewModel post)
        {
            Post = post;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Post is null) return;
            /*
			 * Since we ae opening the post in a new pageit is no longer "quoted" 
			 * So we can reset it's quote depth index to 0
			 * This way it can render new quotes if it quotes any other posts
			 */
            Post.QuoteDepthIndex = 0;
            WeakReferenceMessenger.Default.Send(new SecondaryNavigationMessage(
                new SecondaryNavigation(typeof(PostViewModel), Post)));
        }
    }
}
