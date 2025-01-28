using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Classes
{
	/*
	 * Represents the user's saved feeds, lists and timeline from SavedFeedsPrefV2
	 * Retrieved from this API https://docs.bsky.app/docs/api/app-bsky-actor-get-preferences
	 */
	public class SavedFeedPreference
	{
		public string Name { get; set; }

	}
}
