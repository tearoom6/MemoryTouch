using UnityEngine;
using System.Collections;

/// <summary>
/// ステージ情報を表すダイアログの制御をします。
/// </summary>
public class StageDialog : MonoBehaviour
{
    public GUITexture dialogBg;
    public GUIText level;
    public GUIText levelNo;
    public GUIText stage;
    public GUIText stageNo;
    public GUIText description;

    private int levelInt;
    private int stageInt;

    void Start()
    {
        level.transform.localScale *= 0.4f;
        levelNo.transform.localScale *= 0.4f;
        stage.transform.localScale *= 0.4f;
        stageNo.transform.localScale *= 0.4f;
        description.transform.localScale *= 0.18f;
    }

    public void SlideUpper(float time)
    {
        iTween.MoveTo(dialogBg.gameObject, iTween.Hash("y", 0.85f, "time", time));
        iTween.ScaleBy(dialogBg.gameObject, iTween.Hash("y", 0.3f, "time", time));
        if (levelInt == 0) {
            // in practice mode or challenge mode
            iTween.MoveTo(level.gameObject, iTween.Hash("y", 0.9f, "time", time));
            iTween.ScaleBy(level.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", time));
            iTween.MoveTo(levelNo.gameObject, iTween.Hash("x", 0.42f, "y", 0.9f, "time", time));
            iTween.ScaleBy(levelNo.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", time));
            iTween.MoveTo(stage.gameObject, iTween.Hash("y", 0.9f, "time", time));
            iTween.ScaleBy(stage.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", time));
            iTween.MoveTo(stageNo.gameObject, iTween.Hash("x", 0.42f, "y", 0.9f, "time", time));
            iTween.ScaleBy(stageNo.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", time));
        } else {
            // in quest mode
            iTween.MoveTo(level.gameObject, iTween.Hash("y", 0.9f, "time", time));
            iTween.ScaleBy(level.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", time));
            iTween.MoveTo(levelNo.gameObject, iTween.Hash("x", 0.42f, "y", 0.9f, "time", time));
            iTween.ScaleBy(levelNo.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", time));
            iTween.MoveTo(stage.gameObject, iTween.Hash("x", 0.50f, "y", 0.9f, "time", time));
            iTween.ScaleBy(stage.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", time));
            iTween.MoveTo(stageNo.gameObject, iTween.Hash("x", 0.80f, "y", 0.9f, "time", time));
            iTween.ScaleBy(stageNo.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", time));
        }
        iTween.MoveTo(description.gameObject, iTween.Hash("y", 0.85f, "time", time));
    }

    public void SetStageInfo(int level, int stageNo, string desc)
    {
        SetLevelNo(level);
        SetStageNo(stageNo);
        SetDescription(desc);
        levelInt = level;
        stageInt = stageNo;

        // in practice mode or challenge mode
        if (levelInt == 0) {
            this.level.gameObject.SetActive(false);
            this.levelNo.gameObject.SetActive(false);
        }
    }

    public void SetLevelNo(int num)
    {
        levelNo.text = num.ToString();
    }

    public void SetStageNo(int num)
    {
        stageNo.text = num.ToString();
    }

    public void SetDescription(string desc)
    {
        description.text = desc;
    }

}
