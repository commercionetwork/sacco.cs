using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using commercio.sacco.lib;



namespace sacco_test
{
    [TestClass]
    public class StdSignature_test
    {
        [TestMethod]
        public void TestJson()
        {
            StdSignature origSignature;
            Dictionary<String, Object> origJson;
            String outString;
            Object outValue;

            origSignature = new StdSignature(publicKey: new StdPublicKey("PublicKeyType", "PublicKeyValue"), value: "ValueSignatureSample");
            origJson = origSignature.toJson();

            if (origJson.TryGetValue("pub_key", out outValue))
            {
                outString = (outValue as StdPublicKey).type;
                Assert.AreEqual(outString, "PublicKeyType");
                outString = (outValue as StdPublicKey).value;
                Assert.AreEqual(outString, "PublicKeyValue");
            }
            if (origJson.TryGetValue("value", out outValue))
            {
                outString = outValue as String;
                Assert.AreEqual(outString, "ValueSignatureSample");
            }
        }
    }
}
