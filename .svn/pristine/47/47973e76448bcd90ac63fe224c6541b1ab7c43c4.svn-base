using System;
using System.Linq;

namespace ITUtility
{
    public static class ByteExtensions
    {
        public enum SizeUnits
        {
            Byte, KB, MB, GB, TB, PB, EB, ZB, YB
        }

        public static string ToBase64String(this byte[] i)
        {
            if (i?.Length == 0)
                return null;

            return Convert.ToBase64String(i);
        }

        public static string ToSize(this byte[] input, SizeUnits unit = SizeUnits.Byte)
        {
            try
            {
                if (input != null && input.Any())
                {
                    Int64 value = input.Length;
                    string fileSize = (value / (double)Math.Pow(1024, (Int64)unit)).ToString("0.00");
                    return fileSize + " " + unit.ToString();
                }
                else
                {
                    return "0 " + unit.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
