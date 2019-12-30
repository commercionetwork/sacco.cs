using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using sacco.Lib;



namespace sacco_test
{
    [TestClass]
    public class MsgSend_Test
    {
        [TestMethod]
        public void TestSendMsg()
        {
            Dictionary<String, Object> wk, toJson;
            Object outValue;

            MsgSend message = new MsgSend (fromAddress: "cosmos1huydeevpz37sd9snkgul6070mstupukw00xkw9", 
                                           toAddress: "cosmos12lla7fg3hjd2zj6uvf4pqj7atx273klc487c5k",
                                           amount: new List<StdCoin> { new StdCoin(denom: "uatom", amount: "100") } );
            // Assert of various properties
            Assert.AreEqual(message.type, "cosmos-sdk/MsgSend");

            wk = message.value;
            if (wk.TryGetValue("from_address", out outValue))
            {
                String outString = outValue as String;
                Assert.AreEqual(outString, "cosmos1huydeevpz37sd9snkgul6070mstupukw00xkw9");
            }
            if (wk.TryGetValue("to_address", out outValue))
            {
                String outString = outValue as String;
                Assert.AreEqual(outString, "cosmos12lla7fg3hjd2zj6uvf4pqj7atx273klc487c5k");
            }
            if (wk.TryGetValue("amount", out outValue))
            {
                List<Dictionary<String, Object>> stdCoins = outValue as List<Dictionary<String, Object>>;
                foreach (var item in stdCoins)
                {
                    if (item.TryGetValue("amount", out outValue))
                    {
                        String outString = outValue as String;
                        Assert.AreEqual(outString, "100");
                    }
                }
            }

            toJson = message.toJson();
            if (toJson.TryGetValue("type", out outValue))
            {
                String outString = outValue as String;
                Assert.AreEqual(outString, "cosmos-sdk/MsgSend");
            }
            if (toJson.TryGetValue("value", out outValue))
            {
                Dictionary<String, Object> value = outValue as Dictionary<String, Object>;
                if (value.TryGetValue("from_address", out outValue))
                {
                    String outString = outValue as String;
                    Assert.AreEqual(outString, "cosmos1huydeevpz37sd9snkgul6070mstupukw00xkw9");
                }
                if (value.TryGetValue("to_address", out outValue))
                {
                    String outString = outValue as String;
                    Assert.AreEqual(outString, "cosmos12lla7fg3hjd2zj6uvf4pqj7atx273klc487c5k");
                }
                if (value.TryGetValue("amount", out outValue))
                {
                    List<Dictionary<String, Object>> stdCoins = outValue as List<Dictionary<String, Object>>;
                    foreach (var item in stdCoins)
                    {
                        if (item.TryGetValue("amount", out outValue))
                        {
                            String outString = outValue as String;
                            Assert.AreEqual(outString, "100");
                        }
                    }
                }
            }

            /*
             * test('MsgSend is built correctly', () {
                final message = MsgSend(
                  fromAddress: "cosmos1huydeevpz37sd9snkgul6070mstupukw00xkw9",
                  toAddress: "cosmos12lla7fg3hjd2zj6uvf4pqj7atx273klc487c5k",
                  amount: [StdCoin(denom: "uatom", amount: "100")],
                );

                expect(message.type, "cosmos-sdk/MsgSend");
                expect(message.value, {
                  "from_address": "cosmos1huydeevpz37sd9snkgul6070mstupukw00xkw9",
                  "to_address": "cosmos12lla7fg3hjd2zj6uvf4pqj7atx273klc487c5k",
                  "amount": [
                    {"amount": "100", "denom": "uatom"}
                  ]
                });
                expect(message.toJson(), {
                  "type": "cosmos-sdk/MsgSend",
                  "value": {
                    "from_address": "cosmos1huydeevpz37sd9snkgul6070mstupukw00xkw9",
                    "to_address": "cosmos12lla7fg3hjd2zj6uvf4pqj7atx273klc487c5k",
                    "amount": [
                      {"amount": "100", "denom": "uatom"}
                    ]
                  }
                });
              });
            */
        }
    }
}
