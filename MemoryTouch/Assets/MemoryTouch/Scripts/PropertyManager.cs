using UnityEngine;
using System.Collections;

/// <summary>
/// テキストプロパティを取得するためのクラス。
/// </summary>
public class PropertyManager : MonoBehaviour
{

    private Properties mainProperties;

    void Awake()
    {
        mainProperties = new Properties("main");
    }
	
    public string Get(string key, params object[] args)
    {
        // C#上でstringリテラルとして与えないとエスケープシーケンスが認識されない
        // 現状では、改行のみで用が足りる
        return mainProperties.Get(key, args).Replace("\\n", "\n");
    }
}
