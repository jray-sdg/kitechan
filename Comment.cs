using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kitechan
{
    public class Comment
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Message { get; private set; }

        public DateTime PostedTime { get; private set; }

        public int UserId { get; private set; }

        public string ImageUrl { get; private set; }

        private Comment()
        {
            this.PostedTime = DateTime.Now;
        }

        public static Comment ParseEvent(SocketEvent socketEvent)
        {
            return ParseData(socketEvent.Data);
        }

        public static Comment ParseData(SocketData socketData)
        {
            Comment ret = new Comment();
            ret.Id = int.Parse(socketData.Id);
            ret.Name = socketData.Name;
            ret.Message = socketData.Content;
            ret.PostedTime = socketData.CreatedAt;
            ret.UserId = int.Parse(socketData.UserId);
            ret.ImageUrl = socketData.ImageUrl;
            return ret;
        }

 //       {"event":"comment:created",
 //         "data":"{ \"id\":70161318,\
 //             "content\":\"A biker ran into my car once and was real pissed off at me for existing. He took a blind corner wayyyy too sharp and too fast, coming all the way into my lane and the obvious happened.\",
 //             \"safe_content\":\"A biker ran into my car once and was real pissed off at me for existing. He took a blind corner wayyyy too sharp and too fast, coming all the way into my lane and the obvious happened.\",
 //             \"name\":\"GAF_Man\",
 //             \"url\":\"http://mixlr.com/gaf_man\",
 //             \"image_url\":\"https://mixlr-assets.s3.amazonaws.com/users/7a9ab65a65af40dbab1635e9291c0d5c/thumb.jpg?1389808920\",
 //             \"user_id\":1118863,
 //             \"created_at\":\"2014-05-05T22:23:37+01:00\",
 //             \"timestamp\":1399325017,
 //             \"hearted_user_ids\":[],
 //             \"broadcast_id\":null,
 //             \"broadcast_title\":null,
 //             \"total_comment_count\":68956}",
 //          "channel":"production;user;27902"}
    }
}
