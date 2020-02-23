// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
/// Allows to easily sort a dictionary by its keys - ported by Dart that manages Map instead
//
using System;
using System.Diagnostics;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace commercio.sacco.lib
{
    public class TransactionSigner
    {
        #region Instance Variables

        private static readonly BigInteger _byteMask = new BigInteger ("ff", 16);
        private static readonly BigInteger _prime = new BigInteger("fffffffffffffffffffffffffffffffffffffffffffffffffffffffefffffc2f", 16);

        #endregion

        #region Properties

        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        public static byte[] deriveFrom(byte[] message, ECPrivateKeyParameters privateKey, ECPublicKeyParameters publicKey)
        {
            X9ECParameters curve = ECNamedCurveTable.GetByName("secp256k1");
            ECDomainParameters _params = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            BigInteger _halfCurveOrder = _params.N.ShiftRight(1);

            ECDsaSigner ecdsaSigner = new ECDsaSigner(new HMacDsaKCalculator(new Sha256Digest()));
            ecdsaSigner.Init(true, privateKey);

            ECSignature ecSignature = new ECSignature(ecdsaSigner.GenerateSignature(message));

            if (ecSignature.s.CompareTo(_halfCurveOrder) > 0)
            {
                BigInteger canonicalS = _params.N.Subtract(ecSignature.s);
                ecSignature = new ECSignature(ecSignature.r, canonicalS);
            }

            // Create a signer to check signature
            ECDsaSigner ecdsaChecker = new ECDsaSigner(new HMacDsaKCalculator(new Sha256Digest()));
            ecdsaChecker.Init(false, publicKey);
            bool signatureOK = ecdsaChecker.VerifySignature(message, ecSignature.r, ecSignature.s);
            if (signatureOK == false)
            {
                System.ArgumentException argEx = new System.ArgumentException("TransactionSigner - Error in checking signature!");
                throw argEx;
            }

            // 20200223 Rick - The code to recover the signature is somewhat bugged, and it loks like it's no use.
            // I replaced it with a check for the correcness of the signature above.
            // This code is commented out at the moment.
            //byte[] wkPublicKeyBytes = publicKey.Q.GetEncoded(false);
            //byte[] publicKeyBytes = new byte[wkPublicKeyBytes.Length - 1];
            //Array.Copy(wkPublicKeyBytes, 1, publicKeyBytes, 0, wkPublicKeyBytes.Length - 1);

            //BigInteger publicKeyBigInt = _bytesToInt(publicKeyBytes);

            //int recoveryID = -1;
            //for (int i = 0; i < 4; i++)
            //{
            //    BigInteger k = _recoverFromSignature(i, ecSignature, message, _params);
            //    // Need to check for null here!
            //    if (k != null)
            //    {
            //        if (k.CompareTo(publicKeyBigInt) == 0)
            //        {
            //            recoveryID = i;
            //            break;
            //        }
            //    }
            //    // 20200219 - Removed premature exit from loop
            //    // else
            //    //      break;
            //}
            //Debug.WriteLine($"****** _recoverFromSignature: recoveryId = {recoveryID}");

            //if (recoveryID == -1)
            //{
            //    System.ArgumentException argEx = new System.ArgumentException("Invalid recoverable key!");
            //    throw argEx;
            //}

            // Final assembly
            byte[] r = _intToBytes(ecSignature.r);
            byte[] s = _intToBytes(ecSignature.s);
            byte[] z = new byte[r.Length + s.Length];
            r.CopyTo(z, 0);
            s.CopyTo(z, r.Length);

            return z;
        }


        #endregion

        #region Helpers

        private static BigInteger _bytesToInt(byte[] bytes)
        {
            return(_decodeBigInt(bytes));
        }


        private static byte[] _intToBytes(BigInteger number)
        {
            return(_encodeBigInt(number));
        }

        private static byte[] _encodeBigInt(BigInteger number)
        {
            // We convert BigInt in a string of digits base 16, then to bytes
            return number.ToByteArrayUnsigned();
        }


        private static BigInteger _decodeBigInt(byte[] bytes)
        {
            // We convert bytes in a string of digits base 16, then to BigInt
            return (new BigInteger(HexEncDec.ByteArrayToString(bytes), (int)16));
        }

        private static BigInteger _recoverFromSignature(int recId, ECSignature sig, byte[] msg, ECDomainParameters parameters)
        {
            BigInteger n = parameters.N ;
            BigInteger i = new BigInteger((recId / 2).ToString());
            BigInteger x = sig.r.Add(i.Multiply(n));
            if (x.CompareTo(_prime) >= 0)
            {
                return null;
            }

            ECPoint R = _decompressKey(x, (recId & 1) == 1, parameters.Curve);
            if (!(R.Multiply(n)).IsInfinity)
            {
                return null;
            }

            BigInteger e = _bytesToInt(msg);

            BigInteger eInv = (BigInteger.Zero.Subtract(e));
            eInv = n.DivideAndRemainder(eInv)[1];
            BigInteger rInv = sig.r.ModInverse(n);
            BigInteger srInv = rInv.Multiply(sig.s).DivideAndRemainder(n)[1];
            BigInteger eInvrInv = (rInv.Multiply(eInv)).DivideAndRemainder(n)[1];

            ECPoint q = ((parameters.G).Multiply(eInvrInv)).Add(R.Multiply(srInv));

            byte[] bytes = q.GetEncoded(false);
            byte[] retBytes = new byte[bytes.Length - 1];
            Array.Copy(bytes, 1, retBytes, 0, bytes.Length - 1);
            return _bytesToInt(retBytes);
        }

        private static ECPoint _decompressKey(BigInteger xBN, bool yBit, ECCurve c)
        {
            // Odd way to arrange it...
            byte[] x9IntegerToBytes(BigInteger s, int qLength)
            {
                byte[] bytes = _intToBytes(s);

                byte[] tmp = new byte[qLength];
                // This should not be necessary, here just to be compliant with Dart code
                for (int i = 0; i < qLength; i++)
                    tmp[i] = 0;
                if (qLength < bytes.Length)
                {
                    for (int i = 0; i < qLength; i++)
                        tmp[i] = bytes[i];
                    return tmp;
                    // Looks like an error here - Should I have returned (sublist (0, qLength)?
                    // return bytes.sublist(0, bytes.Length - qLength);
                }
                else if (qLength > bytes.Length)
                {
                    int offset = qLength - bytes.Length;
                    for (int i = 0; i < bytes.Length; i++)
                        tmp[i + offset] = bytes[i];
                    return tmp;
                }

                return bytes;
            }

            byte[] compEnc = x9IntegerToBytes(xBN, 1 + ((c.FieldSize + 7) / 8));
            compEnc[0] = (byte) (yBit ? 0x03 : 0x02);
            return c.DecodePoint(compEnc);
        }


        #endregion

    }
}
