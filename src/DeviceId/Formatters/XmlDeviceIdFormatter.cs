using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DeviceId.Formatters
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdFormatter"/> that combines the components into an XML string.
    /// </summary>
    public class XmlDeviceIdFormatter : IDeviceIdFormatter
    {
        /// <summary>
        /// The <see cref="IDeviceIdComponentEncoder"/> instance to use to encode individual components.
        /// </summary>
        private readonly IDeviceIdComponentEncoder _encoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDeviceIdFormatter"/> class.
        /// </summary>
        /// <param name="encoder">The <see cref="IDeviceIdComponentEncoder"/> instance to use to encode individual components.</param>
        public XmlDeviceIdFormatter(IDeviceIdComponentEncoder encoder)
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

            var document = new XDocument(GetElement(components));
            return document.ToString(SaveOptions.DisableFormatting);
        }

        /// <summary>
        /// Returns an <see cref="XElement"/> representing the specified collection of <see cref="IDeviceIdComponent"/> instances.
        /// </summary>
        /// <param name="components">The sequence of <see cref="IDeviceIdComponent"/> instances to represent.</param>
        /// <returns>An <see cref="XElement"/> representing the specified collection of <see cref="IDeviceIdComponent"/> instances</returns>
        private XElement GetElement(IEnumerable<IDeviceIdComponent> components)
        {
            var elements = components
                .OrderBy(x => x.Name)
                .Select(x => GetElement(x));

            return new XElement("DeviceId", elements);
        }

        /// <summary>
        /// Returns an <see cref="XElement"/> representing the specified <see cref="IDeviceIdComponent"/> instance.
        /// </summary>
        /// <param name="component">The <see cref="IDeviceIdComponent"/> to represent.</param>
        /// <returns>An <see cref="XElement"/> representing the specified <see cref="IDeviceIdComponent"/> instance.</returns>
        private XElement GetElement(IDeviceIdComponent component)
        {
            return new XElement("Component",
                new XAttribute("Name", component.Name),
                new XAttribute("Value", _encoder.Encode(component)));
        }
    }
}
