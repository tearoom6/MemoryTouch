using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Timer needs to be attached to any game object!!
 */
public class Timer : MonoBehaviour
{

    private static Dictionary<string, float> timers = new Dictionary<string, float>();

    void Update()
    {
        // To avoid "InvalidOperationException: out of sync", you must iterate without the directory itself.
        var keys = new List<string>(timers.Keys);
        foreach (string key in keys)
        {
            timers[key] -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Determines if is timeout the specified key.
    /// </summary>
    /// <returns><c>true</c> if is timeout the specified key; otherwise, <c>false</c>.</returns>
    /// <param name="key">Key.</param>
    public static bool IsTimeout(string key)
    {
        if (!timers.ContainsKey(key))
            return false;
        if (timers[key] >= 0f)
            return false;
        Logger.Log(string.Format("Timer (key={0}) timed out.", key));
        timers.Remove(key);
        return true;
    }

    /// <summary>
    /// Determines if is available the specified key.
    /// </summary>
    /// <returns><c>true</c> if is available the specified key; otherwise, <c>false</c>.</returns>
    /// <param name="key">Key.</param>
    public static bool IsAvailable(string key)
    {
        if (!timers.ContainsKey(key))
            return false;
        return true;
    }

    /// <summary>
    /// Sets the timer.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="time">Time.</param>
    public static void SetTimer(string key, float time)
    {
        Logger.Log(string.Format("Timer (key={0} time={1}) set.", key, time.ToString()));
        timers[key] = time;
    }

    /// <summary>
    /// Clears all.
    /// </summary>
    public static void ClearAll()
    {
        timers.Clear();
    }
}
