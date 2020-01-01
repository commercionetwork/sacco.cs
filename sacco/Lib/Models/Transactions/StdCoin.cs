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
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;


namespace sacco.Lib
{
    public class StdCoin
    {
        #region Properties

        public String amount { get; private set; }
        public String denom { get; private set; }

        #endregion

        #region Constructors

        public StdCoin(String denom, String amount)
        {
            Trace.Assert(denom != null);
            Trace.Assert(amount != null);
            this.denom = denom;
            this.amount = amount;
        }

        #endregion

        #region Public Methods
          
        public StdCoin fromJson(Dictionary<String, Object> json)
        {
            Object outValue;
            if (json.TryGetValue("denom", out outValue))
                this.denom = outValue as String;
            if (json.TryGetValue("amount", out outValue))
                this.amount = outValue as String;
            return this;
        }


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
