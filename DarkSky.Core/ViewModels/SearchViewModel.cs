using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Classes;
using DarkSky.Core.Cursors;
using DarkSky.Core.Cursors.Search;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Graph;
using FishyFlip.Lexicon.App.Bsky.Unspecced;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace DarkSky.Core.ViewModels
{
	public partial class SearchViewModel : ObservableObject
	{
		public ObservableCollection<CursorNavigationItem> SearchResults = new();

		[ObservableProperty]
		private CursorNavigationItem? selectedSearchResult;

		[ObservableProperty]
		private string query = "";

		[ObservableProperty]
		private bool isSearching = false; // Whether to show searching UI or not

		private ATProtoService atProtoService;
		public SearchViewModel(ATProtoService atProtoService)
		{
			this.atProtoService = atProtoService;
			Home();
		}

		[RelayCommand]
		public void Search()
		{
			SelectedSearchResult = null;
			SearchResults.Clear();

			if (String.IsNullOrEmpty(Query))
			{
				Home();
				return;
			}
			IsSearching = true;

			SearchResults.Add(new CursorNavigationItem("Top", new SearchPostsCursorSource(Query, "top")));
			SearchResults.Add(new CursorNavigationItem("Latest", new SearchPostsCursorSource(Query, "latest")));
			SearchResults.Add(new CursorNavigationItem("People", new SearchPeopleCursorSource(Query)));
			SearchResults.Add(new CursorNavigationItem("Feeds (preview)", new SearchFeedsCursorSource(Query)));

			SelectedSearchResult = SearchResults[0];
		}

		[RelayCommand]
		public async void Home()
		{
			SelectedSearchResult = null;
			SearchResults.Clear();
			//SearchResults.Add(new CursorNavigationItem("Recents", new StubCursorSource()));
			//SearchResults.Add(new CursorNavigationItem("Trends (preview)", new StubCursorSource()));
			SearchResults.Add(new CursorNavigationItem("Discover Accounts", new SuggestedPeopleCursorSource()));
			SearchResults.Add(new CursorNavigationItem("Suggested Feeds", new SuggestedFeedsCursorSource()));
			SelectedSearchResult = SearchResults[0];
			IsSearching = false;
			Query = "";

			/*try
			{
				GetTrendingTopicsOutput? trends = (await atProtoService.ATProtocolClient.Unspecced.GetTrendingTopicsAsync()).AsT0;
				if(trends is not null)
				{
					foreach (var trend in trends.Topics)
					{
						Debug.WriteLine("TOPICS: " + trend.Topic);
						Debug.WriteLine("TOPICS: " + trend.Link);
					}
					foreach (var trend in trends.Suggested)
					{
						Debug.WriteLine("SUGGESTED: " + trend.Topic);
						Debug.WriteLine("SUGGESTED: " + trend.Link);
					}
				}
			}
			catch (Exception e)
			{
				WeakReferenceMessenger.Default.Send(new ErrorMessage(e));
			}*/
		}
	}
}
