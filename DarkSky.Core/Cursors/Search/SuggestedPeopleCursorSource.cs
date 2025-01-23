using DarkSky.Core.Factories;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Search
{
	public class SuggestedPeopleCursorSource : AbstractCursorSource<ProfileViewModel>
	{
		protected override async Task OnGetMoreItemsAsync(int limit = 50)
		{
			GetSuggestionsOutput? people = (await atProtoService.ATProtocolClient.GetSuggestionsAsync(limit, Cursor)).AsT0;
			Cursor = people?.Cursor!;
			foreach (var item in people?.Actors!)
				Add(ProfileFactory.Create(item));
		}
	}
}
