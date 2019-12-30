// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
/// Allows to easily build and sign a [StdTx] that can later be sent over
/// the network.
//

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace sacco.Lib
{
    public class TxBuilder
    {
        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Public Methods
        // This is different from Dart implementation, as we are not initialising the optional parameters
        public static StdTx buildStdTx(List<StdMsg> stdMsgs, [Optional] String memo, [Optional] StdFee fee )
        {
            // Arrange optional parameters here
            if (memo == null)
                memo = "";
            if (fee == null)
            {
                fee = new StdFee(gas: "200000", amount: new List<StdCoin> { });
            }
            StdTx txMsg = new StdTx(messages: stdMsgs,
              memo: memo,
              fee: fee,
              signatures: null);


            return txMsg;
        }

    #endregion

    #region Helpers

    #endregion


    /*
     class TxBuilder {
      /// Builds a [StdTx] object containing the given [stdMsgs] and having the
      /// optional [memo] and [fee] specified.
      static StdTx buildStdTx({
        @required List<StdMsg> stdMsgs,
        String memo = "",
        StdFee fee = const StdFee(gas: "200000", amount: []),
      }) {
        return StdTx(
          messages: stdMsgs,
          memo: memo,
          fee: fee,
          signatures: null,
        );
      }
    

    */

    }
}
