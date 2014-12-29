using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Scheduler needs to be attached to any game object!!
 */
public class Scheduler : MonoBehaviour
{

    private static Dictionary<string, System.Action> schedules = new Dictionary<string, System.Action>();

    void Update()
    {
        // To avoid "InvalidOperationException: out of sync", you must iterate without the directory itself.
        var keys = new List<string>(schedules.Keys);
        foreach (string key in keys)
        {
            if (!Timer.IsTimeout(key))
            {
                continue;
            }
            schedules[key].Invoke();
            schedules.Remove(key);
        }
    }

    /// <summary>
    /// Adds the schedule.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="time">Time.</param>
    /// <param name="action">Action.</param>
    public static void AddSchedule(string key, float time, System.Action action)
    {
        Timer.SetTimer(key, time);
        schedules[key] = action;
    }

}
