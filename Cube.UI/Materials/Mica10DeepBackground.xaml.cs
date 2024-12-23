using Cube.UI.Brushes;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Cube.UI.Materials
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Mica10DeepBackground : Page
	{
		public Mica10DeepBackground()
		{
			this.InitializeComponent();
			string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
			ulong version = ulong.Parse(deviceFamilyVersion);
			ulong build = (version & 0x00000000FFFF0000L) >> 16;

			if (build >= 22000)
			{
				MicaDeepLayer.Visibility = Visibility.Visible;
				TenMicaLayer.Visibility = Visibility.Collapsed;

				var m = new MicaAltBrush();
				m.Kind = (int)BackdropKind.BaseAlt;
				m.Theme = Application.Current.RequestedTheme == ApplicationTheme.Light ? ElementTheme.Light : ElementTheme.Default;
				this.Background = m;
			}
			else
			{
				MicaDeepLayer.Visibility = Visibility.Collapsed;
				TenMicaLayer.Visibility = Visibility.Visible;
			}

			var Listener = new ThemeListener();
			Listener.ThemeChanged += Listener_ThemeChanged;
		}

		private void Listener_ThemeChanged(ThemeListener sender)
		{
			string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
			ulong version = ulong.Parse(deviceFamilyVersion);
			ulong build = (version & 0x00000000FFFF0000L) >> 16;

			if (build >= 22000)
			{
				MicaDeepLayer.Visibility = Visibility.Visible;
				TenMicaLayer.Visibility = Visibility.Collapsed;

				var m = new MicaAltBrush();
				m.Kind = (int)BackdropKind.BaseAlt;
				m.Theme = Application.Current.RequestedTheme == ApplicationTheme.Light ? ElementTheme.Light : ElementTheme.Default;
				this.Background = m;
			}
		}
	}
}
