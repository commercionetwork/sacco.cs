using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using sacco.Lib;

namespace sacco_test
{
    [TestClass]
    public class NodeInfoRetriever_test
    {
        [TestMethod]
        public void TestNodeInfoParsing()
        {
            // Create the fake Server Answer (hope this is right!!)
            Dictionary<String, Object> fakeAnswer = new Dictionary<String, Object>
            {
                { "node_info", new  Dictionary<String, Object>
                    {
                        { "protocol_version", new Dictionary<String, Object>
                            {
                                { "p2p", "7" },
                                { "block", "10" },
                                { "app", "0" }
                            }
                        },
                        { "id", "1e219e6878d07ddb875fe2732811107e002e236b"},
                        { "listen_addr", "tcp://0.0.0.0:26656"},
                        { "network", "cosmos-hub2"},
                        { "version", "0.32.2"},
                        { "channels", "4020212223303800"},
                        { "moniker", "edge"},
                        { "other",  new Dictionary<String, Object>
                            {
                                { "tx_index", "on" },
                                { "rpc_address", "tcp://0.0.0.0:26657" }
                            }
                        }
                    }
                },
                {
                    "application_version",  new  Dictionary<String, Object>
                    {
                        { "name", "" },
                        { "server_name", "\u003cappd\u003e" },
                        { "client_name", "\u003cappcli\u003e" } ,
                        { "version", "1.1.0" },
                        { "commit", "" },
                        { "build_tags", "" },
                        { "go", "go version go1.12.9 linux/amd64" }
                    }
                }
            };
            // json encode it in a test string
            String testResponse = JsonConvert.SerializeObject(fakeAnswer);

            // Test the method
            NodeInfo niParsed = NodeInfoRetrieval.parseServerNodeInfo(testResponse);

            // Verify the contents
            Assert.AreEqual(niParsed.network, "cosmos-hub2");
        }
    }
}
