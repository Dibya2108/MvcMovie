using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FourthWebApp.Utils
{
    public static class Encryption
    {
        private static string _securityKey = "SpmNew";

        public static string EncryptLogin(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);


            // Get the key from config file


            string key = _securityKey; // (string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = Encoding.UTF8.GetBytes(key);

            var tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
                cTransform.TransformFinalBlock(toEncryptArray, 0,
                    toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);


            // Get the key from config file


            string key = _securityKey; // (string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = Encoding.UTF8.GetBytes(key);

            var tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
                cTransform.TransformFinalBlock(toEncryptArray, 0,
                                               toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return HttpUtility.UrlEncode(Convert.ToBase64String(resultArray, 0, resultArray.Length)).Replace("%", "8PRE9");

        }


        public static string Decrypt(string cipherString, bool useHashing)
        {
            try
            {
                cipherString = HttpUtility.UrlDecode(cipherString.Replace("8PRE9", "%"));

                byte[] keyArray;
                //get the byte code of the string

                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                var settingsReader =
                    new AppSettingsReader();
                //Get your key from config file to open the lock!
                string key = _securityKey; //(string)settingsReader.GetValue("SecurityKey",typeof (String));

                if (useHashing)
                {
                    //if hashing was used get the hash code with regards to your key
                    var hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                    //release any resource held by the MD5CryptoServiceProvider

                    hashmd5.Clear();
                }
                else
                {
                    //if hashing was not implemented get the byte code of the key
                    keyArray = Encoding.UTF8.GetBytes(key);
                }

                var tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)

                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                    toEncryptArray, 0, toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor                
                tdes.Clear();
                //return the Clear decrypted TEXT
                return Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                return "InvalidXX";
            }
        }

        private static string Encrypt(string key, string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);


            // Get the key from config file

            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = Encoding.UTF8.GetBytes(key);

            var tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
                cTransform.TransformFinalBlock(toEncryptArray, 0,
                                               toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format


            return HttpUtility.UrlEncode(Convert.ToBase64String(resultArray, 0, resultArray.Length)).Replace("%", "8PRE9");
        }


        private static string Decrypt(string key, string cipherString, bool useHashing)
        {
            try
            {
                cipherString = HttpUtility.UrlDecode(cipherString.Replace("8PRE9", "%"));
                byte[] keyArray;
                //get the byte code of the string

                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                var settingsReader =
                    new AppSettingsReader();
                //Get your key from config file to open the lock!
                if (useHashing)
                {
                    //if hashing was used get the hash code with regards to your key
                    var hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                    //release any resource held by the MD5CryptoServiceProvider

                    hashmd5.Clear();
                }
                else
                {
                    //if hashing was not implemented get the byte code of the key
                    keyArray = Encoding.UTF8.GetBytes(key);
                }

                var tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)

                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                    toEncryptArray, 0, toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor                
                tdes.Clear();
                //return the Clear decrypted TEXT
                return Encoding.UTF8.GetString(resultArray); ;
            }
            catch
            {
                return "InvalidXX";
            }
        }

        public static string Encrypt(this String str, string key) //String Extension
        {
            if (str == "")
            {
                return "";
            }
            return Encrypt(key, str, true);
        }

        public static string Encrypt(this int str, string key)  //Integer Extension
        {


            return Encrypt(key, str.ToString(), true);
        }

        public static int Decrypt(this String str, string key)
        {
            if (str == "")
            {
                return 0;
            }

            try
            {
                int a = int.Parse(Decrypt(key, str, true));
            }
            catch (Exception e)
            {

            }


            return int.Parse(Decrypt(key, str, true));
        }
    }
}