using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DeviceId
{
    /// <summary>
    /// Provides a fluent interface for constructing unique device identifiers.
    /// </summary>
    public sealed class DeviceIdBuilder
    {
        /// <summary>
        /// The name of the default hash algorithm to use if nothing else is specified.
        /// </summary>
        private const string DefaultHashAlgorithm = "SHA256";

        /// <summary>
        /// The comparer to use when comparing components for equality.
        /// </summary>
        private static IEqualityComparer<IDeviceIdComponent> _comparer;

        /// <summary>
        /// A HashSet containing the components that will make up the device identifier.
        /// </summary>
        private readonly HashSet<IDeviceIdComponent> _components;

        /// <summary>
        /// Initializes static members of the <see cref="DeviceIdBuilder"/> class.
        /// </summary>
        static DeviceIdBuilder()
        {
            _comparer = new DeviceIdComponentEqualityComparer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceIdBuilder"/> class.
        /// </summary>
        public DeviceIdBuilder()
        {
            _components = new HashSet<IDeviceIdComponent>(_comparer);
        }

        /// <summary>
        /// Adds the specified component to this <see cref="DeviceIdBuilder"/> instance.
        /// The component will be used to construct the device identifier.
        /// </summary>
        /// <param name="component">The component to be added.</param>
        /// <returns>This DeviceIdBuilder instance.</returns>
        public DeviceIdBuilder AddComponent(IDeviceIdComponent component)
        {
            _components.Add(component);
            return this;
        }

        /// <summary>
        /// Returns a unique identifier for this device, using the components specified
        /// in this <see cref="DeviceIdBuilder"/> instance.
        /// </summary>
        /// <returns>A unique identifier for this device.</returns>
        public byte[] ToByteArray()
        {
            return ToByteArray(DefaultHashAlgorithm);
        }

        /// <summary>
        /// Returns a unique identifier for this device, using the components specified
        /// in this <see cref="DeviceIdBuilder"/> instance.
        /// </summary>
        /// <param name="hashName">The name of the hash algorithm implementation to use.</param>
        /// <returns>A unique identifier for this device.</returns>
        public byte[] ToByteArray(string hashName)
        {
            using (HashAlgorithm algorithm = HashAlgorithm.Create(hashName))
            {
                return ToByteArray(algorithm);
            }
        }

        /// <summary>
        /// Returns a unique identifier for this device, using the components specified
        /// in this <see cref="DeviceIdBuilder"/> instance.
        /// </summary>
        /// <param name="hashAlgorithm">The <see cref="HashAlgorithm"/> to use to hash the value.</param>
        /// <returns>A unique identifier for this device.</returns>
        public byte[] ToByteArray(HashAlgorithm hashAlgorithm)
        {
            IEnumerable<string> orderedValues = _components.OrderBy(x => x.Name).Select(x => x.Name + ":" + x.GetValue());
            string combinedValue = String.Join(",", orderedValues);
            byte[] data = Encoding.UTF8.GetBytes(combinedValue);
            byte[] hash = hashAlgorithm.ComputeHash(data);

            return hash;
        }

        /// <summary>
        /// Returns a unique identifier for this device, using the components specified
        /// in this <see cref="DeviceIdBuilder"/> instance.
        /// </summary>
        /// <returns>A unique identifier for this device.</returns>
        public override string ToString()
        {
            return HexString(ToByteArray());
        }

        /// <summary>
        /// Returns a unique identifier for this device, using the components specified
        /// in this <see cref="DeviceIdBuilder"/> instance.
        /// </summary>
        /// <param name="hashName">The name of the hash algorithm implementation to use.</param>
        /// <returns>A unique identifier for this device.</returns>
        public string ToString(string hashName)
        {
            return HexString(ToByteArray(hashName));
        }

        /// <summary>
        /// Returns a unique identifier for this device, using the components specified
        /// in this <see cref="DeviceIdBuilder"/> instance.
        /// </summary>
        /// <param name="hashAlgorithm">The <see cref="HashAlgorithm"/> to use to hash the value.</param>
        /// <returns>A unique identifier for this device.</returns>
        public string ToString(HashAlgorithm hashAlgorithm)
        {
            return HexString(ToByteArray(hashAlgorithm));
        }

        /// <summary>
        /// Converts the specified byte array into a hex string.
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A hex string representing the specified byte array.</returns>
        private static string HexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }

            return sb.ToString();
        }
    }
}
