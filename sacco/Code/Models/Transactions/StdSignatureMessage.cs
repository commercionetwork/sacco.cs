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


namespace commercio.sacco.lib
{
    public class StdSignatureMessage
    {
        #region Properties

        public String chainId { get;  }
        public String accountNumber { get;  }
        public String sequence { get;  }
        public String memo { get;  }
        public Dictionary<String, Object> fee { get;  }
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
