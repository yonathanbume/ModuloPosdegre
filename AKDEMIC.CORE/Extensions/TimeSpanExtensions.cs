using AKDEMIC.CORE.Helpers;
using System;
using System.Globalization;

namespace AKDEMIC.CORE.Extensions
{
    public static class TimeSpanExtensions
    {
        #region REGULAR TIMESPAN

        public static TimeZoneInfo ToCustomTimeZone(this TimeSpan timeSpan, TimeZoneInfo.AdjustmentRule[] adjustmentRules = null, bool disableDaylightSavingTime = true)
        {
            var id = "Tauren Time";

            return TimeZoneInfo.CreateCustomTimeZone(
                id,
                timeSpan,
                "(GMT+00:00) Etc/Tauren Time",
                id,
                "Tauren Daylight Time",
                adjustmentRules ?? new TimeZoneInfo.AdjustmentRule[0],
                disableDaylightSavingTime
            );
        }

        public static DateTime ToLocalDateTime(this TimeSpan timeSpan)
        {
            return DateTime.Now.Date.Add(timeSpan);
        }

        public static DateTime ToUtcDateTime(this TimeSpan timeSpan)
        {
            return timeSpan.ToLocalDateTime().ToUniversalTime();
        }

        public static TimeSpan ToUtcTimeSpan(this TimeSpan timeSpan)
        {
            return timeSpan.ToUtcDateTime().TimeOfDay;
        }

        public static string ToLocalDateTimeFormat(this TimeSpan timeSpan)
        {
            return timeSpan.ToLocalDateTime().ToString(ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture);
        }

        public static string ToUtcDateTimeFormat(this TimeSpan timeSpan)
        {
            return timeSpan.ToUtcDateTime().ToString(ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture);
        }

        #endregion

        #region UTC TIMESPAN

        public static DateTime ToUtcDateTimeUtc(this TimeSpan timeSpan)
        {
            return DateTime.UtcNow.Date.Add(timeSpan);
        }

        public static DateTime ToLocalDateTimeUtc(this TimeSpan timeSpan)
        {
            return timeSpan.ToUtcDateTimeUtc().ToDefaultTimeZone();
        }

        public static TimeSpan ToLocalTimeSpanUtc(this TimeSpan timeSpan)
        {
            return timeSpan.ToLocalDateTimeUtc().TimeOfDay;
        }

        public static string ToUtcDateTimeFormatUtc(this TimeSpan timeSpan)
        {
            return timeSpan.ToUtcDateTimeUtc().ToString(ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture);
        }

        public static string ToLocalDateTimeFormatUtc(this TimeSpan timeSpan)
        {
            return timeSpan.ToLocalDateTimeUtc().ToString(ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
