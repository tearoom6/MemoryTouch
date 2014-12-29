using UnityEngine;
using System.Collections;

/// <summary>
/// 設定ボードの制御をします。
/// </summary>
public class SettingBoard : MonoBehaviour
{

    public GUIText nameLabel;
    public GUIText okButtonLabel;

    public void SetNameLabel(string labelText)
    {
        nameLabel.text = labelText;
    }

    public void SetOkButtonLabel(string buttonText)
    {
        okButtonLabel.text = buttonText;
    }
}