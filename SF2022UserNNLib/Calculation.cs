namespace SF2022UserNNLib;

    public class Calculations
    {
        public static string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            if (startTimes == null ||  durations == null  || startTimes.Length != durations.Length)
            {
                throw new ArgumentException("Invalid arguments.");
            }

            var endTime = endWorkingTime - TimeSpan.FromMinutes(consultationTime);

            var availablePeriods = new List<string>();
            var currentEndTime = beginWorkingTime;
            for (int i = 0; i < startTimes.Length; i++)
            {
                var periodStartTime = startTimes[i];
                var periodEndTime = periodStartTime + TimeSpan.FromMinutes(durations[i]);

                if (periodStartTime > currentEndTime)
                {
                    availablePeriods.Add($"{currentEndTime:hh\\:mm}-{periodStartTime:hh\\:mm}");
                }

                currentEndTime = periodEndTime > currentEndTime ? periodEndTime : currentEndTime;
            }

            if (currentEndTime < endTime)
            {
                availablePeriods.Add($"{currentEndTime:hh\\:mm}-{endTime:hh\\:mm}");
            }

            return availablePeriods.ToArray();
        }
        
    }

