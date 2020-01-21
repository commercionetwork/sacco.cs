// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
/// [StdMsg] represents a standard message that can be included inside
/// a transaction.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace commercio.sacco.lib
{
    public class StdMsg
    {
        #region Properties

        /// String representing the type of the message.
        public String type { get; private set; }

          /// Map containing the real value of the message.
        public virtual Dictionary<String, Object> value { get; private set;  }

        #endregion

        #region Constructors

        // Public constructor
        [JsonConstructor]
        public StdMsg(String type, Dictionary<String, Object> value)
        {
            Trace.Assert(value != null);
            Trace.Assert(type != null);
            this.type = type;
            this.value = value;
        }

        // Constructor used by derived class - No parameter, just for init
        protected StdMsg()
        {
            this.type = null;
            this.value = null;
        }

        #endregion

        #region Public Methods

        /// Converts this instance of [StdMsg] into a Dictionary that can be later used
        /// to serialize it as a JSON object.
        public virtual Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> output;
            output = new Dictionary<String, Object>();
            output.Add("type", this.type);
            output.Add("value", this.value);
            return (output);
        }

        // Set properties used from derived class constructor to initialise the instance
        protected StdMsg setProperties(String type, Dictionary<String, Object> value)
        {
            Trace.Assert(value != null);
            Trace.Assert(type != null);
            this.type = type;
            this.value = value;
            return this;
        }

        #endregion

        #region Helpers

        #endregion

        /*
        class StdMsg {
          /// String representing the type of the message.
          final String type;

          /// Map containing the real value of the message.
          Map<String, dynamic> value;

          /// Public constructor.
          StdMsg({
            @required this.type,
            @required this.value,
          })  : assert(type != null),
                assert(value != null);

          /// Converts this instance of [StdMsg] into a map that can be later used
          /// to serialize it as a JSON object.
          Map<String, dynamic> toJson() => {
                'type': type,
                'value': value,
              };
        }
        */
    }
}
