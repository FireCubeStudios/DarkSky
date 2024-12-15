using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Cursors;
using DarkSky.Core.Cursors.Lists;
using DarkSky.Core.Factories;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Lexicon.App.Bsky.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

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
			ListNavigation.SelectedItem = ListNavigation.MenuItems[0];
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			List = e.Parameter as ListViewModel;
			ListNavigation.SelectedItem = ListNavigation.MenuItems[0];
		}

		private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			WeakReferenceMessenger.Default.Send(
				new SecondaryNavigationMessage(
					new SecondaryNavigation(typeof(ProfileViewModel), await ProfileFactory.CreateAsync(List.ListView.Creator))));
		}

		private void ListNavigation_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
		{
			if (sender.SelectedItem is null) return;

			if (sender.SelectedItem == ListNavigation.MenuItems[0])
			{
				PostsList.Visibility = Visibility.Visible;
				UsersLists.Visibility = Visibility.Collapsed;
			}
			else if (sender.SelectedItem == ListNavigation.MenuItems[1])
			{
				PostsList.Visibility = Visibility.Collapsed;
				UsersLists.Visibility = Visibility.Visible;
			}
		}

		private async void UsersLists_ItemClicked(object sender, ItemClickEventArgs e)
		{
			WeakReferenceMessenger.Default.Send(
					new SecondaryNavigationMessage(
						new SecondaryNavigation(typeof(ProfileViewModel), await ProfileFactory.CreateAsync((e.ClickedItem as ListItemView).Subject))));
		}

		private void PostsList_ItemClicked(object sender, ItemClickEventArgs e)
		{
			WeakReferenceMessenger.Default.Send(
				new SecondaryNavigationMessage(
					new SecondaryNavigation(typeof(PostViewModel), e.ClickedItem as PostViewModel)));
		}
	}
}
