using DarkSky.Core.Services;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DarkSky.Core.Factories
{
    /*
	 * Factory method to return ProfileViewModel from ProfileViewBasic, Handles and DID
	 */
    public class ProfileFactory
    {
        public static ProfileViewModel Create(ProfileView profileView)
        {
            return new ProfileViewModel(profileView.Did)
            {
                Handle = profileView.Handle,
                DisplayName = profileView.DisplayName,
                Avatar = profileView.Avatar,
                Description = profileView.Description,
                CreatedAt = profileView.CreatedAt ?? profileView.IndexedAt ?? DateTime.Now
            };
        }

        public static ProfileViewModel Create(ProfileViewBasic profileView)
        {
            return new ProfileViewModel(profileView.Did)
            {
                Handle = profileView.Handle,
                DisplayName = profileView.DisplayName,
                Avatar = profileView.Avatar
            };
        }

        public static async Task<ProfileViewModel> CreateAsync(ATIdentifier Identifier)
        {
            var proto = ServiceContainer.Services.GetService<ATProtoService>().ATProtocolClient;
            var profile = await proto.Actor.GetProfileAsync(Identifier);
            return new ProfileViewModel(profile.AsT0);
        }
    }
}
