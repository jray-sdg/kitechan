using Kitechan.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using WebSocket4Net;

namespace Kitechan
{
    public delegate UserInfo UserInfoDelegate(int userId);

    public delegate bool ValidateCredentialsDelegate(string userName, IEnumerable<char> password);

    class Engine
    {
        public event EventHandler<NewMessageEventArgs> NewMessageEvent;

        public event EventHandler<SystemMessageEventArgs> SystemMessageEvent;

        public event EventHandler<BroadcastStartEventArgs> BroadcastStartEvent;

        public event EventHandler<ImageLoadedEventArgs> ImageLoadedEvent;

        public event EventHandler<CommentDeletedEventArgs> CommentDeletedEvent;

        public event EventHandler<CommentHeartedEventArgs> CommentHeartedEvent;

        public event EventHandler<StreamHeartedEventArgs> StreamHeartedEvent;

        private WebSocket WebSocket { get; set; }

        private string WebSocketUrl { get { return "ws://ws.pusherapp.com/app/2c4e9e540854144b54a9?protocol=5&client=js&version=1.12.7&flash=false"; } }

        private string UserInfoUrl { get { return "http://api.mixlr.com/users/"; } }

        public int JeffUserId { get { return 27902; } }

        private string SubscribeEvent { get { return @"{""event"":""pusher:subscribe"",""data"":{""channel"":""production;user;27902""}}"; } }

        public static string DataDir { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Kitechan"); } }

        public static string StateFile { get { return Path.Combine(DataDir, "state.xml"); } }

        public static string ImageCacheDir { get { return Path.Combine(DataDir, "imageCache"); } }

        private Dictionary<int, UserInfo> userInfo;

        public Engine()
        {
            if (!Directory.Exists(DataDir))
            {
                Directory.CreateDirectory(DataDir);
            }
            if (!Directory.Exists(ImageCacheDir))
            {
                Directory.CreateDirectory(ImageCacheDir);
            }
            this.userInfo = new Dictionary<int, UserInfo>();
            this.LoadState();
            this.WebSocket = new WebSocket(this.WebSocketUrl);
            this.WebSocket.Opened += webSocket_Opened;
            this.WebSocket.Closed += webSocket_Closed;
            this.WebSocket.Error += webSocket_Error;
            this.WebSocket.MessageReceived += webSocket_MessageReceived;
        }

        public void Connect()
        {
            this.WebSocket.Open();
        }

        public void Disconnect()
        {
            this.WebSocket.Close();
        }

        public void LoadState()
        {
            if (File.Exists(StateFile))
            {
                XmlDocument stateFile = new XmlDocument();
                stateFile.Load(StateFile);
                if (stateFile["kitechan"] != null)
                {
                    foreach (XmlNode childNode in stateFile["kitechan"].ChildNodes)
                    {
                        if (childNode.Name == "userInfo")
                        {
                            foreach (XmlNode userNode in childNode.ChildNodes)
                            {
                                if (userNode.Name == "user")
                                {
                                    UserInfo user = UserInfo.FromXml(userNode);
                                    user.ImageLoadedEvent += userInfo_ImageLoadedEvent;
                                    this.userInfo.Add(user.Id, user);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SaveState()
        {
            using (XmlWriter writer = XmlWriter.Create(StateFile))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("kitechan");

                writer.WriteStartElement("userInfo");

                foreach (KeyValuePair<int, UserInfo> user in this.userInfo)
                {
                    user.Value.WriteXml(writer);
                }

                writer.WriteEndElement(); // userInfo

                writer.WriteEndElement(); // kitechan
            }
        }

        public UserInfo GetUserInfo(int userId)
        {
            if (!this.userInfo.ContainsKey(userId))
            {
                this.LoadUserInfo(userId);
            }
            return this.userInfo[userId];
        }

        private void LoadUserInfo(int userId)
        {
            if (!this.userInfo.ContainsKey(userId))
            {
                UserInfo info = UserInfo.FromUrl(this.UserInfoUrl + userId.ToString());
                info.ImageLoadedEvent += userInfo_ImageLoadedEvent;
                this.userInfo.Add(userId, info);
            }
        }

        public bool ValidateCredentials(string userName, IEnumerable<char> password)
        {
            return true;
        }

        public void PostComment(string message)
        {
            //WebWorker.PostComment(message, "", "");  // Straight busted
        }

        public void HeartComment(int commentId)
        {
            //WebWorker.HeartComment(commentId, "", "");  // Straight busted
        }

        private void userInfo_ImageLoadedEvent(object sender, ImageLoadedEventArgs e)
        {
            UserInfo info = this.GetUserInfo(e.UserId);
            if (string.IsNullOrEmpty(info.CachedImagePath))
            {
                info.CachedImagePath = Path.Combine(ImageCacheDir, info.Id + "_" + DateTime.Now.ToString("yyMMddHHmmss"));
                info.UserImage.Save(info.CachedImagePath);
            }
            this.ImageLoadedEvent(this, e);
        }

        private void webSocket_Opened(object sender, EventArgs e)
        {
            this.SystemMessageEvent(this, new SystemMessageEventArgs("Connected"));
            this.WebSocket.Send(this.SubscribeEvent);
        }

        private void webSocket_Closed(object sender, EventArgs e)
        {
            this.SystemMessageEvent(this, new SystemMessageEventArgs("Disconnected"));
        }

        private void webSocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            this.SystemMessageEvent(this, new SystemMessageEventArgs("ERROR: " + e.Exception.ToString()));
        }

        private void webSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            SocketEvent socketEvent = SocketEvent.FromJson(e.Message);
            switch (socketEvent.Event.ToLower())
            {
                case "comment:created":
                    Comment comment = Comment.ParseEvent(socketEvent);
                    if (!this.userInfo.ContainsKey(comment.UserId))
                    {
                        this.LoadUserInfo(comment.UserId);
                    }
                    this.NewMessageEvent(this, new NewMessageEventArgs(comment));
                    break;
                case "comment:hearted":
                    this.CommentHeartedEvent(this, new CommentHeartedEventArgs(int.Parse(socketEvent.Data.CommentId), socketEvent.Data.UserIds.Select(int.Parse)));
                    break;
                case "comment:deleted":
                    this.CommentDeletedEvent(this, new CommentDeletedEventArgs(int.Parse(socketEvent.Data.CommentId)));
                    break;
                case "action:created":
                    if (socketEvent.Data.Type.Equals("heart", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.StreamHeartedEvent(this, new StreamHeartedEventArgs(int.Parse(socketEvent.Data.UserId), int.Parse(socketEvent.Data.Total), int.Parse(socketEvent.Data.GrandTotal)));
                    }
                    else
                    {
                        this.SystemMessageEvent(this, new SystemMessageEventArgs("unknown action:created " + socketEvent.DataString));
                    }
                    break;
                case "broadcast:start":
                    this.BroadcastStartEvent(this, new BroadcastStartEventArgs(socketEvent.Data.Broadcast.Title));
                    break;
                default:
                    this.SystemMessageEvent(this, new SystemMessageEventArgs("Ignoring event type '" + socketEvent.Event + "'"));
                    break;
            }
        }
    }
}
