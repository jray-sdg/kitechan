using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kitechan.Types
{
    public class LogonResult
    {
        [JsonProperty(PropertyName = "success")]
        public string Success { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonIgnore]
        public string MixlrSession { get; set; }

        [JsonIgnore]
        public string MixlrUserLogin { get; set; }

        private LogonResult() { }

        public static LogonResult FromJson(string json)
        {
            LogonResult ret = JsonConvert.DeserializeObject<LogonResult>(json);
            return ret;
        }
    }
}
