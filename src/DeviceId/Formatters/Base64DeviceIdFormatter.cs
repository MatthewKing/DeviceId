using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DeviceId.Formatters
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdFormatter"/> that combines the components into a base-64 string.
    /// </summary>
    public class Base64DeviceIdFormatter : IDeviceIdFormatter
    {
        /// <summary>
        /// The name of the hash algorithm to use.
        /// </summary>
        private readonly string _hashName;

        /// <summary>
        /// A value indicating whether the string should be URL-encoded or not.
        /// </summary>
        private readonly bool _urlEncode;
        /// <summary>
        /// Initializes a new instance of the <see cref="Base64DeviceIdFormatter"/> class.
        /// </summary>
        /// <param name="hashName">The name of the hash algorithm to use.</param>
        public Base64DeviceIdFormatter(string hashName)
            : this(hashName, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Base64DeviceIdFormatter"/> class.
        /// </summary>
        /// <param name="hashName">The name of the hash algorithm to use.</param>
        /// <param name="urlEncode">A value indicating whether the string should be URL-encoded or not.</param>
        public Base64DeviceIdFormatter(string hashName, bool urlEncode)
        {
            _hashName = hashName;
            _urlEncode = urlEncode;
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
                var output = Convert.ToBase64String(hash);
                if (_urlEncode)
                {
                    output = output.TrimEnd('=').Replace('+', '-').Replace('/', '_');
                }
                
                return output;
            }
        }
    }
}
