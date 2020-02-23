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
    public class AccountData
    {
        #region Properties

        [JsonProperty("accountNumber", Order = 1)]
        public String accountNumber { get; private set; }
        [JsonProperty("sequence", Order = 3)]
        public String sequence { get; private set; }
        [JsonProperty("coins", Order = 2)]
        public List<StdCoin> coins { get; private set; }

        #endregion

        #region Constructors

        public AccountData(String accountNumber, String sequence, List<StdCoin> coins)
        {
            Trace.Assert(accountNumber != null);
            Trace.Assert(sequence != null);
            Trace.Assert(coins != null);
            this.accountNumber = accountNumber;
            this.sequence = sequence;
            this.coins = coins;
        }

        #endregion

        #region Public Methods

        // *** Added to pass test on Account Data Retrieval - not clear how it can work in Dart
        public Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> output;

            output = new Dictionary<String, Object>();
            output.Add("account_number", this.accountNumber);
            output.Add("sequence", this.sequence);
            output.Add("coins", this.coins);
            return (output);
        }


        public override String ToString()
        {
            String output;

            output = $"number: {this.accountNumber}, sequence: {this.sequence}, coins: {this.coins}";
            return (output);
        }

        #endregion

        #region Helpers

        #endregion

    }
}
