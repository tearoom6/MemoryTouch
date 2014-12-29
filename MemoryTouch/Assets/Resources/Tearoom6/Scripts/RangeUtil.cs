using System;

public class RangeUtil
{
    /// <summary>
    /// Determines if is between the specified target min max.
    /// </summary>
    /// <returns><c>true</c> if is between the specified target min max; otherwise, <c>false</c>.</returns>
    /// <param name="target">Target.</param>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    public static bool IsBetween(int target, int min, int max)
    {
        if (max < min) {
            int buffer = max;
            max = min;
            min = buffer;
        }
        return min <= target && target <= max;
    }

    /// <summary>
    /// Determines if is between the specified target min max.
    /// </summary>
    /// <returns><c>true</c> if is between the specified target min max; otherwise, <c>false</c>.</returns>
    /// <param name="target">Target.</param>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    public static bool IsBetween(float target, float min, float max)
    {
        if (max < min) {
            float buffer = max;
            max = min;
            min = buffer;
        }
        return min <= target && target <= max;
    }

}
