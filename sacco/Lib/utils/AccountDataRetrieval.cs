// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
/// Allows to easily retrieve the data of an account based on the information
/// contained inside a given [Wallet].
//

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace sacco.Lib
{
    public class AccountDataRetrieval
    {
        #region Instance Variables

        // Use static HttpClient to avoid exhausting system resources for network connections.
        public static HttpClient client = new HttpClient();

        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods

        /// Reads the account endpoint and retrieves data from it.
        /// *** This is ported from Dart Future to C# Task<TResult> 
        /// *** The usage of the class should be mantained - to be checked
        public static async Task<AccountData> getAccountData(Wallet wallet)  
        {
            // Build the models.wallet api url
            String endpoint = $"{wallet.networkInfo.lcdUrl}/auth/accounts/{wallet.bech32Address}";
            
            // Get the server response
            HttpResponseMessage response = await client.GetAsync(endpoint);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                System.ArgumentException argEx = new System.ArgumentException($"Expected status code OK (200) but got ${response.StatusCode} - ${response.ReasonPhrase}");
                throw argEx;
            }

            // Parse the data
            String jsonResponse = await response.Content.ReadAsStringAsync();
            return parseServerReturnedAccount(jsonResponse);
        }


        #endregion

        #region Helpers

        // Moved the parsing code in a helper public method, so I can test it without net...
        public static AccountData parseServerReturnedAccount(String stringResponse)
        {
            AccountData wkAccount;
            Dictionary<String, Object> jsonResponse, json = null, value = null;
            List<StdCoin> coins = null;
            String accountNumber = "", sequence = "";
            Object outValue;

            // Parse the data
            JObject jsonObj = JObject.Parse(stringResponse);
            jsonResponse = jsonObj.ToObject<Dictionary<String, Object>>();
            // Get the "result" data from the response
            if (jsonResponse.TryGetValue("result", out outValue))
            {
                json = (outValue as JObject).ToObject<Dictionary<String, Object>>();
            }
            // Get the "value" data from the result
            if (json.TryGetValue("value", out outValue))
            {
                value = (outValue as JObject).ToObject<Dictionary<String, Object>>();
            }
            // get various data from the value
            if (value.TryGetValue("account_number", out outValue))
            {
                accountNumber = outValue as String;
            }
            if (value.TryGetValue("sequence", out outValue))
            {
                sequence = outValue as String;
            }
            // Get the coins - Careful, this is a List! so it's decoded as a JArray
            if (value.TryGetValue("coins", out outValue))
            {
                coins = (outValue as JArray).ToObject<List<StdCoin>>();
            }
            // Create the account
            wkAccount = new AccountData(accountNumber: accountNumber, sequence: sequence, coins: coins);

            return (wkAccount);
        }

        #endregion

    }
}
