using System.Text.Json.Serialization;

namespace DeviceId.Linux.Serialization;

[JsonSerializable(typeof(LsblkDevice))]
[JsonSerializable(typeof(LsblkOutput))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
internal partial class SourceGenerationContext : JsonSerializerContext { }
