using DarkSky.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DarkSky.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
	{
		private SearchViewModel ViewModel = App.Current.Services.GetService<SearchViewModel>();
		public SearchPage()
        {
            this.InitializeComponent();
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) => ViewModel.Search();
	}
}
