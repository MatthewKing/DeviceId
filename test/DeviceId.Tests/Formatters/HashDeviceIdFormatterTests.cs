using System;
using System.Security.Cryptography;
using DeviceId.Components;
using DeviceId.Encoders;
using DeviceId.Formatters;
using FluentAssertions;
using Xunit;

namespace DeviceId.Tests.Formatters
{
    public class HashDeviceIdFormatterTests
    {
        [Fact]
        public void Constructor_HashAlgorithmIsNull_ThrowsArgumentNullException()
        {
            Action act = () => new HashDeviceIdFormatter(null, new HexByteArrayEncoder());

            act.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: hashAlgorithm");
        }

        [Fact]
        public void Constructor_ByteArrayEncoderIsNull_ThrowsArgumentNullException()
        {
            Action act = () => new HashDeviceIdFormatter(() => MD5.Create(), null);

            act.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: byteArrayEncoder");
        }

        [Fact]
        public void GetDeviceId_ComponentsIsNull_ThrowsArgumentNullException()
        {
            var formatter = new HashDeviceIdFormatter(() => MD5.Create(), new HexByteArrayEncoder());

            Action act = () => formatter.GetDeviceId(null);

            act.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: components");
        }

        [Fact]
        public void GetDeviceId_ComponentsIsEmpty_ReturnsDeviceId()
        {
            var formatter = new HashDeviceIdFormatter(() => MD5.Create(), new HexByteArrayEncoder());

            var deviceId = formatter.GetDeviceId(new IDeviceIdComponent[] { });

            deviceId.Should().Be("d41d8cd98f00b204e9800998ecf8427e");
        }

        [Fact]
        public void GetDeviceId_ComponentsAreValid_ReturnsDeviceId()
        {
            var formatter = new HashDeviceIdFormatter(() => MD5.Create(), new HexByteArrayEncoder());

            var deviceId = formatter.GetDeviceId(new IDeviceIdComponent[]
            {
                new DeviceIdComponent("Test1", "Test1"),
                new DeviceIdComponent("Test2", "Test2"),
            });

            deviceId.Should().Be("b02f4481c190173f05192bc08a1b14bc");
        }
    }
}
