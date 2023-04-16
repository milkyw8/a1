using SF2022UserNNLib;

namespace LibaryTesting;


public class UnitTest1
{
    [Fact]
    public void Method1()
    {
        // Arrange
        TimeSpan[] startTimes = { };
        int[] durations = { };
        var beginWorkingTime = new TimeSpan(8, 0, 0);
        var endWorkingTime = new TimeSpan(18, 0, 0);
        int consultationTime = 30;
        var expected = new[] { "08:00-17:30" };

        // Act
        var actual =
            Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

        // Assert
        Assert.Equal(expected, actual);

    }
}

