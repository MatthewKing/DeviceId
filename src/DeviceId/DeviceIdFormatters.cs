using System.Security.Cryptography;
using DeviceId.Encoders;
using DeviceId.Formatters;

namespace DeviceId
{
    /// <summary>
    /// Provides access to some of the default formatters.
    /// </summary>
    public static class DeviceIdFormatters
    {
        /// <summary>
        /// Returns the default formatter used in version 5 of the DeviceId library.
        /// </summary>
        public static IDeviceIdFormatter DefaultV5 { get; } = new HashDeviceIdFormatter(() => SHA256.Create(), new Base64UrlByteArrayEncoder());

        /// <summary>
        /// Returns the default formatter used in version 4 of the DeviceId library.
        /// </summary>
        public static IDeviceIdFormatter DefaultV6 { get; } = new HashDeviceIdFormatter(() => SHA256.Create(), new Base32ByteArrayEncoder(Base32ByteArrayEncoder.CrockfordAlphabet));
    }
}
