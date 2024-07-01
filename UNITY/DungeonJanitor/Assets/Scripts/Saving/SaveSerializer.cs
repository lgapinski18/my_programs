using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSerializer
{               //12345678901234567890123456789012
    private static string key = "Anoo2p35;[adqop12n4;na00-1u9nuas";

    public static void SaveToFile(string filename, SaveData data)
    {
        string json = JsonUtility.ToJson(data);

        string filepath = Path.Combine(Application.persistentDataPath, filename);


        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }

        File.WriteAllBytes(filepath, Rijndael.Encrypt(json, key));
        //File.WriteAllText(filepath, json);
    }

    public static SaveData LoadFromFile(string filename)
    {
        string filepath = Path.Combine(Application.persistentDataPath, filename);

        byte[] cipher = File.ReadAllBytes(filepath);

        string jsonFromFile = Rijndael.Decrypt(cipher, key);

        SaveData data = JsonUtility.FromJson<SaveData>(jsonFromFile);

        return data;
    }
}
