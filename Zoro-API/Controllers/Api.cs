using Zoro_Web_API.lib;
using Zoro_Web_API.RPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Zoro_Web_API.Controllers
{
    public class Api
    {
        private string netnode { get; set; }
        private string mongodbConnStr { get; set; }
        private string mysqlconnstr { get; set; }
        private string mongodbDatabase { get; set; }
        private string neoCliJsonRPCUrl { get; set; }

        private string rootChain = "0000000000000000000000000000000000000000";
        private string chainhash = "";

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

        public Api(string node)
        {
            netnode = node;
            switch (netnode)
            {
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

        public async Task<object> getResAsync(JsonRPCrequest req, string reqAddr)
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
                        var blockcount = Helper.MakeRpcUrl(ZoroHelper.ZoroUrl, "getblockcount", rootChain);
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
                        result = msq.GetDataBlockHeight(req);
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
                    case "getaddressasset":
                        result = msq.GetAddressAsset(req);
                        break;
                    case "getaddressallasset":
                        result = await msq.GetAddressAllAssetAsync(req);
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
                    case "getaddressnep5txs":
                        result = msq.GetAddressNep5Txs(req);
                        break;
                    #endregion

                    #region 获取balance
                    case "getbalance":
                        result = await msq.GetBalanceByAssetAsync(req);
                        break;
                    case "getbalancebyasset":
                        result = await msq.GetAssetBalanceAsync(req);
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
                    case "gettransferbytxids":
                        result = msq.GetTransferByTxids(req);
                        break;
                    #endregion

                    #region 获取nep5transfer
                    case "getnep5transfer":
                        result = msq.GetNep5Transfer(req);
                        break;
                    case "getappchainnep5transfer":
                        result = msq.GetAppChainNep5Transfer(req);
                        break;
                    case "getnep5transferbyasset":
                        result = msq.GetNep5TransferByAsset(req);
                        break;
                    case "getnep5transferbyassetandaddress":
                        result = msq.GetNep5TransferByAssetAndAddress(req);
                        break;
                    case "getsendallamountbyaddress":
                        result = msq.GetSendAllAmountByAddress(req);
                        break;

                    #endregion                                      

                    #region 获取 nft 相关信息
                    case "getnftfromaddrandhash":
                        result = msq.GetNFTFromAddrAndHash(req);
                        break;
                    case "getnfthashfromaddr":
                        result = msq.GetNFTHashFromAddr(req);
                        break;
                    case "getnftfromhash":
                        result = msq.GetNFTFromHash(req);
                        break;
                    case "gettokenproperties":
                        result = msq.getProperties(req);
                        break;
                    #endregion

                    #region 和链交互
                    case "sendrawtransaction":
                        var tx = "";
                        if (req.@params.Length > 1)
                        {
                            if (req.@params[0].ToString() == "")
                            {
                                chainhash = rootChain;
                            }
                            else
                            {
                                chainhash = req.@params[0].ToString();
                            }

                            tx = await ZoroHelper.SendRawTransaction(req.@params[1].ToString(), chainhash);
                        }
                        else
                        {
                            //byte[] postArray = APIHelper.HexString2Bytes(req.@params[0].ToString());
                            tx = await ZoroHelper.SendRawTransaction(req.@params[0].ToString(), rootChain);
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
                            invokescript = await ZoroHelper.InvokeScript(postArray, rootChain);
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
                            estimategas = await ZoroHelper.EstimateGas(req.@params[0].ToString(), rootChain);
                        }
                        result = new JArray() { new JObject { { "gas", estimategas } } };
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
                            jArray.Add(rootChain);
                            jArray.Add(req.@params[0].ToString());
                            url = Helper.MakeRpcUrlPost(ZoroHelper.ZoroUrl, "getcontractstate", out postArray, jArray);
                            url = await Helper.HttpPost(url, postArray);
                        }
                        result = new JArray() { JObject.Parse(url)["result"] };
                        break;
                    #endregion

                    #region 其他
                    case "getpagemessage":
                        result = msq.getPageMessage(req.@params[0].ToString(), req.@params[1].ToString());
                        break;
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
                                contractChainHash = rootChain;
                            }
                            else
                            {
                                contractChainHash = req.@params[0].ToString();
                            }
                            contract = req.@params[1].ToString();
                            if (!contract.StartsWith("0x"))
                            {
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
                            result = msq.GetContractMessage(rootChain, contract);
                        }
                        break;

                    case "getallnep5assetofaddress":
                        string NEP5addr = (string)req.@params[0];
                        bool isNeedBalance = false;
                        string chainHash = "";
                        if (req.@params.Count() > 1)
                        {
                            isNeedBalance = ((Int64)req.@params[1] == 1) ? true : false;
                        }
                        if (req.@params.Count() > 2)
                        {
                            chainHash = req.@params[2].ToString();
                        }

                        var assetAdds = msq.GetNep5AssetByAddress(chainHash, NEP5addr);
                        List<NEP5.AssetBalanceOfAddr> addrAssetBalances = new List<NEP5.AssetBalanceOfAddr>();
                        if (isNeedBalance)
                        {
                            //UInt160 addr = UInt160.Parse(ThinNeo.Helper.GetPublicKeyHashFromAddress(NEP5addr).ToString());
                            foreach (var assetAdd in assetAdds)
                            {
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
                      
                        if (!isNeedBalance)
                        {
                            result = JArray.FromObject(assetAdds);
                        }
                        else
                        {
                            result = JArray.FromObject(addrAssetBalances);
                        }

                        break;
                        #endregion
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
