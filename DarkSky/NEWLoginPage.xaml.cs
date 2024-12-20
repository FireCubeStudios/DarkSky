using CommunityToolkit.Mvvm.ComponentModel;
using Cube.UI.Brushes;
using Cube.UI.Services;
using DarkSky.Core.Classes;
using DarkSky.Core.Exceptions;
using DarkSky.Core.Services;
using DarkSky.Core.Services.Interfaces;
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
	[INotifyPropertyChanged]
	public sealed partial class NEWLoginPage : Page
	{
		public NEWLoginPage()
		{
			this.InitializeComponent();

			WindowService.Initialize(AppTitleBar, AppTitle);
		}

		private PasswordRevealMode PasswordReveal(bool? IsChecked) => (bool)IsChecked ? PasswordRevealMode.Hidden : PasswordRevealMode.Visible;


		/*
		 * Used to show 2FA UI and also let us know 2FA process is enabled
		 */
		[ObservableProperty]
		private bool is2FA = false;
		private async void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			LoginBar.Visibility = Visibility.Visible;

			try
			{
				ATProtoService proto = ServiceContainer.Services.GetService<ATProtoService>();
				ICredentialService credentialService = ServiceContainer.Services.GetService<ICredentialService>();

				try
				{
					if (Is2FA) await proto.AuthCodeLoginAsync(NameBox.Text, KeyBox.Password, CodeBox.Text, PDSBox.Text);
					else await proto.LoginAsync(NameBox.Text, KeyBox.Password, PDSBox.Text);
				}
				catch (ATErrorException ex)
				{
					// 2FA missing, switch to 2FA page
					if (ex.Error?.Detail?.Error == "AuthFactorTokenRequired")
					{
						Is2FA = true;
						return;
					}
					else
						StatusTitle.Text = ex.Message;
				}

				if (proto.ATProtocolClient.Session is not null)
				{
					Is2FA = false;
					var session = proto.ATProtocolClient.Session;
					credentialService.SaveCredential(new Credential(session.Did.Handler, KeyBox.Password, proto.ATProtocolClient.Session.RefreshJwt));
					((Frame)Window.Current.Content).Navigate(typeof(MainPage));
				}
			}
			catch (Exception ex)
			{
				StatusTitle.Text = ex.Message;
				StatusText.Text = ex.StackTrace;
			}

			LoginBar.Visibility = Visibility.Collapsed;
		}
	}
}
