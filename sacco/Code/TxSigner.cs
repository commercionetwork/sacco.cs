// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
/// Allows to easily sign a [StdTx] object that already contains a message.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace commercio.sacco.lib
{
    public class TxSigner
    {
        #region Instance Variables

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Public Methods

        /// Reads the account endpoint and retrieves data from it.
        /// *** This is ported from Dart Future to C# Task<TResult> 
        /// *** The usage of the class should be mantained - to be checked
        public static async Task<StdTx> signStdTx(Wallet wallet, StdTx stdTx)
        {
            // Get the account data and node info from the network
            AccountData account = await AccountDataRetrieval.getAccountData(wallet);
            NodeInfo nodeInfo = await NodeInfoRetrieval.getNodeInfo(wallet);

            // Sign each message
            List<StdSignature> signatures = new List<StdSignature>();
            // This was corrected in Dart version 25/11/2019
            //foreach (StdMsg msg in stdTx.messages)
            //{
            //    signatures.Add(_getStdSignature(wallet, account, nodeInfo, msg, stdTx.fee, stdTx.memo));
            //}
            signatures.Add(_getStdSignature(wallet, account, nodeInfo, stdTx.messages, stdTx.fee, stdTx.memo));


            // Assemble the transaction
            return new StdTx(
              fee: stdTx.fee,
              memo: stdTx.memo,
              messages: stdTx.messages,
              signatures: signatures
            );
        }
        #endregion

        #region Helpers

        private static StdSignature _getStdSignature(Wallet wallet, AccountData accountData, NodeInfo nodeInfo, List<StdMsg> messages, StdFee fee, String memo)
        {
            // Arrange the msg in a list of Json
            List<Dictionary<String, Object>> msgList = new List<Dictionary<String, Object>>();
            foreach (StdMsg msg in messages)
                msgList.Add(msg.toJson());

            StdSignatureMessage signature = new StdSignatureMessage(
                chainId: nodeInfo.network,
                accountNumber: accountData.accountNumber,
                sequence: accountData.sequence,
                memo: memo,
                fee: fee.toJson(),
                msgs: msgList
            );

            // Convert the signature to a JSON and sort it
            Dictionary<String, Object> jsonSignature = signature.toJson();
            Dictionary<String, Object> sortedJson = MapSorter.sort(jsonSignature);
            // Encode the sorted JSON to a string
            String jsonData = JsonConvert.SerializeObject(sortedJson);
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(jsonData);
           
            // Sign the message
            byte[] signatureData = wallet.signTxData(utf8Bytes);

            // Get the compressed Base64 public key
            byte[] pubKeyCompressed = wallet.ecPublicKey.Q.GetEncoded(true);

            // Build the StdSignature
            return new StdSignature(
                value: Convert.ToBase64String(signatureData),
                publicKey: new StdPublicKey(
                    type: "tendermint/PubKeySecp256k1",
                    value: Convert.ToBase64String(pubKeyCompressed)
                )
            );
        }

        #endregion
    }
}
 
