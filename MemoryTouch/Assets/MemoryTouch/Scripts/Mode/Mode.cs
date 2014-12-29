using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// モードを表す基底クラスです。
/// </summary>
public abstract class Mode
{
    /// <summary>
    /// Gets the mode identifier.
    /// </summary>
    /// <returns>The mode identifier.</returns>
    public abstract string GetModeId();

    /// <summary>
    /// 現在のステージを返します。
    /// </summary>
    /// <returns>The current stage.</returns>
    public abstract StageInfo GetCurrentStage();

    /// <summary>
    /// Gets the index of the current stage.
    /// </summary>
    /// <returns>The current stage index.</returns>
    public abstract int GetCurrentStageIndex();

    /// <summary>
    /// 次のステージを返します。
    /// </summary>
    /// <returns>The stage.</returns>
    public abstract StageInfo NextStage();

    /// <summary>
    /// ステージをリセットします。
    /// </summary>
    public abstract void ResetStage();

    /// <summary>
    /// 各ステージでのステップ情報を与えられた条件からランダム生成します。
    /// </summary>
    /// <returns>The step infos.</returns>
    /// <param name="size">Size.</param>
    /// <param name="panelNum">Panel number.</param>
    /// <param name="minFlush">Minimum flush.</param>
    /// <param name="maxFlush">Max flush.</param>
    /// <param name="weightedIntervals">Weighted intervals.</param>
    protected List<StepInfo> CreateStepInfos(int size, int panelNum, float minFlush, float maxFlush, Dictionary<float, int> weightedIntervals)
    {
        List<StepInfo> stepInfos = new List<StepInfo>();
        float startTime = 0.1f;
        for (int i = 0; i < size; i++) {
            float flushTime = RandomUtil.RandomFloat(minFlush, maxFlush);
            Color color = RandomUtil.RandomColor();
            while (color.grayscale < 0.5f) {
                // 黒っぽい色だと取り直し
                color = RandomUtil.RandomColor();
            }
            stepInfos.Add(new StepInfo(RandomUtil.DrawNewInt(0, panelNum-1), color, startTime, flushTime));
            startTime += (flushTime + RandomUtil.Draw(weightedIntervals));
        }
        return stepInfos;
    }

}
