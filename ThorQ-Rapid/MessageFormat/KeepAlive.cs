using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThorQ_Rapid.MessageFormat
{
    [Serializable]
    internal struct KeepAlive : IMessageContent
    {
        public bool Empty => true;
        public MessageID ID => MessageID.KeepAlive;
    }
}
