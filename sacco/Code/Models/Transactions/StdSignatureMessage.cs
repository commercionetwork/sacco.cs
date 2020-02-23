// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace commercio.sacco.lib
{
    public class StdSignatureMessage
    {
        #region Properties

        [JsonProperty("chainId", Order = 2)]
        public String chainId { get;  }
        [JsonProperty("accountNumber", Order = 1)]
        public String accountNumber { get;  }
        [JsonProperty("sequence", Order = 6)]
        public String sequence { get;  }
        [JsonProperty("memo", Order = 4)]
        public String memo { get;  }
        [JsonProperty("fee", Order = 3)]
        public Dictionary<String, Object> fee { get;  }
        [JsonProperty("msgs", Order = 5)]
        public List<Dictionary<String, Object>> msgs { get;  }

        #endregion

        #region Constructors

        public StdSignatureMessage(String chainId, String accountNumber, String sequence, String memo, Dictionary<String, Object> fee, List<Dictionary<String, Object>> msgs)
        {
            Trace.Assert(chainId != null);
            Trace.Assert(accountNumber != null);
            Trace.Assert(sequence != null);
            Trace.Assert(msgs != null);
            this.chainId = chainId;
            this.accountNumber = accountNumber;
            this.sequence = sequence;
            this.memo = memo;
            this.fee = fee;
            this.msgs = msgs;
        }

        #endregion

        #region Public Methods

        /// Converts this instance of [StdSignatureMessage] into a Dictionary that can be later used
        /// to serialize it as a JSON object.
        public Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> output;

            output = new Dictionary<String, Object>();
            output.Add("chain_id", this.chainId);
            output.Add("account_number", this.accountNumber);
            output.Add("sequence", this.sequence);
            output.Add("memo", this.memo);
            output.Add("fee", this.fee);
            output.Add("msgs", this.msgs);
            return (output);
        }

        #endregion

        #region Helpers

        #endregion
    }
}
