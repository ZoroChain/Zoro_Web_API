using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using NEO_Block_API.RPC;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace NEO_Block_API.lib
{
	public class mySqlHelper
	{
		public static string conf = "database=block;server=115.159.68.43;user id=root;Password=H@ngJIA123;sslmode=None";


		public JArray GetAddress(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
				var addr = req.@params[0].ToString();

				string select = "select firstuse , lastuse , txcount from  address_0000000000000000000000000000000000000000 where addr = @addr";

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

					 bk.Add(new JObject { { "firstuse", adata }, { "lastuse", ldata } , { "txcount", tdata } });


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



				string select = "select a.addr, a.firstuse,a.lastuse, a.txcount, b.blockindex ,b.blocktime ,b.txid from address_0000000000000000000000000000000000000000 as a , address_tx_0000000000000000000000000000000000000000 as b where  a.firstuse = b.blockindex " ;//limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0]; // string select = "select a.addr, a.firstuse,a.lastuse, a.txcount, b.blockindex ,b.blocktime ,b.txid from address_0000000000000000000000000000000000000000 as a limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);
				MySqlDataReader rdr = cmd.ExecuteReader();
				
		
				while (rdr.Read())
				{
					var adata = (rdr["addr"]).ToString();
				
					var f = (rdr["firstuse"]).ToString();
					var lu = (rdr["lastuse"]).ToString();
					var bi = (rdr["blockindex"]).ToString();
					var bt = (rdr["blocktime"]).ToString();
					var txid = (rdr["txid"]).ToString();
					var txc = (rdr["txcount"]).ToString();

					JObject dt = new JObject() {  { "$date", bt }  };
					
					
					JObject j = new JObject() { { "txid", txid }, { "blockindex", bi }, { "blocktime", dt } };
					JObject m = new JObject() { { "txid", txid }, { "blockindex", bi }, { "blocktime", dt } };
					bk.Add(new JObject { { "addr", adata } , { "firstDate",f } , { "lastDate", lu }, { "firstuse", j} , { "lastuse", m }, { "txcount", txc } });
			
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



                string select = "select a.addr, a.firstuse,a.lastuse, a.txcount, b.blockindex ,b.blocktime ,b.txid from address_"+req.@params[0]+ " as a , address_tx_" + req.@params[0] + " as b where  a.firstuse = b.blockindex ";//limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0]; // string select = "select a.addr, a.firstuse,a.lastuse, a.txcount, b.blockindex ,b.blocktime ,b.txid from address_0000000000000000000000000000000000000000 as a limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    var adata = (rdr["addr"]).ToString();

                    var f = (rdr["firstuse"]).ToString();
                    var lu = (rdr["lastuse"]).ToString();
                    var bi = (rdr["blockindex"]).ToString();
                    var bt = (rdr["blocktime"]).ToString();
                    var txid = (rdr["txid"]).ToString();
                    var txc = (rdr["txcount"]).ToString();

                    JObject dt = new JObject() { { "$date", bt } };


                    JObject j = new JObject() { { "txid", txid }, { "blockindex", bi }, { "blocktime", dt } };
                    JObject m = new JObject() { { "txid", txid }, { "blockindex", bi }, { "blocktime", dt } };
                    bk.Add(new JObject { { "addr", adata }, { "firstDate", f }, { "lastDate", lu }, { "firstuse", j }, { "lastuse", m }, { "txcount", txc } });

                }


                return res.result = bk;

            }

        }


        public JArray GetAddr(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();


				string select = "select a.addr, a.firstuse, a.lastuse, a.txcount, b.blockindex ,b.blocktime ,b.txid from address_0000000000000000000000000000000000000000 as a , address_tx_0000000000000000000000000000000000000000 as b where a.addr='" + req.@params[0] + "'"; // + "' and a.firstuse = b.blockindex";

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);
		



				MySqlDataReader rdr = cmd.ExecuteReader();

				JArray bk = new JArray();
				while (rdr.Read())
				{

					var adata = (rdr["addr"]).ToString();
					var fs = (rdr["firstuse"]).ToString();
					var ls = (rdr["lastuse"]).ToString();
					var f = (rdr["txcount"]).ToString();
					var l = (rdr["blockindex"]).ToString();
					var bt= (rdr["blocktime"]).ToString();
					var txid = (rdr["txid"]).ToString();

					JObject dt = new JObject() { { "$date", bt } };

					JObject j = new JObject() { { "txid", txid }, { "blockindex", l }, { "blocktime", dt } };
					JObject m = new JObject() { { "txid", txid }, { "blockindex", l }, { "blocktime", dt } };

					bk.Add(new JObject { { "addr", adata }, { "firstuse", j }, { "lastuse", m }, { "txcount", f }  });
	

				}

				return res.result = bk;

			}
		}

        public JArray GetAppChainAddr(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();


                string select = "select a.addr, a.firstuse, a.lastuse, a.txcount, b.blockindex ,b.blocktime ,b.txid from address_"+req.@params[0]+ " as a , address_tx_" + req.@params[0] + " as b where a.addr='" + req.@params[1] + "'"; // + "' and a.firstuse = b.blockindex";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);




                MySqlDataReader rdr = cmd.ExecuteReader();

                JArray bk = new JArray();
                while (rdr.Read())
                {

                    var adata = (rdr["addr"]).ToString();
                    var fs = (rdr["firstuse"]).ToString();
                    var ls = (rdr["lastuse"]).ToString();
                    var f = (rdr["txcount"]).ToString();
                    var l = (rdr["blockindex"]).ToString();
                    var bt = (rdr["blocktime"]).ToString();
                    var txid = (rdr["txid"]).ToString();

                    JObject dt = new JObject() { { "$date", bt } };

                    JObject j = new JObject() { { "txid", txid }, { "blockindex", l }, { "blocktime", dt } };
                    JObject m = new JObject() { { "txid", txid }, { "blockindex", l }, { "blocktime", dt } };

                    bk.Add(new JObject { { "addr", adata }, { "firstuse", j }, { "lastuse", m }, { "txcount", f } });


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

				string select = "select a.txid,a.addr,a.blocktime,a.blockindex,b.type,b.vout,b.vin from  address_tx_0000000000000000000000000000000000000000 as a , tx_0000000000000000000000000000000000000000 as b where @addr = addr and a.txid = b.txid limit " + req.@params[1];

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
					var type = (rdr["type"]).ToString();
					var vout = (rdr["vout"]).ToString();
					var vin = (rdr["vin"]).ToString();

					JObject t = new JObject() { { "$date", bt } };
					JObject vo = new JObject() { { "$date", bt } };
					bk.Add(new JObject { { "addr", vdata } , { "txid", adata }, { "blockindex", bi }, { "blocktime", t } , { "type", type }, { "vout", JArray.Parse(vout)} , { "vin",JArray.Parse(vin)}});

				}
				JArray c = new JArray() { };
				c.Add(new JObject { { "count", JToken.Parse("10") }, { "list", bk } });
				return res.result = c;

			}
		}

        public JArray GetAppChainAddressTxs(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                var addr = req.@params[1].ToString();

                string select = "select a.txid,a.addr,a.blocktime,a.blockindex,b.type,b.vout,b.vin from  address_tx_"+req.@params[0]+ " as a , tx_" + req.@params[0] + " as b where @addr = addr and a.txid = b.txid limit " + req.@params[2];

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
                    var type = (rdr["type"]).ToString();
                    var vout = (rdr["vout"]).ToString();
                    var vin = (rdr["vin"]).ToString();

                    JObject t = new JObject() { { "$date", bt } };
                    JObject vo = new JObject() { { "$date", bt } };
                    bk.Add(new JObject { { "addr", vdata }, { "txid", adata }, { "blockindex", bi }, { "blocktime", t }, { "type", type }, { "vout", JArray.Parse(vout) }, { "vin", JArray.Parse(vin) } });

                }
                JArray c = new JArray() { };
                c.Add(new JObject { { "count", JToken.Parse("10") }, { "list", bk } });
                return res.result = c;

            }
        }

        public JArray GetTxCount(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
				//var addr = req.@params[0].ToString();
				if (req.@params[0].ToString() == "")
				{
					string select = "select count(*) from tx_0000000000000000000000000000000000000000"; // inherently belongs to all appchains tx count

					JsonPRCresponse res = new JsonPRCresponse();
					MySqlCommand cmd = new MySqlCommand(select, conn);


					MySqlDataReader rdr = cmd.ExecuteReader();
					while (rdr.Read())
					{

						var adata = (rdr["count(*)"]).ToString();



						JArray bk = new JArray {
					new JObject    {
										{"txcount",adata}
								   }

							   };

						res.result = bk;
					}

					return res.result;
				}
				else {
					string select = "select count(*) from tx where type='" + req.@params[0] + "'";

					JsonPRCresponse res = new JsonPRCresponse();
					MySqlCommand cmd = new MySqlCommand(select, conn);


					MySqlDataReader rdr = cmd.ExecuteReader();
					while (rdr.Read())
					{

						var adata = (rdr["count(*)"]).ToString();



						JArray bk = new JArray {
					new JObject    {
										{"txcount",adata}
								   }

							   };

						res.result = bk;
					}

					return res.result;
				}
			
			}
		}

        

        public JArray GetAppchainTxCount(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
				//var addr = req.@params[0].ToString();		
				string select = "select count(*) from tx_"+ req.@params[0]; //where type= + req.@params[0] + "'";

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);


				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{

					var adata = (rdr["count(*)"]).ToString();



					JArray bk = new JArray {
				new JObject    {
									{"txcount",adata}
								}

							};

					res.result = bk;
				}

				return res.result;
			}
		}

		public JArray GetRankByAssetCount(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
		

				{
					string select = "select count(*) from nep5asset_0000000000000000000000000000000000000000 where id='" + req.@params[0].ToString() + "'" ;

					JsonPRCresponse res = new JsonPRCresponse();
					MySqlCommand cmd = new MySqlCommand(select, conn);


					MySqlDataReader rdr = cmd.ExecuteReader();
					while (rdr.Read())
					{

						var adata = (rdr["count(*)"]).ToString();

						JArray bk = new JArray {
					new JObject    {
										{"count",adata}
								   }

							   };

						res.result = bk;
					}



					return res.result;
				}
			}
		}

        public JArray GetAppChainRankByAssetCount(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                {
                    string select = "select count(*) from nep5asset_" + req.@params[0] + " where id='" + req.@params[1].ToString() + "'";

                    JsonPRCresponse res = new JsonPRCresponse();
                    MySqlCommand cmd = new MySqlCommand(select, conn);


                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        var adata = (rdr["count(*)"]).ToString();

                        JArray bk = new JArray {
                    new JObject    {
                                        {"count",adata}
                                   }

                               };

                        res.result = bk;
                    }
                    return res.result;
                }
            }
        }

        public JArray GetRankByAsset(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
				

				{
					string select = "select id, totalsupply , decimals from nep5asset_0000000000000000000000000000000000000000 where id='" + req.@params[0] + "'"; //					string select = "select id, amount , admin from nep5asset_0000000000000000000000000000000000000000 where id='" + req.@params[0] + "'";


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
						 
					   bk.Add(new JObject { { "asset", adata }, { "balance", bl } , { "addr", ad }  });
    	
					}

					return res.result = bk;
				}
			}
		}

        public JArray GetAppChainRankByAsset(JsonRPCrequest req)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();


                {
                    string select = "select id, totalsupply , decimals from nep5asset_" + req.@params[0] + " where id='" + req.@params[1] + "'"; //					string select = "select id, amount , admin from nep5asset_0000000000000000000000000000000000000000 where id='" + req.@params[0] + "'";


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
        }

        public JArray GetAddrCount(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();


				string select = "select count(*) from address_0000000000000000000000000000000000000000";

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);



				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{

					var adata = (rdr["count(*)"]).ToString();



					JArray bk = new JArray {
					new JObject    {
										{"addrcount",adata}
								   }


							   };

					res.result = bk;
				}

				return res.result;

			}
		}


		public JArray GetAppchainAddrCount(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();


				string select = "select count(*) from address_"+req.@params[0];

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);



				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{

					var adata = (rdr["count(*)"]).ToString();



					JArray bk = new JArray {
					new JObject    {
										{"addrcount",adata}
								   }


							   };

					res.result = bk;
				}

				return res.result;

			}
		}
		public JArray GetBalance(JsonRPCrequest req) // needs to be changed for the right balance data
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();


				string select = "select id , addr, asset , value , used from utxo_0000000000000000000000000000000000000000 where used = 0 and addr='" + req.@params[0] + "'";

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);
				JArray bk = new JArray();


				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					var id = (rdr["id"]).ToString();
					var ad = (rdr["addr"]).ToString();
					var adata = (rdr["asset"]).ToString();
					var ldata = (rdr["value"]).ToString();
					var us = (rdr["used"]).ToString();
					

					bk.Add(new JObject { { "rank", id }, { "addr", ad }, { "balance", ldata }, { "asset", adata }, { "used", us } });

				}

				return res.result = bk;
			}
		}

        public JArray GetAppChainBalance(JsonRPCrequest req) // needs to be changed for the right balance data
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();


                string select = "select id , addr, asset , value , used from utxo_"+req.@params[0]+" where used = 0 and addr='" + req.@params[1] + "'";

                JsonPRCresponse res = new JsonPRCresponse();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                JArray bk = new JArray();


                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = (rdr["id"]).ToString();
                    var ad = (rdr["addr"]).ToString();
                    var adata = (rdr["asset"]).ToString();
                    var ldata = (rdr["value"]).ToString();
                    var us = (rdr["used"]).ToString();


                    bk.Add(new JObject { { "rank", id }, { "addr", ad }, { "balance", ldata }, { "asset", adata }, { "used", us } });

                }

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
					
					
					bk.Add(new JObject { { "version", adata }, { "type", tdata },{ "name", JArray.Parse(ndata) }, { "amount", xdata }, { "precision", pdata } , { "available", xdata }, { "owner", odata }, { "admin", fdata }, { "id", adata }});


				}

				return res.result = bk;

			}
		}


		
		public JArray GetAllAsset(JsonRPCrequest req)
		{
			
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				

					conn.Open();
			

					string select = "select type ,name , amount, pprecision ,available ,owner, admin , id from asset";

					JsonPRCresponse res = new JsonPRCresponse();
					MySqlCommand cmd = new MySqlCommand(select, conn);
				

					MySqlDataReader rdr = cmd.ExecuteReader();
					JArray bk = new JArray();
					while (rdr.Read())
					{
					    var tdata = (rdr["type"]).ToString();
					    var ndata = (rdr["name"]).ToString();
					    var mdata = (rdr["amount"]).ToString();
					    var pdata = (rdr["pprecision"]).ToString();
					    var xdata = (rdr["available"]).ToString();  
					    var odata = (rdr["owner"]).ToString();
					    var o =     (rdr["admin"]).ToString();
					    var adata = (rdr["id"]).ToString();



					bk.Add(new JObject { { "type", tdata }, { "name", JArray.Parse(ndata) },{"amount" , mdata}, { "precision", pdata } , { "available", xdata }, { "owner", odata }, { "admin", o }, { "id", adata }});
					}

					return res.result = bk;

			}
		}

		public JArray GetAllAppchains(JsonRPCrequest req)
		{

			using (MySqlConnection conn = new MySqlConnection(conf))
			{


				conn.Open();


				string select = "select version , hash ,name ,owner, timestamp , seedlist , validators from appchainstate"; // + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];
				

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

				return res.result = bk;

			}
		}

		public JArray GetAppchain(JsonRPCrequest req)
		{
			
			using (MySqlConnection conn = new MySqlConnection(conf))
			{


				conn.Open();


				string select = "select version , hash , name , owner, timestamp , seedlist , validators from appchainstate where hash = '"+"0x"+req.@params[0]+"'"; //string select = "select a.version , a.hash , a.name , a.owner, a.timestamp , a.seedlist , a.validators , b.(count(*).tx) , b.(count(*).indexx) , b.chainheight from appchainstate as a and "+ req.@params[0]+"_table"+" where hash = '" + req.@params[0] + "'";


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
					var o     = (rdr["validators"]).ToString();

					//var split = o.Split(',');
					//foreach (string s in split)
					//{
						//vals.Add(new JObject { s });

					 //   array = JArray.FromObject(vals);
					//}

					bk.Add(new JObject { { "version", tdata }, { "hash", ndata }, { "name", mdata }, { "owner", pdata }, { "timestamp", xdata }, { "seedlist", JArray.Parse(odata) }, { "validators", JArray.Parse(o) } });
				}

				return res.result = bk;

			}
		}
		public JArray GetHashlist(JsonRPCrequest req)
		{

			using (MySqlConnection conn = new MySqlConnection(conf))
			{

				conn.Open();

				string select = "select hashlist  from hashlist";
			    
				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);

				MySqlDataReader rdr = cmd.ExecuteReader();
				JArray bk = new JArray();
			
				while (rdr.Read())
				{
					var tdata = (rdr["hashlist"]).ToString();
					
					bk.Add(new JObject { { "hashlist", tdata } });
				}

				return res.result = bk;

			}
		}

		public JArray GetBlock(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				JsonPRCresponse res = new JsonPRCresponse();
				conn.Open();

				string select = "select hash, size , version , previousblockhash , merkleroot , time , indexx , nonce , nextconsensus , script ,tx  from  block_0000000000000000000000000000000000000000  where indexx='" + req.@params[0] + "'";

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

					
					bk.Add(new JObject { { "hash", hash }, { "size", sdata }, { "version", adata }, { "previousblockhash", pdata }, { "merkleroot", mdata }, { "time", tdata }, { "index", ind }, { "nonce", ndata }, { "nextconsensus", nc } , { "script",JObject.Parse(s) }, { "tx", JArray.Parse(tx) } });
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

				string select = "select hash, size , version , previousblockhash , merkleroot , time , indexx , nonce , nextconsensus , script ,tx  from  block_"+ req.@params[0] +" where indexx = '" + req.@params[1] + "'";

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
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				JsonPRCresponse res = new JsonPRCresponse();
				conn.Open();

			
				string select = "select  size , version, hash , previousblockhash , merkleroot , time , indexx , nonce , nextconsensus , script ,tx  from block_0000000000000000000000000000000000000000 limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];

				MySqlCommand cmd = new MySqlCommand(select, conn);
				

				MySqlDataReader rdr = cmd.ExecuteReader();

				JArray bk = new JArray();

				while (rdr.Read())
				{

					var sdata = (rdr["size"]).ToString();
					var adata = (rdr["version"]).ToString();
					var hash = (rdr["hash"]).ToString();
					var pdata = (rdr["previousblockhash"]).ToString();
					var ind = (rdr["indexx"]).ToString();
					var mdata = (rdr["merkleroot"]).ToString();
					var tdata = (rdr["time"]).ToString();
					var ndata = (rdr["nonce"]).ToString();
					var nc = (rdr["nextconsensus"]).ToString();
					var s = (rdr["script"]).ToString();
					var tx = (rdr["tx"]).ToString();


					bk.Add(new JObject { { "size", sdata }, { "version", adata }, { "hash", hash }, { "previousblockhash", pdata }, { "index", ind }, { "merkleroot", mdata }, { "time", tdata }, { "nonce", ndata }, { "nextconsensus", nc } , { "script", s }, {"tx",JArray.Parse(tx) }});
				}

				return res.result = bk;



			}
		}


		public JArray GetAppchainBlocks(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				JsonPRCresponse res = new JsonPRCresponse();
				conn.Open();


				string select = "select  size , version, hash , previousblockhash , merkleroot , time , indexx , nonce , nextconsensus , script ,tx from block_" + req.@params[0] +" limit "+(int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + int.Parse(req.@params[1].ToString()); 

				MySqlCommand cmd = new MySqlCommand(select, conn);


				MySqlDataReader rdr = cmd.ExecuteReader();

				JArray bk = new JArray();

				while (rdr.Read())
				{

					var sdata = (rdr["size"]).ToString();
					var adata = (rdr["version"]).ToString();
					var hash = (rdr["hash"]).ToString();
					var pdata = (rdr["previousblockhash"]).ToString();
					var ind = (rdr["indexx"]).ToString();
					var mdata = (rdr["merkleroot"]).ToString();
					var tdata = (rdr["time"]).ToString();
					var ndata = (rdr["nonce"]).ToString();
					var nc = (rdr["nextconsensus"]).ToString();
				
					var s = (rdr["script"]).ToString();
					var tx = (rdr["tx"]).ToString();


					bk.Add(new JObject { { "size", sdata }, { "version", adata }, { "hash", hash }, { "previousblockhash", pdata }, { "index", ind }, { "merkleroot", mdata }, { "time", tdata }, { "nonce", ndata }, { "nextconsensus", nc }, { "script", s }, { "tx", JArray.Parse(tx) } });
				}

				return res.result = bk;



			}
		}

		public JArray GetNep5Asset(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();

				string select = " select assetid , totalsupply , name , symbol , decimals from nep5asset_0000000000000000000000000000000000000000 where assetid = '" + req.@params[0] + "'"; 
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
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();

				string select = "select assetid , totalsupply , name ,symbol ,decimals  from nep5asset_0000000000000000000000000000000000000000";

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



					bk.Add(new JObject { {"assetid", adata } , { "totalsupply", ts }, { "name", name }, { "symbol", sb } , { "decimals", cd } });

				}
				return res.result = bk;

			}

		}

        public JArray GetAllNep5AssetByChainHash(JsonRPCrequest req)
        {
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



                    bk.Add(new JObject { { "assetid", adata }, { "totalsupply", ts }, { "name", name }, { "symbol", sb }, { "decimals", cd } });

                }
                return res.result = bk;

            }

        }

        public JArray GetNep5Transfer(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{

				conn.Open();
				var txid = req.@params[0].ToString();

				string select = "select blockindex, n , asset , fromx , tox , value from nep5transfer_0000000000000000000000000000000000000000 where txid = @txid";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@txid", txid);


				JsonPRCresponse res = new JsonPRCresponse();

				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					var bdata = (rdr["blockindex"]).ToString();
					var ndata = (rdr["n"]).ToString();
					var adata = (rdr["asset"]).ToString();
					var fdata = (rdr["fromx"]).ToString();
					var tdata = (rdr["tox"]).ToString();
					var vdata = (rdr["value"]).ToString();

					JArray bk = new JArray {
					new JObject    {
										{"blockindex",bdata}
								   },
					new JObject    {
										{"n",ndata}
								   },
					new JObject    {
										{"asset",adata}
								   },
					new JObject    {
										{"from",fdata}
								   },
					new JObject    {
										{"to",tdata}
								   },
					new JObject    {
										{"value",vdata}
								   },


							   };

					res.result = bk;
				}

				return res.result;

			}
		}


		public JArray GetNep5TransfersByAsset(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{

				conn.Open();
		

				string select = "select  asset , blockindex, from , n , to ,txid , value from nep5transfer  where indexx='" + req.@params[0] + "'";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				

				JsonPRCresponse res = new JsonPRCresponse();

		
				MySqlDataReader rdr = cmd.ExecuteReader();

				JArray bk = new JArray();
				while (rdr.Read())
				{
			
				
					var adata = (rdr["asset"]).ToString();
					var bi = (rdr["blockindex"]).ToString();
					var fdata = (rdr["from"]).ToString();
					var n = (rdr["n"]).ToString();
					var tdata = (rdr["to"]).ToString();
					var tx = (rdr["txid"]).ToString();
					var vdata = (rdr["value"]).ToString();
					


					bk.Add(new JObject { { "blockindex", bi }, { "txid", tx }, { "n", n },{ "asset", adata },{ "from", fdata }, { "to", tdata }, { "value", vdata } });


			}

				return res.result = bk;

			}
		}

		public JArray GetNep5TransferByTxid(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{

				conn.Open();
		

				string select = "select  id , asset , fromx , tox , value from nep5transfer_0000000000000000000000000000000000000000 where txid = '" + req.@params[0]+ "'";

				MySqlCommand cmd = new MySqlCommand(select, conn); 
			


				JsonPRCresponse res = new JsonPRCresponse();

				MySqlDataReader rdr = cmd.ExecuteReader();
				JArray bk = new JArray();
				while (rdr.Read())
				{

					var idata = (rdr["id"]).ToString();
					var adata = (rdr["asset"]).ToString();
					var fdata = (rdr["fromx"]).ToString();
					var tdata = (rdr["tox"]).ToString();
					var vdata = (rdr["value"]).ToString();


					bk.Add(new JObject { { "id", idata }, { "asset", adata }, { "from", fdata }, { "to", tdata } , { "value", vdata } });
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



					JArray bk = new JArray {
					new JObject    {
										{"nep5count",adata}
								   }


							   };

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

				string select = "select txid ,size, type ,version, blockheight, sys_fee, net_fee, vin , vout from tx_0000000000000000000000000000000000000000 where txid = '" + req.@params[0] + "'";

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
					var nf = (rdr["net_fee"]).ToString();
					var adata = (rdr["vin"]).ToString();
					var vdata = (rdr["vout"]).ToString();
				

					

					bk.Add(new JObject {{ "txid", tx } , { "size", sz } , { "type", tp } , { "version", vs } , { "blockindex", bh } , { "sys_fee", sf }, { "net_fee", nf }, { "vin", adata }, { "vout", vdata } });


				}

				return res.result = bk;


			}

		}

		public JArray GetAppChainRawTransaction(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();

				string select = "select txid ,size, type ,version, blockheight, sys_fee, net_fee, vin , vout from tx_" + req.@params[0] +" where txid = '" + req.@params[1] + "'";

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
					var nf = (rdr["net_fee"]).ToString();
					var adata = (rdr["vin"]).ToString();
					var vdata = (rdr["vout"]).ToString();




					bk.Add(new JObject { { "txid", tx }, { "size", sz }, { "type", tp }, { "version", vs }, { "blockindex", bh }, { "sys_fee", sf }, { "net_fee", nf }, { "vin", adata }, { "vout", vdata } });


				}

				return res.result = bk;


			}

		}

		public JArray GetRawTransactions(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();

				if (req.@params[2].ToString() == "")
				{
					string select = "select txid ,size, type ,version, blockheight, sys_fee, vin , vout from tx_0000000000000000000000000000000000000000 limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];

					// inherently belongs to all app chain txs

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
						var vin = (rdr["vin"]).ToString();
						var vout = (rdr["vout"]).ToString();



						bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockindex", bdata }, { "gas", sdata }, { "vin", vin }, { "vout", vout } }); //


					}

					return res.result = bk;

				}

				else if (req.@params[1].ToString() == null)
				{
					string select = "select txid ,size, type ,version, blockheight, sys_fee, vin , vout from tx where type='" + req.@params[2] + "'limit " + req.@params[0];

					MySqlCommand cmd = new MySqlCommand(select, conn);



					JsonPRCresponse res = new JsonPRCresponse();

					MySqlDataReader rdr = cmd.ExecuteReader();

					JArray bk = new JArray();
					while (rdr.Read())
					{

						var adata = (rdr["txid"]).ToString();
						var size = (rdr["size"]).ToString();
						var type = (rdr["type"]).ToString();
						var vs = (rdr["version"]).ToString();
						var bdata = (rdr["blockheight"]).ToString();
						var sdata = (rdr["sys_fee"]).ToString();
						var vin = (rdr["vin"]).ToString();
						var vout = (rdr["vout"]).ToString();



						bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockheight", bdata }, { "gas", sdata }, { "vin", JArray.Parse(vin) }, { "vout", JArray.Parse(vout) } });


					}

					return res.result = bk;
				}

				else  //((req.@params[0].ToString() != null ) && (req.@params[1].ToString() != null) && (req.@params[2].ToString() != null))
						{

					string select = "select txid ,size, type ,version, blockheight, sys_fee, vin , vout from tx where type='" + req.@params[2] + "'limit " + (int.Parse(req.@params[0].ToString()) * int.Parse(req.@params[1].ToString())) + ", " + req.@params[0];


					MySqlCommand cmd = new MySqlCommand(select, conn);



					JsonPRCresponse res = new JsonPRCresponse();

					MySqlDataReader rdr = cmd.ExecuteReader();

					JArray bk = new JArray();
					while (rdr.Read())
					{

						var adata = (rdr["txid"]).ToString();
						var size = (rdr["size"]).ToString();
						var type = (rdr["type"]).ToString();
						var vs = (rdr["version"]).ToString();
						var bdata = (rdr["blockheight"]).ToString();
						var sdata = (rdr["sys_fee"]).ToString();
						var vin = (rdr["vin"]).ToString();
						var vout = (rdr["vout"]).ToString();



						bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockheight", bdata }, { "gas", sdata }, { "vin", JArray.Parse(vin) }, { "vout", JArray.Parse(vout) } });


					}

					return res.result = bk;

				}

			}

		}


		public JArray GetAppchainRawTransactions(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();

				if (req.@params[2].ToString() == null)
				{
					string select = "select txid ,size, type ,version, blockheight, sys_fee, vin , vout from tx_" + req.@params[0]+" limit " + (int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + int.Parse(req.@params[1].ToString());


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
						var vin = (rdr["vin"]).ToString();
						var vout = (rdr["vout"]).ToString();



						bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockindex", bdata }, { "gas", sdata }, { "vin", vin }, { "vout", vout } }); //


					}

					return res.result = bk;

				}

				else if (req.@params[1].ToString() == null)
				{
					string select = "select txid ,size, type ,version, blockheight, sys_fee, vin , vout from tx_"+ req.@params[0]+ "where type='" + req.@params[2] + " limit " + req.@params[0];

					MySqlCommand cmd = new MySqlCommand(select, conn);



					JsonPRCresponse res = new JsonPRCresponse();

					MySqlDataReader rdr = cmd.ExecuteReader();

					JArray bk = new JArray();
					while (rdr.Read())
					{

						var adata = (rdr["txid"]).ToString();
						var size = (rdr["size"]).ToString();
						var type = (rdr["type"]).ToString();
						var vs = (rdr["version"]).ToString();
						var bdata = (rdr["blockheight"]).ToString();
						var sdata = (rdr["sys_fee"]).ToString();
						var vin = (rdr["vin"]).ToString();
						var vout = (rdr["vout"]).ToString();



						bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockheight", bdata }, { "gas", sdata }, { "vin", JArray.Parse(vin) }, { "vout", JArray.Parse(vout) } });


					}

					return res.result = bk;
				}

				else  //((req.@params[0].ToString() != null ) && (req.@params[1].ToString() != null) && (req.@params[2].ToString() != null))
				{

					string select = "select txid ,size, type ,version, blockheight, sys_fee, vin , vout from tx_" + req.@params[0]+ " limit "+(int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + int.Parse(req.@params[1].ToString()); 



					MySqlCommand cmd = new MySqlCommand(select, conn);



					JsonPRCresponse res = new JsonPRCresponse();

					MySqlDataReader rdr = cmd.ExecuteReader();

					JArray bk = new JArray();
					while (rdr.Read())
					{

						var adata = (rdr["txid"]).ToString();
						var size = (rdr["size"]).ToString();
						var type = (rdr["type"]).ToString();
						var vs = (rdr["version"]).ToString();
						var bdata = (rdr["blockheight"]).ToString();
						var sdata = (rdr["sys_fee"]).ToString();
						var vin = (rdr["vin"]).ToString();
						var vout = (rdr["vout"]).ToString();



						bk.Add(new JObject { { "txid", adata }, { "size", size }, { "type", type }, { "version", vs }, { "blockindex", bdata }, { "gas", sdata }, { "vin", JArray.Parse(vin) }, { "vout", JArray.Parse(vout) } });


					}

					return res.result = bk;

				}

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
                    string select = "select addr , txid , n , asset , value , used , useHeight , claimed from utxo_0000000000000000000000000000000000000000 where addr='" + req.@params[0] + "' limit " + (int.Parse(req.@params[1].ToString()) * int.Parse(req.@params[2].ToString())) + ", " + int.Parse(req.@params[1].ToString());

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
                    string select = "select addr , txid , n , asset , value , used , useHeight , claimed from utxo_0000000000000000000000000000000000000000 where addr='" + req.@params[0] + "'";

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
                    string select = "select addr , txid , n , asset , value , used ,claimed from utxo_0000000000000000000000000000000000000000 ";

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
                    string select = "select addr , txid , n , asset , value , used , useHeight , claimed from utxo_"+req.@params[0]+" where addr='" + req.@params[1] + "'";

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


        public JArray GetBlockCount(JsonRPCrequest req)   // gets the last 1 in desc
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();

		
				string select = "SELECT indexx FROM block_0000000000000000000000000000000000000000 ORDER BY id DESC LIMIT 1"; //;


				MySqlCommand cmd = new MySqlCommand(select, conn);
		

				JsonPRCresponse res = new JsonPRCresponse();

				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					
						var adata = (rdr["indexx"]).ToString();

						JArray bk = new JArray {
					 new JObject    {
										{"indexx",adata}
								   }

							   }; 

						res.result = bk;
					}
				

				return res.result;
			}


		}


		public JArray GetAppchainBlockCount(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();


				string select = "select count(*) from block_"+req.@params[0]; // "select chainheight from chainheightlist where chainhash='" + req.@params[0] + "'"; 

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);



				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{

					var adata = (rdr["count(*)"]).ToString();

					//var cheight = (rdr["chainheight"]).ToString();



					JArray bk = new JArray {
					new JObject    {
										{"blockcount",adata}
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

				string select = "select hash, size , version , previousblockhash , merkleroot , time , indexx , nonce , nextconsensus , script ,tx  from block_0000000000000000000000000000000000000000 where hash = '" + req.@params[0].ToString() + "'";

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
    }

}




