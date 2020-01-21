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
    public class StdSignature
    {
        #region Properties

        public StdPublicKey publicKey { get; }
        public String value { get; }

        #endregion

        #region Constructors

        public StdSignature(String value, StdPublicKey publicKey)
        {
            Trace.Assert(value != null);
            Trace.Assert(publicKey != null);
            this.value = value;
            this.publicKey = publicKey;
        }

        #endregion

        #region Public Methods

        /// Converts this instance of [StdMsg] into a Dictionary that can be later used
        /// to serialize it as a JSON object.
        public Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> output;

            output = new Dictionary<String, Object>();
            output.Add("pub_key", this.publicKey);
            output.Add("signature", this.value);
            return (output);
        }

        #endregion

        #region Helpers

        #endregion
    }
}
