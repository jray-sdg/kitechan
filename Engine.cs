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

        public string BroadcastId { get; private set; }

        public bool LoggedIn { get; private set; }

        public int LoggedInUserId { get; private set; }

        private string MixlrUserLogin { get; set; }

        private string MixlrSession { get; set; }

        public ClientSettings Settings { get; set; }

        private WebSocket WebSocket { get; set; }

        private string WebSocketUrl { get { return "ws://ws.pusherapp.com/app/2c4e9e540854144b54a9?protocol=5&client=js&version=1.12.7&flash=false"; } }

        public static int JeffUserId { get { return 27902; } }

        private string SubscribeEvent { get { return string.Format(@"{{""event"":""pusher:subscribe"",""data"":{{""channel"":""production;user;{0}""}}}}", JeffUserId); } }

        public static string DataDir { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Kitechan"); } }

        public static string StateFile { get { return Path.Combine(DataDir, "state.xml"); } }

        public static string ImageCacheDir { get { return Path.Combine(DataDir, "imageCache"); } }

        private Dictionary<int, UserInfo> userInfo;

#if LOGCHAT
        private StreamWriter chatLog;
        private DateTime lastLog;
#endif

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
            this.BroadcastId = string.Empty;
            this.LoggedIn = false;
            this.LoggedInUserId = 0;
            this.MixlrUserLogin = string.Empty;
            this.MixlrSession = string.Empty;
            this.Settings = new ClientSettings();
            this.LoadState();
            this.WebSocket = new WebSocket(this.WebSocketUrl);
            this.WebSocket.Opened += webSocket_Opened;
            this.WebSocket.Closed += webSocket_Closed;
            this.WebSocket.Error += webSocket_Error;
            this.WebSocket.MessageReceived += webSocket_MessageReceived;

#if LOGCHAT
            this.chatLog = File.AppendText(string.Format(@"E:\mixlr\{0}", DateTime.Now.ToString("yyMMdd")));
            this.chatLog.AutoFlush = true;
            this.lastLog = DateTime.Now;
#endif
        }

        public void Connect()
        {
            this.WebSocket.Open();
        }

        public void Disconnect()
        {
            if (this.WebSocket.State == WebSocketState.Open)
            {
                this.WebSocket.Close();
            }

#if LOGCHAT
            this.chatLog.Close();
#endif
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
                        if (childNode.Name == "loggedInUser")
                        {
                            this.LoggedIn = true;
                            this.LoggedInUserId = int.Parse(childNode.InnerText);
                        }
                        else if(childNode.Name == "mixlrUserLogin")
                        {
                            this.MixlrUserLogin = childNode.InnerText;
                        }
                        else if (childNode.Name == "mixlrSession")
                        {
                            this.MixlrSession = childNode.InnerText;
                        }
                        else if (childNode.Name == "clientSettings")
                        {
                            this.Settings = ClientSettings.FromXml(childNode);
                        }
                        else if (childNode.Name == "userInfo")
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

                if (this.LoggedIn)
                {
                    writer.WriteElementString("loggedInUser", this.LoggedInUserId.ToString());
                    writer.WriteElementString("mixlrUserLogin", this.MixlrUserLogin);
                    writer.WriteElementString("mixlrSession", this.MixlrSession);
                }

                this.Settings.WriteXml(writer);

                writer.WriteStartElement("userInfo");

                foreach (KeyValuePair<int, UserInfo> user in this.userInfo)
                {
                    user.Value.WriteXml(writer);
                }

                writer.WriteEndElement(); // userInfo

                writer.WriteEndElement(); // kitechan
            }
        }

        public void LoadStreamInfo()
        {
            if (this.Settings.LoadChatHistory)
            {
                UserJson jeffJson = WebWorker.GetUserJson(JeffUserId, true);
                if (bool.Parse(jeffJson.IsLive))
                {
                    this.BroadcastId = jeffJson.BroadcastIds != null && jeffJson.BroadcastIds.Count > 0 ? jeffJson.BroadcastIds[0] : string.Empty;
                }
                if (jeffJson.PageComments != null && this.NewMessageEvent != null)
                {
                    for (int x = Math.Min(25, jeffJson.PageComments.Count - 1); x >= 0; x--)
                    {
                        this.NewMessageEvent(this, new NewMessageEventArgs(Comment.ParseData(jeffJson.PageComments[x])));
                    }
                }
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
                UserInfo info = WebWorker.GetUserInfo(userId);
                info.ImageLoadedEvent += userInfo_ImageLoadedEvent;
                this.userInfo.Add(userId, info);
            }
        }

        public bool ValidateCredentials(string userName, IEnumerable<char> password)
        {
            LogonResult result = WebWorker.Logon(userName, password);
            if (result != null)
            {
                bool success = false;
                if (bool.TryParse(result.Success, out success) || success)
                {
                    this.LoggedIn = true;
                    this.LoggedInUserId = int.Parse(result.UserId);
                    this.MixlrUserLogin = result.MixlrUserLogin;
                    this.MixlrSession = result.MixlrSession;
                    return true;
                }
            }
            return false;
        }

        public void PostComment(string message)
        {
            if (this.LoggedIn)
            {
                WebWorker.PostComment(message, this.MixlrUserLogin, this.MixlrSession);
            }
        }

        public void DeleteComment(int commentId)
        {
            if (this.LoggedIn)
            {
                WebWorker.DeleteComment(commentId, this.MixlrUserLogin, this.MixlrSession);
            }
        }

        public void HeartComment(int commentId)
        {
            if (this.LoggedIn)
            {
                WebWorker.HeartComment(commentId, this.MixlrUserLogin, this.MixlrSession);
            }
        }

        public void UnheartComment(int commentId)
        {
            if (this.LoggedIn)
            {
                WebWorker.UnheartComment(commentId, this.MixlrUserLogin, this.MixlrSession);
            }
        }

        public void HeartStream()
        {
            if (this.LoggedIn && !string.IsNullOrEmpty(this.BroadcastId))
            {
                WebWorker.HeartStream(this.BroadcastId, this.MixlrUserLogin, this.MixlrSession);
            }
        }

        public void MuteUser(int userId)
        {
            if (!this.Settings.MutedUsers.Contains(userId))
            {
                this.Settings.MutedUsers.Add(userId);
            }
        }

        public void UnmuteUser(int userId)
        {
            if (this.Settings.MutedUsers.Contains(userId))
            {
                this.Settings.MutedUsers.Remove(userId);
            }
        }

        public void ClearMutedUsers()
        {
            this.Settings.MutedUsers.Clear();
        }

        private void userInfo_ImageLoadedEvent(object sender, ImageLoadedEventArgs e)
        {
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
                    this.GetUserInfo(comment.UserId).UpdateUser(comment.Name, comment.ImageUrl);
                    if (!this.Settings.MutedUsers.Contains(comment.UserId))
                    {
                        this.NewMessageEvent(this, new NewMessageEventArgs(comment));
                    }

#if LOGCHAT
                    if (this.lastLog.DayOfYear != DateTime.Now.DayOfYear)
                    {
                        this.chatLog.Close();
                        this.chatLog = File.AppendText(string.Format(@"E:\mixlr\{0}", DateTime.Now.ToString("yyMMdd")));
                        this.chatLog.AutoFlush = true;
                        this.lastLog = DateTime.Now;
                    }
                    this.chatLog.WriteLine(string.Format("{0} - {1} - {2}", comment.Name, comment.PostedTime.ToString("HH:mm:ss"), comment.Message));
#endif
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
                        this.StreamHeartedEvent(this, new StreamHeartedEventArgs(int.Parse(socketEvent.Data.UserId), int.Parse(socketEvent.Data.Total), 0));//int.Parse(socketEvent.Data.GrandTotal)));
                    }
                    else
                    {
                        this.SystemMessageEvent(this, new SystemMessageEventArgs("unknown action:created " + socketEvent.DataString));
                    }
                    break;
                case "broadcast:start":
                    this.BroadcastId = socketEvent.Data.Broadcast.Id;
                    this.BroadcastStartEvent(this, new BroadcastStartEventArgs(socketEvent.Data.Broadcast.Title, socketEvent.Data.Broadcast.Id));
                    break;
                default:
                    this.SystemMessageEvent(this, new SystemMessageEventArgs("Ignoring event type '" + socketEvent.Event + "'"));
                    break;
            }
        }
    }
}
