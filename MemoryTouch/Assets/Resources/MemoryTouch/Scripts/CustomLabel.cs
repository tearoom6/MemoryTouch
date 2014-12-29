using UnityEngine;
using System.Collections;

/// <summary>
/// 任意のラベルを与えられます
/// </summary>
public class CustomLabel : MonoBehaviour
{

    public TextMesh label;

    public CustomLabel SetLabel(string label)
    {
        this.label.text = label;
        return this;
    }

    public string GetLabel()
    {
        return this.label.text;
    }

    public CustomLabel SetColor(Color color)
    {
        this.label.color = color;
        return this;
    }

    public CustomLabel SetAnchor(TextAnchor anchor)
    {
        this.label.anchor = anchor;
        return this;
    }

}
