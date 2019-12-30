// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;

namespace sacco.Lib
{
    // *** This is inherited by Equatable in Dart Package!
    public class TransactionResult
    {
        #region Properties
        /// String representing the hash of the transaction.
        /// Note that this hash is always present, even if the transaction was
        /// not sent successfully.
        public String hash { get; private set; }

        /// Tells if the transaction was sent successfully or not.
        public bool success { get; private set; }

        /// Tells which error has verified if the sending was not successful.
        /// Please note that this field is going to be:
        /// - `null` if [success] is `true`.
        /// - a valid [TransactionError] if [success] is `false`
        public TransactionError error { get; private set; }

        #endregion

        #region Constructors

        public TransactionResult(String hash, bool success, TransactionError error)
        {
            Trace.Assert(hash != null);
            Trace.Assert((success == true)|| (error != null));
            this.hash = hash;
            this.success = success;
            this.error = error;
        }

        #endregion

        #region Public Methods

        #endregion

        #region Helpers

        #endregion

    }

    // *** This is inherited by Equatable in Dart Package!
    public class TransactionError
    {
        #region Properties
        public int errorCode { get; private set; }
        public String errorMessage { get; private set; }

        #endregion

        #region Constructors

        public TransactionError(int errorCode, String errorMessage)
        {
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
        }

        #endregion

        #region Public Methods

        #endregion

        #region Helpers

        #endregion

    }
    /*
     * class TransactionResult extends Equatable {
      /// String representing the hash of the transaction.
      /// Note that this hash is always present, even if the transaction was
      /// not sent successfully.
      final String hash;

      /// Tells if the transaction was sent successfully or not.
      final bool success;

      /// Tells which error has verified if the sending was not successful.
      /// Please note that this field is going to be:
      /// - `null` if [success] is `true`.
      /// - a valid [TransactionError] if [success] is `false`
      final TransactionError error;

      TransactionResult({
        @required this.hash,
        @required this.success,
        this.error,
      })  : assert(hash != null),
            assert(success || error != null),
            super([hash, success, error]);
    }

    /// Contains the data related to an error that has occurred when
    /// broadcasting the transaction.
    class TransactionError extends Equatable {
      final int errorCode;
      final String errorMessage;

      TransactionError({
        @required this.errorCode,
        @required this.errorMessage,
      }) : super([errorCode, errorMessage]);
    }

    */

}
