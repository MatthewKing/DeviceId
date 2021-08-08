# DeviceId

A simple library providing functionality to generate a 'device ID' that can be used to uniquely identify a computer.

NOTE: These docs are for version 6, which is currently in an ALPHA and is available as a pre-release NuGet package. Version 5 has a few subtle differences and I'd recommend looking at the readme history if you're using that version.

## Quickstart

### What packages are needed?

If you're using version 5 or below, everything is available in the [DeviceId](https://www.nuget.org/packages/DeviceId) package.

As of version 6, the packages have been split up so that users can pick-and-choose what they need, without having to pull down unnecessary references that they won't use:

* The main [DeviceId](https://www.nuget.org/packages/DeviceId) package contains the core functionality and a number of cross-platform components.
* The [DeviceId.Windows](https://www.nuget.org/packages/DeviceId.Windows) package adds a few Windows-specific components.
* The [DeviceId.Windows.Wmi](https://www.nuget.org/packages/DeviceId.Windows.Wmi) package adds even more Windows-specific components, using WMI.
* The [DeviceId.Windows.Mmi](https://www.nuget.org/packages/DeviceId.Windows.Mmi) package adds the same components as above, but using MMI instead of WMI for those instances where WMI isn't appropriate (such as where no .NET Framework is present on the machine).
* The [DeviceId.Linux](https://www.nuget.org/packages/DeviceId.Linux) package adds a few Linux-specific components.
* The [DeviceId.Mac](https://www.nuget.org/packages/DeviceId.Mac) package adds a few Mac-specific components.

You can pick-and-choose which packages to use based on your use case.

For a standard Windows app, the recommended packages are: `DeviceId`, `DeviceId.Windows`, and `DeviceId.Windows.Wmi`.

```
PM> Install-Package DeviceId
PM> Install-Package DeviceId.Windows
PM> Install-Package DeviceId.Windows.Wmi
```

Alternatively, you can just start with `DeviceId.Windows.Wmi`, as it itself references `DeviceId.Windows` and `DeviceId`.

### Building a device identifier

Use the `DeviceIdBuilder` class to build up a device ID.

Here's a simple cross-platform one, using only the `DeviceId` package, which is valid for both version 5 and version 6 of the library:

```csharp
string deviceId = new DeviceIdBuilder()
    .AddMachineName()
    .AddOSVersion()
    .AddFileToken(@"C:\example-device-token.txt")
    .ToString();
```

Here's a more complex device ID, making use of some of the advanced components from the `DeviceId.Windows.Wmi` (or `DeviceId.Windows.Mmi`) package:

```csharp
string deviceId = new DeviceIdBuilder()
    .AddMachineName()
    .AddOSVersion()
    .OnWindows(windows => windows
        .AddProcessorId()
        .AddMotherboardSerialNumber()
        .AddSystemSerialDriveNumber())
    .ToString();
```

Here's a complex cross-platform device ID, using `DeviceId.Windows.Wmi`, `DeviceId.Linux`, and `DeviceId.Mac`:

```csharp
string deviceId = new DeviceIdBuilder()
    .AddMachineName()
    .AddOSVersion()
    .OnWindows(windows => windows
        .AddProcessorId()
        .AddMotherboardSerialNumber()
        .AddSystemSerialDriveNumber())
    .OnLinux(linux => linux
        .AddMotherboardSerialNumber()
        .AddSystemDriveSerialNumber())
    .OnMac(mac => mac
        .AddSystemDriveSerialNumber()
        .AddPlatformSerialNumber())
    .ToString();
```

### What can you include in a device identifier

The following extension methods are available out of the box to suit some common use cases:

From `DeviceId`:

* `AddUserName` adds the current user's username to the device identifer.
* `AddMachineName` adds the machine name to the device identifier.
* `AddOSVersion` adds the current OS version to the device identifier.
* `AddMacAddress` adds the MAC address to the device identifier.
* `AddFileToken` adds a unique token stored in a file to the device identifier. The file is created if it doesn't already exist. Fails silently if no permissions available to access the file.

From `DeviceId.Windows`:

* `AddRegistryValue` adds a specified registry value to the device identifier.
* `AddMachineGuid` adds the machine GUID from `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography` to the device identifier.

From `DeviceId.Windows.Wmi` and `DeviceId.Windows.Mmi`:

* `AddMacAddressFromWmi` / `AddMacAddressFromMmi` adds the MAC address to the device identifier. These use the improved query functionality from WMI/MMI to provide additional functionality over the basic `AddMacAddress` method (such as being able to exclude non-physical device).
* `AddProcessorId` adds the processor ID to the device identifier.
* `AddSystemSerialDriveNumber` adds the system drive's serial number to the device identifier.
* `AddMotherboardSerialNumber` adds the motherboard serial number to the device identifier.
* `AddSystemUuid` adds the system UUID to the device identifier.

From `DeviceId.Linux`:

* `AddSystemSerialDriveNumber` adds the system drive's serial number to the device identifier.
* `AddMotherboardSerialNumber` adds the motherboard serial number to the device identifier.
* `AddMachineId` adds the machine ID (from `/var/lib/dbus/machine-id` or `/etc/machine-id`) to the device identifier.
* `AddProductUuid` adds the product UUID (from `/sys/class/dmi/id/product_uuid`) to the device identifier.
* `AddCpuInfo` adds  CPU info (from `/proc/cpuinfo`) to the device identifier.

From `DeviceId.Mac`:

* `AddSystemSerialDriveNumber` adds the system drive's serial number to the device identifier.
* `AddPlatformSerialNumber` adds IOPlatformSerialNumber to the device identifier.

#### Dealing with MAC Address randomization and virtual network adapters

Non physical network adapters like VPN connections tend not to have fixed MAC addresses. For wireless (802.11 based) adapters hardware (MAC) address randomization is frequently applied to avoid tracking with many modern operating systems support this out of the box. This makes wireless network adapters bad candidates for device identification.

Using the cross-platform `AddMacAddress`, you can exclude wireless network adapters like so:

```csharp
string deviceId = new DeviceIdBuilder()
    .AddMacAddress(excludeWireless: true)
    .ToString();
```

If you're on Windows, you can also exclude non-physical adapters using the `DeviceId.Windows.Wmi` or `DeviceId.Windows.Mmi` packages like so:

```csharp
string deviceId = new DeviceIdBuilder()
    .AddMacAddress(excludeWireless: true)
    .OnWindows(windows => windows
        .AddMacAddressFromWmi(excludeWireless: true, excludeNonPhysical: true)
    .ToString()
```

### Controlling how the device identifier is formatted

Use the `UseFormatter` method to set the formatter:

```csharp
string deviceId = new DeviceIdBuilder()
    .AddMachineName()
    .AddOSVersion()
    .UseFormatter(new HashDeviceIdFormatter(() => SHA256.Create(), new Base32ByteArrayEncoder()))
    .ToString();
```

The "default" formatters are available in [DeviceIdFormatters](/src/DeviceId/DeviceIdFormatters.cs) for quick reference.
The default formatter changed between version 5 and version 6 of the library. If you're using version 6 but want to revert to the version 5 formatter, you can do so:

```csharp
string deviceId = new DeviceIdBuilder()
    .AddMachineName()
    .AddOSVersion()
    .UseFormatter(DeviceIdFormatters.DefaultV5)
    .ToString();
```

For more advanced usage scenarios, you can use one of the out-of-the-box implementations of `IDeviceIdFormatter` in the `DeviceId.Formatters` namespace, or you can create your own.

* [StringDeviceIdFormatter](/src/DeviceId/Formatters/HashDeviceIdFormatter.cs) - Formats the device ID as a string containing each component ID, using any desired component encoding.
* [HashDeviceIdFormatter](/src/DeviceId/Formatters/HashDeviceIdFormatter.cs) - Formats the device ID as a hash string, using any desired hash algorithm and byte array encoding.
* [XmlDeviceIdFormatter](/src/DeviceId/Formatters/XmlDeviceIdFormatter.cs) - Formats the device ID as an XML document, using any desired component encoding.

There are a number of encoders that can be used customize the formatter. These implement `IDeviceIdComponentEncoder` and `IByteArrayEncoder` and are found in the `DeviceId.Encoders` namespace.

* [PlainTextDeviceIdComponentEncoder](/src/DeviceId/Encoders/PlainTextDeviceIdComponentEncoder.cs) - Encodes a device ID component as plain text.
* [HashDeviceIdComponentEncoder](/src/DeviceId/Encoders/HashDeviceIdComponentEncoder.cs) - Encodes a device ID component as a hash string, using any desired hash algorithm.
* [HexByteArrayEncoder](/src/DeviceId/Encoders/HexByteArrayEncoder.cs) - Encodes a byte array as a hex string.
* [Base32UrlByteArrayEncoder](/src/DeviceId/Encoders/Base32ByteArrayEncoder.cs) - Encodes a byte array as a base 64 url-encoded string.
* [Base64ByteArrayEncoder](/src/DeviceId/Encoders/Base64ByteArrayEncoder.cs) - Encodes a byte array as a base 64 string.
* [Base64UrlByteArrayEncoder](/src/DeviceId/Encoders/Base64UrlByteArrayEncoder.cs) - Encodes a byte array as a base 64 url-encoded string.

## Strong naming

From version 5 onwards, the assemblies in this package are strong named for the convenience of those users who require strong naming. Please note, however, that the key files are checked in to this repository. This means that anyone can compile their own version and strong name it with the original keys. This is a common practice with open source projects, but it does mean that you shouldn't use the strong name as a guarantee of security or identity.

## License and copyright

Copyright Matthew King 2015-2021.
Distributed under the [MIT License](http://opensource.org/licenses/MIT). Refer to license.txt for more information.
