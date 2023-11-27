using AKDEMIC.CORE.Helpers;
using System;
using System.Globalization;

namespace AKDEMIC.CORE.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ElapsedTime(this DateTime dateTime)
        {
            var dateTimeSubstract = DateTime.UtcNow.Subtract(dateTime);
            var dayDifference = (int)dateTimeSubstract.TotalDays;
            var secondsDifference = (int)dateTimeSubstract.TotalSeconds;

            if (dayDifference < 0 || dayDifference >= 31)
            {
                return null;
            }

            if (dayDifference == 0)
            {
                if (secondsDifference < 10)
                {
                    return "ahora";
                }

                if (secondsDifference < 60)
                {
                    return $"{secondsDifference} segundos";
                }

                if (secondsDifference < 120)
                {
                    return "1 minuto";
                }

                if (secondsDifference < 3600)
                {
                    return $"{ Math.Floor((double)secondsDifference / 60) } minutos";
                }
                if (secondsDifference < 7200)
                {
                    return "1 hora";
                }

                if (secondsDifference < 86400)
                {
                    return $"{ Math.Floor((double)secondsDifference / 3600) } horas";
                }
            }

            if (dayDifference == 1)
            {
                return "ayer";
            }

            if (dayDifference < 7)
            {
                return $"{ dayDifference } días";
            }

            if (dayDifference < 31)
            {
                return $"{ Math.Ceiling((double)dayDifference / 7) } semanas";
            }

            return null;
        }

        public static string ElapsedTime(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ElapsedTime();
            }

            return null;
        }

        public static string ElapsedTimeText(this DateTime dateTime, string language = "ES-PE")
        {
            var languageUpper = language.ToUpper();
            var timeSpan = DateTime.UtcNow.Subtract(dateTime);
            var totalDays = (int)timeSpan.TotalDays;
            var totalHours = (int)timeSpan.TotalHours;
            var totalMinutes = (int)timeSpan.TotalMinutes;
            var totalSeconds = (int)timeSpan.TotalSeconds;

            if (totalDays < 0)
            {
                return null;
            }

            var prefix = "";
            var suffix = "";
            var unit = "";
            var value = 0;

            if (totalSeconds < 10)
            {
                switch (languageUpper)
                {
                    case "ES-PE":
                        unit = "justo ahora";

                        break;
                    case "ES-US":
                        unit = "just now";

                        break;
                    default:
                        break;
                }
            }
            else if (totalSeconds < 60)
            {
                value = totalSeconds;

                switch (languageUpper)
                {
                    case "ES-PE":
                        unit = "segundo";

                        break;
                    case "ES-US":
                        unit = "second";

                        break;
                    default:
                        break;
                }
            }
            else if (totalMinutes < 60)
            {
                value = totalMinutes;

                switch (languageUpper)
                {
                    case "ES-PE":
                        unit = "minuto";

                        break;
                    case "ES-US":
                        unit = "minute";

                        break;
                    default:
                        break;
                }
            }
            else if (totalHours < 24)
            {
                value = totalHours;

                switch (languageUpper)
                {
                    case "ES-PE":
                        unit = "hora";

                        break;
                    case "ES-US":
                        unit = "hour";

                        break;
                    default:
                        break;
                }
            }
            else if (totalDays < 7)
            {
                value = totalDays;

                switch (languageUpper)
                {
                    case "ES-PE":
                        unit = "día";

                        break;
                    case "ES-US":
                        unit = "day";

                        break;
                    default:
                        break;
                }
            }
            else if (totalDays / 7 < 53)
            {
                value = totalDays / 7;

                switch (languageUpper)
                {
                    case "ES-PE":
                        unit = "semana";

                        break;
                    case "ES-US":
                        unit = "week";

                        break;
                    default:
                        break;
                }
            }
            else
            {
                value = totalDays / 366;

                switch (languageUpper)
                {
                    case "ES-PE":
                        unit = "año";

                        break;
                    case "ES-US":
                        unit = "year";

                        break;
                    default:
                        break;
                }
            }

            if (totalSeconds > 10)
            {
                switch (languageUpper)
                {
                    case "ES-PE":
                        prefix = "hace ";
                        suffix = $" {unit}{(value != 1 ? "s" : "")}";

                        break;
                    case "ES-US":
                        suffix = $" {unit}{(value != 1 ? "s" : "")} ago";

                        break;
                    default:
                        break;
                }
            }

            return $"{prefix}{value}{suffix}";
        }

        public static string ElapsedTimeText(this DateTime dateTime)
        {
            return ElapsedTimeText(dateTime, null);
        }

        public static DateTime ToDefaultTimeZone(this DateTime dateTime)
        {
            var destinationTimeZone = new TimeSpan(ConstantHelpers.TimeZoneInfo.Gmt, 00, 00).ToCustomTimeZone();
            var result = TimeZoneInfo.ConvertTimeFromUtc(dateTime, destinationTimeZone);
            return result;
        }

        public static DateTime? ToDefaultTimeZone(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToDefaultTimeZone();
            }

            return null;
        }

        public static string ToDateFormat(this DateTime dateTime)
        {
            return dateTime.ToString(ConstantHelpers.FORMATS.DATE, CultureInfo.InvariantCulture);
        }

        public static string ToDateFormat(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToDateFormat();
            }

            return null;
        }

        public static string ToDateTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToString(ConstantHelpers.FORMATS.DATETIME, CultureInfo.InvariantCulture);
        }

        public static string ToDateTimeFormat(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToDateTimeFormat();
            }

            return null;
        }

        public static string ToTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToString(ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture);
        }

        public static string ToTimeFormat(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToTimeFormat();
            }

            return null;
        }

        public static string ToLocalDateFormat(this DateTime dateTime)
        {
            return dateTime.ToDefaultTimeZone().ToString(ConstantHelpers.FORMATS.DATE, CultureInfo.InvariantCulture);
        }

        public static string ToLocalDateFormat(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToLocalDateFormat();
            }

            return null;
        }

        public static string ToLocalDateTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToDefaultTimeZone().ToString(ConstantHelpers.FORMATS.DATETIME, CultureInfo.InvariantCulture);
        }

        public static string ToLocalDateTimeFormat(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToLocalDateTimeFormat();
            }

            return null;
        }

        public static DateTime? ToLocalTime(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToLocalTime();
            }

            return null;
        }

        public static string ToLongDateString(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToLongDateString();
            }
                
            return null;
        }

        public static string ToLocalTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToDefaultTimeZone().ToString(ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture);
        }

        public static string ToLocalTimeFormat(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToLocalTimeFormat();
            }

            return null;
        }

        public static DateTime ToUtcDateTime(this DateTime dateTime)
        {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            var result = TimeZoneInfo.ConvertTimeToUtc(dateTime, ConvertHelpers.FindOperatingSystemTimeZoneById(ConstantHelpers.LINUX_TIMEZONE_ID, ConstantHelpers.OSX_TIMEZONE_ID, ConstantHelpers.WINDOWS_TIMEZONE_ID));
            return result;
        }
    }
}
