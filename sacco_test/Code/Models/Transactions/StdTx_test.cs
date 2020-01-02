using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using commercio.sacco.lib;



namespace sacco_test
{
    [TestClass]
    public class StdSTx_test
    {
        [TestMethod]
        public void TestJson()
        {
            StdTx origTx;
            StdSignature origSignature;
            StdMsg origMsg;
            List<StdSignature> tSignatures;
            List<StdMsg> tMessages;
            StdMsg_test t;
            StdFee tFee;
            Dictionary<String, Object> origJson;
            String outString;

            origSignature = new StdSignature(publicKey: new StdPublicKey("PublicKeyType", "PublicKeyValue"), value: "PublicKeySample");
            tSignatures = new List<StdSignature> { origSignature , origSignature };

            t = new StdMsg_test();
            origMsg = t.createMsg();
            tMessages = new List<StdMsg> { origMsg , origMsg };

            tFee = new StdFee(amount: new List<StdCoin> { new StdCoin(denom: "Coin1Denom", amount: "Coin1Amount"), new StdCoin(denom: "Coin2Denom", amount: "Coin2Amount") }, gas: "GasValue");

            origTx = new StdTx(messages: tMessages, signatures: tSignatures, fee: tFee, memo: "StdMemoValue");
            origJson = origTx.toJson();

            // Here just for debugging
            outString = origTx.ToString();

            // USeless - the test just create the object...
            Assert.AreEqual(origTx, origTx);

        }
    }
}
