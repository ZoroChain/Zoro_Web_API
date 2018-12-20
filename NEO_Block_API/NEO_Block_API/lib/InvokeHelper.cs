using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo.VM;
using Newtonsoft.Json.Linq;
using Zoro;

namespace NEO_Block_API.lib
{
    public class InvokeHelper
    {
        public static async Task<JObject> getBalanceOfAsync(string chainHash, string assetid, string address) {
            using (ScriptBuilder sb = new ScriptBuilder())
            {               
                sb.EmitAppCall(ZoroHelper.Parse(assetid), "balanceOf", address);
                sb.EmitAppCall(ZoroHelper.Parse(assetid), "decimals");
                sb.EmitAppCall(ZoroHelper.Parse(assetid), "symbol");

                var info = await ZoroHelper.InvokeScript(sb.ToArray(), chainHash);
                var value = GetBalanceFromJson(info);
                return value;
            }
        }

        public static async Task<JObject> getNativeBalanceOfAsync(string chainHash, string assetid, string address)
        {
            using (ScriptBuilder sb = new ScriptBuilder())
            {                
                sb.EmitSysCall("Zoro.NativeNEP5.BalanceOf", UInt256.Parse(assetid), ZoroHelper.GetPublicKeyHashFromAddress(address));
                sb.EmitSysCall("Zoro.NativeNEP5.Decimals", UInt256.Parse(assetid));
                sb.EmitSysCall("Zoro.NativeNEP5.Symbol", UInt256.Parse(assetid));
                var info = await ZoroHelper.InvokeScript(sb.ToArray(), chainHash);
                var value = GetBalanceFromJson(info);
                return value;
            }           
        }

        static JObject GetBalanceFromJson(string info)
        {
            JObject result = null;
            try
            {               
                MyJson.JsonNode_Object json = MyJson.Parse(info) as MyJson.JsonNode_Object;

                if (json.ContainsKey("result"))
                {
                    MyJson.JsonNode_Object json_result = json["result"] as MyJson.JsonNode_Object;
                    MyJson.JsonNode_Array stack = json_result["stack"] as MyJson.JsonNode_Array;

                    if (stack != null && stack.Count >= 2)
                    {
                        string balance = ZoroHelper.GetJsonValue(stack[0] as MyJson.JsonNode_Object);
                        string decimals = ZoroHelper.GetJsonValue(stack[1] as MyJson.JsonNode_Object);

                        string symbol = System.Text.Encoding.Default.GetString(ZoroHelper.HexString2Bytes((stack[2] as MyJson.JsonNode_Object)["value"].AsString()));

                        Decimal value = Decimal.Parse(balance) / new Decimal(Math.Pow(10, int.Parse(decimals)));
                        string fmt = "{0:N" + decimals + "}";
                        result = new JObject() { { "balance", string.Format(fmt, value) }, { "name", symbol } };
                    }
                }
                else if (json.ContainsKey("error"))
                {
                    MyJson.JsonNode_Object json_error_obj = json["error"] as MyJson.JsonNode_Object;
                    result = new JObject() { { "error", json_error_obj.ToString() } };
                }
            }
            catch (Exception e) {
                throw e;
            }            
            return result;
        }
    }
}
