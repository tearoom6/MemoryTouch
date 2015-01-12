using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// クエストモード。
/// 決められたステージをどこまで行けるか挑戦します。
/// </summary>
public class QuestMode : Mode
{

    // 内部的なステージNo (レベルも含めた累積値)
    private int currentStageNo = 0;

    public List<StageInfo> stages = new List<StageInfo>();

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestMode"/> class.
    /// </summary>
    public QuestMode()
    {
        // stage level settings
        stages = GetQuestStageInfos();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestMode"/> class.
    /// </summary>
    /// <param name="initStageNo">Init stage no.</param>
    public QuestMode(int initStageNo)
    {
        // stage level settings
        stages = GetQuestStageInfos();
        currentStageNo = initStageNo;
    }

    /// <summary>
    /// クエストモードにおけるステージ情報を取得します。
    /// </summary>
    /// <returns>The quest stage infos.</returns>
    private List<StageInfo> GetQuestStageInfos()
    {
        List<StageInfo> stageInfos = new List<StageInfo>();
        // int level, int stageNo, int panelColNum, int panelRowNum, float limitTime, bool reverse, int skipN, List<StepInfo> stepInfos
        stageInfos.Add(new StageInfo(1, 1, 2, 2, 2.0f, false, 0, CreateStepInfos(3, 4, 0.3f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(1, 2, 2, 2, 2.0f, false, 0, CreateStepInfos(3, 4, 0.3f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(1, 3, 2, 2, 2.0f, false, 0, CreateStepInfos(4, 4, 0.25f, 0.3f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(1, 4, 2, 2, 2.0f, false, 0, CreateStepInfos(4, 4, 0.15f, 0.25f, new Dictionary<float, int>(){{0.2f,1},{0.5f,2},})));
        stageInfos.Add(new StageInfo(1, 5, 3, 3, 2.0f, false, 0, CreateStepInfos(3, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(1, 6, 3, 3, 3.0f, false, 0, CreateStepInfos(3, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.15f,1},})));
        stageInfos.Add(new StageInfo(1, 7, 3, 3, 3.0f, false, 0, CreateStepInfos(3, 9, 0.25f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(1, 8, 3, 3, 3.0f, false, 0, CreateStepInfos(3, 9, 0.15f, 0.3f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(1, 9, 3, 3, 3.0f, false, 0, CreateStepInfos(3, 9, 0.15f, 0.3f, new Dictionary<float, int>(){{0.2f,1},{0.5f,2},})));
        stageInfos.Add(new StageInfo(2, 1, 2, 2, 2.0f, false, 0, CreateStepInfos(5, 4, 0.2f, 0.25f, new Dictionary<float, int>(){{0.1f,2},{0.4f,1},})));
        stageInfos.Add(new StageInfo(2, 2, 2, 2, 2.0f, false, 0, CreateStepInfos(5, 4, 0.15f, 0.25f, new Dictionary<float, int>(){{0.1f,3},{0.4f,1},})));
        stageInfos.Add(new StageInfo(2, 3, 2, 2, 2.0f, false, 0, CreateStepInfos(6, 4, 0.15f, 0.25f, new Dictionary<float, int>(){{0.1f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(2, 4, 2, 2, 2.0f, false, 0, CreateStepInfos(7, 4, 0.1f, 0.20f, new Dictionary<float, int>(){{0f,1},{0.1f,1},})));
        stageInfos.Add(new StageInfo(2, 5, 3, 3, 3.0f, false, 0, CreateStepInfos(5, 9, 0.2f, 0.3f, new Dictionary<float, int>(){{0.2f,2},{0.4f,1},})));
        stageInfos.Add(new StageInfo(2, 6, 3, 3, 3.0f, false, 0, CreateStepInfos(5, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.1f,3},{0.4f,2},})));
        stageInfos.Add(new StageInfo(2, 7, 3, 3, 3.0f, false, 0, CreateStepInfos(6, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.1f,3},{0.4f,1},})));
        stageInfos.Add(new StageInfo(2, 8, 3, 3, 3.0f, false, 0, CreateStepInfos(7, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.1f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(2, 9, 3, 3, 3.0f, false, 0, CreateStepInfos(8, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.1f,1},})));
        return stageInfos;
    }

    /// <summary>
    /// Gets the mode identifier.
    /// </summary>
    /// <returns>The mode identifier.</returns>
    public override string GetModeId()
    {
        return this.GetType().ToString();
    }

    /// <summary>
    /// 現在のステージを返します。
    /// </summary>
    /// <returns>The current stage.</returns>
    public override StageInfo GetCurrentStage()
    {
        return this.stages[currentStageNo];
    }

    /// <summary>
    /// Gets the index of the current stage.
    /// </summary>
    /// <returns>The current stage index.</returns>
    public override int GetCurrentStageIndex() {
        return currentStageNo;
    }

    /// <summary>
    /// 次のステージを返します。
    /// </summary>
    /// <returns>The stage.</returns>
    public override StageInfo NextStage()
    {
        currentStageNo++;
        if (stages.Count <= currentStageNo)
            return null;
        return stages[currentStageNo];
    }

    /// <summary>
    /// ステージをリセットします。
    /// </summary>
    public override void ResetStage()
    {
        currentStageNo = 0;
    }

    /// <summary>
    /// Gets the stage.
    /// </summary>
    /// <returns>The stage.</returns>
    /// <param name="stageIndex">Stage index.</param>
    public StageInfo GetStage(int stageIndex) {
        if (stageIndex >= stages.Count)
            return null;
        return this.stages[stageIndex];
    }

}
