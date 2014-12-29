using UnityEngine;
using System.Collections;

/// <summary>
/// パネルの色を制御します。変色させたり、チカチカさせたりできます。
/// </summary>
public class PanelColor : MonoBehaviour
{
    private static string COLOR_PROPERTY_TYPE = "_Color";

    private string timerKeyPanelColor;
    private string timerKeyPanelFlick;

    private Color originalColor;
    private Color flickColor;
    private float flickPace;

    void Start()
    {
        timerKeyPanelColor = GameConstants.TIMER_KEY_PREFIX_PANEL_COLOR + gameObject.GetInstanceID().ToString();
        timerKeyPanelFlick = GameConstants.TIMER_KEY_PREFIX_PANEL_FLICK + gameObject.GetInstanceID().ToString();
        originalColor = renderer.material.GetColor(COLOR_PROPERTY_TYPE);
    }

    void Update()
    {
        if (Timer.IsAvailable(timerKeyPanelFlick))
        {
            float level = Mathf.Abs(Mathf.Sin(Time.time * flickPace));
            renderer.material.SetColor(COLOR_PROPERTY_TYPE, originalColor + (flickColor - originalColor) * level);
        }
        if (Timer.IsTimeout(timerKeyPanelColor))
        {
            OriginalColor();
        }
        if (Timer.IsTimeout(timerKeyPanelFlick))
        {
            OriginalColor();
        }
    }

    public void Flush(FlushInfo flushInfo)
    {
        renderer.material.SetColor(COLOR_PROPERTY_TYPE, flushInfo.flushColor);
        Timer.SetTimer(timerKeyPanelColor, flushInfo.flushTime);
    }

    public void Flick(FlickInfo flickInfo)
    {
        Timer.SetTimer(timerKeyPanelFlick, flickInfo.flickTime);
        flickColor = flickInfo.flickColor;
        flickPace = flickInfo.flickPace;
    }

    public void ChangeOriginalColor(Color afterColor)
    {
        this.originalColor = afterColor;
        renderer.material.SetColor(COLOR_PROPERTY_TYPE, afterColor);
    }

    public void  OriginalColor()
    {
        renderer.material.SetColor(COLOR_PROPERTY_TYPE, this.originalColor);
    }

}

[System.Serializable]
public class FlushInfo
{
    public Color flushColor;
    public float flushTime;

    public FlushInfo(Color flushColor, float flushTime = 0.1f)
    {
        this.flushColor = flushColor;
        this.flushTime = flushTime;
    }
}

[System.Serializable]
public class FlickInfo
{
    public Color flickColor;
    public float flickPace;
    public float flickTime;

    public FlickInfo(Color flickColor, float flickPace, float flickTime = 2.0f)
    {
        this.flickColor = flickColor;
        this.flickPace = flickPace;
        this.flickTime = flickTime;
    }
}