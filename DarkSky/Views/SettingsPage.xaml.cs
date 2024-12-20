using DarkSky.Core.Services;
using DarkSky.Core.Services.Interfaces;
using DarkSky.Core.ViewModels;
using DarkSky.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
			ATProtoService atProtoService = App.Current.Services.GetService<ATProtoService>();
			ICredentialService credentialService = App.Current.Services.GetService<ICredentialService>();
			credentialService.RemoveCredentials();
			((Frame)Window.Current.Content).Navigate(typeof(NEWLoginPage));
		}
    }
}
