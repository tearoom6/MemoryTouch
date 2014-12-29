using UnityEngine;
using System.Collections;

/// <summary>
/// 一時停止ダイアログの制御をします。
/// </summary>
public class PauseDialog : MonoBehaviour
{

    public GUIText descriptionLabel;
    public GUIText okButtonLabel;
    public GUIText cancelButtonLabel;

    public void SetLabels(string description, string okStr, string cancelStr)
    {
        descriptionLabel.text = description;
        okButtonLabel.text = okStr;
        cancelButtonLabel.text = cancelStr;
    }


}
