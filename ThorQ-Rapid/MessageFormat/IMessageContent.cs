using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ThorQ_Rapid.MessageFormat
{
    internal interface IMessageContent
    {
        bool Empty { get; }
        MessageID ID { get; }
    } 
}
