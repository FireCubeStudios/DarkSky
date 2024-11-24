using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using DarkSky.ViewModels;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DarkSky.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ProfilePage : Page
	{
		private ProfileViewModel ViewModel = App.Current.Services.GetService<ProfileViewModel>();
		public ProfilePage()
		{
			this.InitializeComponent();
			ProfilePostsNavigation.SelectedItem = ProfilePostsNavigation.MenuItems[0];
		}

		public ImageSource img(string uri)
		{
			if (uri == null)
				throw new ArgumentNullException(nameof(uri));

			// Create a BitmapImage and set its UriSource to the provided Uri
			var bitmapImage = new BitmapImage(new Uri(uri));
			return bitmapImage;
		}

		private void ListView_ItemClick(object sender, ItemClickEventArgs e)
		{

			WeakReferenceMessenger.Default.Send(new TemporaryOpenPostMessage((e.ClickedItem as FeedViewPost).Post));
		}
	}
}
