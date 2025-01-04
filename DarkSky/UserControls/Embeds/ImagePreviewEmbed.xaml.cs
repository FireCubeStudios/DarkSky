using FishyFlip.Lexicon.App.Bsky.Embed;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Image = Windows.UI.Xaml.Controls.Image;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.UserControls.Embeds
{
    public sealed partial class ImagePreviewEmbed : UserControl
    {
        private List<BitmapImage> Images = new();
        public ImagePreviewEmbed()
        {
            this.InitializeComponent();
        }

        public void AddImages(ViewImages embed)
        {
            // There will be only 4 images
            foreach (ViewImage imageView in embed.Images)
            {
                Images.Add(new BitmapImage(new Uri(imageView.Thumb)));
            }

            ImagesGrid.Children.Clear();
            ImagesGrid.RowDefinitions.Clear();
            ImagesGrid.ColumnDefinitions.Clear();

            switch (embed.Images.Count())
            {
                case 1:
                    DisplaySingleImage();
                    break;
                case 2:
                    DisplayTwoImages();
                    break;
                case 3:
                    DisplayThreeImages();
                    break;
                case 4:
                    DisplayFourImages();
                    break;
                default:
                    DisplaySingleImage();
                    break;
            }
        }

        private void DisplaySingleImage()
        {
            AddImageToGrid(Images[0], 0, 0, 1, 1, false);
        }

        private void DisplayTwoImages()
        {
            ImagesGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ImagesGrid.ColumnDefinitions.Add(new ColumnDefinition());

            AddImageToGrid(Images[0], 0, 0, 1, 1);
            AddImageToGrid(Images[1], 0, 1, 1, 1);
            ImagesGrid.MaxHeight = 200;
        }

        private void DisplayThreeImages()
        {
            ImagesGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ImagesGrid.ColumnDefinitions.Add(new ColumnDefinition());

            ImagesGrid.RowDefinitions.Add(new RowDefinition());
            ImagesGrid.RowDefinitions.Add(new RowDefinition());

            AddImageToGrid(Images[0], 0, 0, 2, 1);
            AddImageToGrid(Images[1], 0, 1, 1, 1);
            AddImageToGrid(Images[2], 1, 1, 1, 1);
            ImagesGrid.MaxHeight = 400;
        }

        private void DisplayFourImages()
        {
            ImagesGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ImagesGrid.ColumnDefinitions.Add(new ColumnDefinition());

            ImagesGrid.RowDefinitions.Add(new RowDefinition());
            ImagesGrid.RowDefinitions.Add(new RowDefinition());

            AddImageToGrid(Images[0], 0, 0, 1, 1);
            AddImageToGrid(Images[1], 0, 1, 1, 1);
            AddImageToGrid(Images[2], 1, 0, 1, 1);
            AddImageToGrid(Images[3], 1, 1, 1, 1);
            ImagesGrid.MaxHeight = 400;
        }

        private void AddImageToGrid(BitmapImage bitmap, int row, int column, int rowSpan, int columnSpan, bool uniformFill = true)
        {
            var image = new Image
            {
                Source = bitmap,
                Stretch = uniformFill ? Stretch.UniformToFill : Stretch.Fill
            };

            Grid.SetRow(image, row);
            Grid.SetColumn(image, column);
            Grid.SetRowSpan(image, rowSpan);
            Grid.SetColumnSpan(image, columnSpan);

            ImagesGrid.Children.Add(image);
        }
    }
}
