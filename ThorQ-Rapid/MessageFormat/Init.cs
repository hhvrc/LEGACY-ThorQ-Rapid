using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ThorQ_Rapid.MessageFormat
{
    [Serializable]
    internal struct Init : IMessageContent
    {
        public bool Empty => false;
        public MessageID ID => MessageID.Init;

        [JsonProperty(PropertyName = "id")]
        public string id;
        [JsonProperty(PropertyName = "ip")]
        public string ipAddress;
    }
}
