using UnityEngine;
using System.Collections;

/// <summary>
/// ゲーム内で使用する定数の管理。
/// </summary>
public class GameConstants
{
    public static string VERSION = "1.0.0";

    public static float Z_DEPTH_BASE = 5f;

    public static int POINT_STAGE_BONUS = 50;

    public static int POINT_TIME_BONUS_FOR_SECOND = 20;

    public static int GET_RANKING_RECORD_COUNT = 10;

    public static string API_URL_POST_RANKING_RECORD = "http://tearoom6-api.appspot.com/rest/memorytouch/rankingrecords/";

    public static string API_URL_GET_RANKING_RECORDS = "http://tearoom6-api.appspot.com/rest/memorytouch/rankingrecords/{0}/";

    public static string RESOURCE_PREFAB_PANEL = "MemoryTouch/Prefabs/Panel";

    public static string RESOURCE_PREFAB_STAGE_DIALOG = "MemoryTouch/Prefabs/StageDialog";

    public static string RESOURCE_PREFAB_TOUCH_EFFECT = "MemoryTouch/Prefabs/TouchEffect";

    public static string RESOURCE_PREFAB_TRAIL_EFFECT = "MemoryTouch/Prefabs/TrailEffect";

    public static string RESOURCE_PREFAB_TWINKLE_EFFECT = "MemoryTouch/Prefabs/TwinkleEffect";

    public static string RESOURCE_PREFAB_HELP_BOARD_1 = "MemoryTouch/Prefabs/HelpBoard1";

    public static string RESOURCE_PREFAB_HELP_BOARD_2 = "MemoryTouch/Prefabs/HelpBoard2";

    public static string RESOURCE_PREFAB_HELP_BOARD_3 = "MemoryTouch/Prefabs/HelpBoard3";

    public static string RESOURCE_PREFAB_RECORD_BOARD = "MemoryTouch/Prefabs/RecordBoard";

    public static string RESOURCE_PREFAB_SETTING_BOARD = "MemoryTouch/Prefabs/SettingBoard";

    public static string RESOURCE_PREFAB_PAUSE_DIALOG = "MemoryTouch/Prefabs/PauseDialog";

    public static string RESOURCE_PREFAB_INIT_ICONS = "MemoryTouch/Prefabs/InitIcons";

    public static string RESOURCE_PREFAB_SIMPLE_TEXT = "MemoryTouch/Prefabs/SimpleText";

    public static string RESOURCE_PREFAB_CUSTOM_BUTTON = "MemoryTouch/Prefabs/CustomButton";

    public static string RESOURCE_PREFAB_CUSTOM_LABEL = "MemoryTouch/Prefabs/CustomLabel";

    public static string RESOURCE_PREFAB_CUSTOM_BG_PANEL = "MemoryTouch/Prefabs/CustomBgPanel";

    public static string RESOURCE_PREFAB_CUSTOM_DIALOG = "MemoryTouch/Prefabs/CustomDialog";

    public static string RESOURCE_PREFAB_BACK_BUTTON = "MemoryTouch/Prefabs/BackButton";

    public static string RESOURCE_PREFAB_AUTO_BUTTON = "MemoryTouch/Prefabs/AutoButton";

    public static string RESOURCE_PREFAB_FINGER = "MemoryTouch/Prefabs/Finger";

    public static string RESOURCE_PREFAB_CROWN = "MemoryTouch/Prefabs/Crown";

    public static string RESOURCE_SETTINGS_ROOT = "MemoryTouch/Settings/";

    public static string PREF_KEY_POINT = "Point_{0}";

    public static string PREF_KEY_STAGE = "Stage_{0}";

    public static string PREF_KEY_STAGE_CONTINUE = "Stage_Continue_{0}";

    public static string PREF_KEY_QUEST_COMPLETED = "QuestCompleted";

    public static string PREF_KEY_TUTORIAL_LEVEL = "TutorialLevel";

    public static string PREF_KEY_USER_KEY = "UserKey";

    public static string PREF_KEY_USER_NAME = "UserName";

    public static string TIMER_KEY_BEFORE_DEMO = "BeforeDemo";

    public static string TIMER_KEY_TOAST = "Toast";

    public static string TIMER_KEY_WAIT_NEXT_STAGE = "WaitNextStage";

    public static string TIMER_KEY_HELP_PANEL_FLUSH = "HelpPanelFlush_{0}_{1}";

    public static string TIMER_KEY_HELP_PANEL_FINGER = "HelpPanelFinger_{0}_{1}";

    public static string TIMER_KEY_PREFIX_PANEL_COLOR = "PanelColor_";

    public static string TIMER_KEY_PREFIX_PANEL_FLICK = "PanelFlick_";

    public static string TIMER_KEY_PREFIX_PANEL_DEMO = "PanelDemo_";

    public static string TIMER_KEY_PREFIX_AUTO_SWEEP = "AutoSweep_";

    public static string LOCK_KEY_SELECT_MENU = "SelectMenu";

    public static string LOCK_KEY_BEFORE_SWEEP = "BeforeSweep";

    public static string ENCRIPT_PASSWORD = "Djo8Kh3Lnyd7dW";

}
