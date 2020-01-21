// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
/// ECSignature - Elliptic Curve signature added to enhance compatibility between Dart PointyCastle Cryptolibrary porting
/// and original BouncyCastle crypto library (that uses a BigInteger array for this type of data)
//
//
using System;
using Org.BouncyCastle.Math;

namespace commercio.sacco.lib
{
    public class ECSignature
    {

        #region Properties

        public BigInteger r { get; set; }
        public BigInteger s { get; set; }

        #endregion

        #region Constructors

        public ECSignature(BigInteger r, BigInteger s)
        {
            this.r = r;
            this.s = s;
        }

        // This allow init of the object also by Std BouncyCastle Signers
        public ECSignature(BigInteger[] arr)
        {
            this.r = arr[0];
            this.s = arr[1];
        }

        #endregion

        #region Public Static Methods

        public static bool operator == (ECSignature first, ECSignature second)
        {
            if ((first == null) || (second == null))
                return false;
            if ((!(first is ECSignature)) || (!(second is ECSignature)))
                return false;
            return ((first.r == second.r) && (first.s == second.s));
        }

        public static bool operator != (ECSignature first, ECSignature second)
        {
            if ((first == null) || (second == null))
                return false;
            if ((!(first is ECSignature)) || (!(second is ECSignature)))
                return false;
            return ((first.r != second.r) || (first.s != second.s));
        }


        #endregion

        #region Public Instance Methods

        public override String ToString()
        {
            String str;

            str = $"({r.ToString()},${s.ToString()})";
            return str;
        }

        public override int GetHashCode()
        {
            return r.GetHashCode() + s.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is ECSignature))
                return false;
            ECSignature test = obj as ECSignature;
            return ((test.r == this.r) && (test.s == this.s));
        }

        #endregion

        #region Helpers

        #endregion

    }
}

