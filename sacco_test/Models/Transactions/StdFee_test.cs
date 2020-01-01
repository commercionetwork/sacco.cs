using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using System.Text;
using sacco.Lib;



namespace sacco_test
{
    [TestClass]
    public class StdFee_test
    {
        [TestMethod]
        public void TestJson()
        {
            StdFee origFee;
            StdCoin coin1, coin2;
            List<StdCoin> coinList, recoveredList;
            Dictionary<String, Object> origJson;


            coin1 = new StdCoin(denom: "PizzaDiFango", amount: "100");
            coin2 = new StdCoin(denom: "LifeUniverse", amount: "42");
            coinList = new List<StdCoin>();
            coinList.Add(coin1);
            coinList.Add(coin2);

            origFee = new StdFee(amount: coinList, gas: "0.1");

            origJson = origFee.toJson();

            recoveredList = origFee.amount;

            // Verify if the object is correct
            Assert.AreEqual(coinList, recoveredList);
        }
    }
}
