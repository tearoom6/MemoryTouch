using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class NetworkManager : MonoBehaviour
{
    /// <summary>
    /// GET通信を行います。
    /// </summary>
    /// <param name="url">URL.</param>
    /// <param name="header">Header.</param>
    /// <param name="callback">Callback.</param>
    public void GET(string url, Dictionary<string, string> header, System.Action<WWW> callback) {
        // GET送信
        StartCoroutine(Request(url, null, header, callback));
    }

    /// <summary>
    /// POST送信を行います。
    /// </summary>
    /// <param name="url">URL.</param>
    /// <param name="postData">Post data.</param>
    /// <param name="header">Header.</param>
    /// <param name="callback">Callback.</param>
    public void POST(string url, string postData, Dictionary<string, string> header, System.Action<WWW> callback) {
        byte[] postBytes = Encoding.Default.GetBytes(postData);

        // POST送信
        StartCoroutine(Request(url, postBytes, header, callback));
    }

    /// <summary>
    ///  リクエストを送信し、レスポンスを受けてコールバックを実行します。
    /// </summary>
    /// <param name="url">URL.</param>
    /// <param name="postBytes">Post bytes.</param>
    /// <param name="header">Header.</param>
    /// <param name="callback">Callback.</param>
    IEnumerator Request(string url, byte[] postBytes, Dictionary<string, string> header, System.Action<WWW> callback) {
        // HEADERの生成
        if (header == null) {
            header = new Dictionary<string, string>();
            //header.Add("Accept-Language", "ja");
        }

        WWW www = new WWW(url, postBytes, header);
        yield return www;

        // 成功
        if (www.error == null) {
            // 成功時
            Logger.Log(string.Format("Request succeeded. size={0}", www.size.ToString()));
            Logger.Log(www.text);
            callback(www);
        }
        // 失敗
        else{
            // 失敗時
            Logger.Error(string.Format("Request failed. error message={0}", www.error), this);
        }
    }

}
