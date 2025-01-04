using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DarkSky.Helpers
{
    public class UIHelper
    {
        public static string SimpleDate(DateTime? date)
        {
            if (date is null) return "";

            TimeSpan timeDifference = (TimeSpan)(DateTime.Now - date);

            if (timeDifference.TotalDays >= 365) // More than a year ago
                return ((DateTime)date).ToString("MMM d, yyyy");
            else if (timeDifference.TotalDays >= 30) // More than a month ago but less than a year
                return ((DateTime)date).ToString("MMM d");
            else if (timeDifference.TotalDays >= 1) // More than a day but less than a month
                return $"{(int)timeDifference.TotalDays} {GetSingularOrPlural((int)timeDifference.TotalDays, "day")} ago";
            else if (timeDifference.TotalHours >= 1) // More than an hour but less than a day
                return $"{(int)timeDifference.TotalHours} {GetSingularOrPlural((int)timeDifference.TotalHours, "hour")} ago";
            else if (timeDifference.TotalMinutes >= 1) // More than a minute but less than an hour
                return $"{(int)timeDifference.TotalMinutes} {GetSingularOrPlural((int)timeDifference.TotalMinutes, "minute")} ago";
            else if (timeDifference.TotalSeconds >= 1) // More than a second but less than a minute
                return $"{(int)timeDifference.TotalSeconds} {GetSingularOrPlural((int)timeDifference.TotalSeconds, "second")} ago";
            else
                return "just now"; // Less than 1 second ago
        }

        private static string GetSingularOrPlural(int value, string singular)
        {
            return value == 1 ? singular : $"{singular}s";
        }

        public static string FormatDate(DateTime? date)
        {
            if (date is null) return "";
            return ((DateTime)date).ToString("MMMM dd, yyyy 'at' hh:mm tt", CultureInfo.InvariantCulture);
        }

        public static bool none(object obj) => obj is not null;

        public static bool invert(bool b) => !b;

        public static Visibility BoolToVis(bool b) => b ? Visibility.Visible : Visibility.Collapsed;

        public static Visibility InvertBoolToVis(bool b) => b ? Visibility.Collapsed : Visibility.Visible;

        public static ImageSource Img(string uri)
        {
            if (String.IsNullOrEmpty(uri))
                return new BitmapImage();

            // Create a BitmapImage and set its UriSource to the provided Uri
            var bitmapImage = new BitmapImage(new Uri(uri));
            return bitmapImage;
        }

        // if a post has a reply do not show the bottom border
        // used in FeedListView
        public static Thickness Border(bool hasReply) => hasReply ? new Thickness(0) : new Thickness(0, 0, 0, 1);
    }
}
