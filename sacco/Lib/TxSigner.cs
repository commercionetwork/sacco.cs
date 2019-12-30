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

namespace sacco.Lib
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
            foreach (StdMsg msg in stdTx.messages)
            {
                signatures.Add(_getStdSignature(wallet, account, nodeInfo, msg, stdTx.fee, stdTx.memo));
            }

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

        private static StdSignature _getStdSignature(Wallet wallet, AccountData accountData, NodeInfo nodeInfo, StdMsg message, StdFee fee, String memo)
        {
            StdSignatureMessage signature = new StdSignatureMessage(
                chainId: nodeInfo.network,
                accountNumber: accountData.accountNumber,
                sequence: accountData.sequence,
                memo: memo,
                fee: fee.toJson(),
                msgs: new List<Dictionary<String, Object>>{ message.toJson() }
            );

            // Convert the signature to a JSON and sort it
            Dictionary<String, Object> jsonSignature = signature.toJson();
            Dictionary<String, Object> sortedJson = MapSorter.sort(jsonSignature);

            // Sign the message
            byte[] signatureData = wallet.signData(sortedJson);

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

    /*
    class TxSigner {
      /// Signs the given [stdTx] using the info contained inside the
      /// given [wallet] and returns a new [StdTx] containing the signatures
      /// inside it.
      static Future<StdTx> signStdTx({
        @required Wallet wallet,
        @required StdTx stdTx,
      }) async {
        // Get the account data and node info from the network
        final account = await AccountDataRetrieval.getAccountData(wallet);
        final nodeInfo = await NodeInfoRetrieval.getNodeInfo(wallet);

        // Sign each message
        final signatures = stdTx.messages
            .map((msg) => _getStdSignature(
                  wallet,
                  account,
                  nodeInfo,
                  msg,
                  stdTx.fee,
                  stdTx.memo,
                ))
            .toList();

        // Assemble the transaction
        return StdTx(
          fee: stdTx.fee,
          memo: stdTx.memo,
          messages: stdTx.messages,
          signatures: signatures,
        );
      }

      static StdSignature _getStdSignature(
        Wallet wallet,
        AccountData accountData,
        NodeInfo nodeInfo,
        StdMsg message,
        StdFee fee,
        String memo,
      ) {
        // Create the signature object
        final signature = StdSignatureMessage(
          sequence: accountData.sequence,
          accountNumber: accountData.accountNumber,
          chainId: nodeInfo.network,
          fee: fee.toJson(),
          memo: memo,
          msgs: [message.toJson()],
        );

        // Convert the signature to a JSON and sort it
        final jsonSignature = signature.toJson();
        final sortedJson = MapSorter.sort(jsonSignature);

        // Sign the message
        final signatureData = wallet.signData(sortedJson);

        // Get the compressed Base64 public key
        final pubKeyCompressed = wallet.ecPublicKey.Q.getEncoded(true);

        // Build the StdSignature
        return StdSignature(
          value: base64Encode(signatureData),
          publicKey: StdPublicKey(
            type: "tendermint/PubKeySecp256k1",
            value: base64Encode(pubKeyCompressed),
          ),
        );
      }
    }

     */

}
 
