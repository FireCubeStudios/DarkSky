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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.UserControls
{
	public sealed partial class PostControl : UserControl
	{
		public PostView Post
		{
			get => (PostView)GetValue(PostProperty);
			set
			{
				SetValue(PostProperty, value);
				if(value is not null)
					SetPost(value);
			}
		}

		public static readonly DependencyProperty PostProperty =
				   DependencyProperty.Register("Post", typeof(PostView), typeof(PostControl), null);

		public PostControl()
		{
			this.InitializeComponent();
		}

		public void SetPost(PostView post) 
		{
			if (post.Embed is null) return;
			if(post.Embed.Type == "app.bsky.embed.images#view")
			{
				var i = post.Embed as ImageViewEmbed;
				
				//	PostImage.Source = new BitmapImage(new Uri(i.Images[0].Thumb));
			}
			else if(post.Embed.Type == "app.bsky.embed.external")
			{
				var i = post.Embed as ExternalViewEmbed;
			}
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
