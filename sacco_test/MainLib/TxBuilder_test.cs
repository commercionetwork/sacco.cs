using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using sacco.Lib;


namespace sacco_test
{
    [TestClass]
    public class TxBuilder_test
    {
        [TestMethod]
        public void TestTxBuilder()
        {
            // Build the test Msg
            StdMsg message = new StdMsg(type: "cosmos-sdk/MsgSend",
                                        value: new Dictionary<String, Object>
                                        {
                                            { "from_address", "cosmos1huydeevpz37sd9snkgul6070mstupukw00xkw9" },
                                            { "to_address", "cosmos12lla7fg3hjd2zj6uvf4pqj7atx273klc487c5k" },
                                            { "amount", new List<StdCoin> { new StdCoin(denom: "uatom", amount: "100") }  }
                                        });

            // Read the expected test msg string from Test Resources (C# like - This is different from Dart approach)
            String expectedStrMsg = TestResources.SendStdTx;

            // Build the Msg
            List<StdMsg> msgList = new List<StdMsg> { message };
            // Call the builder
            StdTx txMsg = TxBuilder.buildStdTx(msgList);
            // Get the  String
            String strMsg = txMsg.ToString();
            // Check it
            Assert.AreEqual(strMsg, expectedStrMsg);
        }
    }
}
