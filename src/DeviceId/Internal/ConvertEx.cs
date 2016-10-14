using System.Text;

namespace DeviceId.Internal
{
    /// <summary>
    /// Contains additional conversion functions.
    /// </summary>
    internal static class ConvertEx
    {
        /// <summary>
        /// Converts the specified byte array into a hex string.
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A hex string representing the specified byte array.</returns>
        public static string ToHexString(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }

            return sb.ToString();
        }
    }
}
