using DeviceId.Internal;

namespace DeviceId.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that retrieves data from a WMI class.
    /// </summary>
    public class WmiDeviceIdComponent : IDeviceIdComponent
    {
        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The class name.
        /// </summary>
        private readonly string _className;

        /// <summary>
        /// The property name.
        /// </summary>
        private readonly string _propertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="WmiDeviceIdComponent"/> class.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="className">The class name.</param>
        /// <param name="propertyName">The property name.</param>
        public WmiDeviceIdComponent(string name, string className, string propertyName)
        {
            Name = name;
            _className = className;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            var values = Wmi.GetValues(_className, _propertyName);
            return (values != null && values.Count > 0)
                ? string.Join(",", values.ToArray())
                : null;
        }
    }
}
