using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Classes;
using DarkSky.Core.Factories;
using DarkSky.Core.Helpers;
using DarkSky.Core.Messages;
using DarkSky.Core.ViewModels.Temporary;
using DarkSky.UserControls.Embeds;
using FishyFlip.Lexicon;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Lexicon.App.Bsky.Richtext;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Web;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static System.Net.Mime.MediaTypeNames;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.Controls
{
	/*
	 * Facets are rendered as hyperlinks
	 * A link facet has a standard uri and is launched normally
	 * A mention facet has the uri "mention://did?did={mention.Did}"
	 * A tag facet has the uri "tag://tag?tag={tagValue}"
	 * Clicking of Facets are handled here for now
	 * https://docs.bsky.app/docs/advanced-guides/post-richtext
	 */
	public sealed partial class RichFacetTextBlock : UserControl
	{
		public RichText RichText
		{
			get => (RichText)GetValue(RichTextProperty);
			set => SetValue(RichTextProperty, value);
		}

		public static readonly DependencyProperty RichTextProperty =
			DependencyProperty.Register(nameof(RichText), typeof(RichText), typeof(RichFacetTextBlock), new PropertyMetadata(null, OnRichTextChanged));

		// Adding the PropertyChanged method fixes virtualisation duplication bug
		private static void OnRichTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is RichFacetTextBlock t)
			{
				t.Bindings.Update();
				if(e.NewValue is not null)
					t.setup(e.NewValue as RichText);
			}
		}

		public bool IsTextSelectionEnabled
		{
			get { return (bool)GetValue(IsTextSelectionEnabledProperty); }
			set { SetValue(IsTextSelectionEnabledProperty, value); }
		}

		public static readonly DependencyProperty IsTextSelectionEnabledProperty =
			DependencyProperty.Register("IsTextSelectionEnabled", typeof(bool), typeof(RichFacetTextBlock), new PropertyMetadata(false));

		public RichFacetTextBlock()
		{
			this.InitializeComponent();
		}

		private async void setup(RichText text)
		{
			if(text is not null)
			{
				RichTextBlock.Inlines.Clear(); // Clear the old text, prevents duplicates with virtualisation

				var run = new Run	{	Text = text.Text	};
				RichTextBlock.Inlines.Add(run);
				
				if(text.Facets is not null)
				{
					foreach (Facet facet in text.Facets)
					{
							if (facet.Features.Count == 1)
							{
								if (facet.Features[0].Type == "app.bsky.richtext.facet#mention")
								{
									var mention = facet.Features[0] as Mention;
									AddLink(
										RichTextHelper.ByteIndexToCharIndex(text.Text, (long)facet.Index.ByteStart),
										RichTextHelper.ByteIndexToCharIndex(text.Text, (long)facet.Index.ByteEnd),
										new Uri($"mention://did?did={mention.Did}")
									);
								}
								else if (facet.Features[0].Type == "app.bsky.richtext.facet#tag")
								{
									var tag = facet.Features[0] as Tag;
									AddLink(
										RichTextHelper.ByteIndexToCharIndex(text.Text, (long)facet.Index.ByteStart),
										RichTextHelper.ByteIndexToCharIndex(text.Text, (long)facet.Index.ByteEnd),
										new Uri($"tag://{tag.TagValue}")
									);
								}
								else if (facet.Features[0].Type == "app.bsky.richtext.facet#link")
								{
									var link = facet.Features[0] as Link;
									AddLink(
										RichTextHelper.ByteIndexToCharIndex(text.Text, (long)facet.Index.ByteStart),
										RichTextHelper.ByteIndexToCharIndex(text.Text, (long)facet.Index.ByteEnd),
										new Uri(link.Uri)
									);
								}
							}
					}
				}
			}
		}

		// Add a hyperlink based on char index with inclusive start index and an exclusive end index.
		private void AddLink(int startIndex, int endIndex, Uri uri)
		{
			int currentIndex = 0;
			bool linkAdded = false;

			// Iterate through the inlines in the paragraph
			for (int i = 0; i < RichTextBlock.Inlines.Count; i++)
			{
				Inline inline = RichTextBlock.Inlines[i];

				if (inline is Run run)
				{
					// Check if the hyperlink should start within the current Run
					if (!linkAdded && currentIndex + run.Text.Length > startIndex)
					{
						// Determine the position to split the Run text
						int linkStartIndex = startIndex - currentIndex;
						int linkEndIndex = Math.Min(endIndex - currentIndex, run.Text.Length);

						if(linkStartIndex < 0) // error handling
						{
							Debug.WriteLine(RichText.Text);
							return;
						}
						// Split the current Run into three parts
						string beforeLink = run.Text.Substring(0, linkStartIndex);
						string linkText = run.Text.Substring(linkStartIndex, linkEndIndex - linkStartIndex);
						string afterLink = run.Text.Substring(linkEndIndex);

						// Remove the current Run since it will be replaced
						RichTextBlock.Inlines.RemoveAt(i);

						// Add the part before the link, if any
						if (!string.IsNullOrEmpty(beforeLink))
							RichTextBlock.Inlines.Insert(i, new Run { Text = beforeLink });

						// Create and add the Hyperlink
						// We dont set NavigateUri to prevent default navigation behaviour
						Hyperlink hyperlink = new Hyperlink();
						hyperlink.UnderlineStyle = UnderlineStyle.None;
						// We do this to pass the uri when hyperlink clicked for further processing
						hyperlink.Click += async (sender, e) => {
							Hyperlink_Click(sender, uri);
						};
						hyperlink.Inlines.Add(new Run { Text = linkText });


						if (!string.IsNullOrEmpty(beforeLink))	// Avoid bugs if link starts at index 0
							RichTextBlock.Inlines.Insert(i + 1, hyperlink);
						else
							RichTextBlock.Inlines.Insert(i, hyperlink);

						// Add the part after the link, if any
						if (!string.IsNullOrEmpty(afterLink))
						{
							if (!string.IsNullOrEmpty(beforeLink))	// Avoid bugs if link starts at index 0
								RichTextBlock.Inlines.Insert(i + 2, new Run { Text = afterLink });
							else
								RichTextBlock.Inlines.Insert(i + 1, new Run { Text = afterLink });
						}


						linkAdded = true;   // Mark that the link has been added

						// Adjust indices
						currentIndex += beforeLink.Length + linkText.Length + afterLink.Length;
						i += 2;		// Skip the newly inserted hyperlink and any following text
					}
					else
						currentIndex += run.Text.Length;  // Continue checking the next Run
				}
				else if (inline is Hyperlink hyperlink)
				{
					// If it's already a hyperlink, we don't need to do anything
					// This is because we assume Facets do not overlap
					currentIndex += hyperlink.Inlines.OfType<Run>().Sum(r => r.Text.Length);
				}
			}
		}

		// Process clicked URI here
		private async void Hyperlink_Click(Hyperlink sender, Uri uri)
		{
			if(uri.Scheme == "http" || uri.Scheme == "https")
				await Launcher.LaunchUriAsync(uri);
			else if(uri.Scheme == "mention")
			{
				// Format "mention://did?did={mention.Did}"
				string did = HttpUtility.ParseQueryString(uri.Query)["did"];
				WeakReferenceMessenger.Default.Send(new SecondaryNavigationMessage(
					new SecondaryNavigation(typeof(ProfileViewModel), await ProfileFactory.CreateAsync(new ATDid(did)))));
			}
		}
	}
}
