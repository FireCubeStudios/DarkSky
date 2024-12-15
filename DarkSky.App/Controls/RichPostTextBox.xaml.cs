using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
			PostText.Width = e.NewSize.Width - 36;
		}

		public string gettext() => PostText.Text;
	}
}
