using NEO_Block_API.lib;
using NEO_Block_API.RPC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Text;
using Zoro;
using Newtonsoft.Json.Linq;

namespace NEO_Block_API.Controllers
{
    public class Api
    {
        private string netnode { get; set; }
        private string mongodbConnStr { get; set; }
		private string mysqlconnstr { get; set; }
        private string mongodbDatabase { get; set; }
        private string neoCliJsonRPCUrl { get; set; }

        httpHelper hh = new httpHelper();
        mongoHelper mh = new mongoHelper();
		mySqlHelper msq = new mySqlHelper();
        Transaction tx = new Transaction();
        Contract ct = new Contract();
        Claim claim = new Claim();

        private static Api testApi = new Api("testnet");
        private static Api mainApi = new Api("mainnet");
        public static Api getTestApi() { return testApi; }
        public static Api getMainApi() { return mainApi; }

        public Api(string node) {
            netnode = node;
            switch (netnode) {
                case "testnet":
                    mongodbConnStr = mh.mongodbConnStr_testnet;
                    mongodbDatabase = mh.mongodbDatabase_testnet;
                    neoCliJsonRPCUrl = mh.neoCliJsonRPCUrl_testnet;
                    break;
                case "mainnet":
                    mongodbConnStr = mh.mongodbConnStr_mainnet;
                    mongodbDatabase = mh.mongodbDatabase_mainnet;
                    neoCliJsonRPCUrl = mh.neoCliJsonRPCUrl_mainnet;
                    break;
            }
        }

        private JArray getJAbyKV(string key, object value)
        {
            return  new JArray
                        {
                            new JObject
                            {
                                {
                                    key,
                                    value.ToString()
                                }
                            }
                        };
        }

        private JArray getJAbyJ(JObject J)
        {
            return new JArray
                        {
                            J
                        };
        }

        public async Task<object> getResAsync(JsonRPCrequest req,string reqAddr)
        {
            JArray result = new JArray();
            string resultStr = string.Empty;
            string findFliter = string.Empty;
            string sortStr = string.Empty;
            try
            {
                switch (req.method)
                {
                    #region 获取block
                    case "getblock":
                        result = msq.GetBlock(req);
                        break;
                    case "getacblock":
                        result = msq.GetAppChainBlock(req);
                        break;
                    #endregion
                    #region 获取blocks
                    case "getblocks":
                        result = msq.GetBlocks(req);
                        break;
                    case "getappchainblocks":
                        result = msq.GetAppchainBlocks(req);
                        break;
                    case "getblocksdesc":
                        result = msq.GetBlocksDESC(req);
                        break;
                    case "getappchainblocksdesc":
                        result = msq.GetAppchainBlocksDESC(req);
                        break;
                    case "getblocksdesccache":
                        result = msq.GetBlocksDESCCache(req);
                        break;
                    case "getappchainblocksdesccache":
                        result = msq.GetAppchainBlocksDESCCache(req);
                        break;
                    #endregion
                    #region 获取block from hash
                    case "getblockfromhash":
                        result = msq.GetBlockFromHash(req);
                        break;
                    case "getappchainblockfromhash":
                        result = msq.GetAppChainBlockFromHash(req);
                        break;
                    #endregion
                    #region 获取blockcount
                    case "getblockcount":
                        var blockcount = Helper.MakeRpcUrl(ZoroHelper.ZoroUrl, "getblockcount", "0000000000000000000000000000000000000000");
                        var s = await Helper.HttpGet(blockcount);
                        var a = new JArray();
                        a.Add(new JObject { { "blockcount", JObject.Parse(s)["result"].ToString() } });
                        result = a;
                        break;
                    case "getappchainblockcount":
                        var appchainblockcount = Helper.MakeRpcUrl(ZoroHelper.ZoroUrl, "getblockcount", req.@params[0].ToString());
                        var apps = await Helper.HttpGet(appchainblockcount);
                        var appa = new JArray();
                        appa.Add(new JObject { { "blockcount", JObject.Parse(apps)["result"].ToString() } });
                        result = appa;
                        break;
                    #endregion
                    #region 获取爬虫爬到的blockcount
                    case "getdatablockheight":
                        result = msq.GetBlockCount(req);
                        break;
                    case "getappchaindatablockheight":
                        result = msq.GetAppchainBlockCount(req);
                        break;
                    #endregion
                    #region 获取addrcount
                    case "getaddrcount":
                        result = msq.GetAddrCount(req);
                        break;
                    case "getappchainaddrcount":
                        result = msq.GetAppchainAddrCount(req);
                        break;
                    #endregion
                    #region 获取txcount
                    case "gettxcount":
                        result = msq.GetTxCount(req);
                        break;
                    case "getappchaintxcount":
                        result = msq.GetAppchainTxCount(req);
                        break;
                    #endregion
                    #region 获取rawtransaction
                    case "getrawtransaction":
                        result = msq.GetRawTransaction(req);
                        break;
                    case "getacrawtransaction":
                        result = msq.GetAppChainRawTransaction(req);
                        break;
                    #endregion
                    #region 获取transaction
                    case "gettransaction":
                        result = msq.GetTransaction(req);
                        break;
                    case "getactransaction":
                        result = msq.GetAppChainTransaction(req);
                        break;
                    #endregion
                    #region 获取transaction_ex
                    case "gettransactions_ex":
                        result = msq.GetTransactions_EX(req);
                        break;
                    case "getactransactions_ex":
                        result = msq.GetAppChainTransactions_EX(req);
                        break;
                    #endregion
                    #region 获取rawtransactions
                    case "getrawtransactions":
                        result = msq.GetRawTransactions(req);
                        break;
                    case "getappchainrawtransactions":
                        result = msq.GetAppchainRawTransactions(req);
                        break;
                    case "getrawtransactionsdesc":
                        result = msq.GetRawTransactionsDESC(req);
                        break;
                    case "getappchainrawtransactionsdesc":
                        result = msq.GetAppchainRawTransactionsDESC(req);
                        break;
                    case "getrawtransactionsdesccache":
                        result = msq.GetRawTransactionsDESCCache(req);
                        break;
                    case "getappchainrawtransactionsdesccache":
                        result = msq.GetAppchainRawTransactionsDESCCache(req);
                        break;
                    #endregion
                    #region 获取allnep5asset
                    case "getallnep5asset":
                        result = msq.GetAllNep5Asset(req);
                        break;
                    case "getappchainallnep5asset":
                        result = msq.GetAllNep5AssetByChainHash(req);
                        break;
                    #endregion
                    #region 获取hashlist,appchain
                    case "gethashlist":
                        result = msq.GetHashlist(req);
                        break;
                    case "getallappchains":
                        result = msq.GetAllAppchains(req);
                        break;
                    case "getappchain":
                        result = msq.GetAppchain(req);
                        break;
                    #endregion
                    #region 获取nep5asset
                    case "getnep5asset":
                        result = msq.GetNep5Asset(req);
                        break;
                    case "getappchainnep5asset":
                        result = msq.GetAppChainNep5Asset(req);
                        break;
                    #endregion
                    #region 获取address
                    case "getaddress":
                        result = msq.GetAddress(req);
                        break;
                    case "getappchainaddress":
                        result = msq.GetAppChainAddress(req);
                        break;
                    #endregion
                    #region 获取addr
                    case "getaddr":
                        result = msq.GetAddr(req);
                        break;
                    case "getappchainaddr":
                        result = msq.GetAppChainAddr(req);
                        break;
                    #endregion
                    #region 获取addrs
                    case "getaddrs":
                        result = msq.GetAddrs(req);
                        break;
                    case "getappchainaddrs":
                        result = msq.GetAppChainAddrs(req);
                        break;
                    #endregion
                    #region 获取addresstxs
                    case "getaddresstxs":
                        result = msq.GetAddressTxs(req);
                        break;
                    case "getappchainaddresstxs":
                        result = msq.GetAppChainAddressTxs(req);
                        break;
                    #endregion
                    #region 获取balance
                    case "getbalance":
                        result = await msq.GetBalanceAsync(req);
                        break;
                    case "getappchainbalance":
                        result = await msq.GetAppChainBalanceAsync(req);
                        break;
                    #endregion
                    #region 获取utxo
                    case "getutxo":
                        result = msq.GetUTXO(req);
                        break;
                    case "getappchainutxo":
                        result = msq.GetAppChainUTXO(req);
                        break;
                    #endregion
                    #region 获取rankasset
                    case "getrankbyasset":
                        result = msq.GetRankByAsset(req);
                        break;
                    case "getappchainrankbyasset":
                        result = msq.GetAppChainRankByAsset(req);
                        break;                   
                    #endregion
                    #region 获取rankbyassetcount
                    case "getrankbyassetcount":
                        result = msq.GetRankByAssetCount(req);
                        break;
                    case "getappchainrankbyassetcount":
                        result = msq.GetAppChainRankByAssetCount(req);
                        break;
                    #endregion
                    #region 获取nep5transferbytxid
                    case "getnep5transferbytxid":
                        result = msq.GetNep5TransferByTxid(req);
                        break;
                    case "getappchainnep5transferbytxid":
                        result = msq.GetAppChainNep5TransferByTxid(req);
                        break;
                    #endregion
                    #region 获取nep5transferbytxidex
                    case "getnep5transferbytxidex":
                        result = msq.GetNep5TransferByTxidEX(req);
                        break;
                    case "getappchainnep5transferbytxidex":
                        result = msq.GetAppChainNep5TransferByTxidEX(req);
                        break;
                    #endregion
                    #region 获取nep5transferbytxids
                    case "getnep5transferbytxids":
                        result = msq.GetNep5TransferByTxids(req);
                        break;
                    case "getappchainnep5transferbytxids":
                        result = msq.GetAppChainNep5TransferByTxids(req);
                        break;
                    #endregion
                    #region 获取nep5transfer
                    case "getnep5transfer":
                        result = msq.GetNep5Transfer(req);
                        break;
                    case "getappchainnep5transfer":
                        result = msq.GetAppChainNep5Transfer(req);
                        break;
                    #endregion
                    case "getnep5count":
                        result = msq.GetNep5Count(req);
                        break;
                    case "getnep5allnep5assetofaddress":
                        result = msq.GetAllNep5AssetOfAddress(req);
                        break;
                    case "getcontractmessage":
                        string contractChainHash = "";
                        string contract = "";
                        if (req.@params.Length > 1)
                        {
                            if (req.@params[0].ToString() == "")
                            {
                                contractChainHash = "0000000000000000000000000000000000000000";
                            }
                            else
                            {
                                contractChainHash = req.@params[0].ToString();
                            }
                            contract = req.@params[1].ToString();
                            if (!contract.StartsWith("0x")) {
                                contract = "0x" + contract;
                            }
                            result = msq.GetContractMessage(contractChainHash, contract);
                        }
                        else
                        {
                            contract = req.@params[0].ToString();
                            if (!contract.StartsWith("0x"))
                            {
                                contract = "0x" + contract;
                            }
                            result = msq.GetContractMessage("0000000000000000000000000000000000000000", contract);
                        }
                        break;
                    case "getcontractstatemessage":
                        string stateChainHash = "";
                        string state = "";
                        if (req.@params.Length > 1)
                        {
                            if (req.@params[0].ToString() == "")
                            {
                                stateChainHash = "0000000000000000000000000000000000000000";
                            }
                            else
                            {
                                stateChainHash = req.@params[0].ToString();
                            }
                            state = req.@params[1].ToString();
                            if (!state.StartsWith("0x"))
                            {
                                state = "0x" + state;
                            }
                            result = msq.GetContractState(stateChainHash, state);
                        }
                        else
                        {
                            state = req.@params[0].ToString();
                            if (!state.StartsWith("0x"))
                            {
                                state = "0x" + state;
                            }
                            result = msq.GetContractState("0000000000000000000000000000000000000000", state);
                        }
                        break;
                    case "getblockinterval":
                        string blockChainHash = "";
                        string length = "";
                        if (req.@params.Length > 1)
                        {
                            if (req.@params[0].ToString() == "")
                            {
                                blockChainHash = "0000000000000000000000000000000000000000";
                            }
                            else
                            {
                                blockChainHash = req.@params[0].ToString();
                            }
                            length = req.@params[1].ToString();
                            result = msq.GetBlock2Time(blockChainHash, int.Parse(length));
                        }
                        else
                        {
                            length = req.@params[0].ToString();
                            result = msq.GetBlock2Time("0000000000000000000000000000000000000000", int.Parse(length));
                        }                       
                        break;
                    case "getblockintervalnext":
                        string blockChainHashNext = "";
                        if (req.@params.Length > 1)
                        {
                            if (req.@params[0].ToString() == "")
                            {
                                blockChainHashNext = "0000000000000000000000000000000000000000";
                            }
                            else
                            {
                                blockChainHashNext = req.@params[0].ToString();
                            }
                            result = msq.GetBlock2TimeNext(blockChainHashNext);
                        }
                        else
                        {
                            result = msq.GetBlock2TimeNext("0000000000000000000000000000000000000000");
                        }
                        break;
                    case "getscriptmethod":
                        string chainhash = "";
                        if (req.@params.Length > 1)
                        {
                            if (req.@params[0].ToString() == "")
                            {
                                chainhash = "0000000000000000000000000000000000000000";
                            }
                            else {
                                chainhash = req.@params[0].ToString();
                            }
                            result = await msq.GetScriptMethodAsync(chainhash, req.@params[1].ToString());
                        }
                        else
                        {
                            result = await msq.GetScriptMethodAsync("0000000000000000000000000000000000000000", req.@params[0].ToString());
                        }
                        break;
                    case "sendrawtransaction":
                        var tx = "";
                        if (req.@params.Length > 1)
                        {
                            //byte[] postArray = APIHelper.HexString2Bytes(req.@params[1].ToString());
                            tx = await ZoroHelper.SendRawTransaction(req.@params[1].ToString(), req.@params[0].ToString());
                        }
                        else
                        {
                            //byte[] postArray = APIHelper.HexString2Bytes(req.@params[0].ToString());
                            tx = await ZoroHelper.SendRawTransaction(req.@params[0].ToString(), "0000000000000000000000000000000000000000");
                        }
                        if (JObject.Parse(tx)["result"].ToString() == "True") 
                            result = new JArray() { new JObject { { "sendrawtransactionresult", JObject.Parse(tx)["result"] } } };
                        else
                            result = new JArray() { JObject.Parse(tx)["result"] };
                        break;
                    case "invokescript":
                        var invokescript = "";
                        if (req.@params.Length > 1)
                        {
                            byte[] postArray = APIHelper.HexString2Bytes(req.@params[1].ToString());
                            invokescript = await ZoroHelper.InvokeScript(postArray, req.@params[0].ToString());
                        }
                        else
                        {
                            byte[] postArray = APIHelper.HexString2Bytes(req.@params[0].ToString());
                            invokescript = await ZoroHelper.InvokeScript(postArray, "0000000000000000000000000000000000000000");
                        }
                        result = new JArray() { JObject.Parse(invokescript)["result"] };
                        break;
                    case "estimategas":
                        decimal estimategas = 0;
                        if (req.@params.Length > 1)
                        {
                            //byte[] postArray = APIHelper.HexString2Bytes(req.@params[1].ToString());
                            estimategas = await ZoroHelper.EstimateGas(req.@params[1].ToString(), req.@params[0].ToString());
                        }
                        else
                        {
                            //byte[] postArray = APIHelper.HexString2Bytes(req.@params[0].ToString());
                            estimategas = await ZoroHelper.EstimateGas(req.@params[0].ToString(), "0000000000000000000000000000000000000000");
                        }
                        result = new JArray() { new JObject{ { "gas", estimategas } } };
                        break;
                    case "getcontractstate":
                        var url = "";
                        if (req.@params.Length > 1)
                        {
                            byte[] postArray = null;
                            Zoro.IO.Json.JArray jArray = new Zoro.IO.Json.JArray();
                            jArray.Add(req.@params[0].ToString());
                            jArray.Add(req.@params[1].ToString());
                            url = Helper.MakeRpcUrlPost(ZoroHelper.ZoroUrl, "getcontractstate", out postArray, jArray);
                            url = await Helper.HttpPost(url, postArray);
                        }
                        else
                        {
                            byte[] postArray = null;
                            Zoro.IO.Json.JArray jArray = new Zoro.IO.Json.JArray();
                            jArray.Add("0000000000000000000000000000000000000000");
                            jArray.Add(req.@params[0].ToString());
                            url = Helper.MakeRpcUrlPost(ZoroHelper.ZoroUrl, "getcontractstate", out postArray, jArray);
                            url = await Helper.HttpPost(url, postArray);
                        }
                        result = new JArray() { JObject.Parse(url)["result"] };
                        break;
                    case "getallnep5assetofaddress":
                        string NEP5addr = (string)req.@params[0];
                        bool isNeedBalance = false;
                        string chainHash = "";
                        if (req.@params.Count() > 1)
                        {
                            isNeedBalance = ((Int64)req.@params[1] == 1) ? true : false;
                        }
                        if (req.@params.Count() > 2) {
                            chainHash = req.@params[2].ToString();
                        }

                        var assetAdds = msq.GetNep5AssetByAddress(chainHash, NEP5addr);
                        List<NEP5.AssetBalanceOfAddr> addrAssetBalances = new List<NEP5.AssetBalanceOfAddr>();
                        if (isNeedBalance) {
                            //UInt160 addr = UInt160.Parse(ThinNeo.Helper.GetPublicKeyHashFromAddress(NEP5addr).ToString());
                            foreach (var assetAdd in assetAdds) {
                                if (assetAdd["type"].ToString() == "NativeNep5")
                                {
                                    var nep5 = await APIHelper.getNativeBalanceOfAsync(chainHash, assetAdd["assetid"].ToString(), NEP5addr);
                                    if (nep5.balance != "0")
                                        addrAssetBalances.Add(nep5);
                                }
                                else
                                {
                                    var nep5 = await APIHelper.getBalanceOfAsync(chainHash, assetAdd["assetid"].ToString(), NEP5addr);
                                    if (nep5.balance != "0")
                                        addrAssetBalances.Add(nep5);
                                }
                            }
                        }


                        ////按资产汇集收到的钱(仅资产ID)
                        //JArray transferToJA = msq.GetNep5TranferFromToAddress(chainHash, NEP5addr);
                        //List<NEP5.Transfer> tfts = new List<NEP5.Transfer>();
                        //foreach (JObject tfJ in transferToJA)
                        //{
                        //    tfts.Add(new NEP5.Transfer(tfJ));
                        //}
                        //var queryTo = from tft in tfts
                        //            group tft by tft.asset into tftG
                        //            select new { assetid = tftG.Key};
                        //var assetAdds = queryTo.ToList();

                        ////如果需要余额，则通过cli RPC批量获取余额
                        ////List<NEP5.AssetBalanceOfAddr> addrAssetBalances = new List<NEP5.AssetBalanceOfAddr>();
                        //if (isNeedBalance) {
                        //    List<NEP5.AssetBalanceOfAddr> addrAssetBalancesTemp = new List<NEP5.AssetBalanceOfAddr>();
                        //    foreach (var assetAdd in assetAdds)
                        //    {
                        //        string findNep5Asset = "{assetid:'" + assetAdd.assetid + "'}";
                        //        JArray Nep5AssetJA = mh.GetData(mongodbConnStr, mongodbDatabase, "NEP5asset", findNep5Asset);
                        //        string Symbol = (string)Nep5AssetJA[0]["symbol"];
                        //        string resp = hh.Post(neoCliJsonRPCUrl, "{'jsonrpc':'2.0','method':'getcontractstate','params':['" + assetAdd.assetid + "'],'id':1}", System.Text.Encoding.UTF8, 1);
                        //        JObject resultJ = (JObject)JObject.Parse(resp)["result"];
                        //        if (resultJ == null)
                        //            continue;

                        //        addrAssetBalancesTemp.Add(new NEP5.AssetBalanceOfAddr(assetAdd.assetid, Symbol, string.Empty));
                        //    }

                        //    List<string> nep5Hashs = new List<string>();
                        //    JArray queryParams = new JArray();
                        //    byte[] NEP5allAssetOfAddrHash = ThinNeo.Helper.GetPublicKeyHashFromAddress(NEP5addr);
                        //    string NEP5allAssetOfAddrHashHex = ThinNeo.Helper.Bytes2HexString(NEP5allAssetOfAddrHash.Reverse().ToArray());
                        //    foreach (var abt in addrAssetBalancesTemp)
                        //    {
                        //        nep5Hashs.Add(abt.assetid);
                        //        queryParams.Add(JArray.Parse("['(str)balanceOf',['(hex)" + NEP5allAssetOfAddrHashHex + "']]"));                               
                        //    }
                        //    JArray NEP5allAssetBalanceJA = (JArray)ct.callContractForTest(neoCliJsonRPCUrl, nep5Hashs, queryParams)["stack"];
                        //    var a = Newtonsoft.Json.JsonConvert.SerializeObject(NEP5allAssetBalanceJA);
                        //    foreach (var abt in addrAssetBalancesTemp)
                        //    {
                        //        /// ChangeLog:
                        //        /// 升级智能合约带来的数据结构不一致问题，暂时使用try方式临时解决
                        //        try
                        //        {
                        //            string allBalanceStr = (string)NEP5allAssetBalanceJA[addrAssetBalancesTemp.IndexOf(abt)]["value"];
                        //            string allBalanceType = (string)NEP5allAssetBalanceJA[addrAssetBalancesTemp.IndexOf(abt)]["type"];

                        //            //获取NEP5资产信息，获取精度
                        //            NEP5.Asset NEP5asset = new NEP5.Asset(mongodbConnStr, mongodbDatabase, abt.assetid);

                        //            abt.balance = NEP5.getNumStrFromStr(allBalanceType, allBalanceStr, NEP5asset.decimals);
                        //        } catch (Exception e)
                        //        {
                        //            Console.WriteLine(abt.assetid +",ConvertTypeFailed,errMsg:"+e.Message);
                        //            abt.balance = string.Empty;
                        //        }
                        //    }

                        //    //去除余额为0的资产
                        //    foreach (var abt in addrAssetBalancesTemp)
                        //    {
                        //        if (abt.balance != string.Empty && abt.balance != "0")
                        //        {
                        //            addrAssetBalances.Add(abt);
                        //        }
                        //    }
                        //}

                        ////按资产汇集支出的钱
                        //string findTransferFrom = "{ from:'" + NEP5addr + "'}";
                        //JArray transferFromJA = mh.GetData(mongodbConnStr, mongodbDatabase, "NEP5transfer", findTransferFrom);
                        //List<NEP5.Transfer> tffs = new List<NEP5.Transfer>();
                        //foreach (JObject tfJ in transferFromJA)
                        //{
                        //    tffs.Add(new NEP5.Transfer(tfJ));
                        //}
                        //var queryFrom = from tff in tffs
                        //                group tff by tff.asset into tffG
                        //            select new { assetid = tffG.Key, sumOfValue = tffG.Sum(m => m.value) };
                        //var assetRemoves = queryFrom.ToList();

                        ////以支出的钱扣减收到的钱得到余额
                        //JArray JAadds = JArray.FromObject(assetAdds);
                        //foreach (JObject Jadd in JAadds) {
                        //    foreach (var assetRemove in assetRemoves)
                        //    {
                        //        if ((string)Jadd["assetid"] == assetRemove.assetid)
                        //        {
                        //            Jadd["sumOfValue"] = (decimal)Jadd["sumOfValue"] - assetRemove.sumOfValue;
                        //            break;
                        //        }
                        //    }
                        //}
                        //var a = Newtonsoft.Json.JsonConvert.SerializeObject(JAadds);

                        //***********
                        //经简单测试，仅看transfer记录，所有to减去所有from并不一定等于合约查询得到的地址余额(可能有其他非标方法消耗了余额，尤其是测试网)，废弃这种方法，还是采用调用NEP5合约获取地址余额方法的方式
                        //这里给出所有该地址收到过的资产hash，可以配合其他接口获取资产信息和余额
                        //***********
                        if (!isNeedBalance)
                        {
                            result = JArray.FromObject(assetAdds);
                        }
                        else
                        {
                            result = JArray.FromObject(addrAssetBalances);
                        }                   

                        break;                                   
                }
                if (result != null && result.Count > 0 && result[0]["errorCode"] != null)
                {
                    JsonPRCresponse_Error resE = new JsonPRCresponse_Error(req.id, (int)result[0]["errorCode"], (string)result[0]["errorMsg"], (string)result[0]["errorData"]);

                    return resE;
                }
                if (result.Count == 0)
                {
                    JsonPRCresponse_Error resE = new JsonPRCresponse_Error(req.id, -1, "No Data", "Data does not exist");

                    return resE;
                }
            }
            catch (Exception e)
            {
                JsonPRCresponse_Error resE = new JsonPRCresponse_Error(req.id, -100, "Parameter Error", e.Message);

                return resE;

            }

            JsonPRCresponse res = new JsonPRCresponse();
            res.jsonrpc = req.jsonrpc;
            res.id = req.id;
            res.result = result;

            return res;
        }
    }
}
