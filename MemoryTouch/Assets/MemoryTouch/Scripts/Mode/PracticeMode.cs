using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// プラクティスモード。
/// 好きなパネル数で練習できます。
/// </summary>
public class PracticeMode : Mode
{
    // 内部的なステージNo (レベルも含めた累積値)
    private int currentStageNo = 0;
    // 内部的に保持しておく現在のステージ情報
    private StageInfo currentStage;

    private int panelColNum;
    private int panelRowNum;
    private bool reverse;
    private int skipN;

    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeMode"/> class.
    /// </summary>
    /// <param name="panelColNum">Panel col number.</param>
    /// <param name="panelRowNum">Panel row number.</param>
    public PracticeMode(int panelColNum, int panelRowNum)
    {
        this.panelColNum = panelColNum;
        this.panelRowNum = panelRowNum;
        this.reverse = false;
        this.skipN = 0;
    }

    /// <summary>
    /// ステージ情報を生成します。
    /// </summary>
    /// <returns>The stage info.</returns>
    private StageInfo createStageInfo() {
        int panelNum = panelColNum * panelRowNum;
        int stepNum = RandomUtil.RandomInt(3, 7);
        return new StageInfo(0, currentStageNo + 1, panelColNum, panelRowNum, stepNum, reverse, skipN, CreateStepInfos(stepNum, panelNum, 0.1f, 0.2f, new Dictionary<float, int>(){{0.3f,1},}));
    }

    /// <summary>
    /// Gets the mode identifier.
    /// </summary>
    /// <returns>The mode identifier.</returns>
    public override string GetModeId()
    {
        return this.GetType().ToString() + panelColNum.ToString() + panelRowNum.ToString();
    }

    /// <summary>
    /// 現在のステージを返します。
    /// </summary>
    /// <returns>The current stage.</returns>
    public override StageInfo GetCurrentStage()
    {
        if (currentStage == null) {
            currentStage = createStageInfo();
        }
        return currentStage;
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
        currentStage = createStageInfo();
        return currentStage;
    }

    /// <summary>
    /// ステージをリセットします。
    /// </summary>
    public override void ResetStage()
    {
        currentStageNo = 0;
        currentStage = null;
    }

}
