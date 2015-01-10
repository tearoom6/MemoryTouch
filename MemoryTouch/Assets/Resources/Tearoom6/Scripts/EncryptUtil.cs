using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;

/// <summary>
/// 暗号化に関するUtilityクラス。
/// http://dobon.net/vb/dotnet/string/encryptstring.html
/// http://dobon.net/vb/dotnet/string/md5.html
/// </summary>
public class EncryptUtil
{
    /// <summary>
    /// オブジェクトをJSON化したのち、暗号化します。
    /// </summary>
    /// <param name="value">暗号化するオブジェクト</param>
    /// <param name="password">暗号化に使用したパスワード</param>
    /// <returns>オブジェクトをJSON化して暗号化した文字列</returns>
    public static string Encrypt(Object value, string password)
    {
        return EncryptString(ObjectToJson(value), password);
    }

    /// <summary>
    /// JSON化して暗号化した文字列を復元して、オブジェクトを生成します。
    /// </summary>
    /// <param name="sourceString">暗号化された文字列</param>
    /// <param name="password">暗号化に使用したパスワード</param>
    /// <typeparam name="T">元に戻すオブジェクトの型</typeparam>
    /// <returns>元に戻されたオブジェクト</returns>
    public static T Decrypt<T>(string sourceString, string password)
    {
        return JsonToObject<T>(DecryptString(sourceString, password));
    }

    /// <summary>
    /// 文字列を暗号化します。
    /// </summary>
    /// <param name="sourceString">暗号化する文字列</param>
    /// <param name="password">暗号化に使用するパスワード</param>
    /// <returns>暗号化された文字列</returns>
    public static string EncryptString(string sourceString, string password)
    {
        // RijndaelManagedオブジェクトを作成
        System.Security.Cryptography.RijndaelManaged rijndael =
            new System.Security.Cryptography.RijndaelManaged();

        // パスワードから共有キーと初期化ベクタを作成
        byte[] key, iv;
        GenerateKeyFromPassword(
            password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
        rijndael.Key = key;
        rijndael.IV = iv;

        // 文字列をバイト型配列に変換する
        byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(sourceString);

        // 対称暗号化オブジェクトの作成
        System.Security.Cryptography.ICryptoTransform encryptor =
            rijndael.CreateEncryptor();
        // バイト型配列を暗号化する
        byte[] encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
        // 閉じる
        encryptor.Dispose();

        // バイト型配列を文字列に変換して返す
        return System.Convert.ToBase64String(encBytes);
    }

    /// <summary>
    /// 暗号化された文字列を復号化します。
    /// </summary>
    /// <param name="sourceString">暗号化された文字列</param>
    /// <param name="password">暗号化に使用したパスワード</param>
    /// <returns>復号化された文字列</returns>
    public static string DecryptString(string sourceString, string password)
    {
        // RijndaelManagedオブジェクトを作成
        System.Security.Cryptography.RijndaelManaged rijndael =
            new System.Security.Cryptography.RijndaelManaged();

        // パスワードから共有キーと初期化ベクタを作成
        byte[] key, iv;
        GenerateKeyFromPassword(
            password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
        rijndael.Key = key;
        rijndael.IV = iv;

        // 文字列をバイト型配列に戻す
        byte[] strBytes = System.Convert.FromBase64String(sourceString);

        // 対称暗号化オブジェクトの作成
        System.Security.Cryptography.ICryptoTransform decryptor =
            rijndael.CreateDecryptor();
        // バイト型配列を復号化する
        // 復号化に失敗すると例外CryptographicExceptionが発生
        byte[] decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
        // 閉じる
        decryptor.Dispose();

        // バイト型配列を文字列に戻して返す
        return System.Text.Encoding.UTF8.GetString(decBytes);
    }

    /// <summary>
    /// 文字列をMD5ハッシュ化します。
    /// </summary>
    /// <returns><c>true</c> if hash strData; otherwise, <c>false</c>.</returns>
    /// <param name="strData">String data.</param>
    public static string Hash(string strData)
    {
        // 文字列をbyte型配列に変換する
        byte[] byteData = System.Text.Encoding.UTF8.GetBytes(strData);
        // ハッシュ化
        byte[] hashCode = Hash(byteData);
        // byte型配列を16進数の文字列に変換して返す
        return BitConverter.ToString(hashCode).ToLower().Replace("-","");
    }

    /// <summary>
    /// バイト列をMD5ハッシュ化します。
    /// </summary>
    /// <returns><c>true</c> if hash byteData; otherwise, <c>false</c>.</returns>
    /// <param name="byteData">Byte data.</param>
    public static byte[] Hash(byte[] byteData)
    {
        // MD5CryptoServiceProviderオブジェクトを作成
        System.Security.Cryptography.MD5CryptoServiceProvider md5 =
            new System.Security.Cryptography.MD5CryptoServiceProvider();
        // ハッシュ値を計算する
        byte[] hashCode = md5.ComputeHash(byteData);
        // 閉じる
        md5.Clear();

        return hashCode;
    }

    /// <summary>
    /// オブジェクトをJSON化します。
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>JSON文字列</returns>
    public static string ObjectToJson(Object value)
    {
        try {
            return JsonMapper.ToJson(value);
        } catch (JsonException) {
            JsonData data = new JsonData(value);
            return data.ToJson();
        }
    }

    /// <summary>
    /// JSON文字列をオブジェクトにデシリアライズします。
    /// </summary>
    /// <returns>The to object.</returns>
    /// <param name="value">Value.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static T JsonToObject<T>(string value)
    {
        try {
            return JsonMapper.ToObject<T>(value);
        } catch (Exception) {
            List<T> obj = JsonMapper.ToObject<List<T>>(string.Format("[{0}]", value));
            return obj[0];
        }
    }

    /// <summary>
    /// パスワードから共有キーと初期化ベクタを生成する。
    /// </summary>
    /// <param name="password">基になるパスワード</param>
    /// <param name="keySize">共有キーのサイズ（ビット）</param>
    /// <param name="key">作成された共有キー</param>
    /// <param name="blockSize">初期化ベクタのサイズ（ビット）</param>
    /// <param name="iv">作成された初期化ベクタ</param>
    private static void GenerateKeyFromPassword(string password,
        int keySize, out byte[] key, int blockSize, out byte[] iv)
    {
        // パスワードから共有キーと初期化ベクタを作成する
        // saltを決める
        byte[] salt = System.Text.Encoding.UTF8.GetBytes("saltは必ず8バイト以上");
        // Rfc2898DeriveBytesオブジェクトを作成する
        System.Security.Cryptography.Rfc2898DeriveBytes deriveBytes =
            new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt);
        // .NET Framework 1.1以下の時は、PasswordDeriveBytesを使用する
        // System.Security.Cryptography.PasswordDeriveBytes deriveBytes =
        //    new System.Security.Cryptography.PasswordDeriveBytes(password, salt);
        // 反復処理回数を指定する デフォルトで1000回
        deriveBytes.IterationCount = 1000;

        // 共有キーと初期化ベクタを生成する
        key = deriveBytes.GetBytes(keySize / 8);
        iv = deriveBytes.GetBytes(blockSize / 8);
    }
}
