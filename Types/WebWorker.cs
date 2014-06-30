using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kitechan.Types
{
    public static class WebWorker
    {
        private static string PostCommentUrl { get { return "http://mixlr.com/comments"; } }

        private static string CommentHeartUrl { get { return "http://mixlr.com/comments/{0}/heart"; } }

        private static string StreamHeartUrl { get { return "http://mixlr.com/broadcast_actions"; } }

        private static string LogonUrl { get { return "http://mixlr.com/user_session?no_mobile=true"; } }

        private static string UserInfoUrl { get { return "http://api.mixlr.com/users/{0}"; } }

        private static string UserInfoUrlWithComments { get { return string.Format(UserInfoUrl, "{0}?include_comments=true"); } }

        public static void PostComment(string message, string mixlrUserLogin, string mixlrSession)
        {
            Task.Factory.StartNew(() => PerformPostComment(message, mixlrUserLogin, mixlrSession));
        }

        private static void PerformPostComment(string message, string mixlrUserLogin, string mixlrSession)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PostCommentUrl);
            request.Method = "POST";
            request.UserAgent = "Kitechan";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            CookieContainer cookies = new CookieContainer();
            cookies.Add(new Cookie("mixlr_user_login", mixlrUserLogin, "/", "mixlr.com"));
            cookies.Add(new Cookie("mixlr_session", mixlrSession, "/", "mixlr.com"));
            request.CookieContainer = cookies;
            byte[] messageBytes = Encoding.UTF8.GetBytes(message.Replace(' ', '+'));
            List<byte> body = new List<byte>();
            body.AddRange(Encoding.UTF8.GetBytes("comment%5Bcontent%5D="));
            body.AddRange(messageBytes);
            body.AddRange(Encoding.UTF8.GetBytes(string.Format("&comment%5Bbroadcaster_id%5D={0}", Engine.JeffUserId)));
            WebResponse response = null;
            try
            {
                Stream bodyStream = request.GetRequestStream();
                bodyStream.Write(body.ToArray(), 0, body.Count);
                bodyStream.Close();
                response = (HttpWebResponse)request.GetResponse();
            }
            catch
            {
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        public static void HeartComment(int commentId, string mixlrUserLogin, string mixlrSession)
        {
            Task.Factory.StartNew(() => PerformHeartComment(commentId, mixlrUserLogin, mixlrSession));
        }

        private static void PerformHeartComment(int commentId, string mixlrUserLogin, string mixlrSession)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(CommentHeartUrl, commentId));
            request.Method = "POST";
            request.UserAgent = "Kitechan";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/plain";
            request.Headers.Add("X-Requested_With", "XMLHttpRequest");
            CookieContainer cookies = new CookieContainer();
            cookies.Add(new Cookie("mixlr_user_login", mixlrUserLogin, "/", "mixlr.com"));
            cookies.Add(new Cookie("mixlr_session", mixlrSession, "/", "mixlr.com"));
            request.CookieContainer = cookies;
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch
            {
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        public static void UnheartComment(int commentId, string mixlrUserLogin, string mixlrSession)
        {
            Task.Factory.StartNew(() => PerformUnheartComment(commentId, mixlrUserLogin, mixlrSession));
        }

        private static void PerformUnheartComment(int commentId, string mixlrUserLogin, string mixlrSession)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(CommentHeartUrl, commentId));
            request.Method = "DELETE";
            request.UserAgent = "Kitechan";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/plain";
            request.Headers.Add("X-Requested_With", "XMLHttpRequest");
            CookieContainer cookies = new CookieContainer();
            cookies.Add(new Cookie("mixlr_user_login", mixlrUserLogin, "/", "mixlr.com"));
            cookies.Add(new Cookie("mixlr_session", mixlrSession, "/", "mixlr.com"));
            request.CookieContainer = cookies;
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch
            {
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        public static void HeartStream(string broadcastId, string mixlrUserLogin, string mixlrSession)
        {
            Task.Factory.StartNew(() => PerformHeartStream(broadcastId, mixlrUserLogin, mixlrSession));
        }

        private static void PerformHeartStream(string broadcastId, string mixlrUserLogin, string mixlrSession)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(StreamHeartUrl);
            request.Method = "POST";
            request.UserAgent = "Kitechan";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            CookieContainer cookies = new CookieContainer();
            cookies.Add(new Cookie("mixlr_user_login", mixlrUserLogin, "/", "mixlr.com"));
            cookies.Add(new Cookie("mixlr_session", mixlrSession, "/", "mixlr.com"));
            request.CookieContainer = cookies;
            byte[] broadcastBytes = Encoding.UTF8.GetBytes(broadcastId);
            List<byte> body = new List<byte>();
            body.AddRange(Encoding.UTF8.GetBytes("type=heart&broadcast_uid="));
            body.AddRange(broadcastBytes);
            WebResponse response = null;
            try
            {
                Stream bodyStream = request.GetRequestStream();
                bodyStream.Write(body.ToArray(), 0, body.Count);
                bodyStream.Close();
                response = (HttpWebResponse)request.GetResponse();
            }
            catch
            {
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        public static LogonResult Logon(string userName, IEnumerable<char> password)
        {
            LogonResult ret = null;
            CookieContainer requestContainer = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(LogonUrl);
            request.Method = "POST";
            request.UserAgent = "Kitechan";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Accept = "application/json";
            request.Headers.Add("X-Requested_With", "XMLHttpRequest");
            request.CookieContainer = requestContainer;
            byte[] user = Encoding.UTF8.GetBytes(userName);
            char[] pass = password.ToArray();
            byte[] encPass = Encoding.UTF8.GetBytes(pass);
            for (int x = 0; x < pass.Length; x++)
            {
                pass[x] = '\0';
            }
            List<byte> body = new List<byte>();
            body.AddRange(Encoding.UTF8.GetBytes("user_session%5Busername%5D="));
            body.AddRange(user);
            body.AddRange(Encoding.UTF8.GetBytes("&user_session%5Bpassword%5D="));
            body.AddRange(encPass);
            body.AddRange(Encoding.UTF8.GetBytes("&user_session%5Bremember_me%5D=true"));
            HttpWebResponse response = null;
            try
            {
                Stream bodyStream = request.GetRequestStream();
                bodyStream.Write(body.ToArray(), 0, body.Count);
                bodyStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                List<byte> responseBytes = new List<byte>();
                byte[] responseBuffer = new byte[1024];
                int bytesRead = 0;
                do
                {
                    bytesRead = responseStream.Read(responseBuffer, 0, responseBuffer.Length);
                    for (int x = 0; x < bytesRead; x++)
                    {
                        responseBytes.Add(responseBuffer[x]);
                    }
                } while (bytesRead > 0);
                responseStream.Close();
                string responseString = Encoding.UTF8.GetString(responseBytes.ToArray());
                ret = LogonResult.FromJson(responseString);
                foreach (Cookie c in response.Cookies)
                {
                    if (c.Name.Equals("mixlr_user_login", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ret.MixlrUserLogin = c.Value;
                    }
                    else if (c.Name.Equals("mixlr_session", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ret.MixlrSession = c.Value;
                    }
                }
            }
            catch
            {
                ret = null;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                for (int x = 0; x < encPass.Length; x++)
                {
                    encPass[x] = 0;
                }
            }
            return ret;
        }

        public static UserInfo GetUserInfo(int userId)
        {
            UserJson json = GetUserJson(userId, false);
            return new UserInfo(int.Parse(json.Id), json.UserName, json.ProfileImageUrl);
        }

        public static UserJson GetUserJson(int userId, bool includeComments)
        {
            using (WebClient client = new WebClient())
            {
                string userInfo = client.DownloadString(string.Format(!includeComments ? UserInfoUrl : UserInfoUrlWithComments, userId));
                return UserJson.Parse(userInfo);
            }
        }
    }
}
