using SF2022UserNNLib;

namespace LibaryTesting;

/// <summary>
/// Класс с тестами для тестирования билиотеки
/// </summary>
public class UnitTest1
{
    [Fact]
    public void Method1()
    {
        // Данные
        TimeSpan[] startTimes = { };
        int[] durations = { };
        var beginWorkingTime = new TimeSpan(8, 0, 0);
        var endWorkingTime = new TimeSpan(18, 0, 0);
        int consultationTime = 30;
        var expected = new[] { "08:00-17:30" };

        // вызов метода
        var actual =
            Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

        // Проверка результата
        Assert.Equal(expected, actual);

    }
}

