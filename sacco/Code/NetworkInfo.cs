// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
// Contains the information of a generic Cosmos-based network.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace commercio.sacco.lib
{
    // *** This is inherited by Equatable in Dart Package!
    //  There is no such Class in C# - we include Compare-Net-Objects Nuget package for the purpose - see https://github.com/GregFinzer/Compare-Net-Objects
    public class NetworkInfo
    {
        #region Properties

        public String bech32Hrp { get; private set; }           // Bech32 human readable part
        public String lcdUrl { get; private set; }              // Url to call when accessing the LCD
        // Optional fields
        public String name { get; private set; }                // Human readable chain name
        public String iconUrl { get; private set; }             // Chain icon url
        public String defaultTokenDenom { get; private set; }

        #endregion

        #region Constructors

        // I need to mark this constructor in order to avoid confusion in deserialization
        [JsonConstructor]
        public NetworkInfo(String bech32Hrp, String lcdUrl, String name = "", String iconUrl = "", String defaultTokenDenom = "")
        {
            Trace.Assert(bech32Hrp != null);
            Trace.Assert(lcdUrl != null);
            this.bech32Hrp = bech32Hrp;
            this.lcdUrl = lcdUrl;
            this.name = name;
            this.iconUrl = iconUrl;
            this.defaultTokenDenom = defaultTokenDenom;
            /* *** Dart  Use super on equatable when available?
            super([bech32Hrp, lcdUrl, name, iconUrl, defaultTokenDenom]);
            */
        }

        // I abandoned static constructor from json - I am not sure this is the best way to do it, I'd prefer a standard constructor from a dictionary - athough, this was near to Dart approach
        // Std cosntructor from a dictionary - should work!!!
        public NetworkInfo (Dictionary<String, Object> json)
        {
            Object outValue;
            // String wkBech32Hrp = null, wkLcdUrl = null, wkName = null, wkIconUrl = null, wkDefaultTokenDenom = null;
            if (json.TryGetValue("bech32Hrp", out outValue))
                this.bech32Hrp = outValue as String;
            if (json.TryGetValue("lcd_url", out outValue))
                this.lcdUrl = outValue as String;
            if (json.TryGetValue("name", out outValue))
                this.name = outValue as String;
            if (json.TryGetValue("icon_url", out outValue))
                this.iconUrl = outValue as String;
            if (json.TryGetValue("default_token_denom", out outValue))
                this.defaultTokenDenom = outValue as String;
            // return new NetworkInfo(wkBech32Hrp, wkLcdUrl, wkName, wkIconUrl, wkDefaultTokenDenom);
        }


        #endregion

        #region Public Methods

        public Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> output;

            output = new Dictionary<String, Object>();
            output.Add("bech32Hrp", this.bech32Hrp);
            output.Add("lcd_url", this.lcdUrl);
            output.Add("name", this.name);
            output.Add("icon_url", this.iconUrl);
            output.Add("default_token_denom", this.defaultTokenDenom);
            return (output);
        }

        #endregion

        #region Helpers

        #endregion

    }
}
