using DarkSky.Views;
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
using Image = Windows.UI.Xaml.Controls.Image;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.UserControls.Embeds
{
	public sealed partial class ImageEmbed : UserControl
	{
		public ImageEmbed()
		{
			this.InitializeComponent();
		}

		public void AddImages(ImageViewEmbed embed)
		{
			// There will be only 4 images
			foreach(ImageView imageView in embed.Images)
			{
				Image image = new Image();
				image.Source = new BitmapImage(new Uri(imageView.Thumb));

				Images.Children.Add(image);
			}
		}
	}
}
