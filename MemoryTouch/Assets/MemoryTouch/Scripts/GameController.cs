using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ゲーム進行全般の制御を行います。
/// </summary>
public class GameController : MonoBehaviour, PanelTouchListener, TouchListener
{
    private ScreenManager screenManager;
    private TouchHandler touchHandler;
    private StageManager stageManager;
    private PropertyManager propertyManager;
    private NetworkManager networkManager;
    private LocalDataStore localDataStore;

    private GameObject touchEffectPrefab;
    private GameObject twinkleEffectPrefab;
//    private GameObject recordBoardPrefab;
    private GameObject settingBoardPrefab;
    private GameObject pauseDialogPrefab;
    private GameObject initIconsPrefab;
    private GameObject simpleTextPrefab;
    private GameObject crownPrefab;
    private GameObject customButtonPrefab;
    private GameObject customLabelPrefab;
    private GameObject customBgPanelPrefab;
    private List<GameObject> helpBoardPrefabs;

    private GameObject slideBoards;
    private GameObject settingBoard;
    private GameObject tutorialBoard;
    private GameObject pauseDialog;
    private GameObject quitAppConfirmDialog;
    private GameObject initIcons;
    private GameObject infoText;
    private GameObject infoAuthorUrl;
    private GameObject infoPrivacyUrl;
    private GameObject selectMenu;
    private GameObject backButton;
    private GameObject autoButton;

    private Color originalAmbientColor;
    private StateManager.State stateOnTouchDown;
    private float secondsOnTouchDown;
    private int playingTutorialLv = -1; // チュートリアル中以外は-1に戻しておく

    private StepJudgeInfo stepJudgeInfo;
    private Panel currentPanel;
    private int currentStepIndex = 0;
    private int slideBoardPos = 0;
    private int maxBoardPos = 0;

    public GUIText message;
    public GUIText toast;
    public GUIText pointText;
    public GUIText secondsText;
    public GUISkin customGUISkin;

    private int point = 0;
    private float seconds = 0f;

    private string nameFieldValue = "";
    private string userKey = "";

    void Start()
    {
        AudioManager.Initialize("MemoryTouch/Audio/");

        screenManager = GetComponent<ScreenManager>();
        Logger.Info("ScreenManager component is found.", this);
        touchHandler = GetComponent<TouchHandler>();
        Logger.Info("TouchHandler component is found.", this);
        stageManager = GetComponent<StageManager>();
        Logger.Info("StageManager component is found.", this);
        propertyManager = GetComponent<PropertyManager>();
        Logger.Info("PropertyManager component is found.", this);
        networkManager = GetComponent<NetworkManager>();
        Logger.Info("NetworkManager component is found.", this);
        localDataStore = GetComponent<LocalDataStore>();
        Logger.Info("LocalDataStore component is found.", this);

        #if UNITY_EDITOR
        localDataStore.DeleteAll();
        #endif

        touchEffectPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_TOUCH_EFFECT) as GameObject;
        Logger.Info(touchEffectPrefab.name + " prefab is loaded.", this);
        twinkleEffectPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_TWINKLE_EFFECT) as GameObject;
        Logger.Info(twinkleEffectPrefab.name + " prefab is loaded.", this);
//        recordBoardPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_RECORD_BOARD) as GameObject;
//        Logger.Info(recordBoardPrefab.name + " prefab is loaded.", this);
        settingBoardPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_SETTING_BOARD) as GameObject;
        Logger.Info(settingBoardPrefab.name + " prefab is loaded.", this);
        pauseDialogPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_PAUSE_DIALOG) as GameObject;
        Logger.Info(pauseDialogPrefab.name + " prefab is loaded.", this);
        initIconsPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_INIT_ICONS) as GameObject;
        Logger.Info(initIconsPrefab.name + " prefab is loaded.", this);
        simpleTextPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_SIMPLE_TEXT) as GameObject;
        Logger.Info(simpleTextPrefab.name + " prefab is loaded.", this);
        crownPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_CROWN) as GameObject;
        Logger.Info(crownPrefab.name + " prefab is loaded.", this);
        customButtonPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_CUSTOM_BUTTON) as GameObject;
        Logger.Info(customButtonPrefab.name + " prefab is loaded.", this);
        customLabelPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_CUSTOM_LABEL) as GameObject;
        Logger.Info(customLabelPrefab.name + " prefab is loaded.", this);
        customBgPanelPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_CUSTOM_BG_PANEL) as GameObject;
        Logger.Info(customBgPanelPrefab.name + " prefab is loaded.", this);

        helpBoardPrefabs = new List<GameObject>();
        GameObject helpBoardPrefab1 = Resources.Load(GameConstants.RESOURCE_PREFAB_HELP_BOARD_1) as GameObject;
        Logger.Info(helpBoardPrefab1.name + " prefab is loaded.", this);
        helpBoardPrefabs.Add(helpBoardPrefab1);
        GameObject helpBoardPrefab2 = Resources.Load(GameConstants.RESOURCE_PREFAB_HELP_BOARD_2) as GameObject;
        Logger.Info(helpBoardPrefab2.name + " prefab is loaded.", this);
        helpBoardPrefabs.Add(helpBoardPrefab2);
        GameObject helpBoardPrefab3 = Resources.Load(GameConstants.RESOURCE_PREFAB_HELP_BOARD_3) as GameObject;
        Logger.Info(helpBoardPrefab3.name + " prefab is loaded.", this);
        helpBoardPrefabs.Add(helpBoardPrefab3);

//        customButtonPrefab.transform.localScale = new Vector3(
//            screenManager.SCREEN_WIDTH * 0.8f,
//            screenManager.SCREEN_HEIGHT * 0.05f,
//            1f
//        );
//        customLabelPrefab.transform.localScale = new Vector3(
//            screenManager.SCREEN_WIDTH * 0.8f,
//            screenManager.SCREEN_HEIGHT * 0.05f,
//            1f
//        );
        customBgPanelPrefab.transform.localScale = new Vector3(
            screenManager.SCREEN_WIDTH * 0.9f,
            screenManager.SCREEN_HEIGHT * 0.85f,
            1f
        );

        GameObject backButtonPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_BACK_BUTTON) as GameObject;
        Logger.Info(backButtonPrefab.name + " prefab is loaded.", this);
        Vector3 backButtonPoint = screenManager.WPos(0.06f, 0.96f);
        backButton = Instantiate(backButtonPrefab, backButtonPoint, Quaternion.identity) as GameObject;
        backButton.transform.localScale = screenManager.WScale(0.1f, 0.1f * screenManager.WIDTH_HEIGHT_RATIO);
        backButton.SetActive(false);

        GameObject autoButtonPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_AUTO_BUTTON) as GameObject;
        Logger.Info(autoButtonPrefab.name + " prefab is loaded.", this);
        Vector3 autoButtonPoint = screenManager.WPos(0.15f, 0.96f);
        autoButton = Instantiate(autoButtonPrefab, autoButtonPoint, Quaternion.identity) as GameObject;
        autoButton.transform.localScale = screenManager.WScale(0.1f, 0.1f * screenManager.WIDTH_HEIGHT_RATIO);
        autoButton.SetActive(false);

        userKey = LoadUserKey();

        touchHandler.AddPanelTouchDownListener(this);
        touchHandler.AddPanelTouchingListener(this);
        touchHandler.AddPanelTouchUpListener(this);
        touchHandler.AddTouchDownListener(this);
        touchHandler.AddTouchingListener(this);
        touchHandler.AddTouchUpListener(this);

        originalAmbientColor = RenderSettings.ambientLight;

        if (localDataStore.HasKey(GameConstants.PREF_KEY_USER_NAME)) {
            DisplayGameInit();
        } else {
            // 名前の設定がされていないとき(初回起動時)は、名前入力を求める
            StateManager.EditSetting();
            nameFieldValue = localDataStore.Load<string>(GameConstants.PREF_KEY_USER_NAME, "");
            DisplaySettingBoard();
        }
    }

    /* System handling */
    void Update()
    {
        if (StateManager.IsPaused()) {
            return;
        }

        // if android escape bottun pressed
        if ((Application.platform == RuntimePlatform.Android) && Input.GetKeyDown(KeyCode.Escape)) {
            switch (StateManager.state)
            {
            case StateManager.State.INIT_GAME:
                AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
                DisplayConfirmQuitAppDialog();
                StateManager.QuitConfirm();
                break;
            case StateManager.State.SELECT_MODE:
                AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
                Destroy(selectMenu);
                DisplayGameInit();
                StateManager.Reset();
                backButton.SetActive(false);
                break;
            case StateManager.State.BEFORE_DEMO:
                StateManager.Pause();
                DisplayPauseDialog();
                break;
            case StateManager.State.VIEW_HELP:
            case StateManager.State.VIEW_RECORD:
                AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
                CloseSlideBoards();
                DisplayGameInit();
                StateManager.Next();
                break;
            case StateManager.State.VIEW_INFO:
                AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
                Destroy(infoText);
                Destroy(infoAuthorUrl);
                Destroy(infoPrivacyUrl);
                DisplayGameInit();
                StateManager.Next();
                break;
            case StateManager.State.SETTING:
                AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
                CloseSettingBoard();
                DisplayGameInit();
                StateManager.Next();
                break;
            }
            return;
        }

        // update display
        pointText.text = point.ToString();
        secondsText.text = seconds.ToString("0.0");

        // state behavior
        switch (StateManager.state)
        {
        case StateManager.State.INIT_STAGE:
            // チュートリアルのカットイン
            int tutorialLv = GetTutorialLevel();
            if ((tutorialLv & (int) Mathf.Pow(2, 0)) == 0) {
                // 通常設定のステージ
                playingTutorialLv = 0;
            } else if ((tutorialLv & (int) Mathf.Pow(2, 1)) == 0 && stageManager.GetCurrentStage().skipN > 0) {
                // スキップ設定のあるステージ
                playingTutorialLv = 1;
            } else if ((tutorialLv & (int) Mathf.Pow(2, 2)) == 0 && stageManager.GetCurrentStage().reverse) {
                // 逆順設定のステージ
                playingTutorialLv = 2;
            }
            if (playingTutorialLv >= 0) {
                tutorialBoard = Instantiate(helpBoardPrefabs[playingTutorialLv], screenManager.WPos(0.5f, 0.5f), Quaternion.identity) as GameObject;
                backButton.SetActive(false);
                StateManager.InsertTutorial();
                break;
            }

            // ステージ開始
            stageManager.LoadCurrentStageDialog();
            backButton.SetActive(true);
            StateManager.Next();
            break;
        case StateManager.State.PLAY:
            seconds -= Time.deltaTime;
            if (seconds <= 0f)
            {
                InitSeconds();
                stageManager.GetStepGoalPanel(currentStepIndex).Flick(new FlickInfo(Color.red, 10f, 5f));
                AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "baku013", 0.5f);
                stageManager.SetStageDialogDescription(propertyManager.Get("description_failed"));
                Toast(propertyManager.Get("toast_time_up"));
                Logger.Info("Stage Time up!");
                StateManager.Fail();
            }
            break;
        case StateManager.State.BEFORE_SWEEP:
            StateManager.Next(0.5f, GameConstants.LOCK_KEY_BEFORE_SWEEP);
            break;
        case StateManager.State.SWEEP:
        case StateManager.State.AUTO_SWEEP:
            FlickAmbientColor(Time.time, 2f, 0.9f, 0.9f, 0f, originalAmbientColor.a);
            if (stageManager.IsAllPanelsCleared())
            {
                RenderSettings.ambientLight = originalAmbientColor;
                stageManager.DestroyStageDialog();
                autoButton.SetActive(false);
                if (stageManager.NextStage() == null)
                {
                    StateManager.Next();
                    StartCoroutine("CountTimeBonus", true);
                }
                else
                {
                    StateManager.Next();
                    StartCoroutine("CountTimeBonus", false);
                }
            }
            break;
        case StateManager.State.FAILED:
            FlickAmbientColor(Time.time, 3f, 0.9f, 0f, 0f, originalAmbientColor.a);
            break;
        }
    }

    void OnGUI()
    {
        GUI.skin = customGUISkin;
        if (StateManager.state == StateManager.State.SETTING)
        {
            float height = Screen.height * 0.1f;
            string tf = GUI.TextField(new Rect(30, Screen.height * 0.4f, Screen.width - 60, height), nameFieldValue, 8);
            if (tf != nameFieldValue)
            {
                nameFieldValue = tf;
            }
        }
    }

    public void OnPanelTouchDown(PanelTouchInfo touchInfo)
    {
        Logger.Log(this, string.Format("Panel{0} touched down.", touchInfo.touchPanel.ToCoordinatesString()));
    }

    /* User handling */
    public void OnPanelTouching(PanelTouchInfo touchInfo)
    {
        if (StateManager.IsPaused()) {
            // 一時停止中
            return;
        }

        Logger.Log(this, string.Format("Panel{0} touching.", touchInfo.touchPanel.ToCoordinatesString()));
        switch (StateManager.state)
        {
        case StateManager.State.PLAY:
            JudgeCurrectPanelTouch(touchInfo.touchPanel, !touchInfo.isFirstTouch);
            break;
        case StateManager.State.SWEEP:
            GameObject touchEffect = Instantiate(touchEffectPrefab, touchInfo.touchPanel.panelOjcect.transform.position, Quaternion.identity) as GameObject;
            Destroy(touchEffect, 1.0f);
            SweepPanel(touchInfo.touchPanel);
            break;
        }
    }

    public void OnPanelTouchUp(PanelTouchInfo touchInfo)
    {
        Logger.Log(this, string.Format("Panel{0} touched up.", touchInfo.touchPanel.ToCoordinatesString()));
    }

    public void OnTouchDown(TouchInfo touchInfo)
    {
        stateOnTouchDown = StateManager.state;
        secondsOnTouchDown = 0f;

        if (!StateManager.IsWaitingTouch()) {
            // ユーザ操作を受け付けない
            return;
        }

        Ray cameraRay = Camera.main.ViewportPointToRay(touchInfo.touchViewportPoint);
        RaycastHit raycastHit = new RaycastHit();

        if (StateManager.IsPaused()) {
            // 一時停止中
            if (pauseDialog != null)
            {
                if (pauseDialog.transform.Find("OkButton").GetComponent<GUITexture>().HitTest(touchInfo.touchScreenPoint))
                {
                    SaveRecord();
                    SaveStage();
                    InitilizeGame();
                    DisplayGameInit();
                    backButton.SetActive(false);
                    ClosePauseDialog();
                    StateManager.Reset();
                    StateManager.QuitPausing();
                }
                else if (pauseDialog.transform.Find("CancelButton").GetComponent<GUITexture>().HitTest(touchInfo.touchScreenPoint))
                {
                    ClosePauseDialog();
                    StateManager.QuitPausing();
                }
            }
            return;
        }

        switch (StateManager.state)
        {
        case StateManager.State.INIT_GAME:
            message.text = "";
            CloseGameInit();
            if (Physics.Raycast(cameraRay, out raycastHit, 10)) {
                AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
                if (raycastHit.collider.gameObject == backButton) {
                    AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
                    DisplayConfirmQuitAppDialog();
                    StateManager.QuitConfirm();
                    break;
                } else if (raycastHit.collider.gameObject.name == "IconHelp") {
                    StateManager.ViewHelp();
                    slideBoardPos = 0;
                    maxBoardPos = helpBoardPrefabs.Count - 1;
                    DisplayHelpBoard();
                } else if (raycastHit.collider.gameObject.name == "IconInfo") {
                    StateManager.ViewInfo();
                    infoText = Instantiate(simpleTextPrefab, new Vector3(0.5f, 0.65f, 3f), Quaternion.identity) as GameObject;
                    infoText.GetComponent<GUIText>().text = propertyManager.Get("message_app_name");
                    GameObject infoText2 = Instantiate(simpleTextPrefab, new Vector3(0.5f, 0.6f, 3f), Quaternion.identity) as GameObject;
                    infoText2.GetComponent<GUIText>().text = string.Format(propertyManager.Get("message_app_version"), GameConstants.VERSION);
                    infoText2.transform.parent = infoText.transform;
                    GameObject infoText3 = Instantiate(simpleTextPrefab, new Vector3(0.5f, 0.5f, 3f), Quaternion.identity) as GameObject;
                    infoText3.GetComponent<GUIText>().text = propertyManager.Get("message_credit1");
                    infoText3.transform.parent = infoText.transform;
                    GameObject infoText4 = Instantiate(simpleTextPrefab, new Vector3(0.5f, 0.45f, 3f), Quaternion.identity) as GameObject;
                    infoText4.GetComponent<GUIText>().text = propertyManager.Get("message_credit2");
                    infoText4.transform.parent = infoText.transform;
                    infoAuthorUrl = Instantiate(simpleTextPrefab, new Vector3(0.5f, 0.35f, 3f), Quaternion.identity) as GameObject;
                    infoAuthorUrl.GetComponent<GUIText>().text = propertyManager.Get("message_author_text");
                    infoAuthorUrl.transform.parent = infoText.transform;
                    infoPrivacyUrl = Instantiate(simpleTextPrefab, new Vector3(0.5f, 0.30f, 3f), Quaternion.identity) as GameObject;
                    infoPrivacyUrl.GetComponent<GUIText>().text = propertyManager.Get("message_privacy_text");
                    infoPrivacyUrl.transform.parent = infoText.transform;
                } else if (raycastHit.collider.gameObject.name == "IconRanking") {
                    StateManager.ViewRecord();
                    slideBoardPos = 0;
                    maxBoardPos = 2;
                    if (IsQuestCompleted())
                        // クエストモードコンプリートしたら、エクストラモード解禁
                        maxBoardPos = 3;
                    DisplayRecordBoard();
                } else if (raycastHit.collider.gameObject.name == "IconSetting") {
                    StateManager.EditSetting();
                    nameFieldValue = localDataStore.Load<string>(GameConstants.PREF_KEY_USER_NAME, "");
                    DisplaySettingBoard();
                }
                break;
            }
            selectMenu = new GameObject();
            GameObject customLabel1 = Instantiate(customLabelPrefab, screenManager.WPos(0.5f, 0.83f), Quaternion.identity) as GameObject;
            customLabel1.transform.localScale = new Vector3(0.3f, 0.3f);
            customLabel1.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_menu_quest"));
            customLabel1.transform.parent = selectMenu.transform;
            GameObject questStart = Instantiate(customButtonPrefab, screenManager.WPos(0.5f, 0.7f), Quaternion.identity) as GameObject;
            questStart.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("button_menu_start"));
            questStart.transform.parent = selectMenu.transform;
            if (GetSavedStage() != 0) {
                GameObject questContinueStart = Instantiate(customButtonPrefab, screenManager.WPos(0.5f, 0.6f), Quaternion.identity) as GameObject;
                questContinueStart.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("button_menu_continue"));
                questContinueStart.transform.parent = selectMenu.transform;
            }
            GameObject customLabel2 = Instantiate(customLabelPrefab, screenManager.WPos(0.5f, 0.5f), Quaternion.identity) as GameObject;
            customLabel2.transform.localScale = new Vector3(0.3f, 0.3f);
            customLabel2.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_menu_challenge"));
            customLabel2.transform.parent = selectMenu.transform;
            GameObject challengeEasy = Instantiate(customButtonPrefab, screenManager.WPos(0.5f, 0.4f), Quaternion.identity) as GameObject;
            challengeEasy.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("button_menu_easy"));
            challengeEasy.transform.parent = selectMenu.transform;
            GameObject challengeHard = Instantiate(customButtonPrefab, screenManager.WPos(0.5f, 0.3f), Quaternion.identity) as GameObject;
            challengeHard.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("button_menu_hard"));
            challengeHard.transform.parent = selectMenu.transform;
            if (IsQuestCompleted()) {
                GameObject challengeExtra = Instantiate(customButtonPrefab, screenManager.WPos(0.5f, 0.2f), Quaternion.identity) as GameObject;
                challengeExtra.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("button_menu_extra"));
                challengeExtra.transform.parent = selectMenu.transform;
                challengeExtra.GetComponent<Renderer>().material.color = Color.white;
            }
//            GameObject customLabel3 = Instantiate(customLabelPrefab, screenManager.WPos(0.5f, 0.41f), Quaternion.identity) as GameObject;
//            customLabel3.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_menu_practice"));
//            customLabel3.transform.parent = selectMenu.transform;
//            GameObject practice3x3 = Instantiate(customButtonPrefab, screenManager.WPos(0.5f, 0.34f), Quaternion.identity) as GameObject;
//            practice3x3.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("button_menu_3x3"));
//            practice3x3.transform.parent = selectMenu.transform;
//            GameObject practice4x4 = Instantiate(customButtonPrefab, screenManager.WPos(0.5f, 0.27f), Quaternion.identity) as GameObject;
//            practice4x4.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("button_menu_4x4"));
//            practice4x4.transform.parent = selectMenu.transform;
//            GameObject practice5x5 = Instantiate(customButtonPrefab, screenManager.WPos(0.5f, 0.20f), Quaternion.identity) as GameObject;
//            practice5x5.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("button_menu_5x5"));
//            practice5x5.transform.parent = selectMenu.transform;
            backButton.SetActive(true);
            AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
            StateManager.Next();
            break;
        case StateManager.State.SELECT_MODE:
            if (Physics.Raycast(cameraRay, out raycastHit, 10))
            {
                AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
                if (raycastHit.collider.gameObject == backButton) {
                    Destroy(selectMenu);
                    DisplayGameInit();
                    StateManager.Reset();
                    backButton.SetActive(false);
                    break;
                }
                SimpleLock.Lock(GameConstants.LOCK_KEY_SELECT_MENU, 1.0f, (System.Action)(() =>
                    {
                        StateManager.Next();
                        Destroy(selectMenu);
                    }));
                if (raycastHit.collider.gameObject.GetComponent<CustomLabel>().GetLabel() == propertyManager.Get("button_menu_start"))
                {
                    stageManager.mode = new QuestMode();
//                    DeleteSaveStage();
                }
                else if (raycastHit.collider.gameObject.GetComponent<CustomLabel>().GetLabel() == propertyManager.Get("button_menu_continue"))
                {
                    stageManager.mode = new QuestMode(GetSavedStage());
                }
                else if (raycastHit.collider.gameObject.GetComponent<CustomLabel>().GetLabel() == propertyManager.Get("button_menu_easy"))
                {
                    stageManager.mode = new ChallengeMode(1);
                }
                else if (raycastHit.collider.gameObject.GetComponent<CustomLabel>().GetLabel() == propertyManager.Get("button_menu_hard"))
                {
                    stageManager.mode = new ChallengeMode(2);
                }
                else if (raycastHit.collider.gameObject.GetComponent<CustomLabel>().GetLabel() == propertyManager.Get("button_menu_extra"))
                {
                    stageManager.mode = new ChallengeMode(9);
                }
                else if (raycastHit.collider.gameObject.GetComponent<CustomLabel>().GetLabel() == propertyManager.Get("button_menu_3x3"))
                {
                    stageManager.mode = new PracticeMode(3, 3);
                }
                else if (raycastHit.collider.gameObject.GetComponent<CustomLabel>().GetLabel() == propertyManager.Get("button_menu_4x4"))
                {
                    stageManager.mode = new PracticeMode(4, 4);
                }
                else if (raycastHit.collider.gameObject.GetComponent<CustomLabel>().GetLabel() == propertyManager.Get("button_menu_5x5"))
                {
                    stageManager.mode = new PracticeMode(5, 5);
                }
            }
            break;
        case StateManager.State.BEFORE_DEMO:
            if (Physics.Raycast(cameraRay, out raycastHit, 10))
            {
                if (raycastHit.collider.gameObject == backButton) {
                    StateManager.Pause();
                    DisplayPauseDialog();
                    break;
                }
            }
            backButton.SetActive(false);
            StateManager.Next((System.Action)(() =>
                {
                    stageManager.SlideStageDialog(2.0f);
                    Scheduler.AddSchedule(GameConstants.TIMER_KEY_BEFORE_DEMO, 1.7f, (System.Action)(() =>
                        {
                            stageManager.LoadCurrentStagePanels();
                            seconds = stageManager.GetCurrentStage().limitTime;
                            stageManager.PlayCurrentStageDemo();
                            stepJudgeInfo = new StepJudgeInfo(null, stageManager.GetStepGoalPanel(currentStepIndex));
                            StateManager.Next();
                        }));
                }));
            break;
        case StateManager.State.SWEEP:
            if (Physics.Raycast(cameraRay, out raycastHit, 10)) {
                AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
                if (raycastHit.collider.gameObject == autoButton) {
                    StateManager.AutoSweep();
                    autoButton.SetActive(false);
                    // auto sweep のスケジューリング
                    int i = 1;
                    foreach (Panel panel in this.stageManager.panels) {
                        if (panel == null)
                            continue;
                        Panel thisPanel = panel;
                        Scheduler.AddSchedule(GameConstants.TIMER_KEY_PREFIX_AUTO_SWEEP + i.ToString(), 0.2f * i, (System.Action)(() =>
                            {
                                GameObject touchEffect = Instantiate(touchEffectPrefab, thisPanel.panelOjcect.transform.position, Quaternion.identity) as GameObject;
                                Destroy(touchEffect, 1.0f);
                                SweepPanel(thisPanel);
                            })
                        );
                        i++;
                    }
                    break;
                }
            }
            break;
        case StateManager.State.TUTORIAL:
            if (playingTutorialLv >= 0)
                SaveTutorialLevel((int) Mathf.Pow(2, playingTutorialLv));
            playingTutorialLv = -1;
            if (tutorialBoard != null)
                Destroy(tutorialBoard);
            tutorialBoard = null;
            backButton.SetActive(true);
            StateManager.Next();
            break;
        case StateManager.State.VIEW_INFO:
            AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
            if (infoAuthorUrl.GetComponent<GUIText>().GetScreenRect().Contains(touchInfo.touchScreenPoint))
            {
                string authorUrl = propertyManager.Get("message_author_url");
                Application.OpenURL(authorUrl);
                Logger.Log(this, authorUrl);
                break;
            }
            if (infoPrivacyUrl.GetComponent<GUIText>().GetScreenRect().Contains(touchInfo.touchScreenPoint))
            {
                string privacyUrl = propertyManager.Get("message_privacy_url");
                Application.OpenURL(privacyUrl);
                Logger.Log(this, privacyUrl);
                break;
            }
            Destroy(infoText);
            Destroy(infoAuthorUrl);
            Destroy(infoPrivacyUrl);
            DisplayGameInit();
            StateManager.Next();
            break;
        case StateManager.State.SETTING:
            message.text = "";
            CloseGameInit();
            if (Physics.Raycast(cameraRay, out raycastHit, 10))
            {
                AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
                if (raycastHit.collider.gameObject.name == "OkButton")
                {
                    localDataStore.Save(GameConstants.PREF_KEY_USER_NAME, nameFieldValue);
                    Logger.Info(string.Format("User name '{0}' is entered.", nameFieldValue), this);
                    CloseSettingBoard();
                    DisplayGameInit();
                    StateManager.Next();
                }
            }
            break;
        case StateManager.State.FAILED:
            RenderSettings.ambientLight = originalAmbientColor;
            SaveRecord();
            SaveStage();
            InitilizeGame();
            if (stageManager.mode.GetType() == typeof(PracticeMode))
            {
                StateManager.state = StateManager.State.INIT_STAGE;
            }
            else
            {
                DisplayGameInit();
                StateManager.Next();
            }
            break;
        case StateManager.State.COMPLETED:
            message.text = "";
            DisplayGameInit();
            StateManager.Next();
            break;
        case StateManager.State.QUIT_CONFIRM:
            if (Physics.Raycast(cameraRay, out raycastHit, 10)) {
                if (raycastHit.collider.gameObject.GetComponent<CustomLabel>().GetLabel() == propertyManager.Get("button_ok")) {
                    // アプリ終了
                    Logger.Log("Quit application.");
                    Application.Quit();
                } else {
                    CloseConfirmQuitAppDialog();
                    StateManager.Next();
                }
            }
            break;
        case StateManager.State.VIEW_HELP:
        case StateManager.State.VIEW_RECORD:
            break;
        default:
            message.text = "";
            StateManager.Next();
            break;
        }
    }

    public void OnTouching(TouchInfo touchInfo)
    {
        secondsOnTouchDown += touchInfo.deltaTime;

        if (!StateManager.IsWaitingTouch()) {
            // ユーザ操作を受け付けない
            return;
        }

        if (StateManager.IsPaused()) {
            // 一時停止中
            return;
        }

        switch (StateManager.state)
        {
        case StateManager.State.VIEW_HELP:
        case StateManager.State.VIEW_RECORD:
            if (StateManager.state != stateOnTouchDown)
                break;
            float distanceX = touchInfo.touchViewportPoint.x - touchInfo.originViewportPoint.x;
            slideBoards.transform.position = new Vector3(
                (-slideBoardPos + distanceX * 0.5f) * screenManager.SCREEN_WIDTH,
                slideBoards.transform.position.y,
                slideBoards.transform.position.z
            );
            break;
        case StateManager.State.SELECT_MODE:
        case StateManager.State.SETTING:
            // きらきらエフェクト
            GameObject twinkleEffect = Instantiate(twinkleEffectPrefab, screenManager.WPos(touchInfo.touchViewportPoint.x, touchInfo.touchViewportPoint.y), Quaternion.identity) as GameObject;
            Destroy(twinkleEffect, 0.5f);
            break;
        }
    }

    public void OnTouchUp(TouchInfo touchInfo)
    {
        if (!StateManager.IsWaitingTouch()) {
            // ユーザ操作を受け付けない
            return;
        }

        if (StateManager.IsPaused()) {
            // 一時停止中
            return;
        }

        switch (StateManager.state)
        {
        case StateManager.State.PLAY:
            currentPanel = null;
            break;
        case StateManager.State.VIEW_HELP:
        case StateManager.State.VIEW_RECORD:
            if (StateManager.state != stateOnTouchDown)
                break;
            float distanceX = touchInfo.touchViewportPoint.x - touchInfo.originViewportPoint.x;
            if (secondsOnTouchDown < 2.0f && Mathf.Abs(distanceX) < 0.05f) {
                AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
                CloseSlideBoards();
                DisplayGameInit();
                StateManager.Next();
            } else if (Mathf.Abs(distanceX) < 0.05f) {
                iTween.MoveTo(slideBoards, iTween.Hash("x", -slideBoardPos * screenManager.SCREEN_WIDTH, "time", 1.0f));
            } else {
                slideBoardPos += (distanceX > 0f ? -1 : 1);
                if (slideBoardPos < 0)
                    slideBoardPos = 0;
                if (slideBoardPos > maxBoardPos)
                    slideBoardPos = maxBoardPos;
                iTween.MoveTo(slideBoards, iTween.Hash("x", -slideBoardPos * screenManager.SCREEN_WIDTH, "time", 1.0f));
            }
            break;
        }

        secondsOnTouchDown = 0f;
    }

    /// <summary>
    /// 環境光の色をチカチカ点滅させます。
    /// </summary>
    /// <param name="time">Time.</param>
    /// <param name="flickPace">Flick pace.</param>
    /// <param name="rValue">R value.</param>
    /// <param name="gValue">G value.</param>
    /// <param name="bValue">B value.</param>
    /// <param name="aValue">A value.</param>
    private void FlickAmbientColor(float time, float flickPace, float rValue, float gValue, float bValue, float aValue)
    {
        float level = Mathf.Abs(Mathf.Sin(time * flickPace));
        RenderSettings.ambientLight = new Vector4(
            rValue * level,
            gValue * level,
            bValue * level,
            aValue * level
        );
    }

    /// <summary>
    /// 全てのパネルの色を変えます。
    /// </summary>
    private void ChangePanelColors(Color color)
    {
        foreach (Panel panel in stageManager.panels)
        {
            panel.ChangeOriginalColor(color);
        }
    }

    /// <summary>
    /// タイムを初期化します。
    /// </summary>
    private void InitSeconds()
    {
        this.seconds = 0f;
    }

    /// <summary>
    /// ポイントを初期化します。
    /// </summary>
    private void InitPoint()
    {
        this.point = 0;
    }

    /// <summary>
    /// ポイントを加算します。
    /// </summary>
    /// <param name="addPoint">Add point.</param>
    private void AddPoint(int addPoint)
    {
        this.point += addPoint;
        AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "Coin_Pick_Up_03");
    }

    /// <summary>
    /// 新記録の場合に記録します。
    /// </summary>
    /// <returns><c>true</c>, if record was saved, <c>false</c> otherwise.</returns>
    private bool SaveRecord()
    {
        localDataStore.SaveMaxIntWithPWD(string.Format(GameConstants.PREF_KEY_STAGE, stageManager.mode.GetModeId()), this.stageManager.mode.GetCurrentStageIndex(), userKey);
        bool newRecord = localDataStore.SaveMaxIntWithPWD(string.Format(GameConstants.PREF_KEY_POINT, stageManager.mode.GetModeId()), this.point, userKey);
        if (stageManager.mode.GetType() != typeof(PracticeMode)) {
            // 新記録でなくてもサーバ送信
            RankingRecord record = new RankingRecord(this.stageManager.mode.GetModeId(), localDataStore.Load<string>(GameConstants.PREF_KEY_USER_NAME, ""), 0, this.point);
            record.reqCode = EncryptUtil.Hash(userKey) + RandomUtil.GenerateRandomStr(10);
            string recordJson = EncryptUtil.ObjectToJson(record);
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json; charset=UTF-8");
            header.Add("Api-Token", EncryptUtil.Hash(record.reqCode));
            header.Add("App-Version", GameConstants.VERSION);
            networkManager.POST(GameConstants.API_URL_POST_RANKING_RECORD, recordJson, header, (System.Action<WWW>)((WWW www) =>
                {
                }));
        }
        return newRecord;
    }

    /// <summary>
    /// 記録しているポイントを取得します。
    /// </summary>
    /// <returns>The record point.</returns>
    /// <param name="modeId">Mode identifier.</param>
    private int GetRecordPoint(string modeId)
    {
        return localDataStore.LoadWithPWD<int>(string.Format(GameConstants.PREF_KEY_POINT, modeId), userKey, 0);
    }

    /// <summary>
    /// 記録しているステージNoを取得します。
    /// </summary>
    /// <returns>The record stage.</returns>
    /// <param name="modeId">Mode identifier.</param>
    private int GetRecordStage(string modeId)
    {
        return localDataStore.LoadWithPWD<int>(string.Format(GameConstants.PREF_KEY_STAGE, modeId), userKey, 0);
    }

    /// <summary>
    /// クエストモードでコンプリートクリアの達成を記録します。
    /// </summary>
    private void RecordQuestCompleted()
    {
        if (stageManager.mode.GetType() != typeof(QuestMode))
            return;
        localDataStore.SaveWithPWD(GameConstants.PREF_KEY_QUEST_COMPLETED, true, userKey);
    }

    /// <summary>
    /// クエストモードでコンプリートクリアしているかどうかを取得します。
    /// </summary>
    /// <returns><c>true</c> if quest completed; otherwise, <c>false</c>.</returns>
    private bool IsQuestCompleted()
    {
        return localDataStore.LoadWithPWD<bool>(GameConstants.PREF_KEY_QUEST_COMPLETED, userKey, false);
    }

    /// <summary>
    /// クエストモードで進行中のステージをセーブします。
    /// </summary>
    private void SaveStage()
    {
        if (stageManager.mode.GetType() != typeof(QuestMode))
            return;
        localDataStore.SaveMaxIntWithPWD(string.Format(GameConstants.PREF_KEY_STAGE_CONTINUE, stageManager.mode.GetModeId()), this.stageManager.mode.GetCurrentStageIndex(), userKey);
    }

    /// <summary>
    /// クエストモードでセーブした進行中のステージをロードします。
    /// </summary>
    /// <returns>The saved stage.</returns>
    private int GetSavedStage()
    {
        return localDataStore.LoadWithPWD(string.Format(GameConstants.PREF_KEY_STAGE_CONTINUE, new QuestMode().GetModeId()), userKey, 0);
    }

    /// <summary>
    /// クエストモードでセーブした進行中ステージ情報を破棄します。
    /// </summary>
    private void DeleteSaveStage()
    {
        if (stageManager.mode.GetType() != typeof(QuestMode))
            return;
        localDataStore.Delete(string.Format(GameConstants.PREF_KEY_STAGE_CONTINUE, stageManager.mode.GetModeId()));
    }

    /// <summary>
    /// 現在のチュートリアルレベルを更新します。
    /// チュートリアルは出現順を限定しないように2進数のフラグで管理します。
    /// </summary>
    /// <param name="addLv">Add lv.</param>
    private void SaveTutorialLevel(int addLv)
    {
        int currentLv = GetTutorialLevel();
        localDataStore.Save(GameConstants.PREF_KEY_TUTORIAL_LEVEL, currentLv | addLv);
    }

    /// <summary>
    /// 現在のチュートリアルレベルを取得します。
    /// チュートリアルは出現順を限定しないように2進数のフラグで管理します。
    /// </summary>
    /// <returns>The tutorial level.</returns>
    private int GetTutorialLevel()
    {
        return localDataStore.Load(GameConstants.PREF_KEY_TUTORIAL_LEVEL, 0);
    }

    /// <summary>
    /// ユーザキーをロードします。ない場合は新規生成します。
    /// </summary>
    /// <returns>The user key.</returns>
    private string LoadUserKey()
    {
        string userKey = localDataStore.LoadWithPWD(GameConstants.PREF_KEY_USER_KEY, GameConstants.ENCRIPT_PASSWORD, "");
        if (userKey == "") {
            userKey = RandomUtil.GenerateRandomStr(25);
            localDataStore.SaveWithPWD(GameConstants.PREF_KEY_USER_KEY, userKey, GameConstants.ENCRIPT_PASSWORD);
        }
        return userKey;

    }

    /// <summary>
    /// 指定したパネルを消して、ポイントを追加します。
    /// </summary>
    /// <param name="touchPanel">Touch panel.</param>
    private void SweepPanel(Panel touchPanel)
    {
        if (touchPanel == null)
            return;
        AddPoint(10);
        stageManager.ClearPanel(touchPanel);
    }

    /// <summary>
    /// 指定秒数トースト表示します。
    /// </summary>
    /// <param name="msg">Message.</param>
    /// <param name="displayTime">Display time.</param>
    private void Toast(string msg, float displayTime = 3f)
    {
        toast.text = msg;
        Scheduler.AddSchedule(GameConstants.TIMER_KEY_TOAST, displayTime, (System.Action)(() =>
            {
                toast.text = "";
            }));
    }

    /// <summary>
    /// ゲームタイトル画面を表示します。
    /// </summary>
    private void DisplayGameInit()
    {
        message.text = propertyManager.Get("message_game_start");
        initIcons = Instantiate(initIconsPrefab, initIconsPrefab.transform.position, Quaternion.identity) as GameObject;
    }

    /// <summary>
    /// ゲームタイトル画面を閉じます。
    /// </summary>
    private void CloseGameInit()
    {
        if (initIcons != null)
            Destroy(initIcons);
    }

    /// <summary>
    /// ヘルプボードを表示します。
    /// </summary>
    private void DisplayHelpBoard()
    {
        slideBoards = new GameObject();

        for (int i = 0; i < helpBoardPrefabs.Count; i++) {
            GameObject helpBoard = Instantiate(helpBoardPrefabs[i], screenManager.WPos(0.5f + 1f * i, 0.5f), Quaternion.identity) as GameObject;
            helpBoard.transform.parent = slideBoards.transform;
        }
    }

    /// <summary>
    /// レコードボードを表示します。
    /// </summary>
    private void DisplayRecordBoard()
    {
        StartCoroutine("AsyncRecordBoard");
    }

    /// <summary>
    /// スライドボード(ヘルプボード / レコードボード)を閉じます。
    /// </summary>
    private void CloseSlideBoards()
    {
        if (slideBoards != null)
            Destroy(slideBoards);
    }

    /// <summary>
    /// 設定ボードを表示します。
    /// </summary>
    private void DisplaySettingBoard()
    {
        settingBoard = Instantiate(settingBoardPrefab, settingBoardPrefab.transform.position, Quaternion.identity) as GameObject;
        SettingBoard board = settingBoard.GetComponent<SettingBoard>();
        board.SetNameLabel(propertyManager.Get("label_input_name"));
        board.SetOkButtonLabel(propertyManager.Get("button_ok"));
    }

    /// <summary>
    /// 設定ボードを閉じます。
    /// </summary>
    private void CloseSettingBoard()
    {
        if (settingBoard != null)
            Destroy(settingBoard);
    }

    /// <summary>
    /// 一時停止ダイアログを表示します。
    /// </summary>
    private void DisplayPauseDialog()
    {
        pauseDialog = Instantiate(pauseDialogPrefab, pauseDialogPrefab.transform.position, Quaternion.identity) as GameObject;
        PauseDialog dialog = pauseDialog.GetComponent<PauseDialog>();
        dialog.SetLabels(propertyManager.Get("description_quit"),
            propertyManager.Get("button_ok"),
            propertyManager.Get("button_cancel"));
        AudioManager.PlayOneShot(pauseDialog.GetComponent<AudioSource>(), "button25");
        this.GetComponent<AudioSource>().Pause();
    }

    /// <summary>
    /// 一時停止ダイアログを閉じます。
    /// </summary>
    private void ClosePauseDialog()
    {
        this.GetComponent<AudioSource>().Play();
        if (pauseDialog != null)
            Destroy(pauseDialog);
    }

    /// <summary>
    /// アプリ終了確認ダイアログを表示します。
    /// </summary>
    private void DisplayConfirmQuitAppDialog()
    {
        message.text = "";
        quitAppConfirmDialog = new GameObject();
        GameObject quitAppConfirmBg = Instantiate(customBgPanelPrefab, screenManager.WPos(0.5f, 0.5f, 4.5f), Quaternion.identity) as GameObject;
        quitAppConfirmBg.transform.localScale = new Vector3(3f, 3f, 1f);
        quitAppConfirmBg.transform.parent = quitAppConfirmDialog.transform;
        GameObject quitAppConfirmLabel = Instantiate(customLabelPrefab, screenManager.WPos(0.5f, 0.6f, 4.5f), Quaternion.identity) as GameObject;
        quitAppConfirmLabel.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        quitAppConfirmLabel.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("description_quit_app"));
        quitAppConfirmLabel.transform.parent = quitAppConfirmDialog.transform;
        GameObject quitAppConfirmOk = Instantiate(customButtonPrefab, screenManager.WPos(0.5f, 0.5f, 4.5f), Quaternion.identity) as GameObject;
        quitAppConfirmOk.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("button_ok"));
        quitAppConfirmOk.transform.parent = quitAppConfirmDialog.transform;
        GameObject quitAppConfirmCancel = Instantiate(customButtonPrefab, screenManager.WPos(0.5f, 0.4f, 4.5f), Quaternion.identity) as GameObject;
        quitAppConfirmCancel.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("button_cancel"));
        quitAppConfirmCancel.transform.parent = quitAppConfirmDialog.transform;
    }

    /// <summary>
    /// アプリ終了確認ダイアログを閉じます。
    /// </summary>
    private void CloseConfirmQuitAppDialog()
    {
        if (quitAppConfirmDialog != null)
            Destroy(quitAppConfirmDialog);
        message.text = propertyManager.Get("message_game_start");
    }

    /// <summary>
    /// タッチされたパネルが正解かどうか判定して、その結果によって状態遷移します。
    /// </summary>
    /// <param name="touchPanel">Touch panel.</param>
    /// <param name="isKeepingTouch">If set to <c>true</c> is keeping touch.</param>
    private void JudgeCurrectPanelTouch(Panel touchPanel, bool isKeepingTouch)
    {
        if (touchPanel == currentPanel)
        {
            return;
        }
        if (stageManager.IsStageCleared(currentStepIndex))
        {
            return;
        }
        if (stepJudgeInfo == null)
        {
            return;
        }
        currentPanel = touchPanel;
        // judge touched panel
        int restCount = stepJudgeInfo.JudgeTouchPattern(touchPanel, isKeepingTouch);
        if (restCount == 0)
        {
            // touch goal panel
            touchPanel.Flush(stageManager.GetCurrentStep(currentStepIndex));
            AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button01b");
            GameObject touchEffect = Instantiate(touchEffectPrefab, touchPanel.panelOjcect.transform.position, Quaternion.identity) as GameObject;
            Destroy(touchEffect, 1.0f);
            Logger.Info("[Panel Touch] GOAL!");
            currentStepIndex++;
            if (stageManager.IsStageCleared(currentStepIndex))
            {
                // cleared
                stepJudgeInfo = null;
                AddPoint(GameConstants.POINT_STAGE_BONUS);
                Toast(propertyManager.Get("toast_clear_bonus", GameConstants.POINT_STAGE_BONUS));
                Logger.Info("[Panel Touch] CLEAR!");
                // sweep mode
                ChangePanelColors(Color.yellow);
                stageManager.SetStageDialogDescription(propertyManager.Get("description_after_clear"));
                autoButton.SetActive(true);
                StateManager.Next();
                InitilizeStage();
                return;
            }
            // set next route
            stepJudgeInfo = new StepJudgeInfo(currentPanel, stageManager.GetStepGoalPanel(currentStepIndex));
            Logger.Info(string.Format("Next Step: Start={0} Goal={1} Distance={2}", 
                    stepJudgeInfo.startPanel.ToCoordinatesString(), stepJudgeInfo.goalPanel.ToCoordinatesString(), 
                    stepJudgeInfo.restCount.ToString()), this);
        }
        else if (restCount > 0)
        {
            // touch on-route panel
            touchPanel.Flush(new FlushInfo(Color.yellow));
            AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "button04a");
            Logger.Info("[Panel Touch] OK!");
            return;
        }
        else
        {
            // touch wrong panel
//            touchPanel.Flick(new FlickInfo(Color.red, 10f, 5f));
            stageManager.GetStepGoalPanel(currentStepIndex).Flick(new FlickInfo(Color.yellow, 10f, 5f));
            AudioManager.PlayOneShot(this.GetComponent<AudioSource>(), "baku013", 0.5f);
            stageManager.SetStageDialogDescription(propertyManager.Get("description_failed"));
            Toast(propertyManager.Get("toast_touch_wrong_panel"));
            Logger.Info("[Panel Touch] NG!");
            StateManager.Fail();
            return;
        }
    }

    /// <summary>
    /// ゲームを初期化します。
    /// </summary>
    private void InitilizeGame()
    {
        stageManager.ResetStage();
        stageManager.DestroyStageDialog();
        InitilizeStage();
        InitPoint();
        InitSeconds();
        Logger.Info("Game restarted.");
    }

    /// <summary>
    /// ステージを初期化します。
    /// </summary>
    private void InitilizeStage()
    {
        currentStepIndex = 0;
        currentPanel = null;
    }

    /// <summary>
    /// 非同期呼び出しでレコードボードを生成します。
    /// </summary>
    /// <returns>The record board.</returns>
    IEnumerator AsyncRecordBoard()
    {
        slideBoards = new GameObject();

        // quest mode
        float baseX = 0f;
        QuestMode sampleQuestMode = new QuestMode();
        GameObject customBg1 = Instantiate(customBgPanelPrefab, screenManager.WPos(baseX + 0.5f, 0.5f), Quaternion.identity) as GameObject;
        customBg1.transform.parent = slideBoards.transform;
        GameObject customLabel1 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.85f), Quaternion.identity) as GameObject;
        customLabel1.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel1.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_menu_quest"));
        customLabel1.transform.parent = customBg1.transform;
        GameObject customLabel1_1 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.77f), Quaternion.identity) as GameObject;
        customLabel1_1.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel1_1.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_best_record"));
        customLabel1_1.transform.parent = customBg1.transform;
        GameObject customLabel1_2 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.3f, 0.70f), Quaternion.identity) as GameObject;
        customLabel1_2.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel1_2.GetComponent<CustomLabel>().SetLabel(localDataStore.Load<string>(GameConstants.PREF_KEY_USER_NAME, ""))
            .SetAnchor(TextAnchor.MiddleLeft).SetColor(Color.yellow);
        customLabel1_2.transform.parent = customBg1.transform;
        GameObject customLabel1_3 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.2f, 0.60f), Quaternion.identity) as GameObject;
        customLabel1_3.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel1_3.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_point"))
            .SetAnchor(TextAnchor.MiddleLeft).SetColor(Color.yellow);
        customLabel1_3.transform.parent = customBg1.transform;
        GameObject customLabel1_4 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.2f, 0.50f), Quaternion.identity) as GameObject;
        customLabel1_4.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel1_4.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_stage"))
            .SetAnchor(TextAnchor.MiddleLeft).SetColor(Color.yellow);
        customLabel1_4.transform.parent = customBg1.transform;
        GameObject customLabel1_5 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.8f, 0.60f), Quaternion.identity) as GameObject;
        customLabel1_5.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel1_5.GetComponent<CustomLabel>().SetLabel(GetRecordPoint(sampleQuestMode.GetModeId()).ToString())
            .SetAnchor(TextAnchor.MiddleRight).SetColor(Color.white);
        customLabel1_5.transform.parent = customBg1.transform;
        GameObject customLabel1_6 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.8f, 0.50f), Quaternion.identity) as GameObject;
        customLabel1_6.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        StageInfo questStage = sampleQuestMode.GetStage(GetRecordStage(sampleQuestMode.GetModeId()));
        if (questStage != null) {
            // コンプリートしていない場合
            customLabel1_6.GetComponent<CustomLabel>().SetLabel(string.Format("{0} - {1}", questStage.level, questStage.stageNo))
                .SetAnchor(TextAnchor.MiddleRight).SetColor(Color.white);
        }
        customLabel1_6.transform.parent = customBg1.transform;
        if (IsQuestCompleted()) {
            // クエストモードコンプリートしたら、ユーザ名の隣に王冠表示
            GameObject crownObj = Instantiate(crownPrefab, screenManager.WPos(baseX + 0.22f, 0.70f), Quaternion.identity) as GameObject;
            crownObj.transform.parent = customBg1.transform;
            // ステージ表示はCompleted
            customLabel1_6.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("description_completed"))
                .SetAnchor(TextAnchor.MiddleRight).SetColor(Color.white);
        }

        // challenge easy mode
        baseX = 1f;
        GameObject customBg2 = Instantiate(customBgPanelPrefab, screenManager.WPos(baseX + 0.5f, 0.5f), Quaternion.identity) as GameObject;
        customBg2.transform.parent = slideBoards.transform;
        GameObject customLabel2 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.85f), Quaternion.identity) as GameObject;
        customLabel2.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel2.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_menu_challenge"));
        customLabel2.transform.parent = customBg2.transform;
        GameObject customLabel2_0 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.82f), Quaternion.identity) as GameObject;
        customLabel2_0.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel2_0.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_easy"))
            .SetColor(Color.yellow);
        customLabel2_0.transform.parent = customBg2.transform;
        GameObject customLabel2_1 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.78f), Quaternion.identity) as GameObject;
        customLabel2_1.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel2_1.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_best_record"));
        customLabel2_1.transform.parent = customBg2.transform;
        GameObject customLabel2_2 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.60f), Quaternion.identity) as GameObject;
        customLabel2_2.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel2_2.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_ranking"));
        customLabel2_2.transform.parent = customBg2.transform;
        GameObject customLabel2_3 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.2f, 0.72f), Quaternion.identity) as GameObject;
        customLabel2_3.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel2_3.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_point"))
            .SetAnchor(TextAnchor.MiddleLeft).SetColor(Color.yellow);
        customLabel2_3.transform.parent = customBg2.transform;
        GameObject customLabel2_4 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.2f, 0.68f), Quaternion.identity) as GameObject;
        customLabel2_4.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel2_4.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_stage"))
            .SetAnchor(TextAnchor.MiddleLeft).SetColor(Color.yellow);
        customLabel2_4.transform.parent = customBg2.transform;
        GameObject customLabel2_5 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.8f, 0.72f), Quaternion.identity) as GameObject;
        customLabel2_5.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel2_5.GetComponent<CustomLabel>().SetLabel(GetRecordPoint(new ChallengeMode(1).GetModeId()).ToString())
            .SetAnchor(TextAnchor.MiddleRight).SetColor(Color.white);
        customLabel2_5.transform.parent = customBg2.transform;
        GameObject customLabel2_6 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.8f, 0.68f), Quaternion.identity) as GameObject;
        customLabel2_6.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel2_6.GetComponent<CustomLabel>().SetLabel(GetRecordStage(new ChallengeMode(1).GetModeId()).ToString())
            .SetAnchor(TextAnchor.MiddleRight).SetColor(Color.white);
        customLabel2_6.transform.parent = customBg2.transform;
        List<CustomLabel> rankLabels2 = new List<CustomLabel>();
        List<CustomLabel> rankPointLabels2 = new List<CustomLabel>();
        for (int i = 0; i < GameConstants.GET_RANKING_RECORD_COUNT; i++) {
            GameObject rankLabel = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.15f, 0.55f - 0.04f * i), Quaternion.identity) as GameObject;
            rankLabel.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            rankLabel.GetComponent<CustomLabel>().SetAnchor(TextAnchor.MiddleLeft).SetColor(Color.yellow);
            rankLabel.transform.parent = customBg2.transform;
            rankLabels2.Add(rankLabel.GetComponent<CustomLabel>());
            GameObject rankPointLabel = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.85f, 0.55f - 0.04f * i), Quaternion.identity) as GameObject;
            rankPointLabel.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            rankPointLabel.GetComponent<CustomLabel>().SetAnchor(TextAnchor.MiddleRight).SetColor(Color.white);
            rankPointLabel.transform.parent = customBg2.transform;
            rankPointLabels2.Add(rankPointLabel.GetComponent<CustomLabel>());
        }

        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Accept", "application/json");
        header.Add("App-Version", GameConstants.VERSION);
        networkManager.GET(string.Format(GameConstants.API_URL_GET_RANKING_RECORDS, new ChallengeMode(1).GetModeId()).ToString(), header, (System.Action<WWW>)((WWW www) =>
            {
                List<RankingRecord> records2 = EncryptUtil.JsonToObject<List<RankingRecord>>(www.text);
                for(int i = 0; i < records2.Count; i++) {
                    rankLabels2[i].SetLabel(string.Format("{0}  {1}", records2[i].rank, records2[i].name));
                    rankPointLabels2[i].SetLabel(records2[i].point.ToString());
                }
            }));

        // challenge hard mode
        baseX = 2f;
        GameObject customBg3 = Instantiate(customBgPanelPrefab, screenManager.WPos(baseX + 0.5f, 0.5f), Quaternion.identity) as GameObject;
        customBg3.transform.parent = slideBoards.transform;
        GameObject customLabel3 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.85f), Quaternion.identity) as GameObject;
        customLabel3.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel3.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_menu_challenge"));
        customLabel3.transform.parent = customBg3.transform;
        GameObject customLabel3_0 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.82f), Quaternion.identity) as GameObject;
        customLabel3_0.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel3_0.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_hard"))
            .SetColor(Color.yellow);
        customLabel3_0.transform.parent = customBg3.transform;
        GameObject customLabel3_1 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.78f), Quaternion.identity) as GameObject;
        customLabel3_1.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel3_1.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_best_record"));
        customLabel3_1.transform.parent = customBg3.transform;
        GameObject customLabel3_2 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.60f), Quaternion.identity) as GameObject;
        customLabel3_2.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel3_2.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_ranking"));
        customLabel3_2.transform.parent = customBg3.transform;
        GameObject customLabel3_3 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.2f, 0.72f), Quaternion.identity) as GameObject;
        customLabel3_3.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel3_3.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_point"))
            .SetAnchor(TextAnchor.MiddleLeft).SetColor(Color.yellow);
        customLabel3_3.transform.parent = customBg3.transform;
        GameObject customLabel3_4 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.2f, 0.68f), Quaternion.identity) as GameObject;
        customLabel3_4.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel3_4.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_stage"))
            .SetAnchor(TextAnchor.MiddleLeft).SetColor(Color.yellow);
        customLabel3_4.transform.parent = customBg3.transform;
        GameObject customLabel3_5 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.8f, 0.72f), Quaternion.identity) as GameObject;
        customLabel3_5.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel3_5.GetComponent<CustomLabel>().SetLabel(GetRecordPoint(new ChallengeMode(2).GetModeId()).ToString())
            .SetAnchor(TextAnchor.MiddleRight).SetColor(Color.white);
        customLabel3_5.transform.parent = customBg3.transform;
        GameObject customLabel3_6 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.8f, 0.68f), Quaternion.identity) as GameObject;
        customLabel3_6.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        customLabel3_6.GetComponent<CustomLabel>().SetLabel(GetRecordStage(new ChallengeMode(2).GetModeId()).ToString())
            .SetAnchor(TextAnchor.MiddleRight).SetColor(Color.white);
        customLabel3_6.transform.parent = customBg3.transform;
        List<CustomLabel> rankLabels3 = new List<CustomLabel>();
        List<CustomLabel> rankPointLabels3 = new List<CustomLabel>();
        for (int i = 0; i < GameConstants.GET_RANKING_RECORD_COUNT; i++) {
            GameObject rankLabel = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.15f, 0.55f - 0.04f * i), Quaternion.identity) as GameObject;
            rankLabel.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            rankLabel.GetComponent<CustomLabel>().SetAnchor(TextAnchor.MiddleLeft).SetColor(Color.yellow);
            rankLabel.transform.parent = customBg3.transform;
            rankLabels3.Add(rankLabel.GetComponent<CustomLabel>());
            GameObject rankPointLabel = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.85f, 0.55f - 0.04f * i), Quaternion.identity) as GameObject;
            rankPointLabel.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            rankPointLabel.GetComponent<CustomLabel>().SetAnchor(TextAnchor.MiddleRight).SetColor(Color.white);
            rankPointLabel.transform.parent = customBg3.transform;
            rankPointLabels3.Add(rankPointLabel.GetComponent<CustomLabel>());
        }

        networkManager.GET(string.Format(GameConstants.API_URL_GET_RANKING_RECORDS, new ChallengeMode(2).GetModeId()).ToString(), header, (System.Action<WWW>)((WWW www) =>
            {
                List<RankingRecord> records3 = EncryptUtil.JsonToObject<List<RankingRecord>>(www.text);
                for(int i = 0; i < records3.Count; i++) {
                    rankLabels3[i].SetLabel(string.Format("{0}  {1}", records3[i].rank, records3[i].name));
                    rankPointLabels3[i].SetLabel(records3[i].point.ToString());
                }
            }));

        if (IsQuestCompleted()) {
            // クエストモードコンプリートしたら、エクストラモード解禁
            baseX = 3f;
            GameObject customBg4 = Instantiate(customBgPanelPrefab, screenManager.WPos(baseX + 0.5f, 0.5f), Quaternion.identity) as GameObject;
            customBg4.transform.parent = slideBoards.transform;
            GameObject customLabel4 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.85f), Quaternion.identity) as GameObject;
            customLabel4.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            customLabel4.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_menu_challenge"));
            customLabel4.transform.parent = customBg4.transform;
            GameObject customLabel4_0 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.82f), Quaternion.identity) as GameObject;
            customLabel4_0.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            customLabel4_0.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_extra"))
                .SetColor(Color.yellow);
            customLabel4_0.transform.parent = customBg4.transform;
            GameObject customLabel4_1 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.78f), Quaternion.identity) as GameObject;
            customLabel4_1.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            customLabel4_1.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_best_record"));
            customLabel4_1.transform.parent = customBg4.transform;
            GameObject customLabel4_2 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.5f, 0.60f), Quaternion.identity) as GameObject;
            customLabel4_2.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            customLabel4_2.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_ranking"));
            customLabel4_2.transform.parent = customBg4.transform;
            GameObject customLabel4_3 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.2f, 0.72f), Quaternion.identity) as GameObject;
            customLabel4_3.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            customLabel4_3.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_point"))
                .SetAnchor(TextAnchor.MiddleLeft).SetColor(Color.yellow);
            customLabel4_3.transform.parent = customBg4.transform;
            GameObject customLabel4_4 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.2f, 0.68f), Quaternion.identity) as GameObject;
            customLabel4_4.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            customLabel4_4.GetComponent<CustomLabel>().SetLabel(propertyManager.Get("label_stage"))
                .SetAnchor(TextAnchor.MiddleLeft).SetColor(Color.yellow);
            customLabel4_4.transform.parent = customBg4.transform;
            GameObject customLabel4_5 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.8f, 0.72f), Quaternion.identity) as GameObject;
            customLabel4_5.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            customLabel4_5.GetComponent<CustomLabel>().SetLabel(GetRecordPoint(new ChallengeMode(9).GetModeId()).ToString())
                .SetAnchor(TextAnchor.MiddleRight).SetColor(Color.white);
            customLabel4_5.transform.parent = customBg4.transform;
            GameObject customLabel4_6 = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.8f, 0.68f), Quaternion.identity) as GameObject;
            customLabel4_6.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            customLabel4_6.GetComponent<CustomLabel>().SetLabel(GetRecordStage(new ChallengeMode(9).GetModeId()).ToString())
                .SetAnchor(TextAnchor.MiddleRight).SetColor(Color.white);
            customLabel4_6.transform.parent = customBg4.transform;
            List<CustomLabel> rankLabels4 = new List<CustomLabel>();
            List<CustomLabel> rankPointLabels4 = new List<CustomLabel>();
            for (int i = 0; i < GameConstants.GET_RANKING_RECORD_COUNT; i++) {
                GameObject rankLabel = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.15f, 0.55f - 0.04f * i), Quaternion.identity) as GameObject;
                rankLabel.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                rankLabel.GetComponent<CustomLabel>().SetAnchor(TextAnchor.MiddleLeft).SetColor(Color.yellow);
                rankLabel.transform.parent = customBg4.transform;
                rankLabels4.Add(rankLabel.GetComponent<CustomLabel>());
                GameObject rankPointLabel = Instantiate(customLabelPrefab, screenManager.WPos(baseX + 0.85f, 0.55f - 0.04f * i), Quaternion.identity) as GameObject;
                rankPointLabel.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                rankPointLabel.GetComponent<CustomLabel>().SetAnchor(TextAnchor.MiddleRight).SetColor(Color.white);
                rankPointLabel.transform.parent = customBg4.transform;
                rankPointLabels4.Add(rankPointLabel.GetComponent<CustomLabel>());
            }

            networkManager.GET(string.Format(GameConstants.API_URL_GET_RANKING_RECORDS, new ChallengeMode(9).GetModeId()).ToString(), header, (System.Action<WWW>)((WWW www) =>
                {
                    List<RankingRecord> records4 = EncryptUtil.JsonToObject<List<RankingRecord>>(www.text);
                    for(int i = 0; i < records4.Count; i++) {
                        rankLabels4[i].SetLabel(string.Format("{0}  {1}", records4[i].rank, records4[i].name));
                        rankPointLabels4[i].SetLabel(records4[i].point.ToString());
                    }
                }));
        }

        // 旧実装
        //        recordBoard = Instantiate(recordBoardPrefab, recordBoardPrefab.transform.position, Quaternion.identity) as GameObject;
        //        RecordBoard board = recordBoard.GetComponent<RecordBoard>();
        //        board.SetLabels(propertyManager.Get("label_best_record"), propertyManager.Get("label_point"), propertyManager.Get("label_rank"), propertyManager.Get("label_ranking"));
        //        string userName = localDataStore.Load<string>(GameConstants.PREF_KEY_USER_NAME, "");
        //        int userPoint = GetRecordPoint();
        //        board.SetMyRecord(userName, userPoint, 10);
        //        networkManager.GET("http://tearoom6-jp.appspot.com", (System.Action<WWW>)((WWW www) =>
        //            {
        //                Logger.Log(www.text);
        //                board.SetMyRecord(www.text, userPoint, 10);
        //            }));
        //        RankingRecord[] rankingRecords = new RankingRecord[2];
        //        rankingRecords[0] = new RankingRecord("Tomohiro", 1, 100);
        //        rankingRecords[1] = new RankingRecord("Hiroshi", 2, 98);
        //        board.SetRankingRecords(rankingRecords);
        yield return new WaitForEndOfFrame();
    }

    /// <summary>
    /// 非同期でタイムボーナスのカウントを行います。
    /// </summary>
    /// <returns>The time bonus.</returns>
    /// <param name="isCompleted">If set to <c>true</c> is completed.</param>
    IEnumerator CountTimeBonus(bool isCompleted)
    {
        yield return new WaitForSeconds(0.5f);
        float countupSeconds = 0.2f;
        int addPoint = 0;
        int timeBonus = 0;
        for (int i = 0; i < 300; i++)
        {
            yield return new WaitForSeconds(0.05f);
            if (seconds < countupSeconds)
            {
                addPoint = (int)Mathf.Floor(GameConstants.POINT_TIME_BONUS_FOR_SECOND * countupSeconds * seconds);
                timeBonus += addPoint;
                AddPoint(addPoint);
                InitSeconds();
                break;
            }
            addPoint = (int)Mathf.Floor(GameConstants.POINT_TIME_BONUS_FOR_SECOND * countupSeconds);
            timeBonus += addPoint;
            AddPoint(addPoint);
            seconds -= countupSeconds;
        }
        Toast(propertyManager.Get("toast_time_bonus", timeBonus), 2.0f);
        Scheduler.AddSchedule(GameConstants.TIMER_KEY_WAIT_NEXT_STAGE, 2.0f, (System.Action)(() =>
            {
                if (isCompleted) {
                    // Game Complete Clear
                    bool isFirstComplete = !IsQuestCompleted();
                    RecordQuestCompleted();
                    SaveRecord();
                    DeleteSaveStage();
                    InitilizeGame();
                    if (isFirstComplete) {
                        message.text = propertyManager.Get("description_congratulations_first");
                    } else {
                        message.text = propertyManager.Get("description_congratulations");
                    }
                    StateManager.Complete();
                } else {
                    StateManager.Next();
                }
            }));
    }

}

/// <summary>
/// ステップが成功したかどうか判定するための情報。
/// </summary>
[System.Serializable]
public class StepJudgeInfo
{
    public Panel startPanel;
    public Panel goalPanel;
    public int restCount;

    public StepJudgeInfo(Panel startPanel, Panel goalPanel)
    {
        this.startPanel = startPanel;
        this.goalPanel = goalPanel;
        if (startPanel != null)
        {
            this.restCount = startPanel.GetDistanceFrom(goalPanel);
        }
        else
        {
            this.restCount = 1;
        }
    }

    /// <summary>
    /// タッチされたパネルが正解かどうか判定します。
    /// </summary>
    /// <returns>The touch pattern.</returns>
    /// <param name="touchPanel">Touch panel.</param>
    /// <param name="isKeepingTouch">If set to <c>true</c> is keeping touch.</param>
    public int JudgeTouchPattern(Panel touchPanel, bool isKeepingTouch)
    {
        if (touchPanel == goalPanel)
            return 0;
        if (!isKeepingTouch)
            return -1;
        restCount--;
        if (restCount <= 0)
            return -1;
        if (!touchPanel.IsNeighbor(startPanel) || (goalPanel.GetDistanceFrom(touchPanel) >= goalPanel.GetDistanceFrom(startPanel)))
            return -1;
        startPanel = touchPanel;
        return restCount;
    }
}


