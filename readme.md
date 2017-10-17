DeviceId
========

A simple library providing functionality to generate a 'device ID' that can be used to uniquely identify a computer.

Quickstart
----------

### Building a device identifier

Use the `DeviceIdBuilder` class to build up a device ID.

```csharp
string deviceId = new DeviceIdBuilder()
    .AddMachineName()
    .AddMacAddress()
    .AddProcessorId()
    .AddMotherboardSerialNumber()
    .ToString();
```

### Controlling how the device identifier is formatted

Use the `UseFormatter` method to set the formatter.

```csharp
string deviceId = new DeviceIdBuilder()
    .AddProcessorId()
    .AddMotherboardSerialNumber()
    .UseFormatter(new XmlDeviceIdFormatter(new HashDeviceIdComponentEncoder(() => SHA256.Create(), new Base64UrlByteArrayEncoder())))
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

Installation
------------

Just grab it from [NuGet](https://www.nuget.org/packages/DeviceId/)

```
PM> Install-Package DeviceId
```

```
$ dotnet add package DeviceId
```

License and copyright
---------------------

Copyright Matthew King 2015-2017.
Distributed under the [MIT License](http://opensource.org/licenses/MIT). Refer to license.txt for more information.
