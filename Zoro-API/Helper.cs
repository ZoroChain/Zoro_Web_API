﻿using System;
using System.Net;
using System.Threading.Tasks;
using Zoro.Cryptography;
using System.Linq;
using Zoro;
using Zoro.IO.Json;
using System.Text;

namespace Zoro_Web_API
{
    static class Helper
    {
        public static string Bytes2HexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var d in data)
            {
                sb.Append(d.ToString("x02"));
            }
            return sb.ToString();
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

        public static string ToAddress(this UInt160 scriptHash)
        {
            byte[] data = new byte[21];
            data[0] = 23;
            Buffer.BlockCopy(scriptHash.ToArray(), 0, data, 1, 20);
            return data.Base58CheckEncode();
        }

        public static string MakeRpcUrlPost(string url, string method, out byte[] data, JArray _params)
        {
            var json = new JObject();
            json["id"] = 1;
            json["jsonrpc"] = "2.0";
            json["method"] = method;
            StringBuilder sb = new StringBuilder();
            var array = new JArray();
            for (var i = 0; i < _params.Count; i++)
            {
                array.Add(_params[i]);
            }
            json["params"] = array;
            data = Encoding.UTF8.GetBytes(json.ToString());
            return url;
        }

        public static string MakeRpcUrl(string url, string method, params object[] _params)
        {
            StringBuilder sb = new StringBuilder();
            if (url.Last() != '/')
            {
                url = url + "/";
            }
            sb.Append(url + "?jsonrpc=2.0&id=1&method=" + method + "&params=[");
            for (var i = 0; i < _params.Length; i++)
            {
                sb.Append("\"" + _params[i] + "\"");
                if (i != _params.Length - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }



        public static async Task<string> HttpGet(string url)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Proxy = null;
                return await wc.DownloadStringTaskAsync(url);
            }
        }

        public static async Task<string> HttpPost(string url, byte[] data)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Proxy = null;
                wc.Headers["content-type"] = "text/plain;charset=UTF-8";
                byte[] retdata = await wc.UploadDataTaskAsync(url, "POST", data);
                return System.Text.Encoding.UTF8.GetString(retdata);
            }
        }
    }
}
