using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using Zoro_Web_API.RPC;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Timers;

namespace Zoro_Web_API.lib
{
    public class mySqlHelper
    {
        public static string conf = "";
        private int height = 0;
        private static ConcurrentDictionary<string, JArray> jArrays = new ConcurrentDictionary<string, JArray>();
        private string rootChain = "0000000000000000000000000000000000000000";

        public mySqlHelper()
        {
            Timer timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(getHeightChange);
        }

        private void getHeightChange(object a, ElapsedEventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string select = "SELECT chainheight from chainlistheight where chainhash='0x0000000000000000000000000000000000000000'";
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                int i = 0;
                while (rdr.Read())
                {
                    string index = rdr["chainheight"].ToString();
                    i = int.Parse(index);
                }
                if (i != height)
                {
                    height = i;
                    jArrays.Clear();
                }
                conn.Clone();
            }
        }

        public JArray GetAddress(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                var addr = req.@params[0].ToString();

                string select = "select firstuse , lastuse , txcount from  address_" + rootChain + " where addr = @addr";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                cmd.Parameters.AddWithValue("@addr", addr);

                JArray bk = new JArray();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var adata = (rdr["firstuse"]).ToString();
                    var ldata = (rdr["lastuse"]).ToString();
                    var tdata = (rdr["txcount"]).ToString();

                    bk.Add(new JObject { { "firstuse", adata }, { "lastuse", ldata }, { "txcount", tdata } });
                }
                return res.result = bk;
            }
        }

        public JArray GetAppChainAddress(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                var addr = req.@params[1].ToString();

                string select = "select firstuse , lastuse , txcount from  address_" + req.@params[0] + " where addr = @addr";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                cmd.Parameters.AddWithValue("@addr", addr);

                JArray bk = new JArray();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var adata = (rdr["firstuse"]).ToString();
                    var ldata = (rdr["lastuse"]).ToString();
                    var tdata = (rdr["txcount"]).ToString();

                    bk.Add(new JObject { { "firstuse", adata }, { "lastuse", ldata }, { "txcount", tdata } });
                }
                return res.result = bk;

            }
        }

        public JArray GetAddrs(JsonRPCrequest req)
        {
            JArray bk = new JArray();

            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select addr, firstuse, lastuse, txcount from address_" + rootChain + " limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0]; // string select = "select a.addr, a.firstuse,a.lastuse, a.txcount, b.blockindex ,b.blocktime ,b.txid from address_0000000000000000000000000000000000000000 as a limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var adata = (rdr["addr"]).ToString();
                    var f = (rdr["firstuse"]).ToString();
                    var lu = (rdr["lastuse"]).ToString();
                    var txc = (rdr["txcount"]).ToString();

                    bk.Add(new JObject { { "addr", adata }, { "firstDate", f }, { "lastDate", lu }, { "txcount", txc } });
                }

                return res.result = bk;
            }

        }

        public JArray GetAppChainAddrs(JsonRPCrequest req)
        {
            JArray bk = new JArray();

            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select addr, firstuse, lastuse, txcount from address_" + req.@params[0] + " limit " + (int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + req.@params[1]; 

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var adata = (rdr["addr"]).ToString();
                    var f = (rdr["firstuse"]).ToString();
                    var lu = (rdr["lastuse"]).ToString();
                    var txc = (rdr["txcount"]).ToString();

                    bk.Add(new JObject { { "addr", adata }, { "firstDate", f }, { "lastDate", lu }, { "txcount", txc } });
                }
                return res.result = bk;

            }

        }

        public JArray GetAddr(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select addr, firstuse, lastuse, txcount from address_" + rootChain + " where addr='" + req.@params[0] + "'"; // + "' and a.firstuse = b.blockindex";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {

                    var adata = (rdr["addr"]).ToString();
                    var f = (rdr["firstuse"]).ToString();
                    var lu = (rdr["lastuse"]).ToString();
                    var txc = (rdr["txcount"]).ToString();

                    bk.Add(new JObject { { "addr", adata }, { "firstDate", f }, { "lastDate", lu }, { "txcount", txc } });

                }
                return res.result = bk;

            }
        }

        public JArray GetAppChainAddr(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string select = "select addr, firstuse, lastuse, txcount from address_" + req.@params[0] + " where addr='" + req.@params[1] + "'"; // + "' and a.firstuse = b.blockindex";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {

                    var adata = (rdr["addr"]).ToString();
                    var f = (rdr["firstuse"]).ToString();
                    var lu = (rdr["lastuse"]).ToString();
                    var txc = (rdr["txcount"]).ToString();

                    bk.Add(new JObject { { "addr", adata }, { "firstDate", f }, { "lastDate", lu }, { "txcount", txc } });
                }
                return res.result = bk;

            }
        }

        public JArray GetAddressNep5Txs(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                var chainHash = req.@params[0].ToString();
                if (string.IsNullOrEmpty(chainHash) || chainHash == " ") chainHash = rootChain;
                var addr = req.@params[1].ToString();
                string select = "select blockindex,txid,asset,symbol,decimals,value,fromx,tox from nep5transfer_" + chainHash + " tra left join nep5asset_" + chainHash + " nep on tra.asset = nep.assetid where fromx = @addr or tox = @addr order by blockindex desc limit " + (int.Parse(req.@params[2].ToString()) * int.Parse(req.@params[3].ToString())) + ", " + req.@params[2];

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                cmd.Parameters.AddWithValue("@addr", addr);

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var fromx = rdr["fromx"].ToString();
                    var tox = rdr["tox"].ToString();
                    var value = rdr["value"].ToString();
                    var decimals = rdr["decimals"].ToString();

                    Decimal amount = Decimal.Parse(value) / new Decimal(Math.Pow(10, int.Parse(decimals)));
                    if (fromx == addr)
                        value = "-" + amount;
                    else if (tox == addr)
                        value = "+" + amount;                                        

                    bk.Add(new JObject
                    {
                        { "addr", addr },
                        { "txid", rdr["txid"].ToString() },
                        { "blockindex", rdr["blockindex"].ToString() },
                        { "asset", rdr["asset"].ToString()},
                        { "symbol", rdr["symbol"].ToString()},                       
                        { "value", value},
                    });
                }
                return res.result = bk;
            }
        }

        public JArray GetAddressAsset(JsonRPCrequest req)
        {
            var chainHash = req.@params[0].ToString();
            if (string.IsNullOrEmpty(chainHash) || chainHash == " ") chainHash = rootChain;

            JArray jArray = new JArray();
           
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select asset, type from address_asset_" + chainHash + " where addr='" + req.@params[1] + "'";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                JArray bk = new JArray();

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var asset = (rdr["asset"]).ToString();
                    var type = rdr["type"].ToString();

                    bk.Add(new JObject { { "asset", asset }, { "type", type } });
                }               
                return res.result = bk;
            }
        }

        public JArray GetAddressTxs(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                var addr = req.@params[0].ToString();

                string select = "select txid,addr,blocktime,blockindex from address_tx_" + rootChain + " where @addr = addr order by blockindex desc limit " + (int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + req.@params[1];

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                cmd.Parameters.AddWithValue("@addr", addr);

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var adata = (rdr["txid"]).ToString();
                    var vdata = (rdr["addr"]).ToString();
                    var bt = (rdr["blocktime"]).ToString();
                    var bi = (rdr["blockindex"]).ToString();
                    bk.Add(new JObject { { "addr", vdata }, { "txid", adata }, { "blockindex", bi }, { "blocktime", bt } });
                }
                return res.result = bk;

            }
        }

        public JArray GetAppChainAddressTxs(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                var addr = req.@params[1].ToString();

                string select = "select txid,addr,blocktime,blockindex from address_tx_" + req.@params[0].ToString() + " where @addr = addr order by blockindex desc limit " + (int.Parse(req.@params[2].ToString()) * int.Parse(req.@params[3].ToString())) + ", " + req.@params[2];

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                cmd.Parameters.AddWithValue("@addr", addr);

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var adata = (rdr["txid"]).ToString();
                    var vdata = (rdr["addr"]).ToString();
                    var bt = (rdr["blocktime"]).ToString();
                    var bi = (rdr["blockindex"]).ToString();
                    bk.Add(new JObject { { "addr", vdata }, { "txid", adata }, { "blockindex", bi }, { "blocktime", bt } });
                }
                return res.result = bk;
            }
        }

        public JArray GetTxCount(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("txcount", out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                //var addr = req.@params[0].ToString();
                if (req.@params[0].ToString() == "")
                {
                    string select = "select count(*) from tx_" + rootChain + " where type='InvocationTransaction'"; // inherently belongs to all appchains tx count

                    JsonPRCresponse res = new JsonPRCresponse();
                    MySqlCommand cmd = new MySqlCommand(select, conn);

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var adata = (rdr["count(*)"]).ToString();

                        JArray bk = new JArray { new JObject { { "txcount", adata } } };

                        res.result = bk;
                    }
                    jArrays.AddOrUpdate("txcount", res.result, (s, a) => { return a; });
                    return res.result;
                }
                else
                {
                    string select = "select count(*) from tx where type='" + req.@params[0] + "'";

                    JsonPRCresponse res = new JsonPRCresponse();
                    MySqlCommand cmd = new MySqlCommand(select, conn);

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var adata = (rdr["count(*)"]).ToString();

                        JArray bk = new JArray { new JObject { { "txcount", adata } } };

                        res.result = bk;
                    }

                    return res.result;
                }

            }
        }        

        public JArray GetAppchainTxCount(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("appchaintxcount" + req.@params[0], out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                //var addr = req.@params[0].ToString();		
                string select = "select count(*) from tx_" + req.@params[0] + " where type='InvocationTransaction'"; //where type= + req.@params[0] + "'";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var adata = (rdr["count(*)"]).ToString();

                    JArray bk = new JArray { new JObject { { "txcount", adata } } };

                    res.result = bk;
                }
                jArrays.AddOrUpdate("appchaintxcount" + req.@params[0], res.result, (s, a) => { return a; });
                return res.result;
            }
        }

        public JArray GetRankByAssetCount(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select count(*) from nep5asset_" + rootChain + " where id='" + req.@params[0].ToString() + "'";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var adata = (rdr["count(*)"]).ToString();

                    JArray bk = new JArray { new JObject { { "count", adata } } };

                    res.result = bk;
                }
                return res.result;

            }
        }

        public JArray GetAppChainRankByAssetCount(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select count(*) from nep5asset_" + req.@params[0] + " where id='" + req.@params[1].ToString() + "'";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var adata = (rdr["count(*)"]).ToString();

                    JArray bk = new JArray { new JObject { { "count", adata } } };

                    res.result = bk;
                }
                return res.result;

            }
        }

        public JArray GetRankByAsset(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select id, totalsupply , decimals from nep5asset_" + rootChain + " where id='" + req.@params[0] + "'";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                JArray bk = new JArray();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var adata = (rdr["id"]).ToString();
                    var bl = (rdr["amount"]).ToString();

                    var ts = (rdr["totalsupply"]).ToString();
                    var de = (rdr["amount"]).ToString();

                    var ad = (rdr["admin"]).ToString();

                    bk.Add(new JObject { { "asset", adata }, { "balance", bl }, { "addr", ad } });

                }
                return res.result = bk;

            }
        }

        public JArray GetAppChainRankByAsset(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string select = "select id, totalsupply , decimals from nep5asset_" + req.@params[0] + " where id='" + req.@params[1] + "'";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                JArray bk = new JArray();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var adata = (rdr["id"]).ToString();
                    var bl = (rdr["amount"]).ToString();

                    var ts = (rdr["totalsupply"]).ToString();
                    var de = (rdr["amount"]).ToString();

                    var ad = (rdr["admin"]).ToString();

                    bk.Add(new JObject { { "asset", adata }, { "balance", bl }, { "addr", ad } });

                }
                return res.result = bk;

            }
        }

        public JArray GetAddrCount(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("addrcount", out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select count(*) from address_" + rootChain;

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var adata = (rdr["count(*)"]).ToString();

                    JArray bk = new JArray { new JObject { { "addrcount", adata } } };

                    res.result = bk;
                }
                jArrays.AddOrUpdate("addrcount", res.result, (s, a) => { return a; });
                return res.result;

            }
        }

        public JArray GetAppchainAddrCount(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("appchainaddrcount" + req.@params[0], out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select count(*) from address_" + req.@params[0];

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var adata = (rdr["count(*)"]).ToString();

                    JArray bk = new JArray { new JObject { { "addrcount", adata } } };

                    res.result = bk;
                }
                jArrays.AddOrUpdate("appchainaddrcount" + req.@params[0], res.result, (s, a) => { return a; });
                return res.result;

            }
        }
        public async Task<JArray> GetBalanceAsync(JsonRPCrequest req) // needs to be changed for the right balance data
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("balance" + req.@params[0], out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select asset, type from address_asset_" + rootChain + " where addr='" + req.@params[0] + "'";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                JArray bk = new JArray();

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var asset = (rdr["asset"]).ToString();
                    var type = rdr["type"].ToString();
                    var jObject = new JObject();
                    if (type == "NativeNep5")
                    {
                        jObject = await InvokeHelper.getNativeBalanceOfAsync(rootChain, asset, req.@params[0].ToString());
                    }
                    else
                    {
                        jObject = await InvokeHelper.getBalanceOfAsync(rootChain, asset, req.@params[0].ToString());
                    }
                    bk.Add(jObject);

                }
                jArrays.AddOrUpdate("balance" + req.@params[0], bk, (s, a) => { return a; });
                return res.result = bk;
            }
        }

        public async Task<JArray> GetAppChainBalanceAsync(JsonRPCrequest req) // needs to be changed for the right balance data
        {
            var chainHash = req.@params[0].ToString();
            if (string.IsNullOrEmpty(chainHash) || chainHash == " ") chainHash = rootChain;

            JArray jArray = new JArray();
            if (jArrays.TryGetValue("appchainbalance" + chainHash + req.@params[1], out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select asset, type from address_asset_" + chainHash + " where addr='" + req.@params[1] + "'";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                JArray bk = new JArray();

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var asset = (rdr["asset"]).ToString();
                    var type = rdr["type"].ToString();
                    var jObject = new JObject();
                    if (type == "NativeNep5")
                    {
                        jObject = await InvokeHelper.getNativeBalanceOfAsync(chainHash, asset, req.@params[1].ToString());
                    }
                    else
                    {
                        jObject = await InvokeHelper.getBalanceOfAsync(chainHash, asset, req.@params[1].ToString());
                    }

                    jObject.Add("assetid", asset);
                    jObject.Add("type", type);

                    bk.Add(jObject);

                }

                jArrays.AddOrUpdate("appchainbalance" + chainHash + req.@params[1], bk, (s, a) => { return a; });
                return res.result = bk;
            }
        }

        public JArray GetAsset(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select version , id , type , name , amount , available , pprecision , owner , admin, issuer , expiration , frozen  from  asset where id='" + req.@params[0] + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JArray bk = new JArray();
                JsonPRCresponse res = new JsonPRCresponse();
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var adata = (rdr["version"]).ToString();
                    var idata = (rdr["id"]).ToString();
                    var tdata = (rdr["type"]).ToString();
                    var ndata = (rdr["name"]).ToString();
                    var xdata = (rdr["amount"]).ToString();
                    var mdata = (rdr["available"]).ToString();
                    var pdata = (rdr["pprecision"]).ToString();
                    var odata = (rdr["owner"]).ToString();
                    var fdata = (rdr["admin"]).ToString();
                    var qdata = (rdr["issuer"]).ToString();
                    var rdata = (rdr["expiration"]).ToString();
                    var wdata = (rdr["frozen"]).ToString();

                    bk.Add(new JObject { { "version", adata }, { "type", tdata }, { "name", JArray.Parse(ndata) }, { "amount", xdata }, { "precision", pdata }, { "available", xdata }, { "owner", odata }, { "admin", fdata }, { "id", adata } });

                }
                return res.result = bk;

            }
        }

        public JArray GetAllAppchains(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("allappchains", out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select version , hash ,name ,owner, timestamp , seedlist , validators from appchainstate"; 

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var tdata = (rdr["version"]).ToString();
                    var ndata = (rdr["hash"]).ToString();
                    var mdata = (rdr["name"]).ToString();
                    var pdata = (rdr["owner"]).ToString();
                    var xdata = (rdr["timestamp"]).ToString();
                    var odata = (rdr["seedlist"]).ToString();
                    var o = (rdr["validators"]).ToString();

                    bk.Add(new JObject { { "version", tdata }, { "hash", ndata }, { "name", mdata }, { "owner", pdata }, { "timestamp", xdata }, { "seedlist", JArray.Parse(odata) }, { "validators", JArray.Parse(o) } });
                }
                jArrays.AddOrUpdate("allappchains", bk, (s, a) => { return a; });
                return res.result = bk;
            }
        }

        public JArray GetAppchain(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("appchain" + req.@params[0], out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select version , hash , name , owner, timestamp , seedlist , validators from appchainstate where hash = '" + "0x" + req.@params[0] + "'"; 

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                List<JObject> vals = new List<JObject>();
                JArray array = new JArray();
                while (rdr.Read())
                {
                    var tdata = (rdr["version"]).ToString();
                    var ndata = (rdr["hash"]).ToString();
                    var mdata = (rdr["name"]).ToString();
                    var pdata = (rdr["owner"]).ToString();
                    var xdata = (rdr["timestamp"]).ToString();
                    var odata = (rdr["seedlist"]).ToString();
                    var o = (rdr["validators"]).ToString();
                    
                    bk.Add(new JObject { { "version", tdata }, { "hash", ndata }, { "name", mdata }, { "owner", pdata }, { "timestamp", xdata }, { "seedlist", JArray.Parse(odata) }, { "validators", JArray.Parse(o) } });
                }
                jArrays.AddOrUpdate("appchain" + req.@params[0], bk, (s, a) => { return a; });
                return res.result = bk;

            }
        }
        public JArray GetHashlist(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("hashlist", out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select hashlist from hashlist";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var tdata = (rdr["hashlist"]).ToString();

                    bk.Add(new JObject { { "hashlist", tdata } });
                }
                jArrays.AddOrUpdate("hashlist", bk, (s, a) => { return a; });
                return res.result = bk;

            }
        }

        public JArray GetBlock(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select hash, size , version , previousblockhash , merkleroot , time , indexx , nonce , nextconsensus , script ,tx  from  block_" + rootChain + "  where indexx='" + req.@params[0] + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var hash = (rdr["hash"]).ToString();
                    var sdata = (rdr["size"]).ToString();
                    var adata = (rdr["version"]).ToString();
                    var ind = (rdr["indexx"]).ToString();
                    var pdata = (rdr["previousblockhash"]).ToString();
                    var mdata = (rdr["merkleroot"]).ToString();
                    var tdata = (rdr["time"]).ToString();
                    var ndata = (rdr["nonce"]).ToString();
                    var nc = (rdr["nextconsensus"]).ToString();

                    var s = (rdr["script"]).ToString();
                    var tx = (rdr["tx"]).ToString();

                    bk.Add(new JObject { { "hash", hash }, { "size", sdata }, { "version", adata }, { "previousblockhash", pdata }, { "merkleroot", mdata }, { "time", tdata }, { "index", ind }, { "nonce", ndata }, { "nextconsensus", nc }, { "script", JObject.Parse(s) }, { "tx", JArray.Parse(tx) } });
                }
                return res.result = bk;
            }
        }

        public JArray GetAppChainBlock(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select hash, size , version , previousblockhash , merkleroot , time , indexx , nonce , nextconsensus , script ,tx  from  block_" + req.@params[0] + " where indexx = '" + req.@params[1] + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var hash = (rdr["hash"]).ToString();
                    var sdata = (rdr["size"]).ToString();
                    var adata = (rdr["version"]).ToString();
                    var ind = (rdr["indexx"]).ToString();
                    var pdata = (rdr["previousblockhash"]).ToString();
                    var mdata = (rdr["merkleroot"]).ToString();
                    var tdata = (rdr["time"]).ToString();
                    var ndata = (rdr["nonce"]).ToString();
                    var nc = (rdr["nextconsensus"]).ToString();

                    var s = (rdr["script"]).ToString();
                    var tx = (rdr["tx"]).ToString();

                    bk.Add(new JObject { { "hash", hash }, { "size", sdata }, { "version", adata }, { "previousblockhash", pdata }, { "merkleroot", mdata }, { "time", tdata }, { "index", ind }, { "nonce", ndata }, { "nextconsensus", nc }, { "script", JObject.Parse(s) }, { "tx", JArray.Parse(tx) } });
                }
                return res.result = bk;
            }
        }


        public JArray GetBlocks(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("blocks", out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select  size , hash , time , indexx, txcount from block_" + rootChain + " limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var sdata = (rdr["size"]).ToString();
                    var hash = (rdr["hash"]).ToString();
                    var ind = (rdr["indexx"]).ToString();
                    var tdata = (rdr["time"]).ToString();
                    var txcount = (rdr["txcount"]).ToString();

                    bk.Add(new JObject { { "size", sdata }, { "hash", hash }, { "index", ind }, { "time", tdata }, { "txcount", txcount } });
                }
                jArrays.AddOrUpdate("blocks", bk, (s, a) => { return a; });
                return res.result = bk;
            }
        }

        public JArray GetAppchainBlocks(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("appchainblocks" + req.@params[0], out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select size , hash , time , indexx, txcount from block_" + req.@params[0] + " limit " + (int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + int.Parse(req.@params[1].ToString());

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var sdata = (rdr["size"]).ToString();
                    var hash = (rdr["hash"]).ToString();
                    var ind = (rdr["indexx"]).ToString();
                    var tdata = (rdr["time"]).ToString();
                    var txcount = (rdr["txcount"]).ToString();

                    bk.Add(new JObject { { "size", sdata }, { "hash", hash }, { "index", ind }, { "time", tdata }, { "txcount", txcount } });
                }
                jArrays.AddOrUpdate("appchainblocks" + req.@params[0], bk, (s, a) => { return a; });
                return res.result = bk;

            }
        }

        public JArray GetBlocksDESC(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select size , hash , time , indexx, txcount  from block_" + rootChain + " ORDER BY id DESC limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var sdata = (rdr["size"]).ToString();
                    var hash = (rdr["hash"]).ToString();
                    var ind = (rdr["indexx"]).ToString();
                    var tdata = (rdr["time"]).ToString();
                    var txcount = (rdr["txcount"]).ToString();

                    bk.Add(new JObject { { "size", sdata }, { "hash", hash }, { "index", ind }, { "time", tdata }, { "txcount", txcount } });
                }
                return res.result = bk;
            }
        }

        public JArray GetAppchainBlocksDESC(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select  size , hash , time , indexx, txcount from block_" + req.@params[0] + " ORDER BY id DESC limit " + (int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + int.Parse(req.@params[1].ToString());

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var sdata = (rdr["size"]).ToString();
                    var hash = (rdr["hash"]).ToString();
                    var ind = (rdr["indexx"]).ToString();
                    var tdata = (rdr["time"]).ToString();
                    var txcount = (rdr["txcount"]).ToString();

                    bk.Add(new JObject { { "size", sdata }, { "hash", hash }, { "index", ind }, { "time", tdata }, { "txcount", txcount } });
                }
                return res.result = bk;
            }
        }

        public JArray GetBlocksDESCCache(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("blocksdesc", out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select size , hash , time , indexx, txcount  from block_" + rootChain + " ORDER BY id DESC limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var sdata = (rdr["size"]).ToString();
                    var hash = (rdr["hash"]).ToString();
                    var ind = (rdr["indexx"]).ToString();
                    var tdata = (rdr["time"]).ToString();
                    var txcount = (rdr["txcount"]).ToString();

                    bk.Add(new JObject { { "size", sdata }, { "hash", hash }, { "index", ind }, { "time", tdata }, { "txcount", txcount }, { "nowtime", (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds } });
                }
                jArrays.AddOrUpdate("blocksdesc", bk, (s, a) => { return a; });
                return res.result = bk;
            }
        }


        public JArray GetAppchainBlocksDESCCache(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("appchainblocksdesc" + req.@params[0], out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select  size , hash , time , indexx, txcount from block_" + req.@params[0] + " ORDER BY id DESC limit " + (int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + int.Parse(req.@params[1].ToString());

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var sdata = (rdr["size"]).ToString();
                    var hash = (rdr["hash"]).ToString();
                    var ind = (rdr["indexx"]).ToString();
                    var tdata = (rdr["time"]).ToString();
                    var txcount = (rdr["txcount"]).ToString();

                    bk.Add(new JObject { { "size", sdata }, { "hash", hash }, { "index", ind }, { "time", tdata }, { "txcount", txcount } });
                }
                jArrays.AddOrUpdate("appchainblocksdesc" + req.@params[0], bk, (s, a) => { return a; });
                return res.result = bk;
            }
        }

        public JArray GetNep5Asset(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = " select assetid , totalsupply , name , symbol , decimals from nep5asset_" + rootChain + " where assetid = '" + req.@params[0] + "'";
                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var adata = (rdr["assetid"]).ToString();

                    var name = (rdr["name"]).ToString();

                    var ts = (rdr["totalsupply"]).ToString();

                    var sb = (rdr["symbol"]).ToString();

                    var cd = (rdr["decimals"]).ToString();

                    bk.Add(new JObject { { "assetid", adata }, { "totalsupply", ts }, { "name", name }, { "symbol", sb }, { "decimals", cd } });
                }
                return res.result = bk;
            }
        }

        public JArray GetAppChainNep5Asset(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = " select assetid , totalsupply , name , symbol , decimals from nep5asset_" + req.@params[0] + " where assetid = '" + req.@params[1] + "'";
                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var adata = (rdr["assetid"]).ToString();

                    var name = (rdr["name"]).ToString();

                    var ts = (rdr["totalsupply"]).ToString();

                    var sb = (rdr["symbol"]).ToString();

                    var cd = (rdr["decimals"]).ToString();

                    bk.Add(new JObject { { "assetid", adata }, { "totalsupply", ts }, { "name", name }, { "symbol", sb }, { "decimals", cd } });
                }
                return res.result = bk;
            }
        }

        public JArray GetAllNep5Asset(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("allnep5asset", out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select assetid , totalsupply , name ,symbol ,decimals  from nep5asset_" + rootChain;

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var adata = (rdr["assetid"]).ToString();

                    var name = (rdr["name"]).ToString();

                    var ts = (rdr["totalsupply"]).ToString();

                    var sb = (rdr["symbol"]).ToString();

                    var cd = (rdr["decimals"]).ToString();

                    var num = (decimal)(ulong.Parse(ts) / Math.Pow(10, int.Parse(cd)));
                    ts = Convert.ToDouble(num.ToString()).ToString("f8");

                    bk.Add(new JObject { { "assetid", adata }, { "totalsupply", ts }, { "name", name }, { "symbol", sb }, { "decimals", cd } });

                }
                jArrays.AddOrUpdate("allnep5asset", bk, (s, a) => { return a; });
                return res.result = bk;
            }

        }

        public JArray GetNep5AssetByAddress(string chainHash, string address)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string select = "";
                if (chainHash == "")
                    select = "select asset, type from address_asset_" + rootChain + " where addr='" + address + "'";
                else
                    select = "select asset, type from address_asset_" + chainHash + " where addr='" + address + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var assetid = (rdr["asset"]).ToString();
                    var type = (rdr["type"]).ToString();

                    bk.Add(new JObject { { "assetid", assetid }, { "type", type } });

                }
                return res.result = bk;

            }
        }

        public JArray GetAllNep5AssetByChainHash(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("allnep5assetbychainhash" + req.@params[0], out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select assetid , totalsupply , name ,symbol ,decimals  from nep5asset_" + req.@params[0];

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var adata = (rdr["assetid"]).ToString();

                    var name = (rdr["name"]).ToString();

                    var ts = (rdr["totalsupply"]).ToString();

                    var sb = (rdr["symbol"]).ToString();

                    var cd = (rdr["decimals"]).ToString();

                    var num = (decimal)(ulong.Parse(ts) / Math.Pow(10, int.Parse(cd)));
                    ts = Convert.ToDouble(num.ToString()).ToString("f8");

                    bk.Add(new JObject { { "assetid", adata }, { "totalsupply", ts }, { "name", name }, { "symbol", sb }, { "decimals", cd } });

                }
                jArrays.AddOrUpdate("allnep5assetbychainhash" + req.@params[0], bk, (s, a) => { return a; });
                return res.result = bk;

            }
        }

        public JArray GetNep5TranferFromToAddress(string chainHash, string toAddress)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "";
                if (chainHash == "")
                {
                    select = "select  a.id as id, a.asset as asset, a.fromx as fromx, a.tox as tox, a.value as value, b.decimals as decimals from nep5transfer_" + rootChain + " as a, nep5asset_" + rootChain + " as b where a.tox = '" + toAddress + "' and a.asset=b.assetid";
                }
                else
                {
                    select = "select  a.id as id, a.asset as asset, a.fromx as fromx, a.tox as tox, a.value as value, b.decimals as decimals from nep5transfer_" + chainHash + " as a, nep5asset_" + chainHash + " as b where a.tox = '" + toAddress + "' and a.asset=b.assetid";
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var decimals = int.Parse(rdr["decimals"].ToString());
                    var idata = (rdr["id"]).ToString();
                    var adata = (rdr["asset"]).ToString();
                    var fdata = (rdr["fromx"]).ToString();
                    var tdata = (rdr["tox"]).ToString();
                    var vdata = (rdr["value"]).ToString();
                    var num = (decimal)(ulong.Parse(vdata) / Math.Pow(10, decimals));
                    vdata = num.ToString();

                    bk.Add(new JObject { { "id", idata }, { "asset", adata }, { "from", fdata }, { "to", tdata }, { "value", vdata } });
                }
                return res.result;
            }
        }

        public JArray GetScriptMethodAsync(string chainhash, string txid)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string select = "select tx.calltype, tx.method, tx.contract,cs.supportstandard supportstandard from tx_script_method_" + chainhash + " tx left join contract_state_" + chainhash + " cs on tx.contract = cs.hash where txid = '" + txid + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var calltype = (rdr["calltype"]).ToString();
                    var method = (rdr["method"]).ToString();
                    var contract = (rdr["contract"]).ToString();
                    var supportstandard = (rdr["supportstandard"].ToString());

                    var name = getName(chainhash, contract);

                    bk.Add(new JObject { { "calltype", calltype }, { "method", method }, { "contract", contract }, { "name", name }, { "supportstandard", supportstandard } });
                }

                return res.result = bk;

            }
        }

        public JArray GetNep5Transfer(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("nep5transfer", out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string select = "select  a.id as id, a.asset as asset, a.fromx as fromx, a.tox as tox, a.value as value, b.decimals as decimals from nep5transfer_" + rootChain + " as a, nep5asset_" + rootChain + " as b where a.asset=b.assetid";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var decimals = int.Parse(rdr["decimals"].ToString());
                    var idata = (rdr["id"]).ToString();
                    var adata = (rdr["asset"]).ToString();
                    var fdata = (rdr["fromx"]).ToString();
                    var tdata = (rdr["tox"]).ToString();
                    var vdata = (rdr["value"]).ToString();
                    var num = (decimal)(ulong.Parse(vdata) / Math.Pow(10, decimals));
                    vdata = num.ToString();

                    bk.Add(new JObject { { "id", idata }, { "asset", adata }, { "from", fdata }, { "to", tdata }, { "value", vdata } });
                }
                jArrays.AddOrUpdate("nep5transfer", bk, (s, a) => { return a; });
                return res.result = bk;

            }
        }

        public JArray GetAppChainNep5Transfer(JsonRPCrequest req)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("nep5transfer" + req.@params[0], out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string txid = req.@params[1].ToString();
                if (!txid.StartsWith("0x"))
                {
                    txid = "0x" + txid;
                }

                string select = "select  a.id as id, a.asset as asset, a.fromx as fromx, a.tox as tox, a.value as value, b.decimals as decimals from nep5transfer_" + req.@params[0] + " as a, nep5asset_" + req.@params[0] + " as b where a.asset=b.assetid";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var decimals = int.Parse(rdr["decimals"].ToString());
                    var idata = (rdr["id"]).ToString();
                    var adata = (rdr["asset"]).ToString();
                    var fdata = (rdr["fromx"]).ToString();
                    var tdata = (rdr["tox"]).ToString();
                    var vdata = (rdr["value"]).ToString();
                    var num = (decimal)(ulong.Parse(vdata) / Math.Pow(10, decimals));
                    vdata = num.ToString();

                    bk.Add(new JObject { { "id", idata }, { "asset", adata }, { "from", fdata }, { "to", tdata }, { "value", vdata } });
                }
                jArrays.AddOrUpdate("nep5transfer" + req.@params[0], bk, (s, a) => { return a; });
                return res.result = bk;

            }
        }

        private int GetNotifyHeight(string txid, string chainhash)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select blockindex from notify_" + chainhash + " where txid='" + txid + "'";
                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    return int.Parse((rdr["blockindex"]).ToString());
                }
            }
            return -1;
        }

        public JArray GetNep5TransferByTxid(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string txid = req.@params[0].ToString();
                if (!txid.StartsWith("0x"))
                {
                    txid = "0x" + txid;
                }

                string select = "select a.blockindex as blockindex, a.txid as txid, a.asset as asset, a.fromx as fromx, a.tox as tox, a.value as value, b.decimals as decimals, b.symbol as symbol from nep5transfer_" + rootChain + " as a, nep5asset_" + rootChain + " as b where a.txid = '" + txid + "' and a.asset=b.assetid";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var decimals = int.Parse(rdr["decimals"].ToString());
                    var symbol = (rdr["symbol"]).ToString();
                    var idata = (rdr["txid"]).ToString();
                    var adata = (rdr["asset"]).ToString();
                    var fdata = (rdr["fromx"]).ToString();
                    var tdata = (rdr["tox"]).ToString();
                    var vdata = (rdr["value"]).ToString();
                    var num = (decimal)(ulong.Parse(vdata) / Math.Pow(10, decimals));
                    var blockindex = (rdr["blockindex"]).ToString();
                    vdata = num.ToString();

                    //if (int.Parse(blockindex) < height)
                    bk.Add(new JObject { { "blockindex", blockindex }, { "txid", idata }, { "asset", adata }, { "from", fdata }, { "to", tdata }, { "value", vdata }, { "symbol", symbol } });
                }

                return res.result = bk;

            }
        }

        public JArray GetAppChainNep5TransferByTxid(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string txid = req.@params[1].ToString();
                if (!txid.StartsWith("0x"))
                {
                    txid = "0x" + txid;
                }

                string select = "select a.blockindex as blockindex, a.txid as txid, a.asset as asset, a.fromx as fromx, a.tox as tox, a.value as value, b.decimals as decimals, b.symbol as symbol from nep5transfer_" + req.@params[0] + " as a, nep5asset_" + req.@params[0] + " as b where a.txid = '" + txid + "' and a.asset=b.assetid";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var decimals = int.Parse(rdr["decimals"].ToString());
                    var symbol = (rdr["symbol"]).ToString();
                    var idata = (rdr["txid"]).ToString();
                    var adata = (rdr["asset"]).ToString();
                    var fdata = (rdr["fromx"]).ToString();
                    var tdata = (rdr["tox"]).ToString();
                    var vdata = (rdr["value"]).ToString();
                    var num = (decimal)(ulong.Parse(vdata) / Math.Pow(10, decimals));
                    var blockindex = (rdr["blockindex"]).ToString();
                    vdata = num.ToString();

                    bk.Add(new JObject { { "blockindex", blockindex }, { "txid", idata }, { "asset", adata }, { "from", fdata }, { "to", tdata }, { "value", vdata }, { "symbol", symbol } });
                }

                return res.result = bk;

            }
        }

        public JArray GetNep5TransferByTxidEX(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string txid = req.@params[0].ToString();
                if (!txid.StartsWith("0x"))
                {
                    txid = "0x" + txid;
                }

                string select = "select a.blockindex as blockindex, a.txid as txid, a.asset as asset, a.fromx as fromx, a.tox as tox, a.value as value, b.decimals as decimals, b.symbol as symbol from nep5transfer_" + rootChain + " as a, nep5asset_" + rootChain + " as b where a.txid = '" + txid + "' and a.asset=b.assetid";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var decimals = int.Parse(rdr["decimals"].ToString());
                    var symbol = (rdr["symbol"]).ToString();
                    var idata = (rdr["txid"]).ToString();
                    var adata = (rdr["asset"]).ToString();
                    var fdata = (rdr["fromx"]).ToString();
                    var tdata = (rdr["tox"]).ToString();
                    var vdata = (rdr["value"]).ToString();
                    var num = (decimal)(ulong.Parse(vdata) / Math.Pow(10, decimals));
                    var blockindex = (rdr["blockindex"]).ToString();
                    vdata = num.ToString();

                    bk.Add(new JObject { { "blockindex", blockindex }, { "txid", idata }, { "asset", adata }, { "from", fdata }, { "to", tdata }, { "value", vdata }, { "symbol", symbol } });
                }

                if (bk.Count < 1)
                {
                    int blockindex = GetNotifyHeight(txid, rootChain);
                    if (blockindex > -1 && blockindex < height)
                    {
                        bk.Add(new JObject { { "blockindex", blockindex }, { "txid", txid }, { "asset", "" }, { "from", "" }, { "to", "" }, { "value", "" }, { "symbol", "" } });
                    }
                }

                return res.result = bk;

            }
        }

        public JArray GetAppChainNep5TransferByTxidEX(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string txid = req.@params[1].ToString();
                if (!txid.StartsWith("0x"))
                {
                    txid = "0x" + txid;
                }

                string select = "select a.blockindex as blockindex, a.txid as txid, a.asset as asset, a.fromx as fromx, a.tox as tox, a.value as value, b.decimals as decimals, b.symbol as symbol from nep5transfer_" + req.@params[0] + " as a, nep5asset_" + req.@params[0] + " as b where a.txid = '" + txid + "' and a.asset=b.assetid";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var decimals = int.Parse(rdr["decimals"].ToString());
                    var symbol = (rdr["symbol"]).ToString();
                    var idata = (rdr["txid"]).ToString();
                    var adata = (rdr["asset"]).ToString();
                    var fdata = (rdr["fromx"]).ToString();
                    var tdata = (rdr["tox"]).ToString();
                    var vdata = (rdr["value"]).ToString();
                    var num = (decimal)(ulong.Parse(vdata) / Math.Pow(10, decimals));
                    var blockindex = (rdr["blockindex"]).ToString();
                    vdata = num.ToString();

                    bk.Add(new JObject { { "blockindex", blockindex }, { "txid", idata }, { "asset", adata }, { "from", fdata }, { "to", tdata }, { "value", vdata }, { "symbol", symbol } });
                }

                if (bk.Count < 1)
                {
                    int notifyHeight = GetNotifyHeight(txid, req.@params[0].ToString());
                    if (notifyHeight > -1 && notifyHeight < height)
                    {
                        bk.Add(new JObject { { "blockindex", notifyHeight }, { "txid", txid }, { "asset", "" }, { "from", "" }, { "to", "" }, { "value", "" }, { "symbol", "" } });
                    }
                }

                return res.result = bk;

            }
        }

        private JArray GetNotifys(object[] txid, string chainhash, int startNum)
        {
            JArray bk = new JArray();
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select blockindex, txid from notify_" + chainhash + " where txid in (";
                int length = txid.Length;
                for (int i = startNum; i < length; i++)
                {
                    string single = txid[i].ToString();
                    if (!single.StartsWith("0x"))
                    {
                        single = "0x" + single;
                    }
                    if (i == length - 1)
                        select += "'" + single + "') order by blockindex ASC";
                    else
                        select += "'" + single + "',";
                }
                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string blockindex = (rdr["blockindex"]).ToString();
                    string tx = (rdr["txid"]).ToString();

                    bk.Add(new JObject { { "blockindex", blockindex }, { "txid", tx } });
                }
            }
            return bk;
        }

        public JArray GetNep5TransferByTxids(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select a.blockindex as blockindex, a.txid as txid, a.asset as asset, a.fromx as fromx, a.tox as tox, a.value as value, b.decimals as decimals, b.symbol as symbol from ";
                select += "(select blockindex, txid, asset, fromx, tox, value from nep5transfer_" + rootChain + " where txid in (";
                int length = req.@params.Length;
                for (int i = 0; i < length; i++)
                {
                    string txid = req.@params[i].ToString();
                    if (!txid.StartsWith("0x"))
                    {
                        txid = "0x" + txid;
                    }
                    if (i == length - 1)
                        select += "'" + txid + "')";
                    else
                        select += "'" + txid + "',";
                }
                select += ") as a, nep5asset_" + rootChain + " as b where a.asset=b.assetid order by blockindex ASC";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var decimals = int.Parse(rdr["decimals"].ToString());
                    var symbol = (rdr["symbol"]).ToString();
                    var idata = (rdr["txid"]).ToString();
                    var adata = (rdr["asset"]).ToString();
                    var fdata = (rdr["fromx"]).ToString();
                    var tdata = (rdr["tox"]).ToString();
                    var vdata = (rdr["value"]).ToString();
                    var num = (decimal)(ulong.Parse(vdata) / Math.Pow(10, decimals));
                    var blockindex = (rdr["blockindex"]).ToString();
                    vdata = num.ToString();

                    bk.Add(new JObject { { "blockindex", blockindex }, { "txid", idata }, { "asset", adata }, { "from", fdata }, { "to", tdata }, { "value", vdata }, { "symbol", symbol } });
                }

                //JArray notify = GetNotifys(req.@params, rootChain, 0);
                //int j = 0;
                //for (int i = 0; i < notify.Count; i++)
                //{
                //    if (bk.Count == 0 || bk.Count < i - j + 1 || notify[i]["txid"].ToString() != bk[i - j]["txid"].ToString())
                //    {
                //        j++;
                //        bk.Add(new JObject { { "blockindex", notify[i]["blockindex"].ToString() }, { "txid", notify[i]["txid"].ToString() }, { "asset", "" }, { "from", "" }, { "to", "" }, { "value", "" }, { "symbol", "" } });
                //    }
                //}

                return res.result = bk;

            }
        }

        public JArray GetAppChainNep5TransferByTxids(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select a.blockindex as blockindex, a.txid as txid, a.asset as asset, a.fromx as fromx, a.tox as tox, a.value as value, b.decimals as decimals, b.symbol as symbol from ";
                select += "(select blockindex, txid, asset, fromx, tox, value from nep5transfer_" + req.@params[0] + " where txid in (";
                int length = req.@params.Length;
                for (int i = 1; i < length; i++)
                {
                    string txid = req.@params[i].ToString();
                    if (!txid.StartsWith("0x"))
                    {
                        txid = "0x" + txid;
                    }
                    if (i == length - 1)
                        select += "'" + txid + "')";
                    else
                        select += "'" + txid + "',";
                }
                select += ") as a, nep5asset_" + req.@params[0] + " as b where a.asset=b.assetid order by blockindex ASC";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var decimals = int.Parse(rdr["decimals"].ToString());
                    var symbol = (rdr["symbol"]).ToString();
                    var idata = (rdr["txid"]).ToString();
                    var adata = (rdr["asset"]).ToString();
                    var fdata = (rdr["fromx"]).ToString();
                    var tdata = (rdr["tox"]).ToString();
                    var vdata = (rdr["value"]).ToString();
                    var num = (decimal)(ulong.Parse(vdata) / Math.Pow(10, decimals));
                    var blockindex = (rdr["blockindex"]).ToString();
                    vdata = num.ToString();

                    bk.Add(new JObject { { "blockindex", blockindex }, { "txid", idata }, { "asset", adata }, { "from", fdata }, { "to", tdata }, { "value", vdata }, { "symbol", symbol } });
                }

                JArray notify = GetNotifys(req.@params, req.@params[0].ToString(), 1);
                int j = 0;
                for (int i = 0; i < notify.Count; i++)
                {
                    if (bk.Count == 0 || notify[i]["txid"].ToString() != bk[i - j]["txid"].ToString())
                    {
                        j++;
                        bk.Add(new JObject { { "blockindex", notify[i]["blockindex"].ToString() }, { "txid", notify[i]["txid"].ToString() }, { "asset", "" }, { "from", "" }, { "to", "" }, { "value", "" }, { "symbol", "" } });
                    }
                }

                return res.result = bk;

            }
        }

        public JArray GetNep5Count(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string select = "select count(*) from nep5asset";
                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var adata = (rdr["count(*)"]).ToString();

                    JArray bk = new JArray { new JObject { { "nep5count", adata } } };

                    res.result = bk;
                }

                return res.result;

            }
        }

        public JArray GetAllNep5AssetOfAddress(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                var id = req.@params[0].ToString();

                string select = "select  a.id , b.assetid from nep5transfer as a, nep5asset as b where id = @id and a.id = b.id";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                cmd.Parameters.AddWithValue("@id", id);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var idata = (rdr["id"]).ToString();
                    var adata = (rdr["assetid"]).ToString();

                    bk.Add(new JObject { { "id", idata }, { "assetid", adata } });
                }

                return res.result = bk;

            }
        }

        public JArray GetRawTransaction(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string txid = req.@params[0].ToString();
                if (!txid.StartsWith("0x"))
                {
                    txid = "0x" + txid;
                }

                string select = "select a.txid as txid, a.size as size, a.type as type, a.version as version, a.blockheight as blockheight, b.gas_consumed as sys_fee from tx_" + rootChain + " as a, notify_" + rootChain + " as b where a.txid = '" + txid + "' and a.txid=b.txid";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var tx = (rdr["txid"]).ToString();
                    var sz = (rdr["size"]).ToString();
                    var tp = (rdr["type"]).ToString();
                    var vs = (rdr["version"]).ToString();
                    var bh = (rdr["blockheight"]).ToString();
                    var sf = (rdr["sys_fee"]).ToString();

                    bk.Add(new JObject { { "txid", tx }, { "size", sz }, { "type", tp }, { "version", vs }, { "blockindex", bh }, { "sys_fee", sf }, { "net_fee", sf } });
                }
                return res.result = bk;
            }

        }

        public JArray GetAppChainRawTransaction(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string txid = req.@params[1].ToString();
                if (!txid.StartsWith("0x"))
                {
                    txid = "0x" + txid;
                }

                string select = "select a.txid as txid, a.size as size, a.type as type, a.version as version, a.blockheight as blockheight, b.gas_consumed as sys_fee from tx_" + req.@params[0] + " as a, notify_" + req.@params[0] + " as b where a.txid = '" + txid + "' and a.txid=b.txid";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var tx = (rdr["txid"]).ToString();
                    var sz = (rdr["size"]).ToString();
                    var tp = (rdr["type"]).ToString();
                    var vs = (rdr["version"]).ToString();
                    var bh = (rdr["blockheight"]).ToString();
                    var sf = (rdr["sys_fee"]).ToString();

                    bk.Add(new JObject { { "txid", tx }, { "size", sz }, { "type", tp }, { "version", vs }, { "blockindex", bh }, { "sys_fee", sf }, { "net_fee", sf } });
                }
                return res.result = bk;
            }

        }

        public string getGasConsumed(string txid, string chainhash)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select gas_consumed from notify_" + chainhash + " where txid='" + txid + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                var gasConsumed = "";
                while (rdr.Read())
                {
                    gasConsumed = (rdr["gas_consumed"]).ToString();
                }
                if (gasConsumed == "")
                {
                    return "0";
                }
                return gasConsumed;
            }
        }

        public JArray GetTransaction(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string txid = req.@params[0].ToString();
                if (!txid.StartsWith("0x"))
                {
                    txid = "0x" + txid;
                }
                string gas = getGasConsumed(txid, rootChain);
                string select = "select txid, size, type, version, blockheight, gas_limit, gas_price from tx_" + rootChain + " where txid='" + txid + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var adata = (rdr["txid"]).ToString();
                    var size = int.Parse((rdr["size"]).ToString());
                    var type = (rdr["type"]).ToString();
                    var vs = (rdr["version"]).ToString();
                    var bdata = (rdr["blockheight"]).ToString();
                    var gaslimit = (rdr["gas_limit"]).ToString();
                    if (gaslimit == "")
                    {
                        gaslimit = "0";
                    }
                    var gasprice = (rdr["gas_price"]).ToString();
                    if (gasprice == "")
                    {
                        gasprice = "0";
                    }
                    var sdata = decimal.Parse(gas) * decimal.Parse(gasprice);

                    bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockindex", bdata }, { "gas", sdata }, { "gaslimit", gaslimit }, { "gasprice", gasprice } }); //
                }
                return res.result = bk;
            }
        }

        public JArray GetAppChainTransaction(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string txid = req.@params[1].ToString();
                if (!txid.StartsWith("0x"))
                {
                    txid = "0x" + txid;
                }

                string gas = getGasConsumed(txid, req.@params[0].ToString());
                string select = "select txid, size, type, version, blockheight, gas_limit, gas_price from tx_" + req.@params[0] + " where txid='" + txid + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var adata = (rdr["txid"]).ToString();
                    var size = int.Parse((rdr["size"]).ToString());
                    var type = (rdr["type"]).ToString();
                    var vs = (rdr["version"]).ToString();
                    var bdata = (rdr["blockheight"]).ToString();
                    var gaslimit = (rdr["gas_limit"]).ToString();
                    if (gaslimit == "")
                    {
                        gaslimit = "0";
                    }
                    var gasprice = (rdr["gas_price"]).ToString();
                    if (gasprice == "")
                    {
                        gasprice = "0";
                    }

                    var sdata = decimal.Parse(gas) * decimal.Parse(gasprice);

                    bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockindex", bdata }, { "gas", sdata }, { "gaslimit", gaslimit }, { "gasprice", gasprice } }); //
                }
                return res.result = bk;
            }

        }

        public JArray GetTransactions_EX(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select blockheight, txid, sys_fee, gas_limit, gas_price from tx_" + rootChain + " where txid in (";
                int length = req.@params.Length;
                for (int i = 0; i < length; i++)
                {
                    string single = req.@params[i].ToString();
                    if (!single.StartsWith("0x"))
                    {
                        single = "0x" + single;
                    }
                    if (i == length - 1)
                        select += "'" + single + "')";
                    else
                        select += "'" + single + "',";
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var blockindex = (rdr["blockheight"]).ToString();
                    var txid = (rdr["txid"]).ToString();
                    var sys_fee = (rdr["sys_fee"]).ToString();
                    var gas_limit = (rdr["gas_limit"]).ToString();
                    var gas_price = (rdr["gas_price"]).ToString();

                    bk.Add(new JObject { { "txid", txid }, { "blockindex", blockindex }, { "sys_fee", sys_fee }, { "gas_limit", gas_limit }, { "gas_price", gas_price } }); //
                }
                return res.result = bk;
            }

        }

        public JArray GetAppChainTransactions_EX(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select blockheight, txid, sys_fee, gas_limit, gas_price from tx_" + req.@params[0] + " where txid in (";
                int length = req.@params.Length;
                for (int i = 1; i < length; i++)
                {
                    string single = req.@params[i].ToString();
                    if (!single.StartsWith("0x"))
                    {
                        single = "0x" + single;
                    }
                    if (i == length - 1)
                        select += "'" + single + "')";
                    else
                        select += "'" + single + "',";
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var blockindex = (rdr["blockheight"]).ToString();
                    var txid = (rdr["txid"]).ToString();
                    var sys_fee = (rdr["sys_fee"]).ToString();
                    var gas_limit = (rdr["gas_limit"]).ToString();
                    var gas_price = (rdr["gas_price"]).ToString();

                    bk.Add(new JObject { { "txid", txid }, { "blockindex", blockindex }, { "sys_fee", sys_fee }, { "gas_limit", gas_limit }, { "gas_price", gas_price } }); //
                }
                return res.result = bk;
            }

        }

        public JArray GetRawTransactions(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("rawtransactions", out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select txid ,size, type ,version, blockheight, sys_fee from tx_" + rootChain + " where type='InvocationTransaction' limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var adata = (rdr["txid"]).ToString();
                    var size = int.Parse((rdr["size"]).ToString());
                    var type = (rdr["type"]).ToString();
                    var vs = (rdr["version"]).ToString();
                    var bdata = (rdr["blockheight"]).ToString();
                    var sdata = int.Parse((rdr["sys_fee"]).ToString());

                    bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockindex", bdata }, { "gas", sdata } }); //
                }
                jArrays.AddOrUpdate("rawtransactions", bk, (s, a) => { return a; });
                return res.result = bk;
            }

        }

        public JArray GetAppchainRawTransactions(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("rawtransactions" + req.@params[0], out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string select = "select txid ,size, type ,version, blockheight, sys_fee from tx_" + req.@params[0] + " where type='InvocationTransaction' limit " + (int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + int.Parse(req.@params[1].ToString());

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {

                    var adata = (rdr["txid"]).ToString();
                    var size = int.Parse((rdr["size"]).ToString());
                    var type = (rdr["type"]).ToString();
                    var vs = (rdr["version"]).ToString();
                    var bdata = (rdr["blockheight"]).ToString();
                    var sdata = int.Parse((rdr["sys_fee"]).ToString());

                    if (type == "MinerTransaction")
                    {
                        continue;
                    }

                    bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockindex", bdata }, { "gas", sdata } }); //
                }
                jArrays.AddOrUpdate("rawtransactions" + req.@params[0], bk, (s, a) => { return a; });
                return res.result = bk;
            }

        }

        public JArray GetRawTransactionsDESC(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select txid ,size, type ,version, blockheight, sys_fee from tx_" + rootChain + " where type='InvocationTransaction' order by id desc limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var adata = (rdr["txid"]).ToString();
                    var size = int.Parse((rdr["size"]).ToString());
                    var type = (rdr["type"]).ToString();
                    var vs = (rdr["version"]).ToString();
                    var bdata = (rdr["blockheight"]).ToString();
                    var sdata = int.Parse((rdr["sys_fee"]).ToString());

                    if (type == "MinerTransaction")
                    {
                        continue;
                    }

                    bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockindex", bdata }, { "gas", sdata } }); //
                }
                return res.result = bk;
            }

        }


        public JArray GetAppchainRawTransactionsDESC(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string select = "select txid ,size, type ,version, blockheight, sys_fee from tx_" + req.@params[0] + " where type='InvocationTransaction' order by id desc limit " + (int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + int.Parse(req.@params[1].ToString());

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {

                    var adata = (rdr["txid"]).ToString();
                    var size = int.Parse((rdr["size"]).ToString());
                    var type = (rdr["type"]).ToString();
                    var vs = (rdr["version"]).ToString();
                    var bdata = (rdr["blockheight"]).ToString();
                    var sdata = int.Parse((rdr["sys_fee"]).ToString());

                    if (type == "MinerTransaction")
                    {
                        continue;
                    }

                    bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockindex", bdata }, { "gas", sdata } }); //
                }
                return res.result = bk;
            }

        }

        public JArray GetRawTransactionsDESCCache(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("rawtransactionsdesc", out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select txid ,size, type ,version, blockheight, sys_fee from tx_" + rootChain + " where type='InvocationTransaction' order by id desc limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var adata = (rdr["txid"]).ToString();
                    var size = int.Parse((rdr["size"]).ToString());
                    var type = (rdr["type"]).ToString();
                    var vs = (rdr["version"]).ToString();
                    var bdata = (rdr["blockheight"]).ToString();
                    var sdata = int.Parse((rdr["sys_fee"]).ToString());

                    if (type == "MinerTransaction")
                    {
                        continue;
                    }

                    bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockindex", bdata }, { "gas", sdata } }); //
                }
                jArrays.AddOrUpdate("rawtransactionsdesc", bk, (s, a) => { return a; });
                return res.result = bk;
            }

        }


        public JArray GetAppchainRawTransactionsDESCCache(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("rawtransactionsdesc" + req.@params[0], out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string select = "select txid ,size, type ,version, blockheight, sys_fee from tx_" + req.@params[0] + " where type='InvocationTransaction' order by id desc limit " + (int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + int.Parse(req.@params[1].ToString());

                MySqlCommand cmd = new MySqlCommand(select, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {

                    var adata = (rdr["txid"]).ToString();
                    var size = int.Parse((rdr["size"]).ToString());
                    var type = (rdr["type"]).ToString();
                    var vs = (rdr["version"]).ToString();
                    var bdata = (rdr["blockheight"]).ToString();
                    var sdata = int.Parse((rdr["sys_fee"]).ToString());

                    if (type == "MinerTransaction")
                    {
                        continue;
                    }

                    bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockindex", bdata }, { "gas", sdata } }); //
                }
                jArrays.AddOrUpdate("rawtransactionsdesc" + req.@params[0], bk, (s, a) => { return a; });
                return res.result = bk;
            }

        }

        public JArray GetUTXO(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                if (req.@params.Length == 3)
                {
                    string select = "select addr , txid , n , asset , value , used , useHeight , claimed from utxo_" + rootChain + " where addr='" + req.@params[0] + "' limit " + (int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + int.Parse(req.@params[1].ToString());

                    MySqlCommand cmd = new MySqlCommand(select, conn);

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    JArray bk = new JArray();

                    while (rdr.Read())
                    {
                        var add = (rdr["addr"]).ToString();
                        var tid = (rdr["txid"]).ToString();
                        var n = (rdr["n"]).ToString();
                        var adata = (rdr["asset"]).ToString();
                        var vdata = (rdr["value"]).ToString();
                        var usd = (rdr["used"]).ToString();
                        var uh = (rdr["useHeight"]).ToString();
                        var clm = (rdr["claimed"]).ToString();


                        bk.Add(new JObject { { "addr", add }, { "txid", tid }, { "n", n }, { "asset", adata }, { "value", vdata }, { "used", usd }, { "useHeight", uh }, { "name", add } });
                    }

                    return res.result = bk;
                }
                else if (req.@params.Length == 1)
                {
                    string select = "select addr , txid , n , asset , value , used , useHeight , claimed from utxo_" + rootChain + " where addr='" + req.@params[0] + "'";

                    MySqlCommand cmd = new MySqlCommand(select, conn);

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    JArray bk = new JArray();

                    while (rdr.Read())
                    {
                        var add = (rdr["addr"]).ToString();
                        var tid = (rdr["txid"]).ToString();
                        var n = (rdr["n"]).ToString();
                        var adata = (rdr["asset"]).ToString();
                        var vdata = (rdr["value"]).ToString();
                        var usd = (rdr["used"]).ToString();
                        var uh = (rdr["useHeight"]).ToString();
                        var clm = (rdr["claimed"]).ToString();


                        bk.Add(new JObject { { "addr", add }, { "txid", tid }, { "n", n }, { "asset", adata }, { "value", vdata }, { "used", usd }, { "useHeight", uh }, { "name", add } });
                    }

                    return res.result = bk;
                }

                else
                {
                    string select = "select addr , txid , n , asset , value , used ,claimed from utxo_" + rootChain + " ";

                    MySqlCommand cmd = new MySqlCommand(select, conn);

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    JArray bk = new JArray();

                    while (rdr.Read())
                    {
                        var add = (rdr["addr"]).ToString();
                        var tid = (rdr["txid"]).ToString();
                        var n = (rdr["n"]).ToString();
                        var adata = (rdr["asset"]).ToString();
                        var vdata = (rdr["value"]).ToString();
                        var usd = (rdr["used"]).ToString();
                        var clm = (rdr["claimed"]).ToString();

                        bk.Add(new JObject { { "addr", add }, { "txid", tid }, { "n", Int32.Parse(n) }, { "asset", adata }, { "value", vdata }, { "used", usd }, { "claimed", clm } });
                    }

                    return res.result = bk;
                }
            }
        }

        public JArray GetAppChainUTXO(JsonRPCrequest req)
        {

            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();
                if (req.@params.Length == 4)
                {
                    string select = "select addr , txid , n , asset , value , used , useHeight , claimed from utxo_" + req.@params[0] + " where addr='" + req.@params[1] + "' limit " + (int.Parse(req.@params[2].ToString()) * int.Parse(req.@params[3].ToString())) + ", " + int.Parse(req.@params[2].ToString());

                    MySqlCommand cmd = new MySqlCommand(select, conn);

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    JArray bk = new JArray();

                    while (rdr.Read())
                    {
                        var add = (rdr["addr"]).ToString();
                        var tid = (rdr["txid"]).ToString();
                        var n = (rdr["n"]).ToString();
                        var adata = (rdr["asset"]).ToString();
                        var vdata = (rdr["value"]).ToString();
                        var usd = (rdr["used"]).ToString();
                        var uh = (rdr["useHeight"]).ToString();
                        var clm = (rdr["claimed"]).ToString();

                        bk.Add(new JObject { { "addr", add }, { "txid", tid }, { "n", n }, { "asset", adata }, { "value", vdata }, { "used", usd }, { "useHeight", uh }, { "name", add } });
                    }

                    return res.result = bk;
                }
                else if (req.@params.Length == 2)
                {
                    string select = "select addr , txid , n , asset , value , used , useHeight , claimed from utxo_" + req.@params[0] + " where addr='" + req.@params[1] + "'";

                    MySqlCommand cmd = new MySqlCommand(select, conn);

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    JArray bk = new JArray();

                    while (rdr.Read())
                    {
                        var add = (rdr["addr"]).ToString();
                        var tid = (rdr["txid"]).ToString();
                        var n = (rdr["n"]).ToString();
                        var adata = (rdr["asset"]).ToString();
                        var vdata = (rdr["value"]).ToString();
                        var usd = (rdr["used"]).ToString();
                        var uh = (rdr["useHeight"]).ToString();
                        var clm = (rdr["claimed"]).ToString();

                        bk.Add(new JObject { { "addr", add }, { "txid", tid }, { "n", n }, { "asset", adata }, { "value", vdata }, { "used", usd }, { "useHeight", uh }, { "name", add } });
                    }

                    return res.result = bk;
                }

                else
                {
                    string select = "select addr , txid , n , asset , value , used ,claimed from utxo_" + req.@params[0];

                    MySqlCommand cmd = new MySqlCommand(select, conn);

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    JArray bk = new JArray();

                    while (rdr.Read())
                    {
                        var add = (rdr["addr"]).ToString();
                        var tid = (rdr["txid"]).ToString();
                        var n = (rdr["n"]).ToString();
                        var adata = (rdr["asset"]).ToString();
                        var vdata = (rdr["value"]).ToString();
                        var usd = (rdr["used"]).ToString();
                        var clm = (rdr["claimed"]).ToString();

                        bk.Add(new JObject { { "addr", add }, { "txid", tid }, { "n", Int32.Parse(n) }, { "asset", adata }, { "value", vdata }, { "used", usd }, { "claimed", clm } });
                    }

                    return res.result = bk;
                }
            }
        }


        public JArray GetDataBlockHeight(JsonRPCrequest req)   // gets the last 1 in desc
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string sql = "SELECT chainheight FROM `chainlistheight` where chainhash='0x0000000000000000000000000000000000000000'"; //;

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                JsonPRCresponse res = new JsonPRCresponse();

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    res.result = new JArray { new JObject { { "datablockheight", rdr["chainheight"].ToString() } } };
                }
                return res.result;
            }
        }


        public JArray GetAppchainBlockCount(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                string select = "select count(*) from block_" + req.@params[0]; // "select chainheight from chainheightlist where chainhash='" + req.@params[0] + "'"; 

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var adata = (rdr["count(*)"]).ToString();

                    JArray bk = new JArray {
                        new JObject {
                            { "blockDataHeight", adata },
                            { "txDataHeight", adata },
                            { "utxoDataHeight", adata },
                            { "notifyDataHeight", adata },
                            { "fulllogDataHeight", adata }
                        }
                    };

                    res.result = bk;
                }

                return res.result;

            }
        }

        public JArray GetBlockFromHash(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {

                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select hash, size , version , previousblockhash , merkleroot , time , indexx , nonce , nextconsensus , script ,tx  from block_" + rootChain + " where hash = '" + req.@params[0].ToString() + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var hash = (rdr["hash"]).ToString();
                    var sdata = (rdr["size"]).ToString();
                    var adata = (rdr["version"]).ToString();
                    var ind = (rdr["indexx"]).ToString();
                    var pdata = (rdr["previousblockhash"]).ToString();
                    var mdata = (rdr["merkleroot"]).ToString();
                    var tdata = (rdr["time"]).ToString();
                    var ndata = (rdr["nonce"]).ToString();
                    var nc = (rdr["nextconsensus"]).ToString();

                    var s = (rdr["script"]).ToString();
                    var tx = (rdr["tx"]).ToString();

                    bk.Add(new JObject { { "hash", hash }, { "size", sdata }, { "version", adata }, { "previousblockhash", pdata }, { "merkleroot", mdata }, { "time", tdata }, { "index", ind }, { "nonce", ndata }, { "nextconsensus", nc }, { "script", JObject.Parse(s) }, { "tx", JArray.Parse(tx) } });
                }

                return res.result = bk;
            }

        }

        public JArray GetAppChainBlockFromHash(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {

                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select hash, size , version , previousblockhash , merkleroot , time , indexx , nonce , nextconsensus , script ,tx  from block_" + req.@params[0].ToString() + " where hash = '" + req.@params[1].ToString() + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var hash = (rdr["hash"]).ToString();
                    var sdata = (rdr["size"]).ToString();
                    var adata = (rdr["version"]).ToString();
                    var ind = (rdr["indexx"]).ToString();
                    var pdata = (rdr["previousblockhash"]).ToString();
                    var mdata = (rdr["merkleroot"]).ToString();
                    var tdata = (rdr["time"]).ToString();
                    var ndata = (rdr["nonce"]).ToString();
                    var nc = (rdr["nextconsensus"]).ToString();

                    var s = (rdr["script"]).ToString();
                    var tx = (rdr["tx"]).ToString();

                    bk.Add(new JObject { { "hash", hash }, { "size", sdata }, { "version", adata }, { "previousblockhash", pdata }, { "merkleroot", mdata }, { "time", tdata }, { "index", ind }, { "nonce", ndata }, { "nextconsensus", nc }, { "script", JObject.Parse(s) }, { "tx", JArray.Parse(tx) } });
                }

                return res.result = bk;
            }

        }

        public JArray GetBlock2Time(string chainHash, int length)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select time,indexx from block_" + chainHash + " order by indexx desc LIMIT " + (length + 1);

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                long startTime = -1;
                int blockindex = -1;
                while (rdr.Read())
                {
                    var time = (rdr["time"]).ToString();
                    var index = (rdr["indexx"]).ToString();
                    long interval = startTime - long.Parse(time);

                    if (startTime != -1)
                        bk.Add(new JObject { { "blockinterval", interval }, { "blockindex", blockindex } });

                    startTime = long.Parse(time);
                    blockindex = int.Parse(index);
                }
                return res.result = bk;
            }
        }

        public JArray GetBlock2TimeNext(string chainHash)
        {
            JArray jArray = new JArray();
            if (jArrays.TryGetValue("block2time" + chainHash, out jArray))
            {
                return jArray;
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select time,indexx from block_" + chainHash + " order by indexx desc LIMIT 2";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                long startTime = -1;
                int blockindex = -1;
                while (rdr.Read())
                {
                    var time = (rdr["time"]).ToString();
                    var index = (rdr["indexx"]).ToString();
                    long interval = startTime - long.Parse(time);

                    if (startTime != -1)
                        bk.Add(new JObject { { "blockinterval", interval }, { "blockindex", blockindex } });

                    startTime = long.Parse(time);
                    blockindex = int.Parse(index);
                }
                jArrays.AddOrUpdate("block2time" + chainHash, bk, (s, a) => { return a; });
                return res.result = bk;
            }
        }

        public JArray GetContractMessage(string chainHash, string contract)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select totalsupply, name, symbol, decimals from nep5asset_" + chainHash + " where assetid='" + contract + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var totalsupply = (rdr["totalsupply"]).ToString();
                    var name = (rdr["name"]).ToString();
                    var symbol = (rdr["symbol"]).ToString();
                    var decimals = (rdr["decimals"]).ToString();

                    bk.Add(new JObject { { "totalsupply", totalsupply }, { "name", name }, { "symbol", symbol }, { "decimals", decimals } });
                }
                return res.result = bk;
            }
        }

        public JArray GetNFTFromAddr(string chainHash, string address)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select addr, nfttoken, contract, properties from nft_address_" + chainHash + " where addr='" + address + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var addr = (rdr["addr"]).ToString();
                    var nfttoken = (rdr["nfttoken"]).ToString();
                    var contract = (rdr["contract"]).ToString();
                    var properties = (rdr["properties"]).ToString();

                    if (bk.Count == 0)
                    {
                        bk.Add(new JObject { { "addr", addr } });
                    }
                    else if (bk[0][contract] != null)
                    {
                        (bk[0][contract] as JArray).Add(new JObject { { nfttoken, properties } });
                    }
                    else
                    {
                        JArray jContract = new JArray();
                        jContract.Add(nfttoken);
                        (bk[0] as JObject).Add(contract, jContract);
                    }
                }
                return res.result = bk;
            }
        }

        public JArray GetNFTFromAddrAndHash(JsonRPCrequest req)
        {
            string chainHash = req.@params[0].ToString() == "" ? rootChain : req.@params[0].ToString();
            string contract = req.@params[1].ToString();
            string address = req.@params[2].ToString();
            string page = req.@params[3].ToString();
            string pageLength = req.@params[4].ToString();
            string isDesc = "0";
            if (req.@params.Length > 5)
            {
                isDesc = req.@params[5].ToString() == "0" ? "0" : "1";
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string sql = "select nfttoken,properties from nft_address_" + chainHash + " where addr='" + address + "' and contract='" + contract + "' order by id ";
                if (isDesc == "0")
                {
                    sql += "asc ";
                }
                else
                {
                    sql += "desc ";
                }
                sql += "limit " + ((int.Parse(page) - 1) * int.Parse(pageLength)) + ", " + pageLength;

                JArray bk = new JArray();

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var nfttoken = (rdr["nfttoken"]).ToString();
                    var properties = Encoding.Default.GetString(rdr["properties"].ToString().HexString2Bytes());

                    if (bk.Count == 0)
                    {
                        bk.Add(new JObject { { nfttoken, new JObject { { "properties", properties } } } });
                    }
                    else
                    {
                        (bk[0] as JObject).Add(nfttoken, new JObject { { "properties", properties } });
                    }
                }
                rdr.Close();

                return res.result = bk;
            }
        }

        public JArray GetNFTFromHash(JsonRPCrequest req)
        {
            string chainHash = req.@params[0].ToString() == "" ? rootChain : req.@params[0].ToString();
            string contract = req.@params[1].ToString();           
            string page = req.@params[2].ToString();
            string pageLength = req.@params[3].ToString();
            string isDesc = "0";
            if (req.@params.Length > 4)
            {
                isDesc = req.@params[4].ToString() == "0" ? "0" : "1";
            }
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string sql = "select nfttoken from nft_address_" + chainHash + " where contract='" + contract + "' order by id ";
                if (isDesc == "0")
                {
                    sql += "asc ";
                }
                else
                {
                    sql += "desc ";
                }
                sql += "limit " + ((int.Parse(page) - 1) * int.Parse(pageLength)) + ", " + pageLength;

                JArray bk = new JArray();

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var nfttoken = (rdr["nfttoken"]).ToString();                    

                    if (bk.Count == 0)
                    {
                        bk.Add(new JObject { { "nfttoken", new JArray { nfttoken } } });
                    }
                    else
                    {
                        (bk[0]["nfttoken"] as JArray).Add(nfttoken);
                    }
                }
                rdr.Close();

                return res.result = bk;
            }
        }

        public JArray GetNFTHashFromAddr(JsonRPCrequest req)
        {
            string chainHash = req.@params[0].ToString() == "" ? rootChain : req.@params[0].ToString();
            string address = req.@params[1].ToString();
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select distinct contract from nft_address_" + chainHash + " where addr='" + address + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var nfthash = (rdr["contract"]).ToString();

                    if (bk.Count == 0)
                    {
                        bk.Add(new JObject { { "nfthash", new JArray { nfthash } } });
                    }
                    else
                    {
                        (bk[0]["nfthash"] as JArray).Add(nfthash);
                    }

                }
                return res.result = bk;
            }
        }

        public JArray getProperties(JsonRPCrequest req)
        {
            string chainHash = req.@params[0].ToString() == "" ? rootChain : req.@params[0].ToString();
            string contract = req.@params[1].ToString();
            string nfttoken = req.@params[2].ToString();
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                string select = "select properties from nft_address_" + chainHash + " where nfttoken='" + nfttoken + "' and contract='" + contract + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();

                while (rdr.Read())
                {
                    var properties = Encoding.Default.GetString(rdr["properties"].ToString().HexString2Bytes());

                    bk.Add(new JObject { { "properties", properties } });
                }
                return res.result = bk;
            }
        }

        public JArray GetContractState(string chainHash, string contract)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                JsonPRCresponse res = new JsonPRCresponse();
                conn.Open();

                if (!contract.StartsWith("0x"))
                {
                    contract = "0x" + contract;
                }              

                string select = "select name, author, email, description, supportstandard from contract_state_" + chainHash + " where hash='" + contract + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {
                    var name = (rdr["name"]).ToString();
                    var author = (rdr["author"]).ToString();
                    var email = (rdr["email"]).ToString();
                    var description = (rdr["description"]).ToString();
                    var supportstandard = rdr["supportstandard"].ToString();

                    bk.Add(new JObject { { "author", author }, { "name", name }, { "email", email }, { "description", description }, { "supportstandard", supportstandard }, { "contract", contract } });
                }
                return res.result = bk;
            }
        }

        private string getName(string chainHash, string contract)
        {
            string name = "";
            JsonPRCresponse res = new JsonPRCresponse();
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                if (!contract.StartsWith("0x"))
                {
                    contract = "0x" + contract;
                }

                string select = "select name from contract_state_" + chainHash + " where hash='" + contract + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {
                    name = (rdr["name"]).ToString();
                }
            }
            return name;
        }

        public JArray getPageMessage(string chainhash, string id)
        {
            JsonPRCresponse res = new JsonPRCresponse();
            JArray bk = new JArray();
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();

                if (!id.StartsWith("0x"))
                {
                    id = "0x" + id;
                }

                string select = "select * from nep5asset_" + chainhash + " where assetid='" + id + "'";

                using (MySqlCommand cmd = new MySqlCommand(select, conn))
                {
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            bk.Add(new JObject() { { "page", "nep5asset" } });
                        }
                    }
                }
                if (bk.Count > 0)
                    return res.result = bk;

                if (!id.StartsWith("0x"))
                {
                    id = "0x" + id;
                }

                select = "select * from contract_state_" + chainhash + " where hash='" + id + "'";

                using (MySqlCommand cmd = new MySqlCommand(select, conn))
                {
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            bk.Add(new JObject() { { "page", "contract" } });
                        }
                    }
                }
                if (bk.Count > 0)
                    return res.result = bk;

                if (!id.StartsWith("0x"))
                {
                    id = "0x" + id;
                }

                select = "select * from appchainstate where hash='" + id + "'";

                using (MySqlCommand cmd = new MySqlCommand(select, conn))
                {
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            bk.Add(new JObject() { { "page", "appchainstate" } });
                        }
                    }
                }
                if (bk.Count > 0)
                    return res.result = bk;
            }
            return res.result = bk;
        }
    }

}




