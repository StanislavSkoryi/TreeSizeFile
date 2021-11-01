using System;

namespace WPFApp.Extensions
{
    public static class SpaceSizeExtensions
    {
        // 1024 * 1024 * 1024;
        private static double GB_DIVIDER = 1073741824;
        private static double MB_DIVIDER = 1048576;
        private static double KB_DIVIDER = 1024;

        public static double RoundToGb(this long size, int digits)
        {
            return Math.Round(size / GB_DIVIDER, digits);
        }

        public static double RoundToMb(this long size, int digits)
        {
            return Math.Round(size / MB_DIVIDER, digits);
        }
        public static double RoundToKb(this long size, int digits)
        {
            return Math.Round(size / KB_DIVIDER, digits);
        }
    }
}
