namespace Utilities
{
    public struct MathExt
    {
        public static bool IsNumberOutOfBounds(float number, float min, float max, float offset = 0) => number < min + offset || number > max - offset;
        public static bool IsNumberOutOfBounds(int number, int min, int max, int offset = 0) => number < min + offset || number > max - offset;
    }
}
