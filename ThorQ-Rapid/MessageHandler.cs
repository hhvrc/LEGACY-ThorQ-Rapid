using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThorQ_Rapid
{
    internal static class MessageHandler
    {
        public static void HandleMessage(MessageFormat.Init initMsg)
        {
            ThorQ instance = ThorQ.Instance;
            instance.UserID = initMsg.id;
            instance.IPAddress = initMsg.ipAddress;
        }
        public static void HandleMessage(MessageFormat.IsUserOnlineResp userOnline)
        {
            if (userOnline.UserID != null)
            {
                Console.WriteLine("Online: " + userOnline.UserID);
                ThorQ.Instance.SetUserOnline(userOnline.UserID);
            }
        }
        public static void HandleMessage(MessageFormat.SelfProperties selfProperties)
        {

        }
        public static void HandleRawMessage(string data)
        {
            if (data == "BEEP")
            {
                Console.WriteLine("[ThorQ] BEEP!");
                var param = ParamLib.ParamLib.FindParam("Choker", VRC.SDK3.Avatars.ScriptableObjects.VRCExpressionParameters.ValueType.Bool);
                Console.WriteLine("Found param: " + param.Item1 == null);
                if (param.Item1 == null) return;

                enabled = !enabled;

                Console.WriteLine("Set param: " + ParamLib.ParamLib.SetParameter(param.Item1.Value, enabled ? 1 : 0));

                return;
            }
        }
        static bool enabled = true;
    }
}
