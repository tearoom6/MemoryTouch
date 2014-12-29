using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ヘルプ用の画面パーツのテンプレ基底クラスです。
/// StageManagerの一部実装を借用したものをベースに作っているので注意。（あくまでチュートリアルなので、そこまで設計はこだわらない）
/// HelpBoardは外部から操作しないので、メソッドは基本的にprotectedにしています。
/// </summary>
public abstract class HelpBoard : MonoBehaviour {

    // Finger Move の移動速度
    private static float FINGER_MOVE_RATE = 0.05f;

    public List<Panel> panels = new List<Panel>();
    public StageInfo stageInfo = null;
    public List<PathInfo> pathInfos = new List<PathInfo>();
    public GameObject fingerObj;

    protected ScreenManager screenManager;
    protected PropertyManager propertyManager;

    private GameObject panelPrefab;
    private GameObject fingerPrefab;
    private GameObject customDialogPrefab;
    private GameObject customLabelPrefab;

    private CustomLabel description;

    private Vector3 basePos;
    private bool flushDemoPhase = true;
    private float demoPlayTime = 0f;
    private int fingerMoveStep = 0;
    private float stepMoveDistance = 0f;

    void Start () {
        screenManager = GetComponent<ScreenManager>();
        Logger.Info("ScreenManager component is found.", this);
        propertyManager = GetComponent<PropertyManager>();
        Logger.Info("PropertyManager component is found.", this);

        panelPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_PANEL) as GameObject;
        Logger.Info(panelPrefab.name + " prefab is loaded.", this);
        fingerPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_FINGER) as GameObject;
        Logger.Info(fingerPrefab.name + " prefab is loaded.", this);
        customDialogPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_CUSTOM_DIALOG) as GameObject;
        Logger.Info(customDialogPrefab.name + " prefab is loaded.", this);
        customLabelPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_CUSTOM_LABEL) as GameObject;
        Logger.Info(customLabelPrefab.name + " prefab is loaded.", this);

        // 初期位置を記憶しておく
        basePos = this.transform.position;

        // ダイアログ、パネルを生成
        stageInfo = CreateSampleStageInfo();
        LoadStageDialog(stageInfo);
        LoadStagePanels(stageInfo);

        // 指し棒オブジェクトも1度だけ生成して使い回す
        fingerObj = Instantiate(fingerPrefab, fingerPrefab.transform.position, Quaternion.identity) as GameObject;
        fingerObj.SetActive(false);
        fingerObj.transform.parent = this.transform;
        fingerObj.transform.localScale = screenManager.WScale(0.20f, 0.15f);

        // Finger Move の経路情報をあらかじめ生成
        int i = 0;
        StepInfo pastStep = GetCurrentStep(stageInfo, i);
        while (!IsStageCleared(stageInfo, ++i)) {
            StepInfo currentStep = GetCurrentStep(stageInfo, i);
            pathInfos.Add(new PathInfo(panels[pastStep.panelIndex].panelOjcect.transform.position, panels[currentStep.panelIndex].panelOjcect.transform.position));
            pastStep = currentStep;
        }

        // 指し棒の初期位置セット
        // HelpBoard自体を外部から動かすので、this.transformに対して相対移動するように注意する 
        // (もっともTranslate関数を使えば、それほど意識しなくていい)
        fingerObj.transform.position = new Vector3(
            pathInfos[0].Origin.x + screenManager.SCREEN_WIDTH * 0.05f, // パネル中央よりちょい右に
            pathInfos[0].Origin.y - screenManager.SCREEN_HEIGHT * 0.05f, // パネル中央よりちょい下に
            GameConstants.Z_DEPTH_BASE - 0.2f // パネル自体よりちょい前に
        );
    }

    void Update()
    {
        // flick description text
        float level = Mathf.Abs(Mathf.Sin(Time.time * 3));
        description.SetColor(Color.yellow * level);

        if (flushDemoPhase) {
            // Panel Flush Demo Phase
            demoPlayTime -= Time.deltaTime;

            if (demoPlayTime <= 0.0f) {
                // まず指し棒を非表示に
                fingerObj.SetActive(false);

                // Panel Flush Demo Schedule
                float demoEndTime = 0f;
                int i = 0;
                for (; i < stageInfo.stepInfos.Count; i++) {
                    StepInfo step = stageInfo.stepInfos[i];
                    Scheduler.AddSchedule(string.Format(GameConstants.TIMER_KEY_HELP_PANEL_FLUSH, this.GetType().Name, i.ToString()), step.startTime, (System.Action)(() => {
                        if (fingerObj != null)
                            panels[step.panelIndex].Flush(new FlushInfo(step.flushColor, step.flushTime));
                    }));
                    // flush demo 終了時刻を更新 (0.5secは flush demo 終了後のバッファ)
                    demoEndTime = step.startTime + step.flushTime + 0.5f;
                }
                Scheduler.AddSchedule(string.Format(GameConstants.TIMER_KEY_HELP_PANEL_FLUSH, this.GetType().Name, i.ToString()), demoEndTime, (System.Action)(() => {
                    if (fingerObj != null) {
                        // trigger finger move phase
                        // flush demo 終了後、指し棒を初期位置に戻して、再表示
                        // どうしても下の指し棒移動ロジックは毎回ズレが生じるので、この段階で元に戻しておく
                        // ちゃんとHelpBoard自体の移動分(this.transform.position - basePos)も加味しておく
                        fingerObj.transform.position = new Vector3(
                            pathInfos[0].Origin.x + screenManager.SCREEN_WIDTH * 0.05f, // パネル中央よりちょい右に
                            pathInfos[0].Origin.y - screenManager.SCREEN_HEIGHT * 0.05f, // パネル中央よりちょい下に
                            GameConstants.Z_DEPTH_BASE - 0.2f // パネル自体よりちょい前に
                        ) + (this.transform.position - basePos);
                        fingerObj.SetActive(true);
                        flushDemoPhase = false;
                    }
                }));

                // タイマーセット (0.5secは finger move phase 終了後のバッファ)
                demoPlayTime = demoEndTime + 0.5f;
            }
            return;
        }

        // Finger Move Lecture
        if (fingerMoveStep >= pathInfos.Count) {
            // when finger move phase finished
            fingerMoveStep = 0;
            flushDemoPhase = true;
        }
        // move
        fingerObj.transform.Translate(pathInfos[fingerMoveStep].Direction * FINGER_MOVE_RATE, Space.Self);
//        fingerObj.transform.position = new Vector3(
//            fingerObj.transform.position.x + (pathInfos[fingerMoveStep].Direction.x * FINGER_MOVE_RATE),
//            fingerObj.transform.position.y + (pathInfos[fingerMoveStep].Direction.y * FINGER_MOVE_RATE),
//            fingerObj.transform.position.z
//        );
        stepMoveDistance += FINGER_MOVE_RATE;
        if (pathInfos[fingerMoveStep].Distance < stepMoveDistance) {
            // when reached destination
            fingerMoveStep++;
            stepMoveDistance = 0f;
        }
    }

    /// <summary>
    /// サンプル表示用のステージ情報を生成します。
    /// </summary>
    /// <returns>The sample stage info.</returns>
    protected abstract StageInfo CreateSampleStageInfo();

    /// <summary>
    /// ステージのステージダイアログを表示します。
    /// ヘルプ画面専用版です。
    /// </summary>
    /// <param name="stage">Stage.</param>
    /// <see cref="StageManager.LoadCurrentStageDialog"/>
    protected void LoadStageDialog(StageInfo stage)
    {
        GameObject dialogObj = Instantiate(customDialogPrefab, this.transform.position, Quaternion.identity) as GameObject;
        GameObject descriptionObj1 = Instantiate(customLabelPrefab, this.transform.position, Quaternion.identity) as GameObject;
        GameObject descriptionObj2 = Instantiate(customLabelPrefab, this.transform.position, Quaternion.identity) as GameObject;

        // 生成したオブジェクトは、ヘルプボード自身にひも付けておく
        dialogObj.transform.parent = this.transform;
        descriptionObj1.transform.parent = this.transform;
        descriptionObj2.transform.parent = this.transform;
        // 一旦非表示に
        dialogObj.SetActive(false);

        // 配置
        dialogObj.transform.Translate(screenManager.WScale(0.0f, 0.35f, 0f), this.transform);
        dialogObj.transform.localScale = screenManager.WScale(0.9f, 0.15f);
        descriptionObj1.transform.Translate(screenManager.WScale(-0.4f, 0.38f, 0f), this.transform);
        descriptionObj1.transform.localScale = screenManager.WScale(0.08f, 0.08f);
        descriptionObj2.transform.Translate(screenManager.WScale(-0.4f, 0.32f, 0f), this.transform);
        descriptionObj2.transform.localScale = screenManager.WScale(0.06f, 0.06f);
        description = descriptionObj2.GetComponent<CustomLabel>(); // チカチカエフェクト用に取得

        descriptionObj1.GetComponent<CustomLabel>().SetLabel(string.Format(propertyManager.Get("description_tutorial"), stageInfo.stageNo))
            .SetAnchor(TextAnchor.MiddleLeft)
            .SetColor(Color.white);
        descriptionObj2.GetComponent<CustomLabel>().SetLabel(GetClearRequirementDescription(stage))
            .SetAnchor(TextAnchor.MiddleLeft)
            .SetColor(Color.yellow);

        // 再度表示する
        dialogObj.SetActive(true);
    }

    /// <summary>
    /// クリア条件の説明文を取得します。
    /// </summary>
    /// <returns>The clear requirement description.</returns>
    /// <param name="stageInfo">Stage info.</param>
    /// <see cref="StageManager.GetClearRequirementDescription"/>
    private string GetClearRequirementDescription(StageInfo stageInfo)
    {
        if (stageInfo.reverse && stageInfo.skipN > 0)
        {
            return propertyManager.Get("description_stage_reverse_skip", stageInfo.skipN);
        }
        else if (stageInfo.reverse)
        {
            return propertyManager.Get("description_stage_reverse");
        }
        else if (stageInfo.skipN > 0)
        {
            return propertyManager.Get("description_stage_skip", stageInfo.skipN);
        }
        else
        {
            return propertyManager.Get("description_stage_standard");
        }
    }

    /// <summary>
    /// ステージのパネルを配置します。
    /// </summary>
    /// <param name="stageInfo">Stage info.</param>
    /// <see cref="StageManager.LoadCurrentStagePanels"/>
    protected void LoadStagePanels(StageInfo stageInfo)
    {
        panels = new List<Panel>();
        for (int i = 0; i < stageInfo.panelRowNum; i++)
        {
            for (int j = 0; j < stageInfo.panelColNum; j++)
            {
                panels.Add(CreatePanel(stageInfo.panelColNum, stageInfo.panelRowNum, j, i, 0.05f, 0.15f, 0.02f));
            }
        }
    }

    /// <summary>
    /// 指定した条件でパネルを生成します。
    /// </summary>
    /// <returns>The panel.</returns>
    /// <param name="panelColNum">Panel col number.</param>
    /// <param name="panelRowNum">Panel row number.</param>
    /// <param name="indexX">Index x.</param>
    /// <param name="indexY">Index y.</param>
    /// <param name="widthMargin">Width margin.</param>
    /// <param name="bottomMargin">Bottom margin.</param>
    /// <param name="spaceMargin">Space margin.</param>
    /// <see cref="StageManager.CreatePanel"/>
    private Panel CreatePanel(int panelColNum, int panelRowNum, int indexX, int indexY, float widthMargin, float bottomMargin, float spaceMargin)
    {
        float panelWidth = (1 - widthMargin * 2 - spaceMargin * (panelColNum - 1)) / panelColNum;
        float panelWorldWidth = screenManager.SCREEN_WIDTH * panelWidth;
        Vector3 relativePos = new Vector3(
            screenManager.SCREEN_WIDTH * (widthMargin + (panelWidth + spaceMargin) * indexX - 0.5f) + panelWorldWidth / 2,
            screenManager.SCREEN_HEIGHT * (bottomMargin + (panelWidth + spaceMargin) * screenManager.WIDTH_HEIGHT_RATIO * (panelRowNum - 1 - indexY) - 0.5f) + panelWorldWidth / 2,
            0f);
        GameObject panelOjcect = Instantiate(panelPrefab, this.transform.position, Quaternion.identity) as GameObject;
        panelOjcect.transform.parent = this.transform; // 生成したオブジェクトは、ヘルプボード自身にひも付けておく
        panelOjcect.transform.Translate(relativePos, this.transform);
        panelOjcect.transform.localScale = new Vector3(
            panelWorldWidth,
            panelWorldWidth,
            GameConstants.Z_DEPTH_BASE);
        return new Panel(panelOjcect, indexX, indexY, widthMargin + (panelWidth + spaceMargin) * indexX,
            bottomMargin + (panelWidth + spaceMargin) * screenManager.WIDTH_HEIGHT_RATIO * (panelRowNum - 1 - indexY), panelWidth, panelWidth * screenManager.WIDTH_HEIGHT_RATIO);
    }

    /// <summary>
    /// 実際のステップ数を取得します。これはスキップやリバースなどの変則ルールでステップ数を加味してのものです。
    /// </summary>
    /// <returns>The real step index.</returns>
    /// <param name="stage">Stage.</param>
    /// <param name="stepIndex">Step index.</param>
    /// <see cref="StageManager.GetRealStepIndex"/>
    private int GetRealStepIndex(StageInfo stage, int stepIndex)
    {
        stepIndex += stage.skipN * stepIndex;
        if (stage.reverse)
        {
            return stage.stepInfos.Count - stepIndex - 1;
        }
        return stepIndex;
    }

    /// <summary>
    /// 指定したステップの情報を返します。
    /// </summary>
    /// <returns>The current step.</returns>
    /// <param name="stage">Stage.</param>
    /// <param name="stepIndex">Step index.</param>
    /// <see cref="StageManager.GetCurrentStep"/>
    private StepInfo GetCurrentStep(StageInfo stage, int stepIndex)
    {
        return stage.stepInfos[GetRealStepIndex(stage, stepIndex)];
    }

    /// <summary>
    /// 指定したステップでステージをクリアしたかどうか判定します。
    /// </summary>
    /// <returns><c>true</c> if this instance is stage cleared the specified currentStepIndex; otherwise, <c>false</c>.</returns>
    /// <param name="stage">Stage.</param>
    /// <param name="currentStepIndex">Current step index.</param>
    /// <see cref="StageManager.IsStageCleared"/>
    protected bool IsStageCleared(StageInfo stage, int currentStepIndex)
    {
        return !RangeUtil.IsBetween(GetRealStepIndex(stage, currentStepIndex), 0, stage.stepInfos.Count - 1);
    }

}


/// <summary>
/// Finger Move の経路情報。この経路情報を組み合わせて複雑な経路を生成する。
/// </summary>
[System.Serializable]
public class PathInfo
{
    // 起点
    public Vector3 Origin;
    // 終点
    public Vector3 Destination;
    // 方向(単位ベクトル)
    public Vector3 Direction;
    // 距離
    public float Distance;

    /// <summary>
    /// 起点と終点をもとに経路情報を生成。
    /// </summary>
    /// <param name="origin">Origin.</param>
    /// <param name="destination">Destination.</param>
    public PathInfo(Vector3 origin, Vector3 destination) {
        this.Origin = origin;
        this.Destination = destination;
        this.Direction = (destination - origin).normalized;
        this.Distance = (destination - origin).magnitude;
    }

    /// <summary>
    /// 指定した座標が経路途上に含まれるかを返します。
    /// </summary>
    /// <returns><c>true</c>, if path range was withined, <c>false</c> otherwise.</returns>
    /// <param name="position">Position.</param>
    public bool OnPathWay(Vector3 position) {
        if (!RangeUtil.IsBetween(position.x, Origin.x, Destination.x))
            return false;
        if (!RangeUtil.IsBetween(position.y, Origin.y, Destination.y))
            return false;
        return true;
    }

}
