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

namespace sacco.Lib
{
    // *** This is inherited by Equatable in Dart Package!
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

        // Static constructor from json - I am not sure this is the best way to do it, I'd prefer a standard constructor from a dictionary - however, this is near to Dart approach
        public static NetworkInfo fromJson(Dictionary<String, Object> json)
        {
            Object outValue;
            String wkBech32Hrp = null, wkLcdUrl = null, wkName = null, wkIconUrl = null, wkDefaultTokenDenom = null;
            if (json.TryGetValue("bech32Hrp", out outValue))
                wkBech32Hrp = outValue as String;
            if (json.TryGetValue("lcd_url", out outValue))
                wkLcdUrl = outValue as String;
            if (json.TryGetValue("name", out outValue))
                wkName = outValue as String;
            if (json.TryGetValue("icon_url", out outValue))
                wkIconUrl = outValue as String;
            if (json.TryGetValue("default_token_denom", out outValue))
                wkDefaultTokenDenom = outValue as String;
            return new NetworkInfo(wkBech32Hrp, wkLcdUrl, wkName, wkIconUrl, wkDefaultTokenDenom);
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

        /*
        class NetworkInfo extends Equatable {
         final String bech32Hrp; // Bech32 human readable part
         final String lcdUrl; // Url to call when accessing the LCD

         // Optional fields
         final String name; // Human readable chain name
         final String iconUrl; // Chain icon url
         final String defaultTokenDenom;

         NetworkInfo({
           @required this.bech32Hrp,
           @required this.lcdUrl,
           this.name = "",
           this.iconUrl = "",
           this.defaultTokenDenom,
         })  : assert(bech32Hrp != null),
               assert(lcdUrl != null),
               super([bech32Hrp, lcdUrl, name, iconUrl, defaultTokenDenom]);

         factory NetworkInfo.fromJson(Map<String, dynamic> json) {
           return NetworkInfo(
             bech32Hrp: json['bech32_hrp'] as String,
             lcdUrl: json['lcd_url'] as String,
             name: json['name'] as String,
             iconUrl: json['icon_url'] as String,
             defaultTokenDenom: json['default_token_denom'] as String,
           );
         }

         Map<String, dynamic> toJson() => <String, dynamic>{
               'bech32_hrp': this.bech32Hrp,
               'lcd_url': this.lcdUrl,
               'name': this.name,
               'icon_url': this.iconUrl,
               'default_token_denom': this.defaultTokenDenom,
             };
       }

        */
    }
}
