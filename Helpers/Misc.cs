using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Helpers
{
    public class Misc
    {
        public Misc()
        {

        }

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

        public static void RegexChangeFormat()
        {
            // cambio formato del regex, col mese non funziona benissimo, meglio con diverse formati numerici di data

            string input = "adsfasdfsdf 25 novembre 2010 adsfadfssafasdf ";

            Regex.Replace(input,
                       @"\b(?<day>\d{1,2})( )(?<month>:gen(?:naio)?|feb(?:braio)?|mar(?:zo)?|apr(?:ile)?|maggio|giu(?:gno)?|lug(?:lio)?|ago(?:sto)?|set(?:tembre)?|ott(?:obre)?|(nov|dic)(?:embre)?)( )(?<year>\d{2,4})\b",
                      "${day}-${month}-${year}", RegexOptions.None);
        }

        public int ElapsedWorkingHours()
        {
            var start = new DateTime(2021, 04, 28, 16, 30, 12);
            var end = new DateTime(2021, 04, 29, 10, 25, 0);
            int count = 0;

            for (var i = start; i < end; i = i.AddMinutes(1))
            {
                if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday)
                {
                    if (i.TimeOfDay.Hours >= 8 && i.TimeOfDay.Hours < 17)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public void ValidateMacAddress(string macAddress)
        {
            // validazione mac address 48 bit
            string pattern = string.Empty;
            if (macAddress.Length == 12)
            {
                pattern = @"^[0-9a-fA-F]{12}$";
                Regex rg = new(pattern);
                if (!rg.Match(macAddress).Success)
                {
                    //AddError(new BaseValidatorTranslation(ValidatorMessageType.EmptyValidationError, "system.error.invalidmacaddress"));
                }
            }
            else
            {
                pattern = @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})|([0-9a-fA-F]{4}\\.[0-9a-fA-F]{4}\\.[0-9a-fA-F]{4})$";
                Regex rg = new(pattern);
                if (!rg.Match(macAddress).Success)
                {
                    //AddError(new BaseValidatorTranslation(ValidatorMessageType.EmptyValidationError, "system.error.invalidmacaddress"));
                }
            }
        }
    }
}
