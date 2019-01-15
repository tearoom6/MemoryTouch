using UnityEngine;
using System.Collections;

/// <summary>
/// 設定ボードの制御をします。
/// </summary>
public class SettingBoard : MonoBehaviour
{

    public GUIText nameLabel;
    public GUIText nameSubLabel;
    public GUIText okButtonLabel;
    public GUIText privacyPolicyLabel;

    public void SetNameLabel(string labelText)
    {
        nameLabel.text = labelText;
    }

    public void SetNameSubLabel(string labelText)
    {
        nameSubLabel.text = labelText;
    }

    public void SetOkButtonLabel(string buttonText)
    {
        okButtonLabel.text = buttonText;
    }

    public void SetPrivacyPolicyLabel(string labelText)
    {
        privacyPolicyLabel.text = labelText;
    }
}
