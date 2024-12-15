using CommunityToolkit.Mvvm.ComponentModel;
using FishyFlip.Lexicon.App.Bsky.Embed;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.UserControls.Embeds
{
	[INotifyPropertyChanged]
	public sealed partial class LinkEmbed : UserControl
	{
		[ObservableProperty]
		private ViewExternalExternal embed;
		public LinkEmbed()
		{
			this.InitializeComponent();
		}

		public void AddLink(ViewExternal EmbedView)
		{
			this.Embed = EmbedView.External;
			Title.Text = String.IsNullOrEmpty(Embed.Title) ? Embed.Uri : Embed.Title;

			if (!String.IsNullOrEmpty(Embed.Thumb))
			{
				EmbedImage.Visibility = Visibility.Visible;
				EmbedImage.Source = new BitmapImage(new Uri(Embed.Thumb));
			}
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			await Launcher.LaunchUriAsync(new Uri(embed.Uri));
		}
    }
}
