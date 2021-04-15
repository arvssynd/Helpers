﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Helpers
{
    class Misc
    {
        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static ICollection<DateTime> GetBusinessDays(DateTime? startD, DateTime? endD)
        {
            var holidays = GetHolidays();
            if (!startD.HasValue)
            {
                startD = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0).AddMonths(-1).ToUniversalTime();
            }

            if (!endD.HasValue || (startD != endD && endD.Value.Date > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToUniversalTime()))
            {
                endD = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).ToUniversalTime();
            }

            var result = new List<DateTime>();
            for (DateTime date = startD.Value.ToLocalTime(); date <= endD.Value.ToLocalTime(); date = date.AddDays(1))
            {
                if (date.ToUniversalTime().DayOfWeek != DayOfWeek.Saturday && date.ToUniversalTime().DayOfWeek != DayOfWeek.Friday && !holidays.Contains(date.ToUniversalTime()))
                {
                    result.Add(date.ToUniversalTime());
                }
            }

            return result;
        }

        public static ICollection<DateTime> GetHolidays()
        {
            var year = DateTime.Now.Year;
            var hdays = new List<DateTime>();
            hdays.Add(new DateTime(year, 1, 1, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 1, 6, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 4, 25, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 5, 1, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 6, 2, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 8, 15, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 11, 1, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 12, 8, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 12, 25, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 12, 26, 0, 0, 0).ToUniversalTime());

            // Easter calc (sunday)
            int g = year % 19;
            int c = year / 100;
            int h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
            int i = h - h / 28 * (1 - h / 28 * (29 / (h + 1)) * ((21 - g) / 11));

            var easterDay = i - ((year + year / 4 + i + 2 - c + c / 4) % 7) + 28;
            var easterMonth = 3;

            if (easterDay > 31)
            {
                easterMonth++;
                easterDay -= 31;
            }

            // Easter Monday
            hdays.Add(new DateTime(year, easterMonth, easterDay + 1, 0, 0, 0).ToUniversalTime());
            return hdays.OrderBy(x => x.Year).ThenBy(x => x.Month).ThenBy(x => x.Day).ToList();
        }

        public static ICollection<DateTime> GetBusinessDaysMultiYear(DateTime startD, DateTime endD)
        {
            var holidays = GetHolidayByYear(startD.ToLocalTime().Year);
            var holidaysAdd = GetHolidayByYear(endD.ToLocalTime().Year);
            var holidayRange = holidays.Concat(holidaysAdd).ToList();
            //if (!startD.HasValue)
            //{
            //    startD = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0).AddMonths(-1).ToUniversalTime();
            //}

            //if (!endD.HasValue || endD.Value > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToUniversalTime())
            //{
            //    endD = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).ToUniversalTime();
            //}

            var result = new List<DateTime>();
            for (DateTime date = startD.ToLocalTime(); date <= endD.ToLocalTime(); date = date.AddDays(1))
            {
                if (date.ToUniversalTime().DayOfWeek != DayOfWeek.Saturday && date.ToUniversalTime().DayOfWeek != DayOfWeek.Friday && !holidayRange.Contains(date.ToUniversalTime()))
                {
                    result.Add(date.ToUniversalTime());
                }
            }

            return result;
        }

        public static ICollection<DateTime> GetHolidayByYear(int paramYear)
        {
            var year = paramYear;
            var hdays = new List<DateTime>();
            hdays.Add(new DateTime(year, 1, 1, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 1, 6, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 4, 25, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 5, 1, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 6, 2, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 8, 15, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 11, 1, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 12, 8, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 12, 25, 0, 0, 0).ToUniversalTime());
            hdays.Add(new DateTime(year, 12, 26, 0, 0, 0).ToUniversalTime());

            // Easter calc (sunday)
            int g = year % 19;
            int c = year / 100;
            int h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
            int i = h - h / 28 * (1 - h / 28 * (29 / (h + 1)) * ((21 - g) / 11));

            var easterDay = i - ((year + year / 4 + i + 2 - c + c / 4) % 7) + 28;
            var easterMonth = 3;

            if (easterDay > 31)
            {
                easterMonth++;
                easterDay -= 31;
            }

            // Easter Monday
            hdays.Add(new DateTime(year, easterMonth, easterDay + 1, 0, 0, 0).ToUniversalTime());
            return hdays.OrderBy(x => x.Year).ThenBy(x => x.Month).ThenBy(x => x.Day).ToList();
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }
    }
}