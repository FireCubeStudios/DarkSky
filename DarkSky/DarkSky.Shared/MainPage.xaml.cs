using CommunityToolkit.Mvvm.Messaging;
using Cube.UI.Services;
using DarkSky.Core.Messages;
using DarkSky.Services;
using DarkSky.ViewModels;
using DarkSky.Views;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DarkSky
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
	{
		private MainViewModel ViewModel = App.Current.Services.GetService<MainViewModel>();
		public MainPage()
		{
			this.InitializeComponent();
            WindowService.Initialize(AppTitleBar, AppTitle);
            AppNavigation.SelectedItem = AppNavigation.MenuItems[0];
			Bindings.Update();

			// fix weird titlebar bug
			AppTitleBar.Height = 50;
			AppTitleBar.Height = 48;

			// Register a message in some module
			WeakReferenceMessenger.Default.Register<SecondaryNavigationMessage>(this, (r, m) =>
			{
				if(m.Value == 0)
				{
					SecondaryPane.Visibility = Visibility.Collapsed;
					PaneSplitter.Visibility = Visibility.Collapsed;
					Grid.SetColumnSpan(PrimaryPane, 2);
				} else
				{
					SecondaryPane.Visibility = Visibility.Visible;
					PaneSplitter.Visibility = Visibility.Visible;
					Grid.SetColumnSpan(PrimaryPane, 1);
				}
			});

			WeakReferenceMessenger.Default.Register<TemporaryOpenPostMessage>(this, (r, m) =>
			{

				WeakReferenceMessenger.Default.Send(new SecondaryNavigationMessage(1));

				SecondaryPane.Navigate(typeof(PostPage), m.Value);
			});
		}

		// used by URL
		public ImageSource img(string uri)
		{
			if (uri == null)
				throw new ArgumentNullException(nameof(uri));

			// Create a BitmapImage and set its UriSource to the provided Uri
			var bitmapImage = new BitmapImage(new Uri(uri));
			return bitmapImage;
		}

		private void AppNavigation_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
		{
			if (sender.SelectedItem is null) return;

			if(sender.SelectedItem == AppNavigation.MenuItems[0])
			{
				PrimaryPane.Navigate(typeof(FeedPage));
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[1])
			{
				PrimaryPane.Navigate(typeof(NotificationPage));
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[2])
			{
				PrimaryPane.Navigate(typeof(ChatPage));
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[3])
			{
				PrimaryPane.Navigate(typeof(FeedsPage));
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[4])
			{
				PrimaryPane.Navigate(typeof(ListsPage));
			}
			else if (sender.SelectedItem == AppNavigation.FooterMenuItems[1])
			{
				PrimaryPane.Navigate(typeof(ProfilePage));
			}
			else if (sender.SelectedItem == AppNavigation.FooterMenuItems[2])
			{
				PrimaryPane.Navigate(typeof(SettingsPage));
			}
		}

		private void AppNavigation_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
		{
			if (args.InvokedItem is null) return;
			if (args.InvokedItem.ToString() == "New Post")
			{

				WeakReferenceMessenger.Default.Send(new SecondaryNavigationMessage(1));
				SecondaryPane.Navigate(typeof(CreatePostPage));
			}
		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			// fix weird titlebar bug
			AppTitleBar.Height = 50;
			AppTitleBar.Height = 48;
		}
	}
}
