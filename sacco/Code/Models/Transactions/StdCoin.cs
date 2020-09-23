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
using Newtonsoft.Json.Linq;

namespace commercio.sacco.lib
{
    public class StdCoin
    {
        #region Properties

        [JsonProperty("amount", Order = 1)]
        public String amount { get; private set; }
        [JsonProperty("denom", Order = 2)]
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

        // Constructor from a Dictionary
        public StdCoin (Dictionary<String, Object> json)
        {
            Object outValue;
            if (json.TryGetValue("denom", out outValue))
                this.denom = outValue as String;
            if (json.TryGetValue("amount", out outValue))
                this.amount = outValue as String;
        }

        // Alternate Constructor from a Json JObject
        public StdCoin(JObject json)
        {
            this.denom = (String)json["denom"];
            this.amount = (String)json["amount"];

            //Object outValue;
            //if (json.TryGetValue("denom", out outValue))
            //    this.denom = outValue as String;
            //if (json.TryGetValue("amount", out outValue))
            //    this.amount = outValue as String;
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
