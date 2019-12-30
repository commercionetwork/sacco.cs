using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using sacco.Lib;



namespace sacco_test
{
    [TestClass]
    public class StdPublicKey_test
    {
        [TestMethod]
        public void TestJson()
        {
            StdPublicKey origKey;
            Dictionary<String, Object> origJson;
            String outString;
            Object outValue;

            origKey = new StdPublicKey(type: "PublicKeyTypeSample", value: "KeyValueSample");
            origJson = origKey.toJson();

            if (origJson.TryGetValue("type", out outValue))
            {
                outString = outValue as String;
                Assert.AreEqual(outString, "PublicKeyTypeSample");
            }
            if (origJson.TryGetValue("value", out outValue))
            {
                outString = outValue as String;
                Assert.AreEqual(outString, "KeyValueSample");
            }
        }
    }
}
