
namespace HackedDesign
{
    public static class IntExtensions
    {
        /// <summary>
        /// Converts an int to a hex string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHexString(this int value)
        {
            return value.ToString("X");
        }

        /// <summary>
        /// Converts an int to a hex string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHex(this long value)
        {
            return value.ToString("X");
        }

        /// <summary>
        /// Converts an int to a Base64 string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64String(this int value)
        {
            System.Span<byte> bytes = stackalloc byte[4];
            System.BitConverter.TryWriteBytes(bytes, value);
            return System.Convert.ToBase64String(bytes).TrimEnd('=');
        }

        /// <summary>
        /// Converts an int to a Base64 string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64String(this long value)
        {
            System.Span<byte> bytes = stackalloc byte[8];
            System.BitConverter.TryWriteBytes(bytes, value);
            return System.Convert.ToBase64String(bytes);
        }
    }
}
