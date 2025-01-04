using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Factories;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels.Temporary;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DarkSky.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [INotifyPropertyChanged]
    public sealed partial class PostPage : Page
    {
        [ObservableProperty]
        PostViewModel post;
        public PostPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Post = e.Parameter as PostViewModel;
            //await Post.GetThreadAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(new SecondaryNavigationMessage(null));
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(
                new SecondaryNavigationMessage(
                    new SecondaryNavigation(typeof(ProfileViewModel), ProfileFactory.Create(Post.InternalPost.Author))));
        }
    }
}
