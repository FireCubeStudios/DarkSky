﻿using FishyFlip.Lexicon.App.Bsky.Feed;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors.Feeds
{
    public class TimelineFeedCursorSource : AbstractFeedCursorSource
    {
        public TimelineFeedCursorSource() : base() { }

        protected override async Task OnGetMoreItemsAsync(int limit = 20)
        {
            GetTimelineOutput timeLine = (await atProtoService.ATProtocolClient.Feed.GetTimelineAsync(limit: limit, cursor: Cursor)).AsT0;
            Cursor = timeLine.Cursor;
            foreach (var item in timeLine.Feed)
            {
                // Custom logic to hide duplicate posts in the timeline only
                if (item.Reply is null)
                    AddPost(item); // Post has no replies
                else
                {
                    // Post has replies to a root post that was shown before
                    // So we do not show the reply chain
                    // bsky.app timeline has this behaviour
                    if (item.Reply.Root is not PostView) continue;
                    if (!roots.ContainsKey(((PostView)item.Reply.Root).Cid))
                    {
                        AddPost(item);
                    }
                }
            }
            /*
			 * The timeline may contain a post then a reply chain to that post which basically duplicates it
			 * This logic aims to remove duplicates to show reply chains to the original post instead
			 */
            /*	foreach (var item in timeLine.Feed)
                {
                    try
                    {
                        if (item.Reply is null)
                        { // add regular posts
                          // only add if it did not appear before, maybe as part of a reply chain
                            if (!postID.Contains(item.Post.Cid))
                                Add(PostFactory.Create(item));
                        }
                        else // the post is a reply, use logic to filter
                        {
                            ReplyRef reply = item.Reply;
                            if (reply.Root is not PostView) continue;
                            PostView root = (PostView)reply.Root;
                            if (reply.Parent is not PostView) continue;
                            PostView parent = (PostView)reply.Parent;

                            // only allow replies if it replies to same author
                            // Actually we might need to change this
                            if (root.Author.Did.Handler == item.Post.Author.Did.Handler)
                            {
                                // only add if it did not appear before, as part of a reply chain
                                if (!postID.Contains(root.Cid))
                                {
                                    // if a reply was retweeted then do not show parent or root posts
                                    if (item.Reason is null)
                                    {
                                        if (parent.Cid != root.Cid)  // If the root is not the same as the parent add the root
                                                Add(new PostViewModel(root) { HasReply = true });
                                        Add(new PostViewModel(parent) { HasReply = true, IsReply = true });
                                    }
                                    Add(PostFactory.Create(item)); // Show the regular post

                                    postID.Add(root.Cid); // add root to hashset so we can filter if it appears later
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        WeakReferenceMessenger.Default.Send(new ErrorMessage(e));
                    }
                }*/
        }
    }
}
