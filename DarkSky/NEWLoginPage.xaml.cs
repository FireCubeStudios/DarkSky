using Cube.UI.Brushes;
using Cube.UI.Services;
using DarkSky.Core.Classes;
using DarkSky.Core.Services;
using DarkSky.Services;
using FishyFlip.Events;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DarkSky
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class NEWLoginPage : Page
	{
		public NEWLoginPage()
		{
			this.InitializeComponent();

			WindowService.Initialize(AppTitleBar, AppTitle);
			var m = new MicaAltBrush();
			m.Kind = (int)BackdropKind.BaseAlt;
			this.Background = m;
		}

		private PasswordRevealMode PasswordReveal(bool? IsChecked) => (bool)IsChecked ? PasswordRevealMode.Hidden : PasswordRevealMode.Visible;

		private async void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			LoginBar.Visibility = Visibility.Visible;
			ATProtoService proto = new();
			proto.ATProtocolClient.SessionUpdated += ATProtocolClient_SessionUpdated;
			CredentialService credentialService = new CredentialService();
			await proto.LoginAsync(NameBox.Text, KeyBox.Password);
			if (proto.ATProtocolClient.Session is not null)
			{
				var session = proto.ATProtocolClient.Session;
				credentialService.SaveCredential(new Credential(session.Did.Handler, KeyBox.Password, proto.ATProtocolClient.Session.RefreshJwt));
				Session session2 = new Session(session.Did, null, session.Handle, null, session.AccessJwt, "");
				App.Current.Settings.Values["v1_previous_did"] = session.Did.Handler;
				App.Current.Settings.Values["v1_previous_handle"] = session.Handle.Handle;
				App.Current.Settings.Values["v1_previous_accessJWT"] = session.AccessJwt;


				App.Current.Services = ServiceContainer.Services = App.ConfigureServices();
				ATProtoService protoDI = App.Current.Services.GetService<ATProtoService>();
				protoDI.ATProtocolClient = proto.ATProtocolClient; // set the authenticated one
				protoDI.ATProtocolClient.SessionUpdated += ATProtocolClient_SessionUpdated;
				((Frame)Window.Current.Content).Navigate(typeof(MainPage));
			}
			LoginBar.Visibility = Visibility.Collapsed;
		}

		private void ATProtocolClient_SessionUpdated(object sender, SessionUpdatedEventArgs e)
		{
			App.Current.Settings.Values["v1_previous_did"] = e.Session.Session.Did.Handler;
			App.Current.Settings.Values["v1_previous_handle"] = e.Session.Session.Handle.Handle;
			App.Current.Settings.Values["v1_previous_accessJWT"] = e.Session.Session.AccessJwt;
			App.Current.Settings.Values["v1_previous_pds"] = e.InstanceUri;
		}
	}
}
