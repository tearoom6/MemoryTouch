using UnityEngine;
using System.Collections;

/// <summary>
/// チカチカと色が点滅する効果をオブジェクトに与えます
/// </summary>
public class FlickerEffect : MonoBehaviour
{

    private Color originalColor;

    void Start()
    {
        originalColor = guiText.color;
    }

    void Update()
    {
        float level = Mathf.Abs(Mathf.Sin(Time.time * 3));
        guiText.color = originalColor * level;
    }
}