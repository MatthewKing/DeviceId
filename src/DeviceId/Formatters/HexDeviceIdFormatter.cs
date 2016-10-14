using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DeviceId.Internal;

namespace DeviceId.Formatters
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdFormatter"/> that combines the components into a hex string.
    /// </summary>
    public class HexDeviceIdFormatter : IDeviceIdFormatter
    {
        /// <summary>
        /// The name of the hash algorithm to use.
        /// </summary>
        private readonly string _hashName;

        /// <summary>
        /// Initializes a new instance of the <see cref="HexDeviceIdFormatter"/> class.
        /// </summary>
        /// <param name="hashName">The name of the hash algorithm to use.</param>
        public HexDeviceIdFormatter(string hashName)
        {
            _hashName = hashName;
        }

        /// <summary>
        /// Returns the device identifier string created by combining the specified
        /// <see cref="IDeviceIdComponent"/> instances.
        /// </summary>
        /// <param name="components">
        /// A sequence containing the <see cref="IDeviceIdComponent"/> instances
        /// to combine into the device identifier string.
        /// </param>
        /// <returns>The device identifier string.</returns>
        public string GetDeviceId(IEnumerable<IDeviceIdComponent> components)
        {
            var value = String.Join(",", components.OrderBy(x => x.Name).Select(x => $"{x.Name}:{x.GetValue()}"));
            var bytes = Encoding.UTF8.GetBytes(value);

            using (var algorithm = HashAlgorithm.Create(_hashName))
            {
                var hash = algorithm.ComputeHash(bytes);
                var output = ConvertEx.ToHexString(hash);
                return output;
            }
        }
    }
}
