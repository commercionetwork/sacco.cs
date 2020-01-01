using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using sacco.Lib;
using System.Net;
using System.Net.Http;
using Axe.SimpleHttpMock;
using Newtonsoft.Json;


namespace sacco_test
{
    [TestClass]
    public class TxSigner_test
    {
        private readonly String singleVector = "sibling auction sibling flavor judge foil tube dust work mixed crush action menu property project ride crouch hat mom scale start ill spare panther";
        private readonly String localbech32Hrp = "cosmos";
        private readonly String localTestUrl = "http://localhost:1317";

        [TestMethod]
        public void TestTxSigner()
        {

            List<String> mnemonic = new List<String>(singleVector.Split(" ", StringSplitOptions.RemoveEmptyEntries));
            // Create the network info
            NetworkInfo networkInfo = new NetworkInfo(bech32Hrp: localbech32Hrp, lcdUrl: localTestUrl);

            // Build a transaction
            MsgSend msg = new MsgSend(
                fromAddress: "cosmos1hafptm4zxy5nw8rd2pxyg83c5ls2v62tstzuv2",
                toAddress: "cosmos12lla7fg3hjd2zj6uvf4pqj7atx273klc487c5k",
                amount: new List<StdCoin> { new StdCoin(denom: "uatom", amount: "100") }
            );
            // Fee
            StdFee fee = new StdFee(
              gas: "200000",
              amount: new List<StdCoin> { new StdCoin(denom: "uatom", amount: "250") }
            );
            StdTx tx = TxBuilder.buildStdTx(stdMsgs: new List<StdMsg> { msg }, fee: fee);
            // Create a wallet
            Wallet wallet = Wallet.derive(mnemonic, networkInfo);

            // Verify Wallet
            Assert.AreEqual(wallet.networkInfo.bech32Hrp, networkInfo.bech32Hrp);
            Assert.AreEqual(wallet.networkInfo.lcdUrl, networkInfo.lcdUrl);

            // Build the mockup server
            var _server = new MockHttpServer();
            //  I need this in order to get the correct data out of the mock server
            Dictionary<String, Object> accResponse = JsonConvert.DeserializeObject<Dictionary<String, Object>>(TestResources.AccountDataResponse);
            Dictionary<String, Object> NodeResponse = JsonConvert.DeserializeObject<Dictionary<String, Object>>(TestResources.NodeInfoResponse);
            // Initialize Server Response
            _server
                .WithService(localTestUrl)
                .Api("auth/accounts/{wallettAddress}", accResponse)
                .Api("node_info", NodeResponse);

            // Link the client to the retrieval classes
            HttpClient client = new HttpClient(_server);
            AccountDataRetrieval.client = client;
            NodeInfoRetrieval.client = client;

            // Call without await to avoid marking test class as async
            StdTx signedTx = TxSigner.signStdTx(wallet: wallet, stdTx: tx).Result;
            Assert.AreEqual(signedTx.signatures.Count, 1);

            StdSignature signature = (signedTx.signatures.ToArray())[0];
            Assert.AreEqual(signature.publicKey.type, "tendermint/PubKeySecp256k1");
            Assert.AreEqual(signature.publicKey.value, "ArMO2T5FNKkeF2aAZY012p/cpa9+PqKqw2GcQRPhAn3w");
            Assert.AreEqual(signature.value, "m2op4CCBa39fRZD91WiqtBLKbUQI+1OWsc1tJkpDg+8FYB4y51KahGn26MskVMpTJl5gToIC1pX26hLbW1Kxrg==");
        }
    }
}
