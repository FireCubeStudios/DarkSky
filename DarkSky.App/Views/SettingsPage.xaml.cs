using DarkSky.Core.Services;
using DarkSky.Core.Services.Interfaces;
using DarkSky.Core.ViewModels;
using DarkSky.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DarkSky.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SettingsPage : Page
	{
		public SettingsPage()
		{
			this.InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ATProtoService atProtoService = App.Window.Services.GetService<ATProtoService>();
			ICredentialService credentialService = App.Window.Services.GetService<ICredentialService>();
			credentialService.RemoveCredentials();
			((Frame)App.Window.Content).Navigate(typeof(LoginPage));
		}
    }
}
