﻿// 
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
    public class StdPublicKey
    {
        #region Properties

        [JsonProperty("type", Order = 1)]
        public String type { get; }
        [JsonProperty("value", Order = 2)]
        public String value { get; }

        #endregion

        #region Constructors

        public StdPublicKey(String type, String value)
        {
            Trace.Assert(value != null);
            Trace.Assert(type != null);
            this.type = type;
            this.value = value;
        }

        #endregion

        #region Public Methods

        /// Converts this instance of [StdMsg] into a Dictionary that can be later used
        /// to serialize it as a JSON object.
        public Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> output;

            output = new Dictionary<String, Object>();
            output.Add("type", this.type);
            output.Add("value", this.value);
            return (output);
        }

        #endregion

        #region Helpers

        #endregion
    }
}
