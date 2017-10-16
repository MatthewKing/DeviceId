using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DeviceId.Formatters
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdFormatter"/> that combines the components into a hash.
    /// </summary>
    public class HashDeviceIdFormatter : IDeviceIdFormatter
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
        /// Initializes a new instance of the <see cref="HashDeviceIdFormatter"/> class.
        /// </summary>
        /// <param name="hashAlgorithm">A function that returns the hash algorithm to use.</param>
        /// <param name="byteArrayEncoder">The <see cref="IByteArrayEncoder"/> to use to encode the resulting hash.</param>
        public HashDeviceIdFormatter(Func<HashAlgorithm> hashAlgorithm, IByteArrayEncoder byteArrayEncoder)
        {
            _hashAlgorithm = hashAlgorithm ?? throw new ArgumentNullException(nameof(hashAlgorithm));
            _byteArrayEncoder = byteArrayEncoder ?? throw new ArgumentNullException(nameof(byteArrayEncoder));
        }

        /// <summary>
        /// Returns the device identifier string created by combining the specified <see cref="IDeviceIdComponent"/> instances.
        /// </summary>
        /// <param name="components">A sequence containing the <see cref="IDeviceIdComponent"/> instances to combine into the device identifier string.</param>
        /// <returns>The device identifier string.</returns>
        public string GetDeviceId(IEnumerable<IDeviceIdComponent> components)
        {
            if (components == null)
            {
                throw new ArgumentNullException(nameof(components));
            }

            var value = String.Join(",", components.OrderBy(x => x.Name).Select(x => x.GetValue()));
            var bytes = Encoding.UTF8.GetBytes(value);

            using (var algorithm = _hashAlgorithm())
            {
                var hash = algorithm.ComputeHash(bytes);
                return _byteArrayEncoder.Encode(hash);
            }
        }
    }
}
