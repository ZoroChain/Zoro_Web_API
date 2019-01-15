using Neo.VM;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NEO_Block_API
{
    public class APIHelper
    {
        public static async Task<NEP5.AssetBalanceOfAddr> getBalanceFromAddressAsync(string assetid, string address, string chainHash) {
            var nep5 = new NEP5.AssetBalanceOfAddr("","","");
            nep5.assetid = assetid;
            using (ScriptBuilder sb = new ScriptBuilder()) {               
                sb.EmitAppCall(ZoroHelper.Parse(assetid), "balanceOf", address);
                sb.EmitAppCall(ZoroHelper.Parse(assetid), "decimals");
                sb.EmitAppCall(ZoroHelper.Parse(assetid), "symbol");

                var info = await ZoroHelper.InvokeScript(sb.ToArray(), chainHash);
                var value = GetBalanceFromJson(info);
                var symbol = GetSymbolFromJson(info);
                nep5.balance = value;
                nep5.symbol = symbol;
            }

            return nep5;
        }

        public static async Task<NEP5.AssetBalanceOfAddr> getNativeBalanceFromAddressAsync(string assetid, string address, string chainHash)
        {
            var nep5 = new NEP5.AssetBalanceOfAddr("", "", "");
            nep5.assetid = assetid;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitSysCall("Zoro.NativeNEP5.Call", "BalanceOf", ZoroHelper.Parse(assetid), address);
                sb.EmitSysCall("Zoro.NativeNEP5.Call", "Decimals", ZoroHelper.Parse(assetid));
                sb.EmitSysCall("Zoro.NativeNEP5.Call", "Symbol", ZoroHelper.Parse(assetid));

                var info = await ZoroHelper.InvokeScript(sb.ToArray(), chainHash);
                var value = GetBalanceFromJson(info);
                var symbol = GetSymbolFromJson(info);
                nep5.balance = value;
                nep5.symbol = symbol;
            }

            return nep5;
        }

        static string GetSymbolFromJson(string info) {
            string result = "";

            JObject json = JObject.Parse(info) as JObject;

            if (json.ContainsKey("result"))
            {
                JObject json_result = json["result"] as JObject;
                JArray stack = json_result["stack"] as JArray;

                if (stack != null && stack.Count >= 2)
                {
                    string symbol = GetJsonValue(stack[2] as JObject);
                    result = symbol;
                }
            }
            else if (json.ContainsKey("error"))
            {
                JObject json_error_obj = json["error"] as JObject;
                result = json_error_obj.ToString();
            }

            return result;
        }

        static string GetBalanceFromJson(string info)
        {
            string result = "";
            JObject json = JObject.Parse(info) as JObject;

            if (json.ContainsKey("result"))
            {
                JObject json_result = json["result"] as JObject;
                JArray stack = json_result["stack"] as JArray;

                if (stack != null && stack.Count >= 2)
                {
                    string balance = GetJsonValue(stack[0] as JObject);
                    string decimals = GetJsonValue(stack[1] as JObject);

                    Decimal value = Decimal.Parse(balance) / new Decimal(Math.Pow(10, int.Parse(decimals)));
                    string fmt = "{0:N" + decimals + "}";
                    result = string.Format(fmt, value);
                }
            }
            else if (json.ContainsKey("error"))
            {
                JObject json_error_obj = json["error"] as JObject;
                result = json_error_obj.ToString();
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

        public static byte[] HexString2Bytes(string str)
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
