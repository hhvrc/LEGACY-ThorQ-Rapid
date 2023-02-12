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
    internal struct SelfProperties : IMessageContent
    {
        public bool Empty => false;
        public MessageID ID => MessageID.SelfProperties;

        [JsonProperty(PropertyName = "p2p", Required = Required.Always)]
        public bool EnableP2P;
        [JsonProperty(PropertyName = "visible", Required = Required.Always)]
        public bool IsVisible;
        [JsonProperty(PropertyName = "usr_id", Required = Required.Always)]
        public string SelfUserID;
    }
}
