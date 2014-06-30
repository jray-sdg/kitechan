using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Kitechan
{
    public class UserJson
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "profile_image_url")]
        public string ProfileImageUrl { get; set; }

        [JsonProperty(PropertyName = "is_live")]
        public string IsLive { get; set; }

        [JsonProperty(PropertyName = "broadcast_ids")]
        public List<string> BroadcastIds { get; set; }

        [JsonProperty(PropertyName = "live_page_comments")]
        public List<SocketData> PageComments { get; set; }

        public static UserJson Parse(string json)
        {
            return JsonConvert.DeserializeObject<UserJson>(json);
        }
    }
}
