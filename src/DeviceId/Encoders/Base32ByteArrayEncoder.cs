using System;
using System.Text;

namespace DeviceId.Encoders
{
    /// <summary>
    /// An implementation of <see cref="IByteArrayEncoder"/> that encodes byte arrays as Base32 strings.
    /// </summary>
    public class Base32ByteArrayEncoder : IByteArrayEncoder
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        private const int Shift = 5;
        private const int Mask = 31;

        /// <summary>
        /// Initializes a new instance of the <see cref="Base32ByteArrayEncoder"/> class.
        /// </summary>
        public Base32ByteArrayEncoder() { }

        /// <summary>
        /// Encodes the specified byte array as a string.
        /// </summary>
        /// <param name="bytes">The byte array to encode.</param>
        /// <returns>The byte array encoded as a string.</returns>
        public string Encode(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length == 0)
            {
                return string.Empty;
            }

            var outputLength = (bytes.Length * 8 + Shift - 1) / Shift;
            var sb = new StringBuilder(outputLength);

            var offset = 0;
            var last = bytes.Length;
            int buffer = bytes[offset++];
            var bitsLeft = 8;
            while (bitsLeft > 0 || offset < last)
            {
                if (bitsLeft < Shift)
                {
                    if (offset < last)
                    {
                        buffer <<= 8;
                        buffer |= (bytes[offset++] & 0xff);
                        bitsLeft += 8;
                    }
                    else
                    {
                        var pad = Shift - bitsLeft;
                        buffer <<= pad;
                        bitsLeft += pad;
                    }
                }

                var index = Mask & (buffer >> (bitsLeft - Shift));
                bitsLeft -= Shift;
                sb.Append(Alphabet[index]);
            }

            return sb.ToString();
        }
    }
}
