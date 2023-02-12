using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ThorQ_Rapid.MessageFormat
{
    [Serializable]
    internal static class Message
    {
        public static string Serialize<T>(T content)
            where T : IMessageContent
        {
            JObject msg = new JObject();
            msg["t"] = (int)content.ID;

            if (!content.Empty)
            {
                msg["d"] = JObject.FromObject(content);
            }

            return msg.ToString(Formatting.None);
        }
        public static (MessageID id, JToken data) Deserialize(string str)
        {
            try
            {
                JObject msg = JObject.Parse(str);
                if (msg != null)
                {
                    int i = msg.Value<int>("t");
                    if (Enum.IsDefined(typeof(MessageID), i))
                    {
                        MessageID id = (MessageID)i;
                        JToken token = msg["d"];
                        return (id, token);
                    }
                }
            }
            catch (Exception) { }

            return (MessageID.Undefined, null);
        }
    }
}
