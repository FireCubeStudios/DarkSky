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

namespace DarkSky.Views.Temporary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [INotifyPropertyChanged]
    public sealed partial class ListPage : Page
    {
        [ObservableProperty]
        private ListViewModel list;

        public ListPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            List = e.Parameter as ListViewModel;
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(
                new SecondaryNavigationMessage(
                    new SecondaryNavigation(typeof(ProfileViewModel), ProfileFactory.Create(List.ListView.Creator))));
        }

        private void ListNavigation_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (sender.SelectedItem is null || List is null) return;

           /* if (sender.SelectedItem == ListNavigation.MenuItems[0])
            {
                PostsUsersList.CursorSource = List.ListFeedCursorSource;
                PostsUsersList.ItemsSource = List.ListFeedCursorSource.Items;
            }
            else if (sender.SelectedItem == ListNavigation.MenuItems[1])
            {
                PostsUsersList.CursorSource = List.ListUsersCursorSource;
                PostsUsersList.ItemsSource = List.ListUsersCursorSource.Items;
            }*/
        }
    }
}
