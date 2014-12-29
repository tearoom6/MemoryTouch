using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// チャレンジモード。
/// ランダムで現れるパネル数・光る枚数でどこまで行けるか挑戦します。
/// </summary>
public class ChallengeMode : Mode
{
    // 内部的なステージNo (レベルも含めた累積値)
    private int currentStageNo = 0;
    // 内部的に保持しておく現在のステージ情報
    private StageInfo currentStage;

    private int difficultLv;
    private bool reverse;
    private int skipN;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChallengeMode"/> class.
    /// </summary>
    public ChallengeMode(int difficultLv)
    {
        this.difficultLv = difficultLv;
        this.reverse = false;
        this.skipN = 0;
    }

    /// <summary>
    /// ステージ情報を生成します。
    /// </summary>
    /// <returns>The stage info.</returns>
    private StageInfo createStageInfo() {
        int panelSideNum;
        int stepNum;
        float limitTime;
        if (difficultLv == 9) {
            // Extraモード
            panelSideNum = 7;
            stepNum = RandomUtil.RandomInt(8, 9);
            limitTime = RandomUtil.RandomFloat(4, 5);
            bool randomReverse = (RandomUtil.RandomInt(0, 1) == 0);
            int randomSkipN = RandomUtil.RandomInt(0, 1);
            return new StageInfo(0, currentStageNo + 1, panelSideNum, panelSideNum, limitTime, randomReverse, randomSkipN, CreateStepInfos(stepNum, panelSideNum * panelSideNum, 0.2f, 0.2f, new Dictionary<float, int>(){{0.2f,1},{0.3f,1}}));
        }
        if (currentStageNo < 7) {
            panelSideNum = RandomUtil.RandomInt(2, 3) + difficultLv;
            stepNum = RandomUtil.RandomInt(2, 4) + difficultLv;
            limitTime = RandomUtil.RandomFloat(stepNum, stepNum + 2);
        } else if (currentStageNo < 14) {
            panelSideNum = RandomUtil.RandomInt(3, 4) + difficultLv;
            stepNum = RandomUtil.RandomInt(2, 4) + difficultLv;
            limitTime = RandomUtil.RandomFloat(stepNum, stepNum + 1);
        } else if (currentStageNo < 21) {
            panelSideNum = RandomUtil.RandomInt(3, 4) + difficultLv;
            stepNum = RandomUtil.RandomInt(3, 5) + difficultLv;
            limitTime = RandomUtil.RandomFloat(stepNum - 1, stepNum);
        } else if (currentStageNo < 30) {
            panelSideNum = RandomUtil.RandomInt(3, 4) + difficultLv;
            stepNum = RandomUtil.RandomInt(4, 6) + difficultLv;
            limitTime = RandomUtil.RandomFloat(stepNum - 3, stepNum - 1);
        } else {
            panelSideNum = RandomUtil.RandomInt(3, 4) + difficultLv;
            stepNum = RandomUtil.RandomInt(6, 10) + difficultLv;
            limitTime = RandomUtil.RandomFloat(stepNum - 4, stepNum - 2);
        }
        int panelNum = panelSideNum * panelSideNum;
        return new StageInfo(0, currentStageNo + 1, panelSideNum, panelSideNum, limitTime, reverse, skipN, CreateStepInfos(stepNum, panelNum, 0.1f, 0.2f, new Dictionary<float, int>(){{0.2f,2},{0.3f,1},{0.4f,1},{0.5f,1},}));
    }

    /// <summary>
    /// Gets the mode identifier.
    /// </summary>
    /// <returns>The mode identifier.</returns>
    public override string GetModeId()
    {
        return this.GetType().ToString() + difficultLv.ToString();
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
