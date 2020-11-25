using System;
using System.Text;

namespace ITUtility.StringExtensions
{
    public static class StringExtensions
    {
        #region " IsNullOrEmpty "
        public static bool IsNullOrEmpty(this string i)
        {
            return string.IsNullOrEmpty(i);
        }

        public static bool IsNotNullOrEmpty(this string i)
        {
            return !string.IsNullOrEmpty(i);
        }
        #endregion

        #region " ToNullableString "
        public static string ToNullableString(this long? i)
        {
            if (i == null || i == (long?)null)
                return null;
            return i.ToString();
        }

        public static string ToNullableString(this int? i)
        {
            if (i == null || i == (int?)null)
                return null;
            return i.ToString();
        }

        public static string ToNullableString(this short? i)
        {
            if (i == null || i == (short?)null)
                return null;
            return i.ToString();
        }

        public static string ToNullableString(this decimal? i)
        {
            if (i == null || i == (decimal?)null)
                return null;
            return i.ToString();
        }

        public static string ToNullableString(this double? i)
        {
            if (i == null || i == (double?)null)
                return null;
            return i.ToString();
        }

        public static string ToNullableString(this DateTime? i)
        {
            if (i == null) //|| i == (DateTime?)null)
                return null;
            return i.ToString();
        }

        public static string ToNullableString(this DateTime? i, IFormatProvider provider)
        {
            if (i == null) //|| i == (DateTime?)null)
                return null;
            return (i ?? DateTime.Now).ToString(provider);
        }

        public static string ToNullableString(this DateTime? i, string format)
        {
            if (i == null) //|| i == (DateTime?)null)
                return null;
            return (i ?? DateTime.Now).ToString(format);
        }

        public static string ToNullableString(this DateTime? i, string format, IFormatProvider provider)
        {
            if (i == null) //|| i == (DateTime?)null)
                return null;
            return (i ?? DateTime.Now).ToString(format, provider);
        }

        public static string ToNullableString(this char? i)
        {
            if (i == null || i == (char?)null)
                return null;
            return i.ToString();
        }
        #endregion

        public static System.Boolean IsNumeric(this System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }
    }
}
