namespace DeviceId
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Provides a fluent interface for constructing unique device identifiers.
    /// </summary>
    public sealed class DeviceIdBuilder
    {
        /// <summary>
        /// The comparer to use when comparing components for equality.
        /// </summary>
        private static IEqualityComparer<IDeviceIdComponent> comparer;

        /// <summary>
        /// A HashSet containing the components that will make up the device identifier.
        /// </summary>
        private readonly HashSet<IDeviceIdComponent> components;

        /// <summary>
        /// Initializes static members of the DeviceIdBuilder class.
        /// </summary>
        static DeviceIdBuilder()
        {
            comparer = new DeviceIdComponentEqualityComparer();
        }

        /// <summary>
        /// Initializes a new instance of the DeviceIdBuilder class.
        /// </summary>
        public DeviceIdBuilder()
        {
            this.components = new HashSet<IDeviceIdComponent>(comparer);
        }

        /// <summary>
        /// Adds the specified component to this DeviceIdBuilder instance.
        /// The component will be used to construct the device identifier.
        /// </summary>
        /// <param name="component">The component to be added.</param>
        /// <returns>This DeviceIdBuilder instance.</returns>
        public DeviceIdBuilder AddComponent(IDeviceIdComponent component)
        {
            this.components.Add(component);
            return this;
        }

        /// <summary>
        /// Returns a unique identifier for this device, using the components specified
        /// in this DeviceIdBuilder instance.
        /// </summary>
        /// <returns>A unique identifier for this device.</returns>
        public byte[] ToByteArray()
        {
            return this.ToByteArray("SHA256");
        }

        /// <summary>
        /// Returns a unique identifier for this device, using the components specified
        /// in this DeviceIdBuilder instance.
        /// </summary>
        /// <param name="hashName">The hash algorithm implementation to use.</param>
        /// <returns>A unique identifier for this device.</returns>
        public byte[] ToByteArray(string hashName)
        {
            if (this.components.Count == 0)
            {
                return null;
            }

            IEnumerable<string> orderedValues = this.components
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
        /// in this DeviceIdBuilder instance.
        /// </summary>
        /// <returns>A unique identifier for this device.</returns>
        public override string ToString()
        {
            return this.ToString("SHA256");
        }

        /// <summary>
        /// Returns a unique identifier for this device, using the components specified
        /// in this DeviceIdBuilder instance.
        /// </summary>
        /// <param name="hashName">The hash algorithm to use.</param>
        /// <returns>A unique identifier for this device.</returns>
        public string ToString(string hashName)
        {
            byte[] bytes = this.ToByteArray(hashName);
            return bytes != null
                ? Convert.ToBase64String(bytes)
                : null;
        }

        /// <summary>
        /// Returns a string uniquely identifying this device, using the components specified
        /// in this DeviceIdBuilder instance.
        /// </summary>
        /// <returns>A string uniquely identifying the device.</returns>
        [Obsolete("Use ToString() instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetDeviceId()
        {
            return this.GetDeviceId("SHA256");
        }

        /// <summary>
        /// Returns a string uniquely identifying this device, using the components specified
        /// in this DeviceIdBuilder instance.
        /// </summary>
        /// <param name="hashName">The hash algorithm to use.</param>
        /// <returns>A string uniquely identifying the device.</returns>
        [Obsolete("Use ToString(hashName) instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetDeviceId(string hashName)
        {
            return this.ToString(hashName);
        }
    }
}
