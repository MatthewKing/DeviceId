using DeviceId.Linux.Components;
using FluentAssertions;
using Xunit;

namespace DeviceId.Tests.Components;

public class CpuInfoIdComponentTests
{
    [Fact]
    public void GetValue_ShouldReturnSameValueForMultipleExecutions()
    {
        var component = new CpuInfoIdComponent();
        var expectedComponentValue = component.GetValue();

        for (int i = 0; i < 10; i++)
        {
            var currentValue = component.GetValue();
            currentValue.Should().Be(expectedComponentValue);
        }
    }
}
