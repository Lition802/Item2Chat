using CSR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Item2Chat
{
    public class Class1
    {
        public class HandContainer
        {
            /// <summary>
            /// 
            /// </summary>
            public int Slot { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int count { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string item { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string rawnameid { get; set; }
        }


        public static void init(MCCSAPI api)
        {
            string GetUUID(IntPtr p)
            {
                return new CsPlayer(api, p).Uuid;
            }
            api.addBeforeActListener(EventKey.onInputText, x =>
            {
                var a = BaseEvent.getFrom(x) as InputTextEvent;
                if(a.msg.IndexOf("%i") != -1)
                {
                    var ser = new JavaScriptSerializer();
                    try
                    {
                        var bag = new CsPlayer(api, a.playerPtr).HandContainer;
                        var t = ser.Deserialize<List<HandContainer>>(bag).ToArray();
                        if (string.IsNullOrEmpty(t[0].item))
                        {
                            api.sendText(GetUUID(a.playerPtr), "你必须手持物品来使用这个占位符！");
                        }
                        else
                        {
                            api.talkAs(GetUUID(a.playerPtr), a.msg.Replace("%i", "§2" + t[0].item + "§e*§b" + t[0].count + "§r"));
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        api.sendText(GetUUID(a.playerPtr), "发生了某种错误..." + e.Message);
                        return false;
                    }
                }
                return true;
            });
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                if(a.cmd == "/i")
                {
                    var ser = new JavaScriptSerializer();
                    try
                    {
                        var bag = new CsPlayer(api, a.playerPtr).HandContainer;
                        var t = ser.Deserialize<List<HandContainer>>(bag).ToArray();
                        if (string.IsNullOrEmpty(t[0].item))
                        {
                            api.sendText(GetUUID(a.playerPtr), "你必须手持物品来使用这个命令！");
                        }
                        else
                        {
                            api.talkAs(GetUUID(a.playerPtr), "§2" + t[0].item + "§e*§b" + t[0].count+ "§r");
                        }                       
                    }
                    catch(Exception e)
                    {
                        api.sendText(GetUUID(a.playerPtr), "发生了某种错误..."+e.Message);
                    }
                    return false;
                }
                return true;
            });
            api.setCommandDescribe("i", "展示手中物品");
        }
    }
}
namespace CSR
{
    partial class Plugin
    {
        public static void onStart(MCCSAPI api)
        {
            try
            {
                Item2Chat.Class1.init(api);
                Console.WriteLine("[Item2Chat] 加载成功");
                Console.WriteLine("[Item2Chat] 可以使用/i指令发送手中物品到聊天框");
                Console.WriteLine("[Item2Chat] 聊天时可以使用%i占位符表示手中物品");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}