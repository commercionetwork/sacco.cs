// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
/// Hex Encoder - Decoder
//
using System;
using System.Collections.Generic;
using System.Text;

namespace sacco.Lib
{
    public class HexEncDec
    {
        public static String ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        // To be checked against strings with odd number of chars!
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
