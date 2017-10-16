using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceId.Formatters
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdFormatter"/> that combines the components into a concatenated string.
    /// </summary>
    public class StringDeviceIdFormatter : IDeviceIdFormatter
    {
        /// <summary>
        /// The <see cref="IDeviceIdComponentEncoder"/> instance to use to encode individual components.
        /// </summary>
        private readonly IDeviceIdComponentEncoder _encoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDeviceIdFormatter"/> class.
        /// </summary>
        /// <param name="encoder">The <see cref="IDeviceIdComponentEncoder"/> instance to use to encode individual components.</param>
        public StringDeviceIdFormatter(IDeviceIdComponentEncoder encoder)
        {
            _encoder = encoder ?? throw new ArgumentNullException(nameof(encoder));
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

            return String.Join(".", components.OrderBy(x => x.Name).Select(x => _encoder.Encode(x)));
        }
    }
}
