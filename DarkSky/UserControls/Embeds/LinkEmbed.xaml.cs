using CommunityToolkit.Mvvm.ComponentModel;
using FishyFlip.Lexicon.App.Bsky.Embed;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

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
