using CommunityToolkit.Mvvm.ComponentModel;
using DarkSky.Core.Classes;
using DarkSky.Core.Cursors;
using DarkSky.Core.Cursors.Feeds;
using DarkSky.Core.Cursors.Lists;
using DarkSky.Core.Factories;
using DarkSky.Core.Services;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Lexicon.App.Bsky.Richtext;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DarkSky.Core.ViewModels.Temporary
{
	public partial class ProfileViewModel : ObservableObject, INotifyPropertyChanged
	{
		[ObservableProperty]
		private ATHandle? handle;

		[ObservableProperty]
		private ATDid? did;

		#region Built in properties

		[ObservableProperty]
		private string? displayName;

		[ObservableProperty]
		private string? description;

		[ObservableProperty]
		private string? avatar;

		[ObservableProperty]
		private string? banner;

		[ObservableProperty]
		private long followsCount;

		[ObservableProperty]
		private long followersCount;

		[ObservableProperty]
		private long postsCount;

		[ObservableProperty]
		private DateTime createdAt;

		#endregion

		[ObservableProperty]
		private PostViewModel? pinnedPost;

		[ObservableProperty]
		private ProfileViewDetailed profile;

		/* 
		 * Descriptions support Facets however the API only returns a plain string
		 * We need to manually parse the facets from the Description string
		 * We do this in the constructor with FishyFlip Facet.Parse method
		 */
		[ObservableProperty]
		private RichText richDescription;

		#region Feeds
		public ObservableCollection<CursorNavigationItem> ProfileNavigationItems = new();

		[ObservableProperty]
		private CursorNavigationItem selectedProfileNavigationItem;
		#endregion

		/*
		 * Used in scenarios where full profile details are not needed like in search, lists
		 * Used alongside LoadDetailedAsync to get more details
		 */
		private bool IsDetailed = false;
		public ProfileViewModel(ATDid did) => this.did = did;

		public ProfileViewModel(ProfileViewDetailed profileView) => Setup(profileView);

		private async void Setup(ProfileViewDetailed profileView)
		{
			IsDetailed = true; // Profile is now detailed
			this.Profile = profileView;
			this.Handle = profileView.Handle;
			this.Did = profileView.Did;
			this.DisplayName = profileView.DisplayName ?? profileView.Handle.ToString();
			this.Avatar = profileView.Avatar;
			this.Banner = profileView.Banner;
			this.Description = profileView.Description ?? "";
			this.FollowersCount = profileView.FollowersCount ?? 0;
			this.FollowsCount = profileView.FollowsCount ?? 0;
			this.PostsCount = profileView.PostsCount ?? 0;
			this.CreatedAt = profileView.CreatedAt ?? profileView.IndexedAt ?? DateTime.Now;
			ProfileNavigationItems.Add(new CursorNavigationItem("Posts", new ProfileFeedCursorSource(this, "posts_no_replies")));
			SelectedProfileNavigationItem = ProfileNavigationItems[0];
			ProfileNavigationItems.Add(new CursorNavigationItem("Replies", new ProfileFeedCursorSource(this, "posts_with_replies")));
			ProfileNavigationItems.Add(new CursorNavigationItem("Media", new ProfileFeedCursorSource(this, "posts_with_media")));
			if(Profile.Associated.Lists > 0)
				ProfileNavigationItems.Add(new CursorNavigationItem("Lists", new ProfileListsCursorSource(this)));

			// Profile Descriptions supports Facets but the API does not return them
			// To fix this we manually parse the Facets using FishyFlip Facet.Parse method
			RichDescription = new RichText(Description, Facet.Parse(Description, new ProfileViewBasic[0]).ToList());

			if (Profile.PinnedPost is not null)
			{
				this.PinnedPost = await PostFactory.CreateAsync(Profile.PinnedPost.Uri);
				this.PinnedPost.IsPinned = true;
			}
		}

		/*
		 * Used to load the ProfileDetailedView from the DID
		 * Sometimes profiles will only have few details usually used in lists, search etc.
		 * When a profile is opened this method will usually be called to get all details
		 * Used in the ProfilePage when a profile is loaded there
		 */
		public async Task LoadDetailedAsync()
		{
			if (IsDetailed) return; // Don't load if already detailed
			var proto = ServiceContainer.Services.GetService<ATProtoService>().ATProtocolClient;
			var profile = await proto.Actor.GetProfileAsync(Did);
			Setup(profile.AsT0);
		}
	}
}
