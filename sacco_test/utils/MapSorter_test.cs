using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using commercio.sacco.lib;

namespace sacco_test
{
    [TestClass]
    public class MapSorter_test
    {
        [TestMethod]
        public void TestSortMap()
        {
            // Create an account as test Dictionary
            AccountData acData = new AccountData(accountNumber: "MyAccountNumber",
                                sequence: "MyAccountSequence",
                                coins: new List<StdCoin> { new StdCoin(denom: "zDenom1", amount: "zAmount1"), new StdCoin(denom: "aDenom2", amount: "aAmount2") });
            // Create the fake Server Answer (hope this is right!!)
            Dictionary<String, Object> fakeResult = new Dictionary<String, Object> { { "type", "cosmos-sdk/Account" }, { "value", acData.toJson() } };
            Dictionary<String, Object> fakeAnswer = new Dictionary<String, Object> { { "Height", "0" }, { "result", fakeResult } };

            Dictionary<String, Object> testDict = acData.toJson();
            Dictionary<String, Object> sortedDict = MapSorter.sort(testDict);

            // Verify the sorted output
            // Create a system Sorted Dictionary
            SortedDictionary<String, Object> systemSortedDict = new SortedDictionary<String, Object>(testDict);
            List<String> mySort = new List<String>(sortedDict.Keys);
            List<String> systemSort = new List<String>(systemSortedDict.Keys);
            //  Verify just the first level
            int index = 0;
            foreach (String item in mySort)
            {
                Assert.AreEqual(item, systemSort[index++]);
            }
        }
    }
}
