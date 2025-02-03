using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Classes;
using DarkSky.Core.Cursors;
using DarkSky.Core.Cursors.Feeds;
using DarkSky.Core.Cursors.Lists;
using DarkSky.Core.ViewModels.Feeds;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Graph;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DarkSky.Core.ViewModels.Temporary
{
	public partial class FeedViewModel : ObservableObject, IFeedViewModel
	{
		[ObservableProperty]
		private string name = "";

		[ObservableProperty]
		private string description = "";

		[ObservableProperty]
		private RichText? richDescription;

		[ObservableProperty]
		private string avatar = "";

		[ObservableProperty]
		private DateTime createdAt;

		[ObservableProperty]
		private long likeCount = 0;

		[ObservableProperty]
		private ICursorSource? postsCursorSource;

		[ObservableProperty]
		private GeneratorView? feedView;

		public FeedViewModel(GeneratorView feed)
		{
			this.FeedView = feed;
			this.Name = FeedView.DisplayName ?? "";
			this.Description = FeedView.Description ?? "";
			this.Avatar = FeedView.Avatar ?? "https://raw.githubusercontent.com/FireCubeStudios/DarkSky/refs/heads/master/DarkSky/Assets/Bluesky/list.webp";
			this.CreatedAt = FeedView.IndexedAt ?? DateTime.Now;
			this.LikeCount = FeedView.LikeCount ?? 0;

			this.RichDescription = new RichText(Description, FeedView.DescriptionFacets);

			this.PostsCursorSource = new FeedCursorSource(feed.Uri.ToString());
		}
	}
}
