using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using DeviceId.Internal;

namespace DeviceId.Formatters
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdFormatter"/> that combines the components into an XML string.
    /// </summary>
    public class XmlDeviceIdFormatter : IDeviceIdFormatter
    {
        /// <summary>
        /// The name of the hash algorithm to use.
        /// </summary>
        private readonly string _hashName;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDeviceIdFormatter"/> class.
        /// </summary>
        /// <param name="hashName">The name of the hash algorithm to use.</param>
        public XmlDeviceIdFormatter(string hashName)
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
            using (var algorithm = HashAlgorithm.Create(_hashName))
            {
                var document = new XDocument(GetElement(components, algorithm));
                return document.ToString(SaveOptions.DisableFormatting);
            }
        }

        /// <summary>
        /// Returns an <see cref="XElement"/> representing the specified collection of <see cref="IDeviceIdComponent"/> instances.
        /// </summary>
        /// <param name="components">The sequence of <see cref="IDeviceIdComponent"/> instances to represent.</param>
        /// <param name="hashAlgorithm">The <see cref="HashAlgorithm"/> to use to hash the values.</param>
        /// <returns></returns>
        private static XElement GetElement(IEnumerable<IDeviceIdComponent> components, HashAlgorithm hashAlgorithm)
        {
            var elements = components
                .OrderBy(x => x.Name)
                .Select(x => GetElement(x, hashAlgorithm));

            return new XElement("DeviceId", elements);
        }

        /// <summary>
        /// Returns an <see cref="XElement"/> representing the specified <see cref="IDeviceIdComponent"/> instance.
        /// </summary>
        /// <param name="component">The <see cref="IDeviceIdComponent"/> to represent.</param>
        /// <param name="hashAlgorithm">The <see cref="HashAlgorithm"/> to use to hash the value.</param>
        /// <returns></returns>
        private static XElement GetElement(IDeviceIdComponent component, HashAlgorithm hashAlgorithm)
        {
            var valueBytes = Encoding.UTF8.GetBytes(component.GetValue());
            var valueHash = hashAlgorithm.ComputeHash(valueBytes);
            var valueHex = ConvertEx.ToHexString(valueHash);

            return new XElement("Component",
                new XAttribute("Name", component.Name),
                new XAttribute("Value", valueHex));
        }
    }
}
