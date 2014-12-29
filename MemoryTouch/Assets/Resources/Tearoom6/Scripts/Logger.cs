using UnityEngine;
using System.Collections;

public class Logger : MonoBehaviour
{

    private static LogLevel outputLevel = LogLevel.DEBUG;

    public enum LogLevel
    {
        DEBUG,
        INFO,
        WARN,
        ERROR
    }

    /// <summary>
    /// Log by the specified component, msg and level.
    /// </summary>
    /// <param name="component">Component.</param>
    /// <param name="msg">Message.</param>
    /// <param name="level">Level.</param>
    public static void Log(Component component, string msg, LogLevel level = LogLevel.DEBUG)
    {
        if (!CanOutput(level))
            return;
        switch (level)
        {
            case LogLevel.DEBUG:
                Debug.Log(string.Format("[{0}][DEBUG][{1}] {2}", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), component.GetType(), msg));
                break;
            case LogLevel.INFO:
                Debug.Log(string.Format("[{0}][INFO][{1}] {2}", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), component.GetType(), msg));
                break;
            case LogLevel.WARN:
                Debug.LogWarning(string.Format("[{0}][WARN][{1}] {2}", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), component.GetType(), msg));
                break;
            case LogLevel.ERROR:
                Debug.LogError(string.Format("[{0}][ERROR][{1}] {2}\n{3}", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), component.GetType(), msg, 
                        UnityEngine.StackTraceUtility.ExtractStackTrace()));
                break;
        }
    }

    /// <summary>
    /// Log by the specified msg and level.
    /// </summary>
    /// <param name="msg">Message.</param>
    /// <param name="level">Level.</param>
    public static void Log(string msg, LogLevel level = LogLevel.DEBUG)
    {
        if (!CanOutput(level))
            return;
        switch (level)
        {
            case LogLevel.DEBUG:
                Debug.Log(string.Format("[{0}][DEBUG] {1}", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), msg));
                break;
            case LogLevel.INFO:
                Debug.Log(string.Format("[{0}][INFO] {1}", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), msg));
                break;
            case LogLevel.WARN:
                Debug.LogWarning(string.Format("[{0}][WARN] {1}", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), msg));
                break;
            case LogLevel.ERROR:
                Debug.LogError(string.Format("[{0}][ERROR] {1}\n{2}", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), msg, 
                        UnityEngine.StackTraceUtility.ExtractStackTrace()));
                break;
        }
    }

    /// <summary>
    /// Determines if log can be outputed by the specified level.
    /// </summary>
    /// <returns><c>true</c> if can output the specified level; otherwise, <c>false</c>.</returns>
    /// <param name="level">Level.</param>
    public static bool CanOutput(LogLevel level)
    {
        switch (level)
        {
            case LogLevel.DEBUG:
                if (outputLevel == LogLevel.DEBUG)
                    return true;
                return false;
            case LogLevel.INFO:
                if (outputLevel == LogLevel.DEBUG || outputLevel == LogLevel.INFO)
                    return true;
                return false;
            case LogLevel.WARN:
                if (outputLevel == LogLevel.DEBUG || outputLevel == LogLevel.INFO || outputLevel == LogLevel.WARN)
                    return true;
                return false;
            case LogLevel.ERROR:
                return true;
        }
        return false;
    }

    public static void Error(string msg, Component component = null)
    {
        if (component == null)
            Log(msg, LogLevel.ERROR);
        else
            Log(component, msg, LogLevel.ERROR);
    }

    public static void Warn(string msg, Component component = null)
    {
        if (component == null)
            Log(msg, LogLevel.WARN);
        else
            Log(component, msg, LogLevel.WARN);
    }

    public static void Info(string msg, Component component = null)
    {
        if (component == null)
            Log(msg, LogLevel.INFO);
        else
            Log(component, msg, LogLevel.INFO);
    }
}
