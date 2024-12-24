using DarkSky.Core.Factories;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Feed;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Feeds
{
	/*
	 * Special CursorSource to handle FeedViewPost which have posts, parent posts and root posts
	 * Special logic is used to group post threads together and filter out duplicates
	 */
	public abstract class AbstractFeedCursorSource : AbstractCursorSource<PostViewModel>, ICursorSource
	{
		protected Dictionary<string, PostViewModel> parents = new(); // contain all "parent" posts
		protected Dictionary<string, PostViewModel> roots = new(); // contain all "root" posts

		protected override abstract Task OnGetMoreItemsAsync(int limit = 20);

		public override void Clear()
		{
			parents.Clear(); 
			roots.Clear();
			base.Clear();
		}

		/*
		 * Logic to add items
		 */
		protected void AddPost(FeedViewPost item)
		{
			if (item.Reply is null) // post has no replies
			{
				// if the item appeared before as a "root" item then remove it as it will duplicate
				// it would have appeared before with respective replies showing
				if (!roots.ContainsKey(item.Post.Cid))
					Add(PostFactory.Create(item));
			}
			else
			{
				ReplyRef reply = item.Reply;
				if (reply.Root is not PostView || reply.Parent is not PostView) return; // skip if "not found post"
				PostView root = (PostView)reply.Root;
				PostView parent = (PostView)reply.Parent;

				PostViewModel rootV = new PostViewModel(root);
				PostViewModel parentV = new PostViewModel(parent);
				PostViewModel postV = PostFactory.Create(item);
				rootV.AddReply(parentV);
				parentV.AddReply(postV);
				postV.AddParent(parentV);


				// if post has one parent/root where root == parent then add root and post
				if (root.Cid == parent.Cid)
				{
					// if the "root" appeared before do not show it
					if (!roots.ContainsKey(root.Cid))
					{
						Add(rootV);
						Add(postV);
						roots.Add(root.Cid, rootV); // register the root
					}
				}
				else
				{
					// Assume post A is the item.post and post B is the parent of a post added earlier in the same post thread
					// Check if post A appeared before as a parent for post B from earlier
					// If it did then this needs to be added as a "show full thread" scenario
					// to do this we set root of post A or B as "show full thread"
					// then we do not render anything
					// we also set the parent of post B to have a new parent which is the parent of post A
					// furthermore set the root reply to be the parent of post A
					// reference image where post A is 3s in second message and post B is 3s in first message:
					// https://discord.com/channels/714581497222398064/753345992543305808/1320524875789500416

					if (parents.ContainsKey(item.Post.Cid)) // Post appeared as a parent
					{
						PostViewModel parentPostB;
						parents.TryGetValue(item.Post.Cid, out parentPostB);
						if (parentPostB is not null)
							parentPostB.AddParent(parentV);

						PostViewModel rootPost;
						roots.TryGetValue(root.Cid, out rootPost);
						if (rootPost is not null)
						{
							rootPost.HasFullThread = true; // Mark root as "show full thread"
							rootPost.AddReply(parentV);
						}

						return;
					}

					Add(rootV);
					Add(parentV);
					Add(postV);

					parents.Add(parent.Cid, parentV); // register the parent
					if(!roots.ContainsKey(root.Cid))
						roots.Add(root.Cid, rootV); // register the root
				}
			}
		}
	}
}
