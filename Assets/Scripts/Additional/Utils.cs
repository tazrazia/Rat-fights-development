using System;

namespace Additional
{
    public static class Utils
    {
        public static bool IsOutOfBounds<T>(this T general, T minBound, T maxBound) 
            where T : IComparable<T>
            => Clamp(general, minBound, maxBound).CompareTo(general) != 0;
        
        public static T Clamp<T>(T value, T min, T max)
            where T : IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            else if (value.CompareTo(min) < 0)
                result = min;

            return result;
        }
    }
}
