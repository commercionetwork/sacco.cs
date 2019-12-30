using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using sacco.Lib;



namespace sacco_test
{
    [TestClass]
    public class StdSignatureMessage_test
    {
        [TestMethod]
        public void TestJson()
        {
            StdSignatureMessage origSignatureMsg;
            Dictionary<String, Object> testFee, testMsg1, testMsg2, origJson, outFee;
            List<Dictionary<String, Object>> testMsgs, outMsgs;
            String outString;
            Object outValue;

            testFee = new Dictionary<String, Object>();
            testFee.Add("TestFeeKey1", "TestFeeValue1");
            testFee.Add("TestFeeKey2", "TestFeeValue2");

            testMsg1 = new Dictionary<String, Object>();
            testMsg1.Add("TestMsg1Key1", "TestMsg1Value1");
            testMsg1.Add("TestMsg1Key2", "TestMsg1Value2");
            testMsg2 = new Dictionary<String, Object>();
            testMsg2.Add("TestMsg2Key1", "TestMsg2Value1");
            testMsg2.Add("TestMsg2Key2", "TestMsg2Value2");
            testMsgs = new List<Dictionary<String, Object>>();
            testMsgs.Add(testMsg1);
            testMsgs.Add(testMsg2);

            origSignatureMsg = new StdSignatureMessage("TestChainId", "TestAccountNumber", "TestSequence", "TestMemo", testFee, testMsgs);

            origJson = origSignatureMsg.toJson();

            if (origJson.TryGetValue("chain_id", out outValue))
            {
                outString = outValue as String;
                Assert.AreEqual(outString, "TestChainId");
            }
            if (origJson.TryGetValue("account_number", out outValue))
            {
                outString = outValue as String;
                Assert.AreEqual(outString, "TestAccountNumber");
            }
            if (origJson.TryGetValue("sequence", out outValue))
            {
                outString = outValue as String;
                Assert.AreEqual(outString, "TestSequence");
            }
            if (origJson.TryGetValue("memo", out outValue))
            {
                outString = outValue as String;
                Assert.AreEqual(outString, "TestMemo");
            }
            if (origJson.TryGetValue("fee", out outValue))
            {
                outFee = outValue as Dictionary<String, Object>;
                Assert.AreEqual(testFee, outFee);
            }
            if (origJson.TryGetValue("msgs", out outValue))
            {
                outMsgs = outValue as List<Dictionary<String, Object>>;
                Assert.AreEqual(testMsgs, outMsgs);
            }
        }
    }
}
