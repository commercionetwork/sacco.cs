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

namespace commercio.sacco.lib
{
    public class NodeInfoRetrieval
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
        public static async Task<NodeInfo> getNodeInfo(Wallet wallet)
        {
            // Build the models.wallet api url
            String endpoint = $"{wallet.networkInfo.lcdUrl}/node_info";

            // Get the server response
            HttpResponseMessage response = await client.GetAsync(endpoint);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                System.ArgumentException argEx = new System.ArgumentException($"Expected status code OK (200) but got ${response.StatusCode} - ${response.ReasonPhrase}");
                throw argEx;
            }

            // Parse the data
            // Parse the data
            String jsonResponse = await response.Content.ReadAsStringAsync();
            return (parseServerNodeInfo(jsonResponse));
        }


        #endregion

        #region Helpers

        // Moved the parsing code in a helper public method, so I can test it without net...
        public static NodeInfo parseServerNodeInfo(String stringResponse)
        {
            NodeInfo wkNode;
            Dictionary<String, Object> jsonResponse, json = null;
            String networkName = "";
            Object outValue;

            // Parse the data
            JObject jsonObj = JObject.Parse(stringResponse);
            jsonResponse = jsonObj.ToObject<Dictionary<String, Object>>();
            // Get the "node_info" data from the response
            if (jsonResponse.TryGetValue("node_info", out outValue))
            {
                json = (outValue as JObject).ToObject<Dictionary<String, Object>>();
            }
            // Get the "network" data from the node_info
            if (json.TryGetValue("network", out outValue))
            {
                networkName = outValue as String;
            }
            // Create the NodeInfo
            wkNode = new NodeInfo(network: networkName);

            return (wkNode);
        }

        #endregion

    }
}
