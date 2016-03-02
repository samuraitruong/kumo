using System;
using System.Configuration;

namespace System
{
    public static class DateTimeExtensions
    {
        static string timeZoneId = ConfigurationManager.AppSettings["TimeZoneId"] ?? "Singapore Standard Time";

        /// <summary>
        /// see all time zone ID here http://stackoverflow.com/questions/7908343/list-of-timezone-ids-for-use-with-findtimezonebyid-in-c
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timezoneID"></param>
        /// <returns></returns>
        public static DateTime ConvertToZoneTime(this DateTime date, string timezoneID = "Singapore Standard Time")
        {
            DateTime utcTime = date.ToUniversalTime();

            TimeZoneInfo myZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneID);
            DateTime custDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, myZone);
            return custDateTime;

        }


        public static DateTime ToLocalTime(this DateTime dt)
        {
            // dt.DateTimeKind should be Utc!
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dt, DateTimeKind.Utc), tzi);
        }

        public static DateTime ToUtcTime(this DateTime dt)
        {
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(dt, tzi);
        }

        public static DateTime RoundDown(this DateTime dateTime, int minutes)
        {
            return new DateTime(dateTime.Year, dateTime.Month,
                 dateTime.Day, dateTime.Hour, (dateTime.Minute / minutes) * minutes, 0);
        }
    }
}
