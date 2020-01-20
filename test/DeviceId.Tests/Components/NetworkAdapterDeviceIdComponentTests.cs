using DeviceId.Components;
using FluentAssertions;
using Xunit;

namespace DeviceId.Tests.Components
{
    public class NetworkAdapterDeviceIdComponentTests
    {
        [Fact]
        public void GetValue()
        {
            var component = new NetworkAdapterDeviceIdComponent(true, false);
            component.GetValue();
        }

        [Fact]
        public void FormatMac_NonMac()
        {
            var input = "Try me";
            var result = NetworkAdapterDeviceIdComponent.FormatMacAddress(input);
            result.ShouldBeEquivalentTo(input, "Non MAC addresses are not formatted");
        }

        [Fact]
        public void FormatMac_48BitMac()
        {
            var input = "AABBCCDDEEFF";
            var result = NetworkAdapterDeviceIdComponent.FormatMacAddress(input);
            result.ShouldBeEquivalentTo("AA:BB:CC:DD:EE:FF", "MAC address should be formatted");
        }

        [Fact]
        public void FormatMac_64BitMac()
        {
            var input = "AABBCCDDEEFF0011";
            var result = NetworkAdapterDeviceIdComponent.FormatMacAddress(input);
            result.ShouldBeEquivalentTo("AA:BB:CC:DD:EE:FF:00:11", "MAC address should be formatted");
        }
    }
}
