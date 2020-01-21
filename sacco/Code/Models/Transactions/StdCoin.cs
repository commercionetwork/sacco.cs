// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
/// Contains the data of a specific coin
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace commercio.sacco.lib
{
    public class StdCoin
    {
        #region Properties

        public String amount { get; private set; }
        public String denom { get; private set; }

        #endregion

        #region Constructors

        // I need to mark this constructor in order to avoid confusion in deserialization
        [JsonConstructor]
        public StdCoin(String denom, String amount)
        {
            Trace.Assert(denom != null);
            Trace.Assert(amount != null);
            this.denom = denom;
            this.amount = amount;
        }

        public StdCoin (Dictionary<String, Object> json)
        {
            Object outValue;
            if (json.TryGetValue("denom", out outValue))
                this.denom = outValue as String;
            if (json.TryGetValue("amount", out outValue))
                this.amount = outValue as String;
        }

        #endregion

        #region Public Methods


        public Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> output;

            output = new Dictionary<String, Object>();
            output.Add("denom", this.denom);
            output.Add("amount", this.amount);
            return (output);
        }

        #endregion

        #region Helpers

        #endregion
    }
}
