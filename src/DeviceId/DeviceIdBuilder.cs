using System.Collections.Generic;
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
        /// Gets the default formatter to use.
        /// </summary>
        private static IDeviceIdFormatter DefaultFormatter { get; } = new HexDeviceIdFormatter("MD5");

        /// <summary>
        /// A set containing the components that will make up the device identifier.
        /// </summary>
        public ISet<IDeviceIdComponent> Components { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceIdBuilder"/> class.
        /// </summary>
        public DeviceIdBuilder()
        {
            Components = new HashSet<IDeviceIdComponent>(new DeviceIdComponentEqualityComparer());
        }

        /// <summary>
        /// Returns a string representation of the device identifier.
        /// </summary>
        /// <returns>A string representation of the device identifier.</returns>
        public override string ToString()
        {
            return ToString(DefaultFormatter);
        }

        /// <summary>
        /// Returns a string representation of the device identifier.
        /// </summary>
        /// <param name="formatter">The <see cref="IDeviceIdFormatter"/> to use to produce the string.</param>
        /// <returns>A string representation of the device identifier.</returns>
        public string ToString(IDeviceIdFormatter formatter)
        {
            return formatter.GetDeviceId(Components);
        }
    }
}
