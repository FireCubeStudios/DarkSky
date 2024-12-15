using DarkSky.Core.ViewModels;
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
	public sealed partial class NotificationPage : Page
	{
		private NotificationsViewModel ViewModel = App.Window.Services.GetService<NotificationsViewModel>();
		public NotificationPage()
		{
			this.InitializeComponent();
			_ = ViewModel.NotificationsSource.GetMoreItemsAsync();
		}

		public static string format(string reason)
		{
			if (reason == "like") return "liked one of your posts";
			else if (reason == "repost") return "reposted one of your posts";
			else if (reason == "follow") return "followed you";
			else if (reason == "mention") return "mentioned you";
			else if (reason == "reply") return "replied to you";
			else if (reason == "quote") return "quoted one of your posts";
			else if (reason == "starterpack-joined") return "joined your starterpack";
			return reason;
		}

		private async void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
			=> await ViewModel.NotificationsSource.RefreshAsync();

		#region Incremental loading code
		/*
		 * When Listview reaches bottom call FeedSource to generate more posts
		 */
		private ScrollViewer _scrollViewer;
		private void ListView_Loaded(object sender, RoutedEventArgs e)
		{
			_scrollViewer = GetScrollViewer(FeedList);

			if (_scrollViewer != null)
			{
				_scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
			}
		}

		private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			if (_scrollViewer.VerticalOffset >= _scrollViewer.ScrollableHeight - 10) // Threshold to trigger loading
				await ViewModel.NotificationsSource.GetMoreItemsAsync();
		}

		private ScrollViewer GetScrollViewer(DependencyObject element)
		{
			if (element is ScrollViewer)
				return (ScrollViewer)element;

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
			{
				var child = VisualTreeHelper.GetChild(element, i);
				var result = GetScrollViewer(child);
				if (result != null)
					return result;
			}
			return null;
		}

		#endregion
	}
}
