using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DarkSky.Core.Classes;
using DarkSky.Core.Cursors.Search;
using FishyFlip.Lexicon.App.Bsky.Feed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

			SelectedSearchResult = SearchResults[0];
		}

		[RelayCommand]
		public void Home()
		{
			SelectedSearchResult = null;
			SearchResults.Clear();
			IsSearching = false;
			Query = "";
		}
	}
}
