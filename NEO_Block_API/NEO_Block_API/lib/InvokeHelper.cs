using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
                sb.EmitAppCall(ZoroHelper.Parse(assetid), "balanceOf", ZoroHelper.GetPublicKeyHashFromAddress(address));
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
                sb.EmitSysCall("Zoro.NativeNEP5.Call", "BalanceOf", UInt160.Parse(assetid), ZoroHelper.GetPublicKeyHashFromAddress(address));
                sb.EmitSysCall("Zoro.NativeNEP5.Call", "Decimals", UInt160.Parse(assetid));
                sb.EmitSysCall("Zoro.NativeNEP5.Call", "Symbol", UInt160.Parse(assetid));
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
                JObject json = JObject.Parse(info) as JObject;

                if (json.ContainsKey("result"))
                {
                    JObject json_result = json["result"] as JObject;
                    JArray stack = json_result["stack"] as JArray;

                    if (stack != null && stack.Count >= 2)
                    {
                        string balance = GetJsonValue(stack[0] as JObject);
                        string decimals = GetJsonValue(stack[1] as JObject);
                        string symbol = GetJsonSymbol(stack[2] as JObject);

                        Decimal value = Decimal.Parse(balance) / new Decimal(Math.Pow(10, int.Parse(decimals)));
                        //string fmt = "{0:N" + decimals + "}";
                        //result = new JObject{ { "symbol",symbol },{ "balance", string.Format(fmt, value) } };
                        result = new JObject { { "symbol", symbol }, { "balance", value } };
                    }
                }
                else if (json.ContainsKey("error"))
                {
                    JObject json_error_obj = json["error"] as JObject;
                    result = JObject.Parse(json_error_obj.ToString());
                }
            }
            catch (Exception e) {
                throw e;
            }            
            return result;
        }

        static string GetJsonValue(JObject item)
        {
            var type = item["type"].ToString();
            var value = item["value"];
            if (type == "ByteArray")
            {
                var bt = HexString2Bytes(value.ToString());
                var num = new BigInteger(bt);
                return num.ToString();

            }
            else if (type == "Integer")
            {
                return value.ToString();

            }
            return "";
        }

        static string GetJsonSymbol(JObject item) {
            var type = item["type"].ToString();
            var value = item["value"];
            if (type == "ByteArray")
            {
                var bt = HexString2Bytes(value.ToString());
                var num = System.Text.Encoding.UTF8.GetString(bt);
                return num.ToString();
            }
            return "";
        }

        static byte[] HexString2Bytes(string str)
        {
            if (str.IndexOf("0x") == 0)
                str = str.Substring(2);
            byte[] outd = new byte[str.Length / 2];
            for (var i = 0; i < str.Length / 2; i++)
            {
                outd[i] = byte.Parse(str.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return outd;
        }
    }
}
