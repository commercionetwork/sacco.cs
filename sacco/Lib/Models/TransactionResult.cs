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
    //  There is no such Class in C# - we include Compare-Net-Objects Nuget package for the purpose - see https://github.com/GregFinzer/Compare-Net-Objects
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
            Trace.Assert((success == true) || (error != null));
            this.hash = hash;
            this.success = success;
            this.error = error;
        }

        // Static constructor for returning a TransactionResult from json
        public static TransactionResult fromJson(Dictionary<String, Object> json)
        {
            Object outValue;
            String wkHash = null;
            bool wkSuccess = false;
            TransactionError wkError;
 
            if (json.TryGetValue("hash", out outValue))
                wkHash = outValue as String;
            if (json.TryGetValue("success", out outValue))
                wkSuccess = (bool) outValue;
            if (json.TryGetValue("error", out outValue))
                wkError = TransactionError.fromJson(outValue as Dictionary<String, Object>);
            else
                wkError = null;
            return new TransactionResult(wkHash, wkSuccess, wkError);
        }

        #endregion

        #region Public Methods

        public Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> wkDict = new Dictionary<String, Object>();

            wkDict.Add("hash", this.hash);
            wkDict.Add("success", this.success);
            if (this.error != null)
            {
                wkDict.Add("error", this.error.toJson());
            }
            return (wkDict);
        }

        #endregion

        #region Helpers

        #endregion

    }

    // *** This is inherited by Equatable in Dart Package!
    //  There is no such Class in C# - we include Compare-Net-Objects Nuget package for the purpose - see https://github.com/GregFinzer/Compare-Net-Objects
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

        // Static constructor for returning a TransactionResult from json
        public static TransactionError fromJson(Dictionary<String, Object> json)
        {
            Object outValue;
            int wkErrorCode = 0;
            String wkErrorMessage = null;

            if (json.TryGetValue("errorCode", out outValue))
                wkErrorCode = (int) outValue;
            if (json.TryGetValue("errorMessage", out outValue))
                wkErrorMessage = outValue as String;
            return new TransactionError(wkErrorCode, wkErrorMessage);
        }



        #endregion

        #region Public Methods

        public Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> wkDict = new Dictionary<String, Object>();

            wkDict.Add("errorCode", this.errorCode);
            wkDict.Add("errorMessage", this.errorMessage);
            return (wkDict);
        }

        #endregion

        #region Helpers

        #endregion

    }

}
