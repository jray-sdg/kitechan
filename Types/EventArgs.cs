using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kitechan
{
    public class NewMessageEventArgs : EventArgs
    {
        public Comment Comment { get; private set; }

        public NewMessageEventArgs(Comment c)
        {
            this.Comment = c;
        }
    }

    public class SystemMessageEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public SystemMessageEventArgs(string m)
        {
            this.Message = m;
        }
    }

    public class BroadcastStartEventArgs : EventArgs
    {
        public string BroadcastTitle { get; private set; }

        public string BroadcastId { get; private set; }

        public BroadcastStartEventArgs(string t, string i)
        {
            this.BroadcastTitle = t;
            this.BroadcastId = i;
        }
    }

    public class ImageLoadedEventArgs : EventArgs
    {
        public int UserId { get; private set; }

        public bool LoadedFromWeb { get; private set; }

        public ImageLoadedEventArgs(int u, bool w)
        {
            this.UserId = u;
            this.LoadedFromWeb = w;
        }
    }

    public class CommentDeletedEventArgs : EventArgs
    {
        public int CommentId { get; private set; }

        public CommentDeletedEventArgs(int c)
        {
            this.CommentId = c;
        }
    }

    public class CommentHeartedEventArgs : EventArgs
    {
        public int CommentId { get; private set; }

        public List<int> UserIds { get; private set; }

        public CommentHeartedEventArgs(int c, IEnumerable<int> u)
        {
            this.CommentId = c;
            this.UserIds = new List<int>(u);
        }
    }

    public class StreamHeartedEventArgs : EventArgs
    {
        public int UserId { get; private set; }

        public int Total { get; private set; }

        public int GrandTotal { get; private set; }

        public StreamHeartedEventArgs(int u, int t, int g)
        {
            this.UserId = u;
            this.Total = t;
            this.GrandTotal = g;
        }
    }

    public class DeleteCommentEventArgs : EventArgs
    {
        public int CommentId { get; private set; }

        public DeleteCommentEventArgs(int c)
        {
            this.CommentId = c;
        }
    }

    public class HeartCommentEventArgs : EventArgs
    {
        public int CommentId { get; private set; }

        public HeartCommentEventArgs(int c)
        {
            this.CommentId = c;
        }
    }

    public class UnheartCommentEventArgs : EventArgs
    {
        public int CommentId { get; private set; }

        public UnheartCommentEventArgs(int c)
        {
            this.CommentId = c;
        }
    }

    public class ShowUserInfoEventArgs : EventArgs
    {
        public int UserId { get; private set; }

        public ShowUserInfoEventArgs(int u)
        {
            this.UserId = u;
        }
    }

    public class MuteUserEventArgs : EventArgs
    {
        public int UserId { get; private set; }

        public MuteUserEventArgs(int u)
        {
            this.UserId = u;
        }
    }
}
