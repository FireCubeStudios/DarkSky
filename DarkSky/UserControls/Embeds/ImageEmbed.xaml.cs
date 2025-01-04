using FishyFlip.Lexicon.App.Bsky.Embed;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.UserControls.Embeds
{
    public sealed partial class ImageEmbed : UserControl
    {
        private List<ViewImage> Images = new();
        public ImageEmbed()
        {
            this.InitializeComponent();
        }

        public void AddImages(ViewImages embed)
        {
            foreach (ViewImage imageView in embed.Images)
                Images.Add(imageView);
        }
    }
}
