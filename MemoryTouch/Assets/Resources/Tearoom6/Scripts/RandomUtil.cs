using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RandomUtil
{
    private static System.Random random = new System.Random();

    private static int previousInt = 0;

    private static string charSeeds = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+-=!?";

    /// <summary>
    /// Draws new int.
    /// This return value contains min and max value.
    /// </summary>
    /// <returns>The new int.</returns>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    /// <param name="previous">Previous.</param>
    public static int DrawNewInt(int min, int max, int previous)
    {
        // error check & eternally loop prevention
        if (min >= max)
            return min;
        while (true)
        {
            int drawValue = random.Next(min, max + 1);
            if (drawValue != previous)
                return drawValue;
        }
    }

    /// <summary>
    /// Draws new int.
    /// Internal previous value is persisted in this class.
    /// </summary>
    /// <returns>The new int.</returns>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    public static int DrawNewInt(int min, int max)
    {
        previousInt = DrawNewInt(min, max, previousInt);
        return previousInt;
    }

    /// <summary>
    /// Get random int.
    /// This return value contains min and max value.
    /// </summary>
    /// <returns>The int.</returns>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    public static int RandomInt(int min, int max)
    {
        if (min >= max)
            return min;
        return random.Next(min, max + 1);
    }

    /// <summary>
    /// Get random float.
    /// This return value contains min and max value.
    /// </summary>
    /// <returns>The float.</returns>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    public static float RandomFloat(float min = 0f, float max = 1f)
    {
        if (min >= max)
            return min;
        return min + (float)random.NextDouble() * (max - min);
    }

    /// <summary>
    /// Get random color.
    /// </summary>
    /// <returns>The color.</returns>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    public static Color RandomColor(float min = 0f, float max = 1f)
    {
        return new Color(
            RandomFloat(min, max),
            RandomFloat(min, max),
            RandomFloat(min, max)
        );
    }

    /// <summary>
    /// Draw the specified weightedMap.
    /// </summary>
    /// <param name="weightedMap">Weighted map.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static T Draw<T>(Dictionary<T, int> weightedMap)
    {
        int sum = weightedMap.Values.Take(weightedMap.Count).Sum();
        int rand = RandomInt(1, sum);

        int weight = 0;
        foreach (T key in weightedMap.Keys)
        {
            weight += weightedMap[key];
            if (weight >= rand)
                return key;
        }

        return default(T);
    }

    /// <summary>
    /// Get random char.
    /// </summary>
    /// <returns>The char.</returns>
    public static char RandomChar()
    {
        return charSeeds[RandomInt(0, charSeeds.Length - 1)];
    }

    /// <summary>
    /// Generates the random string.
    /// </summary>
    /// <returns>The random string.</returns>
    /// <param name="length">Length.</param>
    public static string GenerateRandomStr(int length)
    {
        StringBuilder builder = new StringBuilder();
        for (int i=0; i < length; i++)
            builder.Append(RandomChar());
        return builder.ToString();
    }

}
