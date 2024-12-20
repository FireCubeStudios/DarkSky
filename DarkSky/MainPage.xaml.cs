using CommunityToolkit.Mvvm.Messaging;
using Cube.UI.Services;
using DarkSky.Core.Messages;
using DarkSky.Services;
using DarkSky.Core.ViewModels;
using DarkSky.Views;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using DarkSky.Core.ViewModels.Temporary;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using Cube.UI.Brushes;
using DarkSky.Views.Temporary;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DarkSky
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[INotifyPropertyChanged]
    public sealed partial class MainPage : Page
	{
		[ObservableProperty]
		private MainViewModel viewModel;
		private readonly Dictionary<Type, Type> viewModelsToViews = new();

		/*
		 * Used to determine if primary pane is collapsed or expanded
		 * If colapsed then the columnspan is 1, otherwise if expanded it is 3
		 */
		[ObservableProperty]
		private bool primaryPaneCollapsed = true;
		private bool CollapseByDefault = true;
		private int BoolToColumnSpan(bool value) => value ? 1 : 3; // Bound by primary pane

		public MainPage()
		{
			this.InitializeComponent();
			PrimaryPaneCollapsed = CollapseByDefault;
			AppNavigation.SelectedItem = AppNavigation.MenuItems[0];
			Bindings.Update();
			/*
			 * Navigate the secondary page
			 * The SecondaryNavigationMessage contains a "ViewModel" Type and a "payload" object
			 * The ViewModel type is mapped to a Page that is navigated to
			 */
			viewModelsToViews[typeof(PostViewModel)] = typeof(PostPage);
			viewModelsToViews[typeof(ProfileViewModel)] = typeof(ProfilePage);
			viewModelsToViews[typeof(ListViewModel)] = typeof(ListPage);
			WeakReferenceMessenger.Default.Register<SecondaryNavigationMessage>(this, (r, m) =>
			{
				if (m.Value is not null)
				{
					if (m.Value.payload is ProfileViewModel || m.Value.payload is ListViewModel)
					{
						PrimaryPane.Navigate(viewModelsToViews[m.Value.ViewModel], m.Value.payload);
						AppNavigation.SelectedItem = null;
					}
					else
					{
						SecondaryPaneContainer.Visibility = Visibility.Visible;
						PrimaryPaneCollapsed = true;
						SecondaryPane.Navigate(viewModelsToViews[m.Value.ViewModel], m.Value.payload);
					}
				}
				else //new SecondaryNavigation(null) go to null{
				{
					SecondaryPaneContainer.Visibility = Visibility.Collapsed;
					PrimaryPaneCollapsed = CollapseByDefault; // expand if user chooses too
				}
			});

			WeakReferenceMessenger.Default.Register<ErrorMessage>(this, async (r, m) =>
			{
				await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async() =>
				{
					Errorbar.IsOpen = true;
					Errorbar.Title = m.Value.Message;
					Errorbar.Content = m.Value.StackTrace;
					await Task.Delay(5000);
					Errorbar.IsOpen = false;
				});
			});
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModel = App.Current.Services.GetService<MainViewModel>();
			WindowService.Initialize(AppTitleBar, AppTitle);

			// fix weird titlebar bug
			AppTitleBar.Height = 50;
			AppTitleBar.Height = 48;
		}

		// used by URL
		public ImageSource img(string uri)
		{
			if (uri == null)
				throw new ArgumentNullException(nameof(uri));

			// Create a BitmapImage and set its UriSource to the provided Uri
			var bitmapImage = new BitmapImage(new Uri(uri));
			return bitmapImage;
		}

		private void AppNavigation_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
		{
			if (args.IsSettingsSelected == true)
			{
				PrimaryPane.Navigate(typeof(SettingsPage), args.RecommendedNavigationTransitionInfo);
				return;
			}
			if (sender.SelectedItem is null) return;

			if(sender.SelectedItem == AppNavigation.MenuItems[0])
			{
				PrimaryPane.Navigate(typeof(HomePage), args.RecommendedNavigationTransitionInfo);
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[1])
			{
				PrimaryPane.Navigate(typeof(NotificationPage), args.RecommendedNavigationTransitionInfo);
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[2])
			{
				PrimaryPane.Navigate(typeof(SearchPage), args.RecommendedNavigationTransitionInfo);
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[3])
			{
				PrimaryPane.Navigate(typeof(ChatPage), args.RecommendedNavigationTransitionInfo);
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[4])
			{
				PrimaryPane.Navigate(typeof(FeedsPage), args.RecommendedNavigationTransitionInfo);
			}
			else if (sender.SelectedItem == AppNavigation.MenuItems[5])
			{
				PrimaryPane.Navigate(typeof(ListsPage), args.RecommendedNavigationTransitionInfo);
			}
			else if (sender.SelectedItem == AppNavigation.FooterMenuItems[1])
			{
				PrimaryPane.Navigate(typeof(ProfilePage), ViewModel.CurrentProfile);
			}
		}

		private void AppNavigation_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
		{
			if (args.InvokedItem is null) return;
			if (args.InvokedItem.ToString() == "New Post")
			{
				SecondaryPaneContainer.Visibility = Visibility.Visible;
				PrimaryPaneCollapsed = true;
				SecondaryPane.Navigate(typeof(CreatePostPage));
			}
		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			// fix weird titlebar bug
			AppTitleBar.Height = 50;
			AppTitleBar.Height = 48;

			try
			{
				if (e.NewSize.Width > 500)
				{
					VisualStateManager.GoToState(this, "WideState", true);
					PrimaryPaneCollapsed = CollapseByDefault;
				}
				else
				{
					VisualStateManager.GoToState(this, "NarrowState", true);
					PrimaryPaneCollapsed = false;
				}
			}
			catch { }
		}

		private void PrimaryPaneToggle_Click(object sender, RoutedEventArgs e)
		{
			CollapseByDefault = (bool)PrimaryPaneToggle.IsChecked;
		}
	}
}
