namespace DeviceId
{
    using System;

    /// <summary>
    /// An implementation of IDeviceIdComponent that uses either a specified value or the result
    /// of a specified function as its component value.
    /// </summary>
    public sealed class DeviceIdComponent : IDeviceIdComponent
    {
        /// <summary>
        /// The name of the component.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// A function that returns the component value.
        /// </summary>
        private readonly Func<string> value;

        /// <summary>
        /// Initializes a new instance of the DeviceIdComponent class.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="value">The component value.</param>
        public DeviceIdComponent(string name, string value)
            : this(name, () => value) { }

        /// <summary>
        /// Initializes a new instance of the DeviceIdComponent class.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="value">A function that returns the component value.</param>
        public DeviceIdComponent(string name, Func<string> value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get { return this.name; } }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            return this.value();
        }
    }
}
