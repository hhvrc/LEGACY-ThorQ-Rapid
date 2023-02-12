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
    internal struct IsUserOnline : IMessageContent
    {
        public IsUserOnline(string userID)
        {
            UserID = userID;
        }

        public bool Empty => false;
        public MessageID ID => MessageID.IsUserOnline;

        [JsonProperty(PropertyName = "usr_id")]
        public string UserID;

    }
}
