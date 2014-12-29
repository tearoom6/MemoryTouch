using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ヘルプ用の画面パーツその3です。
/// </summary>
public class HelpBoard3 : HelpBoard {

    /// <summary>
    /// サンプル表示用のステージ情報を生成します。
    /// </summary>
    /// <returns>The sample stage info.</returns>
    protected override StageInfo CreateSampleStageInfo()
    {
        return new StageInfo(1, 3, 3, 3, 5.0f, true, 0, new List<StepInfo>(new StepInfo[]{
            new StepInfo(8, Color.cyan, 0.1f, 0.3f),
            new StepInfo(3, Color.green, 0.5f, 0.3f),
            new StepInfo(0, Color.blue, 0.9f, 0.3f),
        }));
    }

}
