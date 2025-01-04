using CommunityToolkit.Mvvm.Messaging;
using Cube.UI.Services;
using DarkSky.Core.Messages;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DarkSky
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        //	private LoginViewModel ViewModel = App.Current.Services.GetService<LoginViewModel>();
        public LoginPage()
        {
            this.InitializeComponent();
            WindowService.Initialize(AppTitleBar, AppTitle);

            WeakReferenceMessenger.Default.Register<ErrorMessage>(this, async (r, m) =>
            {
                Errorbar.IsOpen = true;
                Errorbar.Title = m.Value.Message;
                Errorbar.Content = m.Value.StackTrace;
                await Task.Delay(5000);
                Errorbar.IsOpen = false;
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            /*	LoginBar.Visibility = Visibility.Visible;
                ATProtoService proto = new();
                CredentialService credentialService = new CredentialService();
                await proto.LoginAsync(UsernameBox.Text, PasswordBox.Password);
                if (proto.ATProtocolClient.Session is not null)
                {
                    credentialService.SaveCredential(new Credential(proto.ATProtocolClient.Session.Handle.Handle, PasswordBox.Password, proto.ATProtocolClient.Session.RefreshJwt));

                    App.Current.Services = ServiceContainer.Services = App.ConfigureServices();

                    ATProtoService protoDI = App.Current.Services.GetService<ATProtoService>();
                    protoDI.ATProtocolClient = proto.ATProtocolClient;
                    ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
                }
                LoginBar.Visibility = Visibility.Collapsed;*/
        }
    }
}
