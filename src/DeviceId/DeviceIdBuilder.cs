using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using DeviceId.Encoders;
using DeviceId.Formatters;
using DeviceId.Internal;

namespace DeviceId
{
    /// <summary>
    /// Provides a fluent interface for constructing unique device identifiers.
    /// </summary>
    public class DeviceIdBuilder
    {
        /// <summary>
        /// Gets or sets the formatter to use.
        /// </summary>
        public IDeviceIdFormatter Formatter { get; set; }

        /// <summary>
        /// A set containing the components that will make up the device identifier.
        /// </summary>
#if NET35
        public HashSet<IDeviceIdComponent> Components { get; }
#else
        public ISet<IDeviceIdComponent> Components { get; }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceIdBuilder"/> class.
        /// </summary>
        public DeviceIdBuilder()
        {
            Formatter = new HashDeviceIdFormatter(() => SHA256.Create(), new Base64UrlByteArrayEncoder());
            Components = new HashSet<IDeviceIdComponent>(new DeviceIdComponentEqualityComparer());
        }

        /// <summary>
        /// Returns a string representation of the device identifier.
        /// </summary>
        /// <returns>A string representation of the device identifier.</returns>
        public override string ToString()
        {
            if (Formatter == null)
            {
                throw new InvalidOperationException($"The {nameof(Formatter)} property must not be null in order for {nameof(ToString)} to be called.");
            }

            return Formatter.GetDeviceId(Components);
        }
    }
}
