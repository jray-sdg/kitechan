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
            request.Headers.Add(HttpRequestHeader.Cookie, string.Format("mixlr_user_login={0}; mixlr_session={1}", mixlrUserLogin, mixlrSession));
            string body = "comment%5Bcontent%5D=" + message + "&comment%5Bbroadcaster_id%5D=" + 27902;
            request.BeginGetRequestStream(ContinuePostData, new PostData() { Request = request, Message = HttpUtility.UrlEncode(message) });
        }

        private static void ContinuePostData(IAsyncResult result)
        {
            PostData postData = (PostData)result.AsyncState;
            Stream requestStream = postData.Request.EndGetRequestStream(result);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(postData.Message);
            requestStream.Write(bodyBytes, 0, bodyBytes.Length);
            requestStream.Close();
            try
            {
                WebResponse response = postData.Request.GetResponse();
            }
            catch
            {
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
            request.Headers.Add(HttpRequestHeader.Cookie, string.Format("mixlr_user_login={0}; mixlr_session={1}", mixlrUserLogin, mixlrSession));
            try
            {
                WebResponse response = request.GetResponse();
            }
            catch
            {
            }
        }
    }
}
