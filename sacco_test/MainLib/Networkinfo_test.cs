using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using sacco.Lib;


namespace sacco_test
{
    [TestClass]
    public class Networkinfo_test
    {
        [TestMethod]
        public void TestJson()
        {
            /*
            test('toJson and fromJson work properly with optional fields', () {
                final networkInfo = NetworkInfo(
                  bech32Hrp: "bech32",
                  lcdUrl: "lcd-url",
                  iconUrl: "icon-url",
                  name: "name",
            
                );

                final json = networkInfo.toJson();
                expect(networkInfo, NetworkInfo.fromJson(json));
            });
            */
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
            // Cannot use AreEqual on objects  as it test for reference equality - I do not want to override Equals for the class...
            // Assert.AreEqual(testNetworkInfo, recoveredNetworkInfo);
        }
    }
}
