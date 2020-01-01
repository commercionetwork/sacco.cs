using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using sacco.Lib;


namespace sacco_test
{
    [TestClass]
    public class Wallet_test
    {
        // Const to be used for testing
        readonly NetworkInfo networkInfo = new NetworkInfo(bech32Hrp: "cosmos", lcdUrl: "");

        readonly Dictionary<String,String> testVectors = new Dictionary<String, String>
        {
            { "cosmos1huydeevpz37sd9snkgul6070mstupukw00xkw9",
                "final random flame cinnamon grunt hazard easily mutual resist pond solution define knife female tongue crime atom jaguar alert library best forum lesson rigid" },
            { "cosmos12lla7fg3hjd2zj6uvf4pqj7atx273klc487c5k",
                "must lottery surround bike cash option split aspect cram volume repeat goose enemy mouse ostrich crowd thing huge fiscal fuel canal tuna hair educate" },
            { "cosmos1wclftxxzt2sshqz0xtq4rxtk82wawyg6y2uafs",
                "pencil flat shed laundry idle phone glow hint dilemma roast bulb shop spice birth rigid project bar night song pluck then illegal obvious syrup" },
            { "cosmos1sc0ppj28frtyeqgs9gjk39lfd78507s3cu9e5k",
                "embrace subway again gift toilet price security ordinary zoo owner orbit age destroy invest little scheme crumble leisure remove muffin shoe deliver defy draw" },
            { "cosmos1l8yr93zkltwzdphd8g6073jgxslmf2pax7ml77",
                "garage jungle error orient puzzle crater cancel walk tissue fence dynamic bean aisle ring adult truth dog chapter claw six exhaust soda planet cycle" },
            { "cosmos1gmf3mqgxy6s89ac0n2uwxaw7ax5js88e7a5jgh",
                "seven confirm glass lawsuit flower test power rain animal argue fetch play local erupt curious certain february hover zone carpet pipe alarm capable box" },
            { "cosmos17c7nap702zdjwlqu6aystxy23kk252my4gkkfp",
                "minor craft between drive depart endorse fresh blade drill help skull hub evolve door sea comic pulse chicken awesome rebel leave series live brain" },
            { "cosmos1m87tazfksqu8d6cxwaexzg2e7w9a5q9nwjt2sc",
                "hurdle satisfy excess hub month great ordinary crane begin laugh evoke domain humor absent dawn blanket prefer ice ripple auto boost vast version soup" },
            { "cosmos1ase8zsfkqgvxw8yynfklq73u5utff0xxyzam58",
                "pipe apple lobster gadget front cloud reject whip village idle ready concert general scrub silver neutral crop oyster tackle enlist winner milk duty tomato" },
            { "cosmos1vwf547ntuvt69u46vzyzwwffmuxyhx9c7kx7st",
                "solve retire concert illegal garage recall skill power lyrics bunker vintage silver situate gadget talent settle left snow fire bubble bar robot swing senior" }
        };

        readonly String singleVector = "final random flame cinnamon grunt hazard easily mutual resist pond solution define knife female tongue crime atom jaguar alert library best forum lesson rigid";

        readonly String singleVector2 = "will hard topic spray beyond ostrich moral morning gas loyal couch horn boss across age post october blur piece wheel film notable word man";

        [TestMethod]
        public void TestWallet()
        {
            {
                foreach (var item in testVectors)
                {
                    List<String> mnemonic = new List<String>(item.Value.Split(" ", StringSplitOptions.RemoveEmptyEntries));
                    Wallet wallet = Wallet.derive(mnemonic, networkInfo);
                    // Check it
                    Assert.AreEqual(wallet.bech32Address, item.Key);
                };
            };
        }

        [TestMethod]
        public void TestJsonWallet()
        {
            //This is the comparison class
            CompareLogic compareLogic = new CompareLogic();

            List<String> mnemonic = new List<String>(singleVector.Split(" ", StringSplitOptions.RemoveEmptyEntries));
            Wallet wallet = Wallet.derive(mnemonic, networkInfo);
            Dictionary<String, Object> json = new Dictionary<String, Object>(wallet.toJson());
            byte[] privateKey = wallet.privateKey;
            Wallet retrievedWallet = Wallet.fromJson(json, privateKey);
            // Check it - we use compareNet objects here
            ComparisonResult result = compareLogic.Compare(wallet, retrievedWallet);
            Assert.AreEqual(result.AreEqual, true);
        }

        [TestMethod]
        public void TestWalletSignaturesNonDeterministic()
        {
            //This is the comparison class
            CompareLogic compareLogic = new CompareLogic();

            List<String> mnemonic = new List<String>(singleVector2.Split(" ", StringSplitOptions.RemoveEmptyEntries));
            NetworkInfo info = new NetworkInfo(bech32Hrp: "did:com:", lcdUrl: "");
            Wallet wallet = Wallet.derive(mnemonic, networkInfo);
            String data = "Quos Iupiter perdere vult, dementat prius";

            String signature1 = HexEncDec.ByteArrayToString(wallet.sign(Encoding.UTF8.GetBytes(data)));
            String signature2 = HexEncDec.ByteArrayToString(wallet.sign(Encoding.UTF8.GetBytes(data)));

            // Check it - we use compareNet objects here
            // a String comparison would have been enough...
            ComparisonResult result = compareLogic.Compare(signature1, signature2);
            Assert.AreEqual(result.AreEqual, false);
        }

    }
}
