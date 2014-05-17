using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Kitechan.Types
{
    public static class WebWorker
    {
        private static string PostCommentUrl { get { return "http://mixlr.com/comments"; } }

        private static string CommentHeartUrl { get { return "http://mixlr.com/comments/{0}/heart"; } }

        private static string LogonUrl { get { return "http://mixlr.com/user_session?no_mobile=true"; } }

        private struct PostData
        {
            public HttpWebRequest Request { get; set; }
            public string Message { get; set; }
        }

        public static void PostComment(string message, string mixlrUserLogin, string mixlrSession) // jeffid magic
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PostCommentUrl);
            request.Method = "POST";
            request.UserAgent = "";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/plain";
            request.Headers.Add("X-Requested_With", "XMLHttpRequest");
            CookieContainer cookies = new CookieContainer();
            cookies.Add(new Cookie("mixlr_user_login", mixlrUserLogin));
            cookies.Add(new Cookie("mixlr_session", mixlrSession));
            request.CookieContainer = cookies;
            string body = "comment%5Bcontent%5D=" + message + "&comment%5Bbroadcaster_id%5D=" + 27902;
            request.BeginGetRequestStream(ContinuePostData, new PostData() { Request = request, Message = message });
        }

        private static void ContinuePostData(IAsyncResult result)
        {
            PostData postData = (PostData)result.AsyncState;
            Stream requestStream = postData.Request.EndGetRequestStream(result);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(postData.Message);
            requestStream.Write(bodyBytes, 0, bodyBytes.Length);
            requestStream.Close();
            WebResponse response = null;
            try
            {
                response = postData.Request.GetResponse();
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
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(CommentHeartUrl, commentId));
            request.Method = "POST";
            request.UserAgent = "";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/plain";
            request.Headers.Add("X-Requested_With", "XMLHttpRequest");
            CookieContainer cookies = new CookieContainer();
            cookies.Add(new Cookie("mixlr_user_login", mixlrUserLogin));
            cookies.Add(new Cookie("mixlr_session", mixlrSession));
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

        public static LogonResult Logon(string userName, IEnumerable<char> password)
        {
            LogonResult ret = null;
            CookieContainer requestContainer = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(LogonUrl);
            request.Method = "POST";
            request.UserAgent = "";
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
    }
}
