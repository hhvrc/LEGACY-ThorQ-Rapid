using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThorQ_Rapid.MessageFormat
{
    public enum MessageID
    {
        Undefined = -1,
        KeepAlive = 0,
        Init = 1,
        SelfProperties = 5,
        IsUserOnline = 10,
        IsUserOnlineResp = 11,
    }
}
