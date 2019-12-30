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
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;


namespace sacco.Lib
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
            var curve = ECNamedCurveTable.GetByName("secp256k1");
            ECDomainParameters _params = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            BigInteger _halfCurveOrder = _params.N.ShiftRight(1);

            ECDsaSigner ecdsaSigner = new ECDsaSigner(new HMacDsaKCalculator (new Sha256Digest( )));
            ecdsaSigner.Init(true, privateKey) ;

            ECSignature ecSignature = new ECSignature(ecdsaSigner.GenerateSignature(message));

            if (ecSignature.s.CompareTo(_halfCurveOrder) > 0)
            {
                BigInteger canonicalS = _params.N.Subtract(ecSignature.s);
                ecSignature = new ECSignature(ecSignature.r, canonicalS);
            }

            byte[] wkPublicKeyBytes = publicKey.Q.GetEncoded(false);
            byte[] publicKeyBytes = new byte[wkPublicKeyBytes.Length-1];
            Array.Copy(wkPublicKeyBytes, 1, publicKeyBytes, 0, wkPublicKeyBytes.Length - 1);

            BigInteger publicKeyBigInt = _bytesToInt(publicKeyBytes);

            int recoveryID = -1;
            for (int i = 0; i < 4; i++)
            {
                BigInteger k = _recoverFromSignature(i, ecSignature, message, _params);
                // Need to check for null here!
                if (k != null)
                {
                    if (k.CompareTo(publicKeyBigInt) == 0)
                    {
                        recoveryID = i;
                        break;
                    }
                }
                else
                    break;
            }

            if (recoveryID == -1)
            {
                System.ArgumentException argEx = new System.ArgumentException("Invalid recoverable key!");
                throw argEx;
            }

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

            ECPoint q = (parameters.G).Multiply(eInvrInv).Add(R.Multiply(srInv));

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

        /*
        class TransactionSigner {
          // Constants
          static final BigInt _byteMask = BigInt.from(0xff);
          static final BigInt _prime = BigInt.parse(
            'fffffffffffffffffffffffffffffffffffffffffffffffffffffffefffffc2f',
            radix: 16,
          );

          static BigInt _bytesToInt(List<int> bytes) => _decodeBigInt(bytes);

          static Uint8List _intToBytes(BigInt number) => _encodeBigInt(number);

          static Uint8List _encodeBigInt(BigInt number) {
            final size = (number.bitLength + 7) >> 3;
            final result = Uint8List(size);
            var num = number;
            for (var i = 0; i < size; i++) {
              result[size - i - 1] = (num & _byteMask).toInt();
              num = num >> 8;
            }
            return result;
          }

          static BigInt _decodeBigInt(Uint8List bytes) {
            var result = BigInt.from(0);
            for (var i = 0; i < bytes.length; i++) {
              result += BigInt.from(bytes[bytes.length - i - 1]) << (8 * i);
            }
            return result;
          }

          static BigInt _recoverFromSignature(
              int recId, ECSignature sig, Uint8List msg, ECDomainParameters params) {
            final n = params.n;
            final i = BigInt.from(recId ~/ 2);
            final x = sig.r + (i * n);

            if (x.compareTo(_prime) >= 0) {
              return null;
            }

            final R = _decompressKey(x, (recId & 1) == 1, params.curve);
            if (!(R * n).isInfinity) {
              return null;
            }

            final e = _bytesToInt(msg);

            final eInv = (BigInt.zero - e) % n;
            final rInv = sig.r.modInverse(n);
            final srInv = (rInv * sig.s) % n;
            final eInvrInv = (rInv * eInv) % n;

            final q = (params.G * eInvrInv) + (R * srInv);

            final bytes = q.getEncoded(false);
            return _bytesToInt(bytes.sublist(1));
          }

          static ECPoint _decompressKey(BigInt xBN, bool yBit, ECCurve c) {
            List<int> x9IntegerToBytes(BigInt s, int qLength) {
              final bytes = _intToBytes(s);

              if (qLength < bytes.length) {
                return bytes.sublist(0, bytes.length - qLength);
              } else if (qLength > bytes.length) {
                final tmp = List<int>.filled(qLength, 0);

                final offset = qLength - bytes.length;
                for (var i = 0; i < bytes.length; i++) {
                  tmp[i + offset] = bytes[i];
                }

                return tmp;
              }

              return bytes;
            }

            final compEnc = x9IntegerToBytes(xBN, 1 + ((c.fieldSize + 7) ~/ 8));
            compEnc[0] = yBit ? 0x03 : 0x02;
            return c.decodePoint(compEnc);
          }

          static Uint8List deriveFrom(
            Uint8List message,
            ECPrivateKey privateKey,
            ECPublicKey publicKey,
          ) {
            final ECDomainParameters _params = ECCurve_secp256k1();
            final _halfCurveOrder = _params.n >> 1;

            final ecdsaSigner = ECDSASigner(null, HMac(SHA256Digest(), 64))
              ..init(true, PrivateKeyParameter(privateKey));

            ECSignature ecSignature = ecdsaSigner.generateSignature(message);

            if (ecSignature.s.compareTo(_halfCurveOrder) > 0) {
              final canonicalS = _params.n - ecSignature.s;
              ecSignature = ECSignature(ecSignature.r, canonicalS);
            }

            final publicKeyBytes =
                Uint8List.view(publicKey.Q.getEncoded(false).buffer, 1);

            final publicKeyBigInt = _bytesToInt(publicKeyBytes);

            var recoveryID = -1;
            for (var i = 0; i < 4; i++) {
              final k = _recoverFromSignature(i, ecSignature, message, _params);
              if (k == publicKeyBigInt) {
                recoveryID = i;
                break;
              }
            }

            if (recoveryID == -1) {
              throw Exception('Invalid recoverable key');
            }

            return Uint8List.fromList(
              _intToBytes(ecSignature.r) + _intToBytes(ecSignature.s),
            );
          }
        }

        */
    }
}
