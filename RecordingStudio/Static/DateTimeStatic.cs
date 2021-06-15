using System;

namespace RecordingStudio.Static
{
    public static class DateTimeStatic
    {
        public static DateTime StartDay(this DateTime dateTime) => dateTime.AddHours(-dateTime.TimeOfDay.Hours);
        public static DateTime StartDay(this DateTime dateTime, int hours) => dateTime.AddHours(-dateTime.TimeOfDay.Hours).AddHours(hours);
    }
}