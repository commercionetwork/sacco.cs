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
    public class StdFee
    {
        #region Properties

        // Properties Read Only
        [JsonProperty("amount", Order = 1)]
        public List<StdCoin> amount { get; }
        [JsonProperty("gas", Order = 2)]
        public String gas { get;  }

        #endregion

        #region Constructors

        public StdFee(List<StdCoin> amount, String gas)
        {
            Trace.Assert(gas != null);
            this.amount = amount;
            this.gas = gas;
        }

        #endregion

        #region Public Methods

        public Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> output;
            List<Object> wkList;
            // I am forced to iterate explicitly
            output = new Dictionary<String, Object>();
            wkList = new List<Object>();
            foreach (StdCoin coin in this.amount)
            {
                wkList.Add(coin.toJson());
            }
            output.Add("amount", wkList);
            output.Add("gas", this.gas);
            return (output);
        }

        #endregion

        #region Helpers

        #endregion
    }
}
