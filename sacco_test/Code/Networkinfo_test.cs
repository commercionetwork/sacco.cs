using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using commercio.sacco.lib;


namespace sacco_test
{
    [TestClass]
    public class Networkinfo_test
    {
        [TestMethod]
        public void TestJson()
        {
            //This is the comparison class
            CompareLogic compareLogic = new CompareLogic();

            NetworkInfo testNetworkInfo, recoveredNetworkInfo;
            Dictionary<String, Object> netJson, recJson;

            testNetworkInfo = new NetworkInfo("test_bech32", "test_lcdUrl", "test_name", "test_iconUrl");

            netJson = testNetworkInfo.toJson();
            recoveredNetworkInfo = NetworkInfo.fromJson(netJson);
            recJson = recoveredNetworkInfo.toJson();
            
            // Verify if all the values in the Dictionary are the same
            foreach (var item in netJson)
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
            ComparisonResult result = compareLogic.Compare(testNetworkInfo, recoveredNetworkInfo);
            Assert.AreEqual(result.AreEqual, true);
        }
    }
}
