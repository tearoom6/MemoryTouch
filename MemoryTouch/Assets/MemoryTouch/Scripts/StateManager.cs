using UnityEngine;
using System.Collections;

/// <summary>
/// 状態と状態遷移の管理をします。
/// </summary>
public class StateManager
{

    public static State state = State.INIT_GAME;

    public static bool pause = false;

    public enum State
    {
        INIT_GAME,
        SELECT_MODE,
        INIT_STAGE,
        BEFORE_DEMO,
        DEMO,
        BEFORE_PLAY,
        PLAY,
        BEFORE_SWEEP,
        SWEEP,
        AUTO_SWEEP,
        COUNT_TIME_BONUS,
        TUTORIAL,
        VIEW_HELP,
        VIEW_INFO,
        VIEW_RECORD,
        SETTING,
        FAILED,
        COMPLETED,
        END,
    }

    /// <summary>
    /// 次の状態に遷移します。
    /// </summary>
    public static void Next()
    {
        switch (state)
        {
        case State.INIT_GAME:
            state = State.SELECT_MODE;
            break;
        case State.SELECT_MODE:
            state = State.INIT_STAGE;
            break;
        case State.INIT_STAGE:
            state = State.BEFORE_DEMO;
            break;
        case State.BEFORE_DEMO:
            state = State.DEMO;
            break;
        case State.DEMO:
            state = State.BEFORE_PLAY;
            break;
        case State.BEFORE_PLAY:
            state = State.PLAY;
            break;
        case State.PLAY:
            state = State.BEFORE_SWEEP;
            break;
        case State.BEFORE_SWEEP:
            state = State.SWEEP;
            break;
        case State.SWEEP:
            state = State.COUNT_TIME_BONUS;
            break;
        case State.AUTO_SWEEP:
            state = State.COUNT_TIME_BONUS;
            break;
        case State.COUNT_TIME_BONUS:
            state = State.INIT_STAGE;
            break;
        case State.TUTORIAL:
            state = State.INIT_STAGE;
            break;
        case State.VIEW_HELP:
            state = State.INIT_GAME;
            break;
        case State.VIEW_INFO:
            state = State.INIT_GAME;
            break;
        case State.VIEW_RECORD:
            state = State.INIT_GAME;
            break;
        case State.SETTING:
            state = State.INIT_GAME;
            break;
        case State.FAILED:
            state = State.INIT_GAME;
            break;
        case State.COMPLETED:
            state = State.INIT_GAME;
            break;
        }
    }

    /// <summary>
    /// 次の状態に遷移すると同時に、指定したアクションを呼び出します。
    /// </summary>
    /// <param name="nextAction">Next action.</param>
    public static void Next(System.Action nextAction)
    {
        Next();
        nextAction.Invoke();
    }

    /// <summary>
    /// 指定秒数後に次の状態に遷移します。
    /// </summary>
    /// <param name="delay">Delay.</param>
    /// <param name="lockKey">Lock key.</param>
    public static void Next(float delay, string lockKey)
    {
        SimpleLock.Lock(lockKey, delay, (System.Action)(() =>
            {
                StateManager.Next();
            }));
    }

    public static void AutoSweep()
    {
        state = State.AUTO_SWEEP;
    }

    public static void InsertTutorial()
    {
        state = State.TUTORIAL;
    }

    public static void ViewHelp()
    {
        state = State.VIEW_HELP;
    }

    public static void ViewRecord()
    {
        state = State.VIEW_RECORD;
    }

    public static void ViewInfo()
    {
        state = State.VIEW_INFO;
    }

    public static void EditSetting()
    {
        state = State.SETTING;
    }

    public static void Fail()
    {
        state = State.FAILED;
    }

    public static void Complete()
    {
        state = State.COMPLETED;
    }

    public static void Reset()
    {
        state = State.INIT_GAME;
    }

    /// <summary>
    /// 一時停止します。
    /// </summary>
    public static void Pause()
    {
        pause = true;
    }

    public static void QuitPausing()
    {
        pause = false;
    }

    /// <summary>
    /// 一時停止しているかどうかを返します。
    /// </summary>
    /// <returns><c>true</c> if is paused; otherwise, <c>false</c>.</returns>
    public static bool IsPaused()
    {
        return pause;
    }

    /// <summary>
    /// ユーザーのタッチ操作を受け付けるか判定します(パネルタッチは除く)。
    /// </summary>
    /// <returns><c>true</c> if is waiting touch; otherwise, <c>false</c>.</returns>
    public static bool IsWaitingTouch()
    {
        switch (state)
        {
        case State.INIT_GAME:
        case State.SELECT_MODE:
        case State.BEFORE_DEMO:
        case State.BEFORE_PLAY:
        case State.SWEEP:
        case State.TUTORIAL:
        case State.VIEW_INFO:
        case State.VIEW_HELP:
        case State.VIEW_RECORD:
        case State.SETTING:
        case State.FAILED:
        case State.COMPLETED:
            return true;
        }
        return false;
    }

}
