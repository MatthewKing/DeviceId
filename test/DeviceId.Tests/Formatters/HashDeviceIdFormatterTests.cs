using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using DeviceId.Components;
using DeviceId.Encoders;
using DeviceId.Formatters;
using FluentAssertions;
using Xunit;

namespace DeviceId.Tests.Formatters;

public class HashDeviceIdFormatterTests
{
    [Fact]
    public void Constructor_HashAlgorithmIsNull_ThrowsArgumentNullException()
    {
        Action act = () => new HashDeviceIdFormatter(null, new HexByteArrayEncoder());

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_ByteArrayEncoderIsNull_ThrowsArgumentNullException()
    {
        Action act = () => new HashDeviceIdFormatter(() => MD5.Create(), null);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetDeviceId_ComponentsIsNull_ThrowsArgumentNullException()
    {
        var formatter = new HashDeviceIdFormatter(() => MD5.Create(), new HexByteArrayEncoder());

        Action act = () => formatter.GetDeviceId(null);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetDeviceId_ComponentsIsEmpty_ReturnsDeviceId()
    {
        var formatter = new HashDeviceIdFormatter(() => MD5.Create(), new HexByteArrayEncoder());

        var deviceId = formatter.GetDeviceId(new Dictionary<string, IDeviceIdComponent>(StringComparer.OrdinalIgnoreCase));

        deviceId.Should().Be("d41d8cd98f00b204e9800998ecf8427e");
    }

    [Fact]
    public void GetDeviceId_ComponentsAreValid_ReturnsDeviceId()
    {
        var formatter = new HashDeviceIdFormatter(() => MD5.Create(), new HexByteArrayEncoder());

        var deviceId = formatter.GetDeviceId(new Dictionary<string, IDeviceIdComponent>(StringComparer.OrdinalIgnoreCase)
        {
            ["Test1"] = new DeviceIdComponent("Test1"),
            ["Test2"] = new DeviceIdComponent("Test2"),
        });

        deviceId.Should().Be("b02f4481c190173f05192bc08a1b14bc");
    }

    [Fact]
    public void GetDeviceId_ComponentReturnsNull_ReturnsDeviceId()
    {
        var formatter = new HashDeviceIdFormatter(() => MD5.Create(), new HexByteArrayEncoder());

        var deviceId = formatter.GetDeviceId(new Dictionary<string, IDeviceIdComponent>(StringComparer.OrdinalIgnoreCase)
        {
            ["Test1"] = new DeviceIdComponent(default(string)),
        });

        deviceId.Should().Be("d41d8cd98f00b204e9800998ecf8427e");
    }
}
