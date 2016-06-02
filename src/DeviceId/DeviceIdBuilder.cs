using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            if (_components.Count == 0)
            {
                return null;
            }

            IEnumerable<string> orderedValues = _components
                .OrderBy(o => o.Name)
                .Select(o => String.Concat(o.Name, ":", o.GetValue()));

            string combinedValue = String.Join(",", orderedValues) ?? String.Empty;

            byte[] data, hash;
            using (HashAlgorithm algorithm = HashAlgorithm.Create(hashName))
            {
                data = Encoding.UTF8.GetBytes(combinedValue);
                hash = algorithm.ComputeHash(data);
            }

            return hash;
        }

        /// <summary>
        /// Returns a unique identifier for this device, using the components specified
        /// in this <see cref="DeviceIdBuilder"/> instance.
        /// </summary>
        /// <returns>A unique identifier for this device.</returns>
        public override string ToString()
        {
            return ToString(DefaultHashAlgorithm);
        }

        /// <summary>
        /// Returns a unique identifier for this device, using the components specified
        /// in this <see cref="DeviceIdBuilder"/> instance.
        /// </summary>
        /// <param name="hashName">The name of the hash algorithm to use.</param>
        /// <returns>A unique identifier for this device.</returns>
        public string ToString(string hashName)
        {
            byte[] bytes = ToByteArray(hashName);
            return bytes != null
                ? Convert.ToBase64String(bytes)
                : null;
        }

        /// <summary>
        /// Returns a string uniquely identifying this device, using the components specified
        /// in this <see cref="DeviceIdBuilder"/> instance.
        /// </summary>
        /// <returns>A string uniquely identifying the device.</returns>
        [Obsolete("Use ToString() instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetDeviceId()
        {
            return GetDeviceId(DefaultHashAlgorithm);
        }

        /// <summary>
        /// Returns a string uniquely identifying this device, using the components specified
        /// in this <see cref="DeviceIdBuilder"/> instance.
        /// </summary>
        /// <param name="hashName">The name of the hash algorithm to use.</param>
        /// <returns>A string uniquely identifying the device.</returns>
        [Obsolete("Use ToString(hashName) instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetDeviceId(string hashName)
        {
            return ToString(hashName);
        }
    }
}
