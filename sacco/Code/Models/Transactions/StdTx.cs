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
    public class StdTx
    {
        #region Properties

        public List<StdMsg> messages { get;  }
        public List<StdSignature> signatures { get;  }
        public StdFee fee { get;  }
        public String memo { get;  }

        #endregion

        #region Constructors

        public StdTx(List<StdMsg> messages, List<StdSignature> signatures, StdFee fee, String memo)
        {
            Trace.Assert(messages != null);
            Trace.Assert((signatures == null) || (signatures.Count > 0));
            Trace.Assert(fee != null);
            this.messages = messages;
            this.signatures = signatures;
            this.fee = fee;
            this.memo = memo;
        }

        #endregion

        #region Public Methods

        /// Converts this instance of [StdSignatureMessage] into a Dictionary that can be later used
        /// to serialize it as a JSON object.
        public Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> output;
            List<Object> wkList;
            // I am forced to iterate explicitly - Messages
            output = new Dictionary<String, Object>();
            wkList = new List<Object>();
            foreach (StdMsg msg in this.messages)
            {
                wkList.Add(msg.toJson());
            }
            output.Add("msg", wkList);
            output.Add("fee", this.fee.toJson());
            // I am forced to iterate explicitly - Signatures - ATTENTION - I need to account for "?" in Dart code - ToBecompleted
            // Signatures can be null!!
            if (signatures != null)
            {
                wkList = new List<Object>();
                foreach (StdSignature sig in this.signatures)
                {
                    wkList.Add(sig.toJson());
                }
                output.Add("signatures", wkList);
            }
            else
            {
                output.Add("signatures", null);
            }
            output.Add("memo", this.memo);
            return (output);
        }

        public override String ToString()
        {
            Dictionary<String, Object> tx;

            tx = new Dictionary<String, Object>();
            tx.Add("type", "cosmos-sdk/StdTx");
            tx.Add("value", this.toJson());
            return JsonConvert.SerializeObject(tx);
        }

    #endregion

    #region Helpers

    #endregion

    }
}
