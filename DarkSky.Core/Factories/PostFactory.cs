using DarkSky.Core.Services;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Embed;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DarkSky.Core.Factories
{
    /*
	 * Factory method to return PostViewModels from FeedViewPost, ViewRecord, links etc
	 */
    public class PostFactory
    {
        public static PostViewModel Create(FeedViewPost post)
        {
            PostViewModel ViewModel = new(post.Post) { InternalFeedViewPost = post };
            if (post.Reason is not null) // Post might be reposted TODO: (or pinned)
            {
                ViewModel.RepostedBy = (post.Reason as ReasonRepost).By.DisplayName;
            }
            if (post.Reply is not null) // the post is a reply to another post
                ViewModel.IsReply = true;
            return ViewModel;
        }

        public async static Task<PostViewModel> CreateAsync(ViewRecord viewRecord)
            => await PostFactory.CreateAsync(viewRecord.Uri);

        public async static Task<PostViewModel> CreateAsync(ATUri Uri)
        {
            var proto = ServiceContainer.Services.GetService<ATProtoService>().ATProtocolClient;
            var result = await proto.GetPostsAsync(new List<ATUri> { Uri });
            return new PostViewModel(result.AsT0.Posts[0]);
        }
    }
}
