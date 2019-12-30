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

namespace sacco.Lib
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
        public static async Task<TransactionResult> broadcastStdTx(Wallet wallet, StdTx stdTx)
        {
            // Get the endpoint
            String apiUrl = $"{wallet.networkInfo.lcdUrl}/txs";

            // Build the request body
            Dictionary<String, Object> requestBody = new Dictionary<String, Object>
            {
                { "tx", stdTx.toJson() },
                { "mode", "sync"}
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
            Object outValue;

            if (json.TryGetValue("raw_log", out outValue))
            {
                // Some error happened - report it
                int errCode = 0;
                String message = "", hash = "";

                Dictionary<String, Object> rawLog = outValue as Dictionary<String, Object>;
                if (json.TryGetValue("txhash", out outValue))
                    hash = outValue as String;
                if (json.TryGetValue("code", out outValue))
                    errCode = (int)outValue;
                if (json.TryGetValue("message", out outValue))
                    message = outValue as String;

                return new TransactionResult(
                    hash: hash,
                    success: false,
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

        /*
        class TxSender {
          /// Broadcasts the given [stdTx] using the info contained
          /// inside the given [wallet].
          /// Returns the hash of the transaction once it has been send, or throws an
          /// exception if an error is risen during the sending.
          static Future<TransactionResult> broadcastStdTx({
            @required Wallet wallet,
            @required StdTx stdTx,
          }) async {
            // Get the endpoint
            final apiUrl = "${wallet.networkInfo.lcdUrl}/txs";

            // Build the request body
            final requestBody = {"tx": stdTx.toJson(), "mode": "sync"};
            final requestBodyJson = jsonEncode(requestBody);

            // Get the response
            final response = await http.Client().post(apiUrl, body: requestBodyJson);
            if (response.statusCode != 200) {
              throw Exception(
                "Expected status code 200 but got ${response.statusCode} - ${response.body}",
              );
            }

            // Convert the response
            final json = jsonDecode(response.body) as Map<String, dynamic>;
            return _convertJson(json);
          }

          /// Converts the given [json] to a [TransactionResult] object.
          static TransactionResult _convertJson(Map<String, dynamic> json) {
            final rawLog = jsonDecode(json["raw_log"] as String) as dynamic;
            if (rawLog is Map<String, dynamic>) {
              return TransactionResult(
                hash: json["txhash"],
                success: false,
                error: TransactionError(
                  errorCode: rawLog["code"] as int,
                  errorMessage: rawLog["message"] as String,
                ),
              );
            }

            return TransactionResult(hash: json["txhash"], success: true, error: null);
          }
        }

        */
    }
}
