using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 各モードでのステージの管理をします。
/// ステージは複数ステップから構成されています。
/// 複数のステージは、より大きな単位であるレベルにまとめられます。
/// Array must be handled by 0-origin!!
/// </summary>
public class StageManager : MonoBehaviour
{

    public List<Panel> panels = new List<Panel>();
    public Mode mode;

    private ScreenManager screenManager;
    private PropertyManager propertyManager;

    private GameObject panelPrefab;
    private GameObject stageDialogPrefab;

    private StageDialog stageDialog;

    void Start()
    {
        screenManager = GetComponent<ScreenManager>();
        Logger.Info("ScreenManager component is found.", this);
        propertyManager = GetComponent<PropertyManager>();
        Logger.Info("PropertyManager component is found.", this);

        panelPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_PANEL) as GameObject;
        Logger.Info(panelPrefab.name + " prefab is loaded.", this);
        stageDialogPrefab = Resources.Load(GameConstants.RESOURCE_PREFAB_STAGE_DIALOG) as GameObject;
        Logger.Info(stageDialogPrefab.name + " prefab is loaded.", this);
    }

    /// <summary>
    /// クリア条件の説明文を取得します。
    /// </summary>
    /// <returns>The clear requirement description.</returns>
    /// <param name="stageInfo">Stage info.</param>
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
    /// 現在のステージのステージダイアログを表示します。
    /// </summary>
    public void LoadCurrentStageDialog()
    {
        StageInfo stage = GetCurrentStage();
        GameObject stageDialogObj = Instantiate(stageDialogPrefab, stageDialogPrefab.transform.position, Quaternion.identity) as GameObject;
        stageDialog = stageDialogObj.GetComponent<StageDialog>();
        stageDialog.SetStageInfo(stage.level, stage.stageNo, GetClearRequirementDescription(stage));
        if (this.mode.GetType() == typeof(QuestMode)) {
            stageDialog.SetBgColor(Color.red);
        }
    }

    /// <summary>
    /// ステージダイアログをスライドします。
    /// </summary>
    /// <param name="time">Time.</param>
    public void SlideStageDialog(float time)
    {
        if (stageDialog == null)
            return;
        stageDialog.SlideUpper(time);
    }

    /// <summary>
    /// ステージダイアログの説明文をセットします。
    /// </summary>
    /// <param name="description">Description.</param>
    public void SetStageDialogDescription(string description)
    {
        if (stageDialog == null)
            return;
        stageDialog.description.text = description;
    }

    /// <summary>
    /// ステージダイアログを破棄します。
    /// </summary>
    public void DestroyStageDialog()
    {
        if (stageDialog == null)
            return;
        Destroy(stageDialog.gameObject);
    }

    /// <summary>
    /// 現在のステージのパネルを配置します。
    /// </summary>
    public void LoadCurrentStagePanels()
    {
        StageInfo stage = GetCurrentStage();
        panels = new List<Panel>();
        for (int i = 0; i < stage.panelRowNum; i++)
        {
            for (int j = 0; j < stage.panelColNum; j++)
            {
                panels.Add(CreatePanel(stage.panelColNum, stage.panelRowNum, j, i, 0.05f, 0.15f, 0.02f));
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
    private Panel CreatePanel(int panelColNum, int panelRowNum, int indexX, int indexY, float widthMargin, float bottomMargin, float spaceMargin)
    {
        float panelWidth = (1 - widthMargin * 2 - spaceMargin * (panelColNum - 1)) / panelColNum;
        float panelWorldWidth = screenManager.SCREEN_WIDTH * panelWidth;
        Vector3 worldPoint = new Vector3(
            screenManager.SCREEN_LEFT + screenManager.SCREEN_WIDTH * (widthMargin + (panelWidth + spaceMargin) * indexX) + panelWorldWidth / 2,
            screenManager.SCREEN_BOTTOM + screenManager.SCREEN_HEIGHT * (bottomMargin + (panelWidth + spaceMargin) * screenManager.WIDTH_HEIGHT_RATIO * (panelRowNum - 1 - indexY)) + panelWorldWidth / 2,
            GameConstants.Z_DEPTH_BASE);
        GameObject panelOjcect = Instantiate(panelPrefab, worldPoint, Quaternion.identity) as GameObject;
        panelOjcect.transform.localScale = new Vector3(
            panelWorldWidth,
            panelWorldWidth,
            GameConstants.Z_DEPTH_BASE);
        return new Panel(panelOjcect, indexX, indexY, widthMargin + (panelWidth + spaceMargin) * indexX,
            bottomMargin + (panelWidth + spaceMargin) * screenManager.WIDTH_HEIGHT_RATIO * (panelRowNum - 1 - indexY), panelWidth, panelWidth * screenManager.WIDTH_HEIGHT_RATIO);
    }

    /// <summary>
    /// パネルを消します。
    /// </summary>
    /// <param name="panel">Panel.</param>
    public void ClearPanel(Panel panel)
    {
        if (panel == null)
            return;
        Destroy(panel.panelOjcect);
        panels.Remove(panel);
    }

    /// <summary>
    /// 全てのパネルを消します。
    /// </summary>
    public void ClearAllPanels()
    {
        foreach (Panel panel in panels)
        {
            Destroy(panel.panelOjcect);
        }
        panels = new List<Panel>();
    }

    /// <summary>
    /// 全てのパネルが消されたか確認します。
    /// </summary>
    /// <returns><c>true</c> if this instance is all panels cleared; otherwise, <c>false</c>.</returns>
    public bool IsAllPanelsCleared()
    {
        return panels == null || panels.Count == 0;
    }

    /// <summary>
    /// 現在のパネルのデモを再生します。
    /// </summary>
    public void PlayCurrentStageDemo()
    {
        StageInfo stage = GetCurrentStage();
        int i = 0;
        foreach (StepInfo stepInfo in stage.stepInfos)
        {
            StepInfo cloneFlushInfo = stepInfo.Clone();
            Scheduler.AddSchedule(GameConstants.TIMER_KEY_PREFIX_PANEL_DEMO + i++.ToString(), stepInfo.startTime, (System.Action)(() =>
                {
                    AudioManager.PlayOneShot(this.audio, "button01b");
                    this.panels[cloneFlushInfo.panelIndex].Flush(cloneFlushInfo);
                }));
        }
    }

    /// <summary>
    /// 現在のステージ情報を取得します。
    /// </summary>
    /// <returns>The current stage.</returns>
    public StageInfo GetCurrentStage()
    {
        return this.mode.GetCurrentStage();
    }

    /// <summary>
    /// 実際のステップ数を取得します。これはスキップやリバースなどの変則ルールでステップ数を加味してのものです。
    /// </summary>
    /// <returns>The real step index.</returns>
    /// <param name="stage">Stage.</param>
    /// <param name="stepIndex">Step index.</param>
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
    /// <param name="stepIndex">Step index.</param>
    public StepInfo GetCurrentStep(int stepIndex)
    {
        StageInfo currentStage = GetCurrentStage();
        return currentStage.stepInfos[GetRealStepIndex(currentStage, stepIndex)];
    }

    /// <summary>
    /// 現在のステップの目的地のパネルを取得します。
    /// </summary>
    /// <returns>The step goal panel.</returns>
    /// <param name="stepIndex">Step index.</param>
    public Panel GetStepGoalPanel(int stepIndex)
    {
        return this.panels[GetCurrentStep(stepIndex).panelIndex];
    }

    /// <summary>
    /// ステージをクリアしたかどうか判定します。
    /// </summary>
    /// <returns><c>true</c> if this instance is stage cleared the specified currentStepIndex; otherwise, <c>false</c>.</returns>
    /// <param name="currentStepIndex">Current step index.</param>
    public bool IsStageCleared(int currentStepIndex)
    {
        StageInfo stage = GetCurrentStage();
        return !RangeUtil.IsBetween(GetRealStepIndex(stage, currentStepIndex), 0, stage.stepInfos.Count - 1);
    }

    /// <summary>
    /// 次のステージ情報を取得します。
    /// </summary>
    /// <returns>The stage.</returns>
    public StageInfo NextStage()
    {
        return this.mode.NextStage();
    }

    /// <summary>
    /// ステージをリセットします。
    /// </summary>
    public void ResetStage()
    {
        this.mode.ResetStage();
        ClearAllPanels();
    }

}

/// <summary>
/// ステップ情報。
/// </summary>
[System.Serializable]
public class StepInfo : FlushInfo
{
    public int panelIndex;
    public float startTime;

    public StepInfo(int panelIndex, Color flushColor, float startTime, float flushTime = 0.1f)
        : base(flushColor, flushTime)
    {
        this.panelIndex = panelIndex;
        this.startTime = startTime;
    }

    public StepInfo Clone()
    {
        return (StepInfo)MemberwiseClone();
    }
}

/// <summary>
/// ステージ情報。
/// </summary>
[System.Serializable]
public class StageInfo
{
    // レベルはステージの上位概念
    public int level;
    public int stageNo;
    // パネルの横の数
    public int panelColNum;
    // パネルの縦の数
    public int panelRowNum;
    // 制限時間
    public float limitTime;
    // 光ったのと逆順
    public bool reverse;
    // 飛ばす数(1つ飛ばし, 2つ飛ばし, ...)
    public int skipN;
    public List<StepInfo> stepInfos = new List<StepInfo>();

    public StageInfo(int level, int stageNo, int panelColNum, int panelRowNum, float limitTime, bool reverse, int skipN, List<StepInfo> stepInfos)
    {
        this.level = level;
        this.stageNo = stageNo;
        this.panelColNum = panelColNum;
        this.panelRowNum = panelRowNum;
        this.limitTime = limitTime;
        this.reverse = reverse;
        this.skipN = skipN;
        this.stepInfos = stepInfos;
    }
}

