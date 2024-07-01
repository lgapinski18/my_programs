using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Rijndael
{
    public static byte[] Encrypt(string original, string key) //key has 32chars
    {
        byte[] encrypted = { };

        try
        {                                      //1234567890123456
            byte[] IV = Encoding.ASCII.GetBytes("ZAQWSXCDE123!@#]");
            byte[] Key = Encoding.ASCII.GetBytes(key);

            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                myRijndael.Key = Key;
                myRijndael.IV = IV;

                encrypted = EncryptStringToBytes(original, myRijndael.Key, myRijndael.IV);
            }
        }
        catch (Exception e)
        {
            Debug.LogFormat("Error: {0}", e.Message);
        }

        return encrypted;
    }

    public static string Decrypt(byte[] cipher, string key)
    {
        string decrypted = "";

        try
        {                                      //1234567890123456
            byte[] IV = Encoding.ASCII.GetBytes("ZAQWSXCDE123!@#]");
            byte[] Key = Encoding.ASCII.GetBytes(key);

            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                myRijndael.Key = Key;
                myRijndael.IV = IV;

                decrypted = DecryptStringFromBytes(cipher, myRijndael.Key, myRijndael.IV);
            }
        }
        catch (Exception e)
        {
            Debug.LogFormat("Error: {0}", e.Message);
        }

        return decrypted;
    }

    private static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        byte[] encrypted;
        // Create an RijndaelManaged object
        // with the specified key and IV.
        using (RijndaelManaged rijAlg = new RijndaelManaged())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    private static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext = null;

        // Create an RijndaelManaged object
        // with the specified key and IV.
        using (RijndaelManaged rijAlg = new RijndaelManaged())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }
}
