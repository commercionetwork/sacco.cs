// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
/// [MsgSend] extends [StdMsg] and represents the message that should be
/// used when sending tokens from one user to another one.
/// It requires to specify the address from which to send the tokens,
/// the one that should receive the tokens and the amount of tokens
/// to send.
/// 
///

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;


namespace sacco.Lib
{
    public class MsgSend : StdMsg
    {
        #region Properties

        /// Coins that will be sent.
        public List<StdCoin> amount { get; private set; }
        /// Bech32 address of the sender.
        public String fromAddress { get; private set; }
        /// Bech32 address of the recipient.
        public String toAddress { get; private set; }

        // The override of the value getter is mandatory to obtain a correct codified Json
        public new Dictionary<String, Object> value
        {
            get
            {
                Dictionary<String, Object> wk = new Dictionary<String, Object>();
                wk.Add("from_address", this.fromAddress);
                wk.Add("to_address", this.toAddress);
                List<Dictionary<String, Object>> wkCoins = new List<Dictionary<String, Object>>();
                foreach (StdCoin coin in this.amount)
                {
                    Dictionary<String, Object> coinJson = coin.toJson();
                    wkCoins.Add(coinJson);
                }
                wk.Add("amount", wkCoins);
                return wk;
            }
        }

        #endregion

        #region Constructors

        /// Public constructor.
        public MsgSend(String fromAddress, String toAddress, List<StdCoin> amount)
        {
            Dictionary<String, Object> wk;

            Trace.Assert(fromAddress != null);
            Trace.Assert(toAddress != null);
            Trace.Assert(amount != null);
            // Assigns the properties
            this.fromAddress = fromAddress;
            this.toAddress = toAddress;
            this.amount = amount;

            wk = new Dictionary<String, Object>();
            wk.Add("from_address", this.fromAddress);
            wk.Add("to_address", this.toAddress);
            wk.Add("amount", this.amount);
            base.setProperties ("cosmos-sdk/MsgSend", wk);
        }

        #endregion

        #region Public Methods

        /// Converts this instance of [StdMsg] into a Dictionary that can be later used
        /// to serialize it as a JSON object.
        /// The override is necessary to get a correct JSon
        public new Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> output;
            output = new Dictionary<String, Object>();
            output.Add("type", this.type);
            // This is the override of the json returned!
            output.Add("value", this.value);
            return (output);
        }


        #endregion

        #region Helpers

        #endregion

        /*
        class MsgSend extends StdMsg {
          /// Bech32 address of the sender.
          final String fromAddress;

          /// Bech32 address of the recipient.
          final String toAddress;

          /// Coins that will be sent.
          final List<StdCoin> amount;

          /// Public constructor.
          MsgSend({
            @required this.fromAddress,
            @required this.toAddress,
            @required this.amount,
          })  : assert(fromAddress != null),
                assert(toAddress != null),
                assert(amount != null),
                super(type: "cosmos-sdk/MsgSend", value: Map());

          @override
          Map<String, dynamic> get value => {
                'from_address': this.fromAddress,
                'to_address': this.toAddress,
                'amount': this.amount.map((coin) => coin.toJson()).toList(),
              };
        }

        */

    }
}
