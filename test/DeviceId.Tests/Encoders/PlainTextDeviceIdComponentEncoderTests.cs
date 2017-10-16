using DeviceId.Components;
using DeviceId.Encoders;
using FluentAssertions;
using Xunit;

namespace DeviceId.Tests.Encoders
{
    public class PlainTextDeviceIdComponentEncoderTests
    {
        [Fact]
        public void Encode_ReturnsPlainTextComponentValue()
        {
            var encoder = new PlainTextDeviceIdComponentEncoder();

            var component = new DeviceIdComponent("Name", "Value");

            encoder.Encode(component).Should().Be("Value");
        }
    }
}
