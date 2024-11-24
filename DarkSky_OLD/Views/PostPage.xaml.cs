using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using FishyFlip.Models;
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
	[INotifyPropertyChanged]
	public sealed partial class PostPage : Page
	{
		[ObservableProperty]
		PostView post;
		public PostPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			Post = e.Parameter as PostView;
			SetPost(post);
		}
			public void SetPost(PostView post)
		{
			if (post.Embed is null) return;
			if (post.Embed.Type == "app.bsky.embed.images#view")
			{
				var i = post.Embed as ImageViewEmbed;

				PostImage.Source = new BitmapImage(new Uri(i.Images[0].Fullsize));
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			WeakReferenceMessenger.Default.Send(new SecondaryNavigationMessage(0));
		}

		private ImageSource img(string uri)
		{
			if (uri == null)
				throw new ArgumentNullException(nameof(uri));

			// Create a BitmapImage and set its UriSource to the provided Uri
			var bitmapImage = new BitmapImage(new Uri(uri));
			return bitmapImage;
		}
	}
}
