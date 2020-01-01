using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using System.Text;
using sacco.Lib;



namespace sacco_test
{
    [TestClass]
    public class StdCoin_test
    {
        [TestMethod]
        public void TestJson()
        {
            StdCoin origCoin, recoveredCoin;
            Dictionary<String, Object> origJson, recJson;

            //This is the comparison class
            CompareLogic compareLogic = new CompareLogic();

            origCoin = new StdCoin(denom: "PizzaDiFango", amount: "100");

            origJson = origCoin.toJson();
            recoveredCoin = origCoin.fromJson(origJson);
            recJson = recoveredCoin.toJson();

            // Verify if the object is the same
            Assert.AreEqual(origCoin, recoveredCoin);
            // Verify if all the values in the Dictionary are the same
            foreach (var item in origJson)
            {
                Object outValue;
                String netString, recString;

                if (recJson.TryGetValue(item.Key, out outValue))
                {
                    netString = item.Value as String;
                    recString = outValue as String;
                    Assert.AreEqual(netString, recString);
                }
            }
            // Check it - we use compareNet objects here
            ComparisonResult result = compareLogic.Compare(origCoin, recoveredCoin);
            Assert.AreEqual(result.AreEqual, true);
        }
    }
}
