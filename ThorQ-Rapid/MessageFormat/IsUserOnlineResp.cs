using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThorQ_Rapid.MessageFormat
{
    [Serializable]
    internal struct IsUserOnlineResp : IMessageContent
    {
        public bool Empty => false;
        public MessageID ID => MessageID.IsUserOnlineResp;

        [JsonProperty(PropertyName = "uid")]
        public string UserID;
    }
}
