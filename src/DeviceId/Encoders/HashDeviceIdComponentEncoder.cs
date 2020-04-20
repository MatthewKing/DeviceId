using System;
using System.Security.Cryptography;
using System.Text;

namespace DeviceId.Encoders
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponentEncoder"/> that encodes components as hashes.
    /// </summary>
    public class HashDeviceIdComponentEncoder : IDeviceIdComponentEncoder
    {
        /// <summary>
        /// A function that returns the hash algorithm to use.
        /// </summary>
        private readonly Func<HashAlgorithm> _hashAlgorithm;

        /// <summary>
        /// The <see cref="IByteArrayEncoder"/> to use to encode the resulting hash.
        /// </summary>
        private readonly IByteArrayEncoder _byteArrayEncoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashDeviceIdComponentEncoder"/> class.
        /// </summary>
        /// <param name="hashAlgorithm">A function that returns the hash algorithm to use.</param>
        /// <param name="byteArrayEncoder">The <see cref="IByteArrayEncoder"/> to use to encode the resulting hash.</param>
        public HashDeviceIdComponentEncoder(Func<HashAlgorithm> hashAlgorithm, IByteArrayEncoder byteArrayEncoder)
        {
            _hashAlgorithm = hashAlgorithm ?? throw new ArgumentNullException(nameof(hashAlgorithm));
            _byteArrayEncoder = byteArrayEncoder ?? throw new ArgumentNullException(nameof(byteArrayEncoder));
        }

        /// <summary>
        /// Encodes the specified <see cref="IDeviceIdComponent"/> as a string.
        /// </summary>
        /// <param name="component">The component to encode.</param>
        /// <returns>The component encoded as a string.</returns>
        public string Encode(IDeviceIdComponent component)
        {
            var value = component.GetValue() ?? string.Empty;
            var bytes = Encoding.UTF8.GetBytes(value);
            using var algorithm = _hashAlgorithm.Invoke();
            var hash = algorithm.ComputeHash(bytes);
            var output = _byteArrayEncoder.Encode(hash);
            return output;
        }
    }
}
