using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleLock
{

    private static Dictionary<string, int> locks = new Dictionary<string, int>();

    /// <summary>
    /// allowNumで指定した回数に達するまでロックを掛けます。
    /// delay秒の遅延後に非同期でアクションを実行します。実行後、ロックを解除します。
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="delay">Delay.</param>
    /// <param name="action">Action.</param>
    /// <param name="allowNum">Allow number.</param>
    public static int Lock(string key, float delay, System.Action action, int allowNum=1)
    {
        int restNum = SetLock(key, allowNum);

        if (restNum >= 0)
        {
            Scheduler.AddSchedule(key + "_" + restNum.ToString(), delay, (System.Action)(() =>
                {
                    action.Invoke();
                    ReleaseLock(key);
                }));
        }

        return restNum;
    }

    /// <summary>
    /// allowNumで指定した回数に達するまでロックを掛けます。
    /// 非同期でアクションを実行します。実行後、ロックを解除します。
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="action">Action.</param>
    /// <param name="allowNum">Allow number.</param>
    public static int Lock(string key, System.Action action, int allowNum = 1)
    {
        int restNum = SetLock(key, allowNum);

        if (restNum >= 0)
        {
            action.BeginInvoke((System.AsyncCallback)((System.IAsyncResult result) =>
                {
                    ReleaseLock(key);
                }), null);
        }

        return restNum;
    }

    /// <summary>
    /// allowNumで指定した回数に達するまでロックを掛けます。
    /// int返却値を見て、実際にロックを掛けられたかどうか判断しなくてはいけません。
    /// また、実行後にロックを明示的に解除する必要もあります。
    /// </summary>
    /// <returns>The lock.</returns>
    /// <param name="key">Key.</param>
    /// <param name="allowNum">Allow number.</param>
    public static int SetLock(string key, int allowNum = 1)
    {
        // 本来なら C# の lock キーワードを使うべきだが、Unityはそこまで厳密で無くてよさそうなので
        // check rest number
        if (locks.ContainsKey(key))
        {
            if (locks[key] <= 0)
            {
                // if locked (none rest number), return -1
                Logger.Log(string.Format("Cannot set lock (key={0}).", key));
                return -1;
            }
        }
        else
        {
            locks[key] = allowNum;
        }
        // set lock
        locks[key] = locks[key] - 1;
        Logger.Log(string.Format("Lock (key={0}) set. (rest number={1}).", key, locks[key]));

        return locks[key];
    }

    /// <summary>
    /// ロックを明示的に解除します。
    /// </summary>
    /// <returns>The lock.</returns>
    /// <param name="key">Key.</param>
    public static int ReleaseLock(string key)
    {
        if (!locks.ContainsKey(key))
        {
            return -1;
        }

        locks[key] = locks[key] + 1;
        Logger.Log(string.Format("Lock (key={0}) released. (rest number={1}).", key, locks[key]));
        return locks[key];
    }

}
