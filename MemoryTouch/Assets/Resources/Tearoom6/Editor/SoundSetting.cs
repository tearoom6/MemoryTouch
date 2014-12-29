using UnityEngine;
using UnityEditor;

/// <summary>
/// [Unity Editor 拡張]
/// Sound setting.
/// http://bribser.co.jp/blog/mute-function-with-unity/
/// </summary>
public class SoundSetting : EditorWindow {

    private static bool soundMute = false;

    /// <summary>
    /// Changes the state of the mute.
    /// EditにSoundMuteという項目を加え、ショートカットキー「Command+Shift+M」を割り当てる。
    /// </summary>
    [MenuItem("Edit/SoundMute #%m")]
    static void ChangeMuteState() {
        if (!soundMute) {
            AudioListener.volume = 0; //ミュートにする
            soundMute = true;
        } else {
            AudioListener.volume = 1; //音量を元に戻す
            soundMute = false;
        }
    }
}
