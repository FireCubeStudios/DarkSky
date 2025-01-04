using Windows.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DarkSky.Controls
{
    public sealed partial class RichPostTextBox : UserControl
    {
        public RichPostTextBox()
        {
            this.InitializeComponent();
        }

        // used in progress ring to show number of characters left
        public double LimitValue(string text) => 300 - text.Length;
        public string LimitValueStr(string text) => LimitValue(text).ToString();

        private void UserControl_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 36)
                PostText.Width = e.NewSize.Width - 36;
        }

        public string gettext() => PostText.Text;
    }
}
