using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.ViewModels.Temporary;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DarkSky.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [INotifyPropertyChanged]
    public sealed partial class ProfilePage : Page
    {
        [ObservableProperty]
        ProfileViewModel profile;

        public ProfilePage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ProfileViewModel)
            {
                Profile = e.Parameter as ProfileViewModel;
                await Profile.LoadDetailedAsync();
            }
        }
    }
}
