using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels.Temporary;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.UserControls
{
    public sealed partial class PostControl : UserControl
    {
        public PostViewModel Post
        {
            get => (PostViewModel)GetValue(PostProperty);
            set => SetValue(PostProperty, value);
        }

        public static readonly DependencyProperty PostProperty =
                   DependencyProperty.Register("Post", typeof(PostViewModel), typeof(PostControl), new PropertyMetadata(null, OnPostChanged));

        // Adding the PropertyChanged method fixes virtualisation duplication bug
        private static void OnPostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PostControl p)
                p.Bindings.Update();
        }

        public PostControl()
        {
            this.InitializeComponent();
        }

        private void ShowFullThreadButton_Click(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(
                    new SecondaryNavigationMessage(
                        new SecondaryNavigation(typeof(PostViewModel), Post)));
        }
    }
}
