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
        // stage 1 2x2
        stageInfos.Add(new StageInfo(1, 1, 2, 2, 2.0f, false, 0, CreateStepInfos(3, 4, 0.3f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(1, 2, 2, 2, 2.0f, false, 0, CreateStepInfos(3, 4, 0.3f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(1, 3, 2, 2, 2.5f, false, 0, CreateStepInfos(4, 4, 0.25f, 0.3f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(1, 4, 2, 2, 2.5f, false, 0, CreateStepInfos(4, 4, 0.15f, 0.25f, new Dictionary<float, int>(){{0.2f,1},{0.4f,2},})));
        stageInfos.Add(new StageInfo(1, 5, 2, 2, 3.0f, false, 0, CreateStepInfos(5, 4, 0.15f, 0.25f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(1, 6, 2, 2, 3.0f, false, 0, CreateStepInfos(5, 4, 0.15f, 0.25f, new Dictionary<float, int>(){{0.15f,1},{0.4f,1},})));
        stageInfos.Add(new StageInfo(1, 7, 2, 2, 4.0f, false, 0, CreateStepInfos(6, 4, 0.1f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(1, 8, 2, 2, 4.0f, false, 0, CreateStepInfos(6, 4, 0.1f, 0.2f, new Dictionary<float, int>(){{0.1f,1},{0.3f,1},})));
        stageInfos.Add(new StageInfo(1, 9, 2, 2, 5.0f, false, 0, CreateStepInfos(8, 4, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 2 3x3
        stageInfos.Add(new StageInfo(2, 1, 3, 3, 2.0f, false, 0, CreateStepInfos(3, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(2, 2, 3, 3, 2.0f, false, 0, CreateStepInfos(3, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(2, 3, 3, 3, 2.5f, false, 0, CreateStepInfos(4, 9, 0.25f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(2, 4, 3, 3, 2.5f, false, 0, CreateStepInfos(4, 9, 0.15f, 0.25f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(2, 5, 3, 3, 2.5f, false, 0, CreateStepInfos(4, 9, 0.15f, 0.25f, new Dictionary<float, int>(){{0.2f,2},{0.4f,1},})));
        stageInfos.Add(new StageInfo(2, 6, 3, 3, 3.0f, false, 0, CreateStepInfos(5, 9, 0.15f, 0.25f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(2, 7, 3, 3, 3.0f, false, 0, CreateStepInfos(5, 9, 0.15f, 0.2f, new Dictionary<float, int>(){{0.2f,3},{0.4f,1},})));
        stageInfos.Add(new StageInfo(2, 8, 3, 3, 3.0f, false, 0, CreateStepInfos(5, 9, 0.15f, 0.2f, new Dictionary<float, int>(){{0.1f,2},{0.3f,3},})));
        stageInfos.Add(new StageInfo(2, 9, 3, 3, 4.0f, false, 0, CreateStepInfos(6, 9, 0.15f, 0.15f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 3 3x3
        stageInfos.Add(new StageInfo(3, 1, 3, 3, 2.5f, false, 0, CreateStepInfos(4, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(3, 2, 3, 3, 2.5f, false, 0, CreateStepInfos(4, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.15f,1},})));
        stageInfos.Add(new StageInfo(3, 3, 3, 3, 2.5f, false, 0, CreateStepInfos(4, 9, 0.25f, 0.3f, new Dictionary<float, int>(){{0.1f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(3, 4, 3, 3, 3.0f, false, 0, CreateStepInfos(5, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.2f,1},{0.3f,1},})));
        stageInfos.Add(new StageInfo(3, 5, 3, 3, 3.0f, false, 0, CreateStepInfos(5, 9, 0.2f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(3, 6, 3, 3, 4.0f, false, 0, CreateStepInfos(6, 9, 0.2f, 0.3f, new Dictionary<float, int>(){{0.1f,3},{0.4f,1},})));
        stageInfos.Add(new StageInfo(3, 7, 3, 3, 4.0f, false, 0, CreateStepInfos(6, 9, 0.15f, 0.2f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(3, 8, 3, 3, 5.0f, false, 0, CreateStepInfos(7, 9, 0.15f, 0.2f, new Dictionary<float, int>(){{0.15f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(3, 9, 3, 3, 6.0f, false, 0, CreateStepInfos(8, 9, 0.15f, 0.15f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 4 4x4
        stageInfos.Add(new StageInfo(4, 1, 4, 4, 2.0f, false, 0, CreateStepInfos(3, 16, 0.25f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(4, 2, 4, 4, 2.0f, false, 0, CreateStepInfos(3, 16, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(4, 3, 4, 4, 2.0f, false, 0, CreateStepInfos(3, 16, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(4, 4, 4, 4, 2.5f, false, 0, CreateStepInfos(4, 16, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(4, 5, 4, 4, 2.5f, false, 0, CreateStepInfos(4, 16, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(4, 6, 4, 4, 2.5f, false, 0, CreateStepInfos(4, 16, 0.15f, 0.15f, new Dictionary<float, int>(){{0.1f,1},{0.3f,2},})));
        stageInfos.Add(new StageInfo(4, 7, 4, 4, 3.0f, false, 0, CreateStepInfos(5, 16, 0.15f, 0.15f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(4, 8, 4, 4, 3.0f, false, 0, CreateStepInfos(5, 16, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(4, 9, 4, 4, 4.0f, false, 0, CreateStepInfos(6, 16, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 5 4x4
        stageInfos.Add(new StageInfo(5, 1, 4, 4, 2.5f, false, 0, CreateStepInfos(4, 16, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,2},{0.3f,1},})));
        stageInfos.Add(new StageInfo(5, 2, 4, 4, 2.5f, false, 0, CreateStepInfos(4, 16, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(5, 3, 4, 4, 2.5f, false, 0, CreateStepInfos(4, 16, 0.2f, 0.25f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(5, 4, 4, 4, 3.0f, false, 0, CreateStepInfos(5, 16, 0.2f, 0.20f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(5, 5, 4, 4, 3.0f, false, 0, CreateStepInfos(5, 16, 0.15f, 0.2f, new Dictionary<float, int>(){{0.2f,2},{0.4f,1},})));
        stageInfos.Add(new StageInfo(5, 6, 4, 4, 3.0f, false, 0, CreateStepInfos(5, 16, 0.15f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(5, 7, 4, 4, 4.0f, false, 0, CreateStepInfos(6, 16, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(5, 8, 4, 4, 4.0f, false, 0, CreateStepInfos(6, 16, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(5, 9, 4, 4, 5.0f, false, 0, CreateStepInfos(7, 16, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 6 4x4
        stageInfos.Add(new StageInfo(6, 1, 4, 4, 3.0f, false, 0, CreateStepInfos(5, 16, 0.3f, 0.3f, new Dictionary<float, int>(){{0.3f,2},})));
        stageInfos.Add(new StageInfo(6, 2, 4, 4, 3.0f, false, 0, CreateStepInfos(5, 16, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,3},})));
        stageInfos.Add(new StageInfo(6, 3, 4, 4, 3.0f, false, 0, CreateStepInfos(5, 16, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,3},})));
        stageInfos.Add(new StageInfo(6, 4, 4, 4, 3.0f, false, 0, CreateStepInfos(5, 16, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(6, 5, 4, 4, 3.0f, false, 0, CreateStepInfos(5, 16, 0.15f, 0.2f, new Dictionary<float, int>(){{0.2f,2},{0.4f,1},})));
        stageInfos.Add(new StageInfo(6, 6, 4, 4, 4.0f, false, 0, CreateStepInfos(6, 16, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,3},})));
        stageInfos.Add(new StageInfo(6, 7, 4, 4, 4.0f, false, 0, CreateStepInfos(6, 16, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,3},})));
        stageInfos.Add(new StageInfo(6, 8, 4, 4, 5.0f, false, 0, CreateStepInfos(7, 16, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(6, 9, 4, 4, 6.5f, false, 0, CreateStepInfos(9, 16, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 7 5x5
        stageInfos.Add(new StageInfo(7, 1, 5, 5, 2.0f, false, 0, CreateStepInfos(3, 25, 0.3f, 0.3f, new Dictionary<float, int>(){{0.3f,1},})));
        stageInfos.Add(new StageInfo(7, 2, 5, 5, 2.0f, false, 0, CreateStepInfos(3, 25, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(7, 3, 5, 5, 2.5f, false, 0, CreateStepInfos(4, 25, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(7, 4, 5, 5, 2.5f, false, 0, CreateStepInfos(4, 25, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(7, 5, 5, 5, 3.0f, false, 0, CreateStepInfos(5, 25, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(7, 6, 5, 5, 3.0f, false, 0, CreateStepInfos(5, 25, 0.15f, 0.15f, new Dictionary<float, int>(){{0.1f,1},{0.2f,2},})));
        stageInfos.Add(new StageInfo(7, 7, 5, 5, 4.0f, false, 0, CreateStepInfos(6, 25, 0.15f, 0.15f, new Dictionary<float, int>(){{0.1f,3},{0.2f,1},})));
        stageInfos.Add(new StageInfo(7, 8, 5, 5, 4.0f, false, 0, CreateStepInfos(6, 25, 0.15f, 0.15f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(7, 9, 5, 5, 5.0f, false, 0, CreateStepInfos(7, 25, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 8 5x5
        stageInfos.Add(new StageInfo(8, 1, 5, 5, 2.5f, false, 0, CreateStepInfos(4, 25, 0.3f, 0.3f, new Dictionary<float, int>(){{0.2f,2},})));
        stageInfos.Add(new StageInfo(8, 2, 5, 5, 2.5f, false, 0, CreateStepInfos(4, 25, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,3},})));
        stageInfos.Add(new StageInfo(8, 3, 5, 5, 2.5f, false, 0, CreateStepInfos(4, 25, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,3},})));
        stageInfos.Add(new StageInfo(8, 4, 5, 5, 3.0f, false, 0, CreateStepInfos(5, 25, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,1},{0.1f,1},})));
        stageInfos.Add(new StageInfo(8, 5, 5, 5, 3.0f, false, 0, CreateStepInfos(5, 25, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,2},})));
        stageInfos.Add(new StageInfo(8, 6, 5, 5, 4.0f, false, 0, CreateStepInfos(6, 25, 0.2f, 0.2f, new Dictionary<float, int>(){{0.1f,3},})));
        stageInfos.Add(new StageInfo(8, 7, 5, 5, 4.0f, false, 0, CreateStepInfos(6, 25, 0.15f, 0.15f, new Dictionary<float, int>(){{0.1f,3},{0.2f,1},})));
        stageInfos.Add(new StageInfo(8, 8, 5, 5, 5.0f, false, 0, CreateStepInfos(7, 25, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,3},})));
        stageInfos.Add(new StageInfo(8, 9, 5, 5, 6.0f, false, 0, CreateStepInfos(8, 25, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 9 5x5
        stageInfos.Add(new StageInfo(9, 1, 5, 5, 3.0f, false, 0, CreateStepInfos(5, 25, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,2},})));
        stageInfos.Add(new StageInfo(9, 2, 5, 5, 3.0f, false, 0, CreateStepInfos(5, 25, 0.15f, 0.2f, new Dictionary<float, int>(){{0.2f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(9, 3, 5, 5, 4.0f, false, 0, CreateStepInfos(6, 25, 0.15f, 0.2f, new Dictionary<float, int>(){{0.2f,3},})));
        stageInfos.Add(new StageInfo(9, 4, 5, 5, 4.0f, false, 0, CreateStepInfos(6, 25, 0.15f, 0.15f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(9, 5, 5, 5, 4.0f, false, 0, CreateStepInfos(6, 25, 0.15f, 0.15f, new Dictionary<float, int>(){{0.2f,2},{0.3f,1},})));
        stageInfos.Add(new StageInfo(9, 6, 5, 5, 5.0f, false, 0, CreateStepInfos(7, 25, 0.15f, 0.15f, new Dictionary<float, int>(){{0.2f,3},})));
        stageInfos.Add(new StageInfo(9, 7, 5, 5, 5.0f, false, 0, CreateStepInfos(7, 25, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,3},})));
        stageInfos.Add(new StageInfo(9, 8, 5, 5, 6.0f, false, 0, CreateStepInfos(8, 25, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,3},{0.2f,1},})));
        stageInfos.Add(new StageInfo(9, 9, 5, 5, 6.5f, false, 0, CreateStepInfos(9, 25, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 10 高速
        stageInfos.Add(new StageInfo(10, 1, 2, 2, 6.0f, false, 0, CreateStepInfos(8, 4, 0.15f, 0.15f, new Dictionary<float, int>(){{0.15f,1},})));
        stageInfos.Add(new StageInfo(10, 2, 2, 2, 6.0f, false, 0, CreateStepInfos(8, 4, 0.15f, 0.15f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(10, 3, 2, 2, 6.0f, false, 0, CreateStepInfos(9, 4, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(10, 4, 3, 3, 6.0f, false, 0, CreateStepInfos(8, 9, 0.15f, 0.15f, new Dictionary<float, int>(){{0.15f,1},})));
        stageInfos.Add(new StageInfo(10, 5, 3, 3, 6.0f, false, 0, CreateStepInfos(9, 9, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(10, 6, 4, 4, 6.0f, false, 0, CreateStepInfos(8, 16, 0.15f, 0.15f, new Dictionary<float, int>(){{0.15f,1},})));
        stageInfos.Add(new StageInfo(10, 7, 4, 4, 6.5f, false, 0, CreateStepInfos(9, 16, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(10, 8, 5, 5, 6.5f, false, 0, CreateStepInfos(9, 25, 0.15f, 0.15f, new Dictionary<float, int>(){{0.15f,1},})));
        stageInfos.Add(new StageInfo(10, 9, 5, 5, 7.0f, false, 0, CreateStepInfos(10, 25, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 11 1-skip
        stageInfos.Add(new StageInfo(11, 1, 3, 3, 2.0f, false, 1, CreateStepInfos(3, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.3f,1},})));
        stageInfos.Add(new StageInfo(11, 2, 3, 3, 2.0f, false, 1, CreateStepInfos(3, 9, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(11, 3, 3, 3, 2.5f, false, 1, CreateStepInfos(5, 9, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(11, 4, 3, 3, 2.5f, false, 1, CreateStepInfos(5, 9, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,1},{0.1f,1},})));
        stageInfos.Add(new StageInfo(11, 5, 3, 3, 3.0f, false, 1, CreateStepInfos(7, 9, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(11, 6, 4, 4, 3.0f, false, 1, CreateStepInfos(7, 16, 0.15f, 0.2f, new Dictionary<float, int>(){{0.1f,3},{0.2f,2},})));
        stageInfos.Add(new StageInfo(11, 7, 4, 4, 3.0f, false, 1, CreateStepInfos(7, 16, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(11, 8, 4, 4, 3.0f, false, 1, CreateStepInfos(7, 16, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(11, 9, 4, 4, 4.0f, false, 1, CreateStepInfos(9, 16, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 12 reverse
        stageInfos.Add(new StageInfo(12, 1, 3, 3, 2.0f, true, 0, CreateStepInfos(3, 9, 0.3f, 0.3f, new Dictionary<float, int>(){{0.3f,1},})));
        stageInfos.Add(new StageInfo(12, 2, 3, 3, 2.0f, true, 0, CreateStepInfos(3, 9, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(12, 3, 3, 3, 3.0f, true, 0, CreateStepInfos(4, 9, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(12, 4, 3, 3, 3.0f, true, 0, CreateStepInfos(4, 9, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(12, 5, 3, 3, 3.0f, true, 0, CreateStepInfos(4, 9, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(12, 6, 4, 4, 2.0f, true, 0, CreateStepInfos(3, 16, 0.15f, 0.2f, new Dictionary<float, int>(){{0.1f,3},{0.2f,2},})));
        stageInfos.Add(new StageInfo(12, 7, 4, 4, 3.0f, true, 0, CreateStepInfos(4, 16, 0.15f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(12, 8, 4, 4, 3.0f, true, 0, CreateStepInfos(4, 16, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(12, 9, 4, 4, 4.0f, true, 0, CreateStepInfos(5, 16, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 13 1-skip
        stageInfos.Add(new StageInfo(13, 1, 5, 5, 2.5f, false, 1, CreateStepInfos(5, 25, 0.3f, 0.3f, new Dictionary<float, int>(){{0.2f,2},{0.3f,1},})));
        stageInfos.Add(new StageInfo(13, 2, 5, 5, 3.0f, false, 1, CreateStepInfos(7, 25, 0.25f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(13, 3, 5, 5, 3.0f, false, 1, CreateStepInfos(7, 25, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(13, 4, 5, 5, 3.0f, false, 1, CreateStepInfos(7, 25, 0.2f, 0.25f, new Dictionary<float, int>(){{0.2f,1},{0.3f,1},})));
        stageInfos.Add(new StageInfo(13, 5, 5, 5, 4.0f, false, 1, CreateStepInfos(9, 25, 0.15f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(13, 6, 5, 5, 4.0f, false, 1, CreateStepInfos(9, 25, 0.15f, 0.2f, new Dictionary<float, int>(){{0.1f,3},{0.2f,2},})));
        stageInfos.Add(new StageInfo(13, 7, 5, 5, 4.0f, false, 1, CreateStepInfos(9, 25, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(13, 8, 5, 5, 4.0f, false, 1, CreateStepInfos(9, 25, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(13, 9, 5, 5, 5.0f, false, 1, CreateStepInfos(13, 25, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 14 reverse
        stageInfos.Add(new StageInfo(14, 1, 5, 5, 2.0f, true, 0, CreateStepInfos(3, 25, 0.25f, 0.3f, new Dictionary<float, int>(){{0.3f,1},})));
        stageInfos.Add(new StageInfo(14, 2, 5, 5, 2.0f, true, 0, CreateStepInfos(3, 25, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(14, 3, 5, 5, 3.0f, true, 0, CreateStepInfos(4, 25, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(14, 4, 5, 5, 3.0f, true, 0, CreateStepInfos(4, 25, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(14, 5, 5, 5, 3.0f, true, 0, CreateStepInfos(4, 25, 0.15f, 0.15f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(14, 6, 5, 5, 4.0f, true, 0, CreateStepInfos(5, 25, 0.15f, 0.15f, new Dictionary<float, int>(){{0.1f,3},{0.2f,2},})));
        stageInfos.Add(new StageInfo(14, 7, 5, 5, 4.0f, true, 0, CreateStepInfos(5, 25, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(14, 8, 5, 5, 4.0f, true, 0, CreateStepInfos(5, 25, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,2},{0.3f,1},})));
        stageInfos.Add(new StageInfo(14, 9, 5, 5, 5.0f, true, 0, CreateStepInfos(7, 25, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        // stage 15 6x6
        stageInfos.Add(new StageInfo(15, 1, 6, 6, 2.0f, false, 0, CreateStepInfos(3, 36, 0.3f, 0.3f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(15, 2, 6, 6, 2.5f, false, 0, CreateStepInfos(4, 36, 0.25f, 0.3f, new Dictionary<float, int>(){{0.2f,3},{0.3f,1},})));
        stageInfos.Add(new StageInfo(15, 3, 6, 6, 3.0f, false, 0, CreateStepInfos(5, 36, 0.25f, 0.25f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(15, 4, 6, 6, 3.0f, false, 0, CreateStepInfos(5, 36, 0.2f, 0.25f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(15, 5, 6, 6, 3.0f, false, 0, CreateStepInfos(5, 36, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,2},{0.4f,1},})));
        stageInfos.Add(new StageInfo(15, 6, 6, 6, 4.0f, false, 0, CreateStepInfos(6, 36, 0.15f, 0.2f, new Dictionary<float, int>(){{0.2f,1},})));
        stageInfos.Add(new StageInfo(15, 7, 6, 6, 4.0f, false, 0, CreateStepInfos(6, 36, 0.15f, 0.15f, new Dictionary<float, int>(){{0.15f,1},})));
        stageInfos.Add(new StageInfo(15, 8, 6, 6, 4.0f, false, 0, CreateStepInfos(6, 36, 0.1f, 0.15f, new Dictionary<float, int>(){{0.1f,3},{0.15f,1},})));
        stageInfos.Add(new StageInfo(15, 9, 7, 7, 5.0f, false, 0, CreateStepInfos(7, 49, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
        stageInfos.Add(new StageInfo(15, 10, 7, 7, 7.0f, false, 0, CreateStepInfos(10, 49, 0.1f, 0.1f, new Dictionary<float, int>(){{0.1f,1},})));
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
