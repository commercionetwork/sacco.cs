﻿// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
/// Represents a wallet which contains the hex private key, the hex public key
/// and the hex address.
/// In order to create one properly, the [Wallet.derive] method should always
/// be used.
/// The associated [networkInfo] will be used when computing the [bech32Address]
/// associated with the wallet.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using NBitcoin;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Signers;




namespace commercio.sacco.lib
{
    // *** This is inherited by Equatable in Dart Package!
    //  There is no such Class in C# - we include Compare-Net-Objects Nuget package for the purpose - see https://github.com/GregFinzer/Compare-Net-Objects
    public class Wallet
    {
        #region Instance Variables
        // Careful with this...
        const String DERIVATION_PATH = @"m/44'/118'/0'/0/0";
        // This need to be fixed lenght for Address Decoding capabilities
        const int ADDRESS_LENGTH = 20;
        // const int SHA256DIGEST_OUT_LENGTH = 32;

        #endregion

        #region Properties

        public byte[] address { get; private set; }
        public byte[] privateKey { get; private set; }
        public byte[] publicKey { get; private set; }

        public NetworkInfo networkInfo { get; private set; }

        /// Returns the associated [address] as a Bech32 string.
        public String bech32Address
        {
            get
            {
                String s;

                s = Bech32Engine.Encode(networkInfo.bech32Hrp, address);
                return (s);
            }
        }


        /// Returns the associated [privateKey] as an [ECPrivateKey] instance - in C# we use instead ECPrivateKeyParameters
        private ECPrivateKeyParameters ecPrivateKey
        {
            get
            {
                // Convert private key
                BigInteger privateKeyInt = new BigInteger(HexEncDec.ByteArrayToString(privateKey), (int)16);
                // Look for domain params
                var curve = ECNamedCurveTable.GetByName("secp256k1");
                ECDomainParameters wkParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
                // Return the key
                ECPrivateKeyParameters wkPrivKey = new ECPrivateKeyParameters(privateKeyInt, wkParams);
                return wkPrivKey;
            }
        }

        /// Returns the associated [publicKey] as an [ECPublicKey] instance - in C# we use instead ECPublicKeyParameters
        public ECPublicKeyParameters ecPublicKey
        {
            get
            {
                // Look for domain params
                var curve = ECNamedCurveTable.GetByName("secp256k1");
                ECDomainParameters wkParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
                ECPoint point = curve.G;
                ECPoint curvePoint = point.Multiply(this.ecPrivateKey.D);
                // Return the key
                ECPublicKeyParameters wkPubKey = new ECPublicKeyParameters(curvePoint, wkParams);
                return wkPubKey;
            }
        }

        #endregion

        #region Constructors

        public Wallet(NetworkInfo networkInfo, byte[] address, byte[] privateKey, byte[] publicKey)
        {
            Trace.Assert(networkInfo != null);
            Trace.Assert(privateKey != null);
            Trace.Assert(publicKey != null);
            this.networkInfo = networkInfo;
            this.address = address;
            this.privateKey = privateKey;
            this.publicKey = publicKey;
        }

        #endregion

        #region static altContructors
        // This are implented as static constructors to translate Dart factory

        /// Creates a new [Wallet] instance from the given [json] and [privateKey].
        public static Wallet fromJson(Dictionary<String, Object> json, byte[] privateKey)
        {
            byte[] wkAddress = null, wkPublicKey = null;
            NetworkInfo wkNetworkInfo = null;
            Object outValue;

            if (json.TryGetValue("hex_address", out outValue))
                wkAddress = HexEncDec.StringToByteArray(outValue as String);
            if (json.TryGetValue("public_key", out outValue))
                wkPublicKey = HexEncDec.StringToByteArray(outValue as String);
            if (json.TryGetValue("network_info", out outValue))
                wkNetworkInfo = NetworkInfo.fromJson(outValue as Dictionary<String, Object>);
            return new Wallet(
              address: wkAddress,
              publicKey: wkPublicKey,
              privateKey: privateKey,
              networkInfo: wkNetworkInfo
            );
        }


        /// Derives the private key from the given [mnemonic] using the specified
        /// [networkInfo].
        public static Wallet derive(List<String> mnemonic, NetworkInfo networkInfo)
        {
            Mnemonic bip39;

            // Get the mnemonic as a single concatenated string
            String mnemonicString = String.Join(' ', mnemonic);
            // Check the mnemonic
            try
            {
                bip39 = new Mnemonic(mnemonicString);
            }
            catch (ArgumentException e)
            {
                System.ArgumentException argEx = new System.ArgumentException($"Invalid mnemonic string - Exception ${e.Message} - '${mnemonicString}'");
                throw argEx;
            }
            // Get the seed - byte[]. !!!Note the empty password - OK!
            byte[] seed = bip39.DeriveSeed();
            // Using the seed in a BIP32 instance
            ExtKey root = new ExtKey(seed);
            //  Create the Keypath
            KeyPath derivationPath = new KeyPath(DERIVATION_PATH);
            ExtKey derivedNode = root.Derive(derivationPath);
            // Get the private key
            byte[] privateKey = derivedNode.PrivateKey.ToBytes();

            // Get the curve data
            var secp256k1 = ECNamedCurveTable.GetByName("secp256k1");
            ECPoint point = secp256k1.G;

            // Compute the curve point associated to the private key
            BigInteger bigInt = new BigInteger(HexEncDec.ByteArrayToString(privateKey), (int) 16);
            ECPoint curvePoint = point.Multiply(bigInt);

            // Get the public key - Be Careful, this MUST be compressed format!!!
            byte[] publicKeyBytes = curvePoint.GetEncoded(true);

            // Get the address
            Sha256Digest sha256digest = new Sha256Digest();
            sha256digest.BlockUpdate(publicKeyBytes, 0, publicKeyBytes.Length);
            // byte[] sha256digestOut = new byte[sha256digest.GetByteLength()];
            byte[] sha256digestOut = new byte[sha256digest.GetDigestSize()];
            sha256digest.DoFinal(sha256digestOut, 0);

            RipeMD160Digest ripeMD160Digest = new RipeMD160Digest();
            ripeMD160Digest.BlockUpdate(sha256digestOut, 0, sha256digestOut.Length);
            // byte[] address = new byte[ripeMD160Digest.GetByteLength()];
            byte[] address = new byte[ADDRESS_LENGTH];
            ripeMD160Digest.DoFinal(address, 0);

            // Return the key bytes
            return new Wallet(address: address, publicKey: publicKeyBytes, privateKey: privateKey, networkInfo: networkInfo);
        }

        /// Creates a new [Wallet] instance based on the existent [wallet] for
        /// the given [networkInfo].
        public static Wallet convert(Wallet wallet, NetworkInfo networkInfo)
        {
            return new Wallet(networkInfo: networkInfo, address: wallet.address, privateKey: wallet.privateKey, publicKey: wallet.publicKey );
        }


        /// Generates a SecureRandom
        /// C#h as not a fortuna random generator, Just trying an aproach with DigestRandomGenerator from BouncyCastle
        private static SecureRandom _getSecureRandom()
        {
            // Start from a crypto seed from C# libraries
            System.Security.Cryptography.RNGCryptoServiceProvider rngCsp = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[32];
            rngCsp.GetBytes(randomBytes);
            // Get a frist random generator from BouncyCastle
            VmpcRandomGenerator firstRandomGenerator = new Org.BouncyCastle.Crypto.Prng.VmpcRandomGenerator();
            firstRandomGenerator.AddSeedMaterial(randomBytes);
            byte[] seed = new byte[32];
            firstRandomGenerator.NextBytes(seed, 0, 32);
            // Create and seed the final Randon Generator
            DigestRandomGenerator wkRandomGenerator = new Org.BouncyCastle.Crypto.Prng.DigestRandomGenerator(new Sha512Digest());
            SecureRandom secureRandomGenerator = new SecureRandom(wkRandomGenerator);
            secureRandomGenerator.SetSeed(seed);
            return secureRandomGenerator;
        }


        #endregion

        #region Public Methods

        /// Signs the given [data] using the associated [privateKey] and encodes
        /// the signature bytes to be included inside a transaction.
        public byte[] signTxData(byte[] data)
        {
            Sha256Digest sha256digest = new Sha256Digest();
            sha256digest.BlockUpdate(data, 0, data.Length);
            // byte[] hash = new byte[SHA256DIGEST_OUT_LENGTH];
            byte[] hash = new byte[sha256digest.GetDigestSize()];
            sha256digest.DoFinal(hash, 0);

            // Compute the signature
            return TransactionSigner.deriveFrom(hash, ecPrivateKey, ecPublicKey);
        }

        /// Signs the given [data] using the private key associated with this wallet,
        /// returning the signature bytes ASN.1 DER encoded.
        public byte[] sign(byte[] data)
        {
            ECDsaSigner ecdsaSigner = new ECDsaSigner();
            ecdsaSigner.Init(true, new ParametersWithRandom(ecPrivateKey, _getSecureRandom()));
            ECSignature ecSignature = new ECSignature(ecdsaSigner.GenerateSignature(data));
            // Create the Asn1 DER sequence for the signature
            // Quite different from Dart approach
            Asn1EncodableVector asn1Vect = new Asn1EncodableVector();
            asn1Vect.Add(new DerInteger(ecSignature.r));
            asn1Vect.Add(new DerInteger(ecSignature.s));
            DerSequence sequence = new DerSequence(asn1Vect);
            return sequence.GetEncoded();
        }

        /// Converts the current [Wallet] instance into a JSON object.
        /// Note that the private key is not serialized for safety reasons.
        public Dictionary<String, Object> toJson()
        {
            Dictionary<String, Object> wkDict = new Dictionary<String, Object>();
            wkDict.Add("hex_address", HexEncDec.ByteArrayToString(this.address));
            wkDict.Add("bech32_address", this.bech32Address);
            wkDict.Add("public_key", HexEncDec.ByteArrayToString(this.publicKey));
            wkDict.Add("network_info", this.networkInfo.toJson());
            return wkDict;
        }



    #endregion

    #region Helpers

    #endregion

    }
}