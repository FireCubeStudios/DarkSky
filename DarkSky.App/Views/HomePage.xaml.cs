using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
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
	public sealed partial class HomePage : Page
	{
		private HomeFeedViewModel ViewModel = App.Window.Services.GetService<HomeFeedViewModel>();
		public HomePage()
		{
			this.InitializeComponent();
		}

		private void CursorListView_ItemClicked(object sender, ItemClickEventArgs e)
		{
			WeakReferenceMessenger.Default.Send(
				new SecondaryNavigationMessage(
					new SecondaryNavigation(typeof(PostViewModel), e.ClickedItem as PostViewModel)));
		}
	}
}
