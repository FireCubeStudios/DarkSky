using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.ViewModels.Temporary
{
	public partial class AccountViewModel : ProfileViewModel
	{
		public AccountViewModel(ATDid did) : base(did) { }

		public AccountViewModel(ProfileViewDetailed profileView) : base(profileView)
		{
		}
	}
}
