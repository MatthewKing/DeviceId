# DeviceId

A simple library providing functionality to generate a 'device ID' that can be used to uniquely identify a computer.

## Quickstart

### Building a device identifier

Use the `DeviceIdBuilder` class to build up a device ID.

```csharp
string deviceId = new DeviceIdBuilder()
    .AddMachineName()
    .AddProcessorId()
    .AddMotherboardSerialNumber()
    .AddSystemDriveSerialNumber()
    .ToString();
```

### What can you include in a device identifier

The following extension methods are available out of the box to suit some common use cases:

* `AddUserName()` adds the current user's username to the device ID.
* `AddMachineName()` adds the machine name to the device ID.
* `AddOSVersion()` adds the current OS version (as returned by `Environment.OSVersion`) to the device ID.
* `AddMacAddress()` adds the MAC address to the device ID.
* `AddProcessorId()` adds the processor ID to the device ID.
* `AddMotherboardSerialNumber()` adds the motherboard serial number to the device ID.
* `AddSystemDriveSerialNumber()` adds the system drive's serial number to the device ID.
* `AddSystemUUID()` adds the system UUID to the device ID.
* `AddOSInstallationID()` adds the OS installation ID.
* `AddFileToken(path)` adds a token stored at the specified path to the device ID.
* `AddRegistryValue()` adds a value from the registry.
* `AddComponent(component)` adds a custom component (see below) to the device ID.

Custom components can be built by implementing `IDeviceIdComponent`. There is also a simple `DeviceIdComponent` class that allows you to specify an arbitrary component value to use, and a `WmiDeviceIdComponent` class that uses a specified WMI property (example: `new WmiDeviceIdComponent("MACAddress", "Win32_NetworkAdapterConfiguration", "MACAddress"`).

#### Dealing with MAC Address randomization and virtual network adapters

Non physical network adapters like VPN connections tend not to have fixed MAC addresses. For wireless (802.11 based) adapters hardware (MAC) address randomization is frequently applied to avoid tracking with many modern operating systems support this out of the box. This makes wireless network adapters bad candidates for device identification.

Use `AddMacAddress(true, true)` to exclude both virtual and wireless network adapters.

### Controlling how the device identifier is formatted

Use the `UseFormatter` method to set the formatter.

```csharp
string deviceId = new DeviceIdBuilder()
    .AddProcessorId()
    .AddMotherboardSerialNumber()
    .UseFormatter(new HashDeviceIdFormatter(() => SHA256.Create(), new Base64UrlByteArrayEncoder()))
    .ToString();
```

You can use one of the out-of-the-box implementations of `IDeviceIdFormatter` in the `DeviceId.Formatters` namespace, or you can create your own.

* [StringDeviceIdFormatter](/src/DeviceId/Formatters/HashDeviceIdFormatter.cs) - Formats the device ID as a string containing each component ID, using any desired component encoding.
* [HashDeviceIdFormatter](/src/DeviceId/Formatters/HashDeviceIdFormatter.cs) - Formats the device ID as a hash string, using any desired hash algorithm and byte array encoding.
* [XmlDeviceIdFormatter](/src/DeviceId/Formatters/XmlDeviceIdFormatter.cs) - Formats the device ID as an XML document, using any desired component encoding.

There are a number of encoders that can be used customize the formatter. These implement `IDeviceIdComponentEncoder` and `IByteArrayEncoder` and are found in the `DeviceId.Encoders` namespace.

* [PlainTextDeviceIdComponentEncoder](/src/DeviceId/Encoders/PlainTextDeviceIdComponentEncoder.cs) - Encodes a device ID component as plain text.
* [HashDeviceIdComponentEncoder](/src/DeviceId/Encoders/HashDeviceIdComponentEncoder.cs) - Encodes a device ID component as a hash string, using any desired hash algorithm.
* [HexByteArrayEncoder](/src/DeviceId/Encoders/HexByteArrayEncoder.cs) - Encodes a byte array as a hex string.
* [Base64ByteArrayEncoder](/src/DeviceId/Encoders/Base64ByteArrayEncoder.cs) - Encodes a byte array as a base 64 string.
* [Base64UrlByteArrayEncoder](/src/DeviceId/Encoders/Base64UrlByteArrayEncoder.cs) - Encodes a byte array as a base 64 url-encoded string.

## Cross-platform support

The following cross-platform support is available:

| Component                  | Windows | Linux   | OSX     |
| -------------------------- | ------- | ------- | ------- |
| User name                  | **Yes** | **Yes** | **Yes** |
| Machine name               | **Yes** | **Yes** | **Yes** |
| OS version                 | **Yes** | **Yes** | **Yes** |
| Processor ID               | **Yes** | **Yes** | No      |
| MAC address                | **Yes** | **Yes** | **Yes** |
| Motherboard serial number  | **Yes** | **Yes** | No      |
| System drive serial number | **Yes** | **Yes** | **Yes** |
| System UUID                | **Yes** | **Yes** | No      |
| OS installation ID         | **Yes** | **Yes** | **Yes** |
| Registry value             | **Yes** | No      | No      |
| File token                 | **Yes** | **Yes** | **Yes** |

## Installation

Just grab it from [NuGet](https://www.nuget.org/packages/DeviceId/)

```
PM> Install-Package DeviceId
```

```
$ dotnet add package DeviceId
```

## Strong naming

From version 5 onwards, the assemblies in this package are strong named for the convenience of those users who require strong naming. Please note, however, that the key files are checked in to this repository. This means that anyone can compile their own version and strong name it with the original keys. This is a common practice with open source projects, but it does mean that you shouldn't use the strong name as a guarantee of security or identity.

## License and copyright

Copyright Matthew King 2015-2020.
Distributed under the [MIT License](http://opensource.org/licenses/MIT). Refer to license.txt for more information.
