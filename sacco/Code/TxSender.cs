// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
/// Broadcasts the given [stdTx] using the info contained
/// inside the given [wallet].
/// Returns the hash of the transaction once it has been send, or throws an
/// exception if an error is risen during the sending.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace commercio.sacco.lib
{
    public class TxSender
    {
        #region Instance Variables

        // Use static HttpClient to avoid exhausting system resources for network connections.
        private static HttpClient client = new HttpClient();

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Public Methods
        /// *** This is ported from Dart Future to C# Task<TResult> 
        /// *** The usage of the class should be mantained - to be checked
        public static async Task<TransactionResult> broadcastStdTx(Wallet wallet, StdTx stdTx, String mode = "sync")
        {
            // Get the endpoint
            String apiUrl = $"{wallet.networkInfo.lcdUrl}/txs";

            // Build the request body
            Dictionary<String, Object> requestBody = new Dictionary<String, Object>
            {
                { "tx", stdTx.toJson() },
                { "mode", mode}
            };
            String payload = JsonConvert.SerializeObject(requestBody);

            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
            // Get the server response
            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                System.ArgumentException argEx = new System.ArgumentException($"Expected status code OK (200) but got ${response.StatusCode} - ${response.ReasonPhrase} - ${response.Content}");
                throw argEx;
            }

            // Convert the response
            String encodedJson = await response.Content.ReadAsStringAsync();
            Dictionary<String, Object> json = JsonConvert.DeserializeObject<Dictionary<String, Object>>(encodedJson);
            return _convertJson(json);
        }

        #endregion

        #region Helpers

        /// Converts the given [json] to a [TransactionResult] object.
        private static TransactionResult _convertJson(Dictionary<String, Object> json)
        {
            Object outValue, jsonRawLog;
            List<Object> ListRawLog;
            Dictionary<String, Object> rawlog;
            Boolean successResult;

            if (json.TryGetValue("raw_log", out jsonRawLog))
            {
                // Some error happened - report it
                int errCode = 0;
                successResult = false;
                String message = "No Message Reported", hash = "";
                if (json.TryGetValue("txhash", out outValue))
                    hash = outValue as String;
                if (json.TryGetValue("code", out outValue))
                    errCode = Convert.ToInt32(outValue);   // 20200217 - Careful here - the errorcode returned is a 64bit integer!
                // 20200217 - Get the details about the error
                try
                {
                    rawlog = new Dictionary<String, Object>();
                    try
                    {
                        // First Try - Assume we have a list of rawlogs
                        ListRawLog = JsonConvert.DeserializeObject<List<Object>>(jsonRawLog as string);
                        // Get the first element of the list
                        rawlog = JsonConvert.DeserializeObject<Dictionary<String, Object>>(ListRawLog[0].ToString());
                    }
                    catch
                    {
                        // Second try - we have just a single rawlog
                        rawlog = JsonConvert.DeserializeObject<Dictionary<String, Object>>(jsonRawLog.ToString());
                    }
                    finally
                    {
                        // Here we get the details of the message
                        if (rawlog.TryGetValue("message", out outValue))
                            message = outValue as String;
                        if (rawlog.TryGetValue("success", out outValue))
                            successResult = (Boolean)outValue;
                    }
                }
                catch
                {
                    // Here if we have a failure in decoding json
                    message = $"Internal error - unable to decode message result - '${jsonRawLog.ToString()}'";
                    successResult = false;
                }

                return new TransactionResult(
                    hash: hash,
                    success: successResult,
                    error: new TransactionError(
                        errorCode: errCode,
                        errorMessage: message
                    )
                );
            }
            else
            {
                // Otherwise, result OK
                String hash = "";
                if (json.TryGetValue("txhash", out outValue))
                    hash = outValue as String;
                return new TransactionResult(
                    hash: hash,
                    success: true,
                    error: null
                );
            }
        }


        #endregion

    }
}
