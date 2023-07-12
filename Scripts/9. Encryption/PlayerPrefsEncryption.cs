using UnityEngine;
using System.Security.Cryptography;
using System.Text;

public class PlayerPrefsEncryption
{
    private static PlayerPrefsEncryption instance;
    private static readonly byte[] Key = Encoding.ASCII.GetBytes("01234567890123456789012345678901"); // 32����Ʈ
    private static readonly byte[] IV = Encoding.ASCII.GetBytes("0123456789012345"); // 16����Ʈ
    //private static readonly byte[] Key = Encoding.ASCII.GetBytes("01234567890123456789012345678901"); // 32����Ʈ
    //private static readonly byte[] IV = Encoding.ASCII.GetBytes("0123456789012345"); // 16����Ʈ

    public static PlayerPrefsEncryption Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerPrefsEncryption();
            }

            return instance;
        }
    }

    private PlayerPrefsEncryption() { }

    /// <summary>
    /// ���ڷ� ���� key������ ��ȣȭ�� ���� ����, 
    /// PlayerPrefs�� ������ Key, ��ȣȭ�Ͽ� ������ �� value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetGeneric<T>(string key, T value)
    {
        byte[] encryptedValue = EncryptValue(value);
        string base64String = System.Convert.ToBase64String(encryptedValue);
        Debug.Log("Encrypted Value: " + base64String);

        PlayerPrefs.SetString(key, base64String);
    }

    /// <summary>
    /// ��ȣȭ�� ���� �����ͼ� ��ȣȭ�� �� ��ȯ,
    /// defaultValue �־��� key���� �������������� ��ȯ�� �⺻��
    /// try catch�� �⺻�� ��ȯ ������˴ϴ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public T GetGeneric<T>(string key, T defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
            return defaultValue;

        string base64String = PlayerPrefs.GetString(key);
        byte[] encryptedValue = System.Convert.FromBase64String(base64String);
        Debug.Log("Encrypted Value: " + Encoding.ASCII.GetString(encryptedValue));

        T decryptedValue = DecryptValue<T>(encryptedValue);
        Debug.Log("Decrypted Value: " + decryptedValue.ToString());

        return decryptedValue;
    }

    /// <summary>
    /// ��ȣȭ �ϴ� �޼���. ���ڷ� ���� value�� ����ȭ �� AES��ĪŰ �˰������� ��ȣȭ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public byte[] EncryptValue<T>(T value)
    {
        // ��(value)�� ����ȭ�Ͽ� ����Ʈ �迭�� ��ȯ�մϴ�.
        byte[] serializedValue = SerializeValue(value);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key; // ��ȣȭ�� ����� ��ĪŰ(Key)�� �����մϴ�.
            aes.IV = IV; // ��ȣȭ�� ����� �ʱ�ȭ ����(IV)�� �����մϴ�.

            // ��ĪŰ ��ȣȭ�� ������ �� �ִ� ICryptoTransform ��ü�� �����մϴ�.
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encryptedBytes;

            using (var ms = new System.IO.MemoryStream())
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                // ��ȣȭ�� �����͸� ���� ���� CryptoStream�� �����մϴ�.
                // �� CryptoStream�� ��ȣȭ�� �����Ͽ� �����͸� �� �� �ֵ��� �մϴ�.
                cs.Write(serializedValue, 0, serializedValue.Length);
                cs.FlushFinalBlock();
                encryptedBytes = ms.ToArray(); // ��ȣȭ�� �����͸� ����Ʈ �迭�� ��ȯ�մϴ�.
            }

            return encryptedBytes; // ��ȣȭ�� �������� ����Ʈ �迭�� ��ȯ�մϴ�.
        }
    }
    /// <summary>
    /// ��ȣȭ�� byte�迭�� AES��ĪŰ �˰������� ��ȣȭ, ��ȣȭ�� ����Ʈ �迭�� ������ȭ �� ���� �����ͷ� ��ȯ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="encryptedBytes"></param>
    /// <returns></returns>
    public T DecryptValue<T>(byte[] encryptedBytes)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key; // ��ȣȭ�� ����� ��ĪŰ(Key)�� �����մϴ�.
            aes.IV = IV; // ��ȣȭ�� ����� �ʱ�ȭ ����(IV)�� �����մϴ�.

            // ��ĪŰ ��ȣȭ�� ������ �� �ִ� ICryptoTransform ��ü�� �����մϴ�.
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] decryptedBytes;

            using (var ms = new System.IO.MemoryStream(encryptedBytes))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var decryptedStream = new System.IO.MemoryStream())
            {
                // ��ȣȭ�� �����͸� �б� ���� CryptoStream�� �����մϴ�.
                // �� CryptoStream�� ��ȣȭ�� �����͸� �о ��ȣȭ�� �����͸� �����մϴ�.
                cs.CopyTo(decryptedStream);
                decryptedBytes = decryptedStream.ToArray(); // ��ȣȭ�� �����͸� ����Ʈ �迭�� ��ȯ�մϴ�.
            }

            // ��ȣȭ�� ����Ʈ �迭�� ������ȭ�Ͽ� ������ ������ ��ȯ�մϴ�.
            T deserializedValue = DeserializeValue<T>(decryptedBytes);
            return deserializedValue; // ��ȣȭ�� ��(T)�� ��ȯ�մϴ�.
        }
    }
    /// <summary>
    /// value���� ����ȭ�ϰ� ����Ʈ �迭�� ��ȯ(��ȣȭ)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public byte[] SerializeValue<T>(T value)
    {
        // �޸𸮿� �����͸� �����ϱ� ���� MemoryStream �ν��Ͻ� ����
        using (var ms = new System.IO.MemoryStream())
        {   // ���� ����ȭ�ϱ� ���� BinaryFormatter �ν��Ͻ� ����
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            // MemoryStream�� ��(value)�� ����ȭ�Ͽ� ����
            formatter.Serialize(ms, value);
            // MemoryStream�� ����Ʈ �迭�� ��ȯ�Ͽ� ��ȯ
            return ms.ToArray();
        }
    }
    /// <summary>
    /// �־��� ����Ʈ �迭�� ������ȭ �ϰ� ���� ������ �ǵ���(��ȣȭ)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public T DeserializeValue<T>(byte[] bytes)
    {    // �־��� ����Ʈ �迭(bytes)�� �б� ���� MemoryStream �ν��Ͻ� ����
        using (var ms = new System.IO.MemoryStream(bytes))
        {   // ������ȭ�� ���� BinaryFormatter �ν��Ͻ� ����
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            // MemoryStream���� ����Ʈ �迭(bytes)�� ������ȭ�Ͽ� ������ ������ ��ȯ
            return (T)formatter.Deserialize(ms);
        }
    }

    public void Remove(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public void RemoveAll()
    {
        PlayerPrefs.DeleteAll();
    }
}