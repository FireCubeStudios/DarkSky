using DarkSky.Core.Cursors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.ViewModels.Feeds
{
	/*
	 * Interface for feeds, lists, timeline with common properties
	 * Used in Homepage and Feedpage
	 */
	public interface IFeedViewModel
	{
		public string Name { get; set; }
		public string Avatar { get; set; }
		public ICursorSource PostsCursorSource { get; set; }
	}
}
