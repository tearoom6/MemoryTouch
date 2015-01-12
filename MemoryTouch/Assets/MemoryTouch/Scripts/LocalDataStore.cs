using System;
using UnityEngine;

public class LocalDataStore : MonoBehaviour
{
    /// <summary>
    /// オブジェクトをJSON化してローカル保存します。
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    public void Save(string key, System.Object value)
    {
        if (value == null)
            return;
        PlayerPrefs.SetString(key, EncryptUtil.ObjectToJson(value));
    }

    /// <summary>
    /// intのvalueがすでに保存されているものより大きい場合にローカル保存します。
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    /// <returns>保存した場合にtrueを返す</returns>
    public bool SaveMaxInt(string key, int value)
    {
        int loadData = Load<int>(key, int.MinValue);
        if (value <= loadData)
            return false;
        Save(key, value);
        return true;
    }

    /// <summary>
    /// JSON化してローカル保存したオブジェクトをロードします。
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="defaultValue">Default value.</param>
    /// <typeparam name="T">オブジェクトの型</typeparam>
    /// <returns>保存されていたオブジェクト</returns>
    public T Load<T>(string key, T defaultValue)
    {
        string jsonData = PlayerPrefs.GetString(key, "");
        if (jsonData == "")
            return defaultValue;
        return EncryptUtil.JsonToObject<T>(jsonData);
    }

    /// <summary>
    /// オブジェクトを暗号化してローカル保存します。
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    /// <param name="passwd">Passwd.</param>
    public void SaveWithPWD(string key, System.Object value, string passwd)
    {
        if (value == null)
            return;
        PlayerPrefs.SetString(key, EncryptUtil.Encrypt(value, key+passwd));
    }

    /// <summary>
    /// intのvalueがすでに保存されているものより大きい場合にローカル保存します。
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    /// <param name="passwd">Passwd.</param>
    /// <returns>保存した場合にtrueを返す</returns>
    public bool SaveMaxIntWithPWD(string key, int value, string passwd)
    {
        int loadData = LoadWithPWD<int>(key, passwd, int.MinValue);
        if (value <= loadData)
            return false;
        SaveWithPWD(key, value, passwd);
        return true;
    }

    /// <summary>
    /// 暗号化してローカル保存したオブジェクトをロードします。
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="passwd">Passwd.</param>
    /// <param name="defaultValue">Default value.</param>
    /// <typeparam name="T">オブジェクトの型</typeparam>
    /// <returns>保存されていたオブジェクト</returns>
    public T LoadWithPWD<T>(string key, string passwd, T defaultValue)
    {
        string encryptedData = PlayerPrefs.GetString(key, "");
        if (encryptedData == "")
            return defaultValue;
        return EncryptUtil.Decrypt<T>(encryptedData, key+passwd);
    }

    /// <summary>
    /// 指定したキーのローカル保存が存在する場合にtrueを返します。
    /// </summary>
    /// <returns><c>true</c> if this instance has key the specified key; otherwise, <c>false</c>.</returns>
    /// <param name="key">Key.</param>
    public bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    /// <summary>
    /// 指定したキーのローカル保存を破棄します。
    /// </summary>
    /// <param name="key">Key.</param>
    public void Delete(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    /// <summary>
    /// すべてのローカル保存を破棄します。
    /// </summary>
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
