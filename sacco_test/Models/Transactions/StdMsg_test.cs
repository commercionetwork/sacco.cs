using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using sacco.Lib;



namespace sacco_test
{
    [TestClass]
    public class StdMsg_test
    {
        [TestMethod]
        public void TestJson()
        {
            StdMsg origMsg;
            Dictionary<String, Object> origJson, msgRecoveredValue;

            origMsg = createMsg();

            origJson = origMsg.toJson();

            msgRecoveredValue = origMsg.value;

            // Verify if all the values in the Dictionary are the same
            foreach (var item in origJson)
            {
                Object outValue;
                String netString, recString;

                if (msgRecoveredValue.TryGetValue(item.Key, out outValue))
                {
                    netString = item.Value as String;
                    recString = outValue as String;
                    Assert.AreEqual(netString, recString);
                }
            }
        }

        // Method to create a msg
        public StdMsg createMsg()
        {
            StdMsg newMsg;

            newMsg = new StdMsg(type: "MessaggioTipo", value: new Dictionary<String, Object> { {"Msg1", "Primo messaggio di test" }, { "Msg2", "Secondo messaggio di test" } });
            return newMsg;
        }
    }
}
