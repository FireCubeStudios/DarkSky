using DarkSky.Core.Cursors;
using DarkSky.Core.Cursors.Feeds;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.ViewModels.Feeds
{
	/*
	 * Represents the user's personal timeline
	 * The avatar is the "Following" feed icon shown in bsky "My Feeds" page
	 */
	public record Timeline() : IFeedViewModel
	{
		public string Name { get; set; } = "Following";
		public string Avatar { get; set; } = "https://raw.githubusercontent.com/FireCubeStudios/DarkSky/refs/heads/master/DarkSky/Assets/Bluesky/timeline.png";
		public ICursorSource PostsCursorSource { get; set; } = new TimelineFeedCursorSource();
	}
}
