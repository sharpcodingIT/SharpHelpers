// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.Globalization;

namespace SharpCoding.SharpHelpers
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Add the specific weeks to DateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="weeks"></param>
        /// <returns></returns>
        public static DateTime AddWeeks(this DateTime dateTime, int weeks)
        {
            return dateTime.AddDays(weeks * 7);
        }

        /// <summary>
        /// Return the 12:00:00 AM instance of a DateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// Return the 11:59:59 PM instance of a DateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime AbsoluteEnd(this DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Return the UTC value
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime? ToUniversalTime(this DateTime? dateTime)
        {
            return dateTime?.ToUniversalTime();
        }

        /// <summary>
        /// Return the start of the current day
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetStartOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Return the end of the current day
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetEndOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month,
                                 dateTime.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// Try to parse the string value with specific DateTime formats
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        public static DateTime? AsDateTime(this string value, string[] formats)
        {
            if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind,
                out DateTime utcDateTime))
            {
                return utcDateTime;
            }

            return null;
        }

        /// <summary>
        /// Check if a date is between two dates
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rangeBeg"></param>
        /// <param name="rangeEnd"></param>
        /// <returns></returns>
        public static bool IsBetween(this DateTime dt, DateTime rangeBeg, DateTime rangeEnd)
        {
            return dt.Ticks >= rangeBeg.Ticks && dt.Ticks <= rangeEnd.Ticks;
        }

        /// <summary>
        /// Check if a day is a weekend.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Add business days to a DateTime.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="businessDays"></param>
        /// <returns></returns>
        public static DateTime AddBusinessDays(this DateTime dateTime, int businessDays)
        {
            if (businessDays == 0)
                return dateTime;

            int direction = businessDays > 0 ? 1 : -1;
            int daysToAdd = Math.Abs(businessDays);

            while (daysToAdd > 0)
            {
                dateTime = dateTime.AddDays(direction);
                if (!dateTime.IsWeekend())
                    daysToAdd--;
            }

            return dateTime;
        }

        /// <summary>
        /// Return the age of a person.
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns></returns>
        public static int Age(this DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        /// <summary>
        /// Check if the year is a leap year.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsLeapYear(this DateTime dateTime)
        {
            int year = dateTime.Year;
            return DateTime.IsLeapYear(year);
        }
    }
}