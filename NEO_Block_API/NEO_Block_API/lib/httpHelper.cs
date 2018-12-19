using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NEO_Block_API.lib
{
    public class httpHelper
    {
        /// <summary>
        /// 同步get请求
        /// </summary>
        /// <param name="url">链接地址</param>    
        /// <param name="formData">写在header中的键值对</param>
        /// <returns></returns>

        public string HttpGet(string url, List<KeyValuePair<string, string>> formData = null)

        {

            HttpClient httpClient = new HttpClient();
            //HttpContent content = new FormUrlEncodedContent(formData);

            //if (formData != null)
            //{
            //    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            //    content.Headers.ContentType.CharSet = "UTF-8";
            //    for (int i = 0; i < formData.Count; i++)
            //    {
            //        content.Headers.Add(formData[i].Key, formData[i].Value);
            //    }
            //}

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };

            //for (int i = 0; i < formData.Count; i++)
            //{
            //    request.Headers.Add(formData[i].Key, formData[i].Value);
            //}

            var res = httpClient.SendAsync(request);
            res.Wait();
            var resp = res.Result;

            Task<string> temp = resp.Content.ReadAsStringAsync();

            temp.Wait();

            return temp.Result;
        }

        /// <summary>
        /// 同步请求post（键值对形式）
        /// </summary>
        /// <param name="uri">网络基址("http://localhost:59315")</param>
        /// <param name="url">网络的地址("/api/UMeng")</param>
        /// <param name="formData">键值对List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>();formData.Add(new KeyValuePair<string, string>("userid", "29122"));formData.Add(new KeyValuePair<string, string>("umengids", "29122"));</param>
        /// <param name="charset">编码格式</param>
        /// <param name="mediaType">头媒体类型</param>
        /// <returns></returns>
        public string HttpPost(string uri, string url, List<KeyValuePair<string, string>> formData = null, string charset = "UTF-8", string mediaType = "application/x-www-form-urlencoded")
        {
            string tokenUri = url;
            var client = new HttpClient();
            client.BaseAddress = new Uri(uri);
            HttpContent content = new FormUrlEncodedContent(formData);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            content.Headers.ContentType.CharSet = charset;

            for (int i = 0; i < formData.Count; i++)
            {
                content.Headers.Add(formData[i].Key, formData[i].Value);
            }

            var res = client.PostAsync(tokenUri, content);
            res.Wait();
            HttpResponseMessage resp = res.Result;

            var res2 = resp.Content.ReadAsStringAsync();
            res2.Wait();

            string token = res2.Result;
            return token;
        }

        /// <summary>
        /// 异步请求get(UTF-8)
        /// </summary>
        /// <param name="url">链接地址</param>    
        /// <param name="formData">写在header中的内容</param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, List<KeyValuePair<string, string>> formData = null)
        {

            HttpClient httpClient = new HttpClient();

            //HttpContent content = new FormUrlEncodedContent(formData);

            //if (formData != null)
            //{
            //    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            //    content.Headers.ContentType.CharSet = "UTF-8";

            //    for (int i = 0; i < formData.Count; i++)
            //    {
            //        content.Headers.Add(formData[i].Key, formData[i].Value);
            //    }
            //}

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };

            //for (int i = 0; i < formData.Count; i++)
            //{
            //    request.Headers.Add(formData[i].Key, formData[i].Value);
            //}

            var resp = await httpClient.SendAsync(request);
            resp.EnsureSuccessStatusCode();

            string token = await resp.Content.ReadAsStringAsync();

            return token;
        }

        /// <summary>
        /// 异步请求post（键值对形式,可等待的）
        /// </summary>
        /// <param name="uri">网络基址("http://localhost:59315")</param>
        /// <param name="url">网络的地址("/api/UMeng")</param>
        /// <param name="formData">键值对List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>();formData.Add(new KeyValuePair<string, string>("userid", "29122"));formData.Add(new KeyValuePair<string, string>("umengids", "29122"));</param>
        /// <param name="charset">编码格式</param>
        /// <param name="mediaType">头媒体类型</param>
        /// <returns></returns>
        public async Task<string> HttpPostAsync(string uri, string url, List<KeyValuePair<string, string>> formData = null, string charset = "UTF-8", string mediaType = "application/x-www-form-urlencoded")
        {
            string tokenUri = url;
            var client = new HttpClient();
            client.BaseAddress = new Uri(uri);
            HttpContent content = new FormUrlEncodedContent(formData);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            content.Headers.ContentType.CharSet = charset;

            for (int i = 0; i < formData.Count; i++)
            {
                content.Headers.Add(formData[i].Key, formData[i].Value);
            }

            HttpResponseMessage resp = await client.PostAsync(tokenUri, content);
            resp.EnsureSuccessStatusCode();
            string token = await resp.Content.ReadAsStringAsync();

            return token;
        }

        //流模式post
        public string Post(string url, string data, Encoding encoding, int type = 3)
        {
            HttpWebRequest req = null;
            HttpWebResponse rsp = null;
            Stream reqStream = null;
            //Stream resStream = null;

            try
            {
                req = WebRequest.CreateHttp(new Uri(url));
                if (type == 1)
                {
                    req.ContentType = "application/json;charset=utf-8";
                }
                else if (type == 2)
                {
                    req.ContentType = "application/xml;charset=utf-8";
                }
                else
                {
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                }

                req.Method = "POST";
                //req.Accept = "text/xml,text/javascript";
                req.ContinueTimeout = 60000;

                byte[] postData = encoding.GetBytes(data);
                reqStream = req.GetRequestStreamAsync().Result;
                reqStream.Write(postData, 0, postData.Length);
                //reqStream.Dispose();

                rsp = (HttpWebResponse)req.GetResponseAsync().Result;
                string result = GetResponseAsString(rsp, encoding);              

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // 释放资源
                if (reqStream != null)
                {
                    reqStream.Close();
                    reqStream = null;
                }
                if (rsp != null)
                {
                    rsp.Close();
                    rsp = null;
                }
                if (req != null)
                {
                    req.Abort();

                    req = null;
                }
            }
        }

        private string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);

                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null)
                    reader.Close();
                if (stream != null)
                    stream.Close();

                reader = null;
                stream = null;

            }
        }

        public static string MakeRpcUrlPost(string url, string method, out byte[] data, params MyJson.IJsonNode[] _params)
        {
            var json = new MyJson.JsonNode_Object();
            json["id"] = new MyJson.JsonNode_ValueNumber(1);
            json["jsonrpc"] = new MyJson.JsonNode_ValueString("2.0");
            json["method"] = new MyJson.JsonNode_ValueString(method);
            StringBuilder sb = new StringBuilder();
            var array = new MyJson.JsonNode_Array();
            for (var i = 0; i < _params.Length; i++)
            {
                array.Add(_params[i]);
            }
            json["params"] = array;
            data = System.Text.Encoding.UTF8.GetBytes(json.ToString());
            return url;
        }

        public static string MakeRpcUrl(string url, string method, params MyJson.IJsonNode[] _params)
        {
            StringBuilder sb = new StringBuilder();
            if (url.Last() != '/')
            {
                url = url + "/";
            }
            sb.Append(url + "?jsonrpc=2.0&id=1&method=" + method + "&params=[");
            for (var i = 0; i < _params.Length; i++)
            {
                _params[i].ConvertToString(sb);
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
            WebClient wc = new WebClient();
            return await wc.DownloadStringTaskAsync(url);
        }

        public static async Task<string> HttpPost(string url, byte[] data)
        {
            WebClient wc = new WebClient();
            wc.Headers["content-type"] = "text/plain;charset=UTF-8";
            byte[] retdata = await wc.UploadDataTaskAsync(url, "POST", data);
            return System.Text.Encoding.UTF8.GetString(retdata);
        }
    }
}
