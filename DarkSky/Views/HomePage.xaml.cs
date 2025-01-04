using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels;
using DarkSky.Core.ViewModels.Temporary;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DarkSky.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private HomeFeedViewModel ViewModel = App.Current.Services.GetService<HomeFeedViewModel>();
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void CursorListView_ItemClicked(object sender, ItemClickEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(
                new SecondaryNavigationMessage(
                    new SecondaryNavigation(typeof(PostViewModel), e.ClickedItem as PostViewModel)));
        }
    }
}
