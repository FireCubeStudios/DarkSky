using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Factories;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DarkSky.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreatePostPage : Page
    {
        private ATProtoService ATProto = App.Current.Services.GetService<ATProtoService>();
        public CreatePostPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(new SecondaryNavigationMessage(null));
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (PostTextBox.gettext().Length > 300) return;
            Post post = new Post();
            post.CreatedAt = DateTime.Now;
            post.Langs = new List<string>();
            post.Langs.Add("en");
            post.Text = PostTextBox.gettext();
            try
            {
                var x = (await ATProto.ATProtocolClient.CreatePostAsync(post)).AsT0;
                WeakReferenceMessenger.Default.Send(
                    new SecondaryNavigationMessage(
                        new SecondaryNavigation(typeof(PostViewModel), await PostFactory.CreateAsync(x.Uri))));
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new ErrorMessage(ex));
            }
        }
    }
}
