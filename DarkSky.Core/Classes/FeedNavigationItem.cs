﻿using DarkSky.Core.Cursors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Classes
{
	public record FeedNavigationItem(string Name, IFeedCursorSource FeedSource);
}
