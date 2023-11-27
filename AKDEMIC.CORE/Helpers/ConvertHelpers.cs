using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace AKDEMIC.CORE.Helpers
{
    public class ConvertHelpers
    {
        #region TEXT CONVERTERS

        public static string ErrorCodeToText(int errorCode)
        {
            switch (errorCode)
            {
                case 401:
                    return "Acceso no autorizado";
                case 403:
                    return "Acceso denegado";
                case 404:
                    return "Recurso no encontrado";
                case 500:
                    return "Ocurrió un problema en el servidor";
                default:
                    return "Ocurrió un problema en el servidor";
            }
        }

        public static string ErrorCodeToText(string errorCode)
        {
            if (int.TryParse(errorCode, out int tmpErrorCode))
            {
                return ErrorCodeToText(tmpErrorCode);
            }

            return null;
        }

        public static string NumberToText(int number)
        {
            switch (number)
            {
                case 0:
                    return "CERO";
                case 1:
                    return "UNO";
                case 2:
                    return "DOS";
                case 3:
                    return "TRES";
                case 4:
                    return "CUATRO";
                case 5:
                    return "CINCO";
                case 6:
                    return "SEIS";
                case 7:
                    return "SIETE";
                case 8:
                    return "OCHO";
                case 9:
                    return "NUEVE";
                case 10:
                    return "DIEZ";
                case 11:
                    return "ONCE";
                case 12:
                    return "DOCE";
                case 13:
                    return "TRECE";
                case 14:
                    return "CATORCE";
                case 15:
                    return "QUINCE";
                case 16:
                    return "DIECISÉIS";
                case 17:
                    return "DIECISIETE";
                case 18:
                    return "DIECIOCHO";
                case 19:
                    return "DIECINUEVE";
                case 20:
                    return "VEINTE";
                default:
                    return null;
            }
        }

        public static string NumberToText(string number)
        {
            if (int.TryParse(number, out int tmpNumber))
            {
                return NumberToText(tmpNumber);
            }

            return null;
        }

        public static string RemoveAccent(string text)
        {
            //Tailspin uses Cyrillic (ISO-8859-5); others use Hebraw (ISO-8859-8)
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);

            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        #endregion

        #region DATETIME CONVERTERS

        public static DateTime RandomDateTime(DateTime minValue, DateTime maxValue)
        {
            var random = new Random();
            var dateTimeDifference = maxValue - minValue;
            var randomTimeSpan = new TimeSpan((long)(random.NextDouble() * dateTimeDifference.Ticks));

            return minValue + randomTimeSpan;
        }

        public static bool DateTimeConflict(DateTime startA, DateTime endA, DateTime startB, DateTime endB) => startA < endB && startB < endA;

        public static bool TimeSpanConflict(TimeSpan st1, TimeSpan et1, TimeSpan st2, TimeSpan et2) => (st1 <= st2 && et1 > st2) || (st1 < et2 && et1 >= et2) || (st2 <= st1 && et2 > st1) || (st2 < et1 && et2 >= et1);
        
        #endregion

        #region DATEPICKER CONVERTERS

        public static DateTime DatepickerToDatetime(string date)
        {
            return DateTime.ParseExact(date, ConstantHelpers.FORMATS.DATE, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DateTime DatepickerToUtcDateTime(string date)
        {
            var dt = DateTime.ParseExact(date, ConstantHelpers.FORMATS.DATE, System.Globalization.CultureInfo.InvariantCulture);//.ToUniversalTime();
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dt, FindOperatingSystemTimeZoneById(ConstantHelpers.LINUX_TIMEZONE_ID, ConstantHelpers.OSX_TIMEZONE_ID, ConstantHelpers.WINDOWS_TIMEZONE_ID));
        }

        public static DateTime DatepickerCustomToUtcDateTime(string date)
        {
            var dt = DateTime.ParseExact(date, ConstantHelpers.FORMATS.DATETIME_CUSTOM, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime();
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dt, FindOperatingSystemTimeZoneById(ConstantHelpers.LINUX_TIMEZONE_ID, ConstantHelpers.OSX_TIMEZONE_ID, ConstantHelpers.WINDOWS_TIMEZONE_ID));
        }

        #endregion

        #region DATETIMEPICKER CONVERTERS

        public static DateTime DatetimepickerToDateTime(string datetime)
        {
            return DateTime.ParseExact(datetime, ConstantHelpers.FORMATS.DATETIME, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DateTime DatetimepickerToUtcDateTime(string datetime)
        {
            var dt = DateTime.ParseExact(datetime, ConstantHelpers.FORMATS.DATETIME, System.Globalization.CultureInfo.InvariantCulture);
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dt, ConvertHelpers.FindOperatingSystemTimeZoneById(ConstantHelpers.LINUX_TIMEZONE_ID, ConstantHelpers.OSX_TIMEZONE_ID, ConstantHelpers.WINDOWS_TIMEZONE_ID));
        }
        public static DateTime DatetimepickerCustomToUtcDateTime(string datetime)
        {
            var dt = DateTime.ParseExact(datetime, ConstantHelpers.FORMATS.DATETIME_CUSTOM, System.Globalization.CultureInfo.InvariantCulture);
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dt, ConvertHelpers.FindOperatingSystemTimeZoneById(ConstantHelpers.LINUX_TIMEZONE_ID, ConstantHelpers.OSX_TIMEZONE_ID, ConstantHelpers.WINDOWS_TIMEZONE_ID));
        }
        #endregion

        #region TIMEPICKER CONVERTERS

        public static DateTime TimepickerToDateTime(string time)
        {
            var dt = DateTime.ParseExact(time, ConstantHelpers.FORMATS.TIME, System.Globalization.CultureInfo.InvariantCulture);
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(dt, FindOperatingSystemTimeZoneById(ConstantHelpers.LINUX_TIMEZONE_ID, ConstantHelpers.OSX_TIMEZONE_ID, ConstantHelpers.WINDOWS_TIMEZONE_ID), ConvertHelpers.FindOperatingSystemTimeZoneById(ConstantHelpers.LINUX_TIMEZONE_ID, ConstantHelpers.OSX_TIMEZONE_ID, ConstantHelpers.WINDOWS_TIMEZONE_ID));
        }

        public static DateTime TimepickerToUtcDateTime(string time)
        {
            var dt = DateTime.ParseExact(time, ConstantHelpers.FORMATS.TIME, System.Globalization.CultureInfo.InvariantCulture);
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dt, FindOperatingSystemTimeZoneById(ConstantHelpers.LINUX_TIMEZONE_ID, ConstantHelpers.OSX_TIMEZONE_ID, ConstantHelpers.WINDOWS_TIMEZONE_ID));
        }

        public static DateTime TimeToUtcDateTime(string time)
        {
            var dt = DateTime.ParseExact(time, "H:mm", System.Globalization.CultureInfo.InvariantCulture);
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dt, ConvertHelpers.FindOperatingSystemTimeZoneById(ConstantHelpers.LINUX_TIMEZONE_ID, ConstantHelpers.OSX_TIMEZONE_ID, ConstantHelpers.WINDOWS_TIMEZONE_ID));
        }

        public static TimeSpan TimepickerToTimeSpan(string time)
        {
            return TimepickerToDateTime(time).TimeOfDay;
        }
        public static TimeSpan TimepickerToUtcTimeSpan(string time)
        {
            return TimepickerToUtcDateTime(time).TimeOfDay;
        }

        #endregion

        #region TIMEZONEINFO CONVERTERS

        public static TimeZoneInfo FindOperatingSystemTimeZoneById(string linuxId, string osxId, string windowsId)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return TimeZoneInfo.FindSystemTimeZoneById(windowsId);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return TimeZoneInfo.FindSystemTimeZoneById(osxId);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return TimeZoneInfo.FindSystemTimeZoneById(linuxId);
            }

            return null;
        }

        #endregion

        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static string StringToMySQLPasswordHash(string key)
        {
            var keyArray = Encoding.UTF8.GetBytes(key);
            var enc = new SHA1Managed();
            var encodedKey = enc.ComputeHash(enc.ComputeHash(keyArray));
            var myBuilder = new StringBuilder(encodedKey.Length);

            foreach (byte b in encodedKey)
                myBuilder.Append(b.ToString("X2"));

            return "*" + myBuilder.ToString();
        }

        public static string StringToMySQLOldPasswordHash(string key)
        {
            var result = new uint[2];
            var nr = (uint)1345345333;
            var add = (uint)7;
            var nr2 = (uint)0x12345671;

            //uint tmp;
            var password = key.ToCharArray();

            for (var i = 0; i < key.Length; i++)
            {
                if (password[i] == ' ' || password[i] == '\t')
                    continue;

                var tmp = password[i];
                nr ^= (((nr & 63) + add) * tmp) + (nr << 8);
                nr2 += (nr2 << 8) ^ nr;
                add += tmp;
            }

            result[0] = nr & (((uint)1 << 31) - (uint)1);
            var val = (((uint)1 << 31) - (uint)1);
            result[1] = nr2 & val;
            var hash = string.Format("{0:X}{1:X}", result[0], result[1]);
            return hash.ToLower();
        }
    }
}
