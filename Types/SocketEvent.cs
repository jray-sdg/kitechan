using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kitechan
{
    public class BroadcastData
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }

    public class SocketData
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "user_ids")]
        public List<string> UserIds { get; set; }

        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty(PropertyName = "comment_id")]
        public string CommentId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "total")]
        public string Total { get; set; }

        [JsonProperty(PropertyName="grand_total")]
        public string GrandTotal { get; set; }

        [JsonProperty(PropertyName = "broadcast")]
        public BroadcastData Broadcast { get; private set; }

        public static SocketData FromJson(string json)
        {
            SocketData ret = JsonConvert.DeserializeObject<SocketData>(json);
            //if (!string.IsNullOrEmpty(ret.BroadcastString))
            //{
            //    ret.Broadcast = JsonConvert.DeserializeObject<BroadcastData>(ret.BroadcastString);
            //}
            return ret;
        }
    }

    public class SocketEvent
    {
        [JsonProperty(PropertyName = "event")]
        public string Event { get; set; }

        [JsonProperty(PropertyName = "data")]
        public string DataString { get; set; }

        [JsonIgnore]
        public SocketData Data { get; private set; }

        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        public static SocketEvent FromJson(string json)
        {
            SocketEvent ret = JsonConvert.DeserializeObject<SocketEvent>(json);
            ret.Data = SocketData.FromJson(ret.DataString);
            return ret;
        }
    }
}
