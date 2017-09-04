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

Use the various implementations of `IDeviceIdFormatter` in the `DeviceId.Formatters` namespace (or create your own).

```csharp
string deviceId = new DeviceIdBuilder()
    .AddProcessorId()
    .AddMotherboardSerialNumber()
    .ToString(new Base64DeviceIdFormatter(hashName: "MD5", urlEncode: true));
```

Installation
------------

Just grab it from [NuGet](https://www.nuget.org/packages/DeviceId/)

`PM> Install-Package DeviceId`

License and copyright
---------------------

Copyright Matthew King 2015-2016.
Distributed under the [MIT License](http://opensource.org/licenses/MIT). Refer to license.txt for more information.
