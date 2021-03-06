﻿namespace DeviceId.Internal
{
    internal static class Json
    {
#if (NETSTANDARD || NET5_0_OR_GREATER)
        private static System.Text.Json.JsonSerializerOptions JsonSerializerOptions { get; } = new System.Text.Json.JsonSerializerOptions()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true,
        };
#endif

        public static T Deserialize<T>(string value)
        {
#if (NETSTANDARD || NET5_0_OR_GREATER)
            return System.Text.Json.JsonSerializer.Deserialize<T>(value, JsonSerializerOptions);
#else
            throw new System.NotImplementedException();
#endif
        }
    }
}
