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
        return mainProperties.Get(key, args);
    }
}
