using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PassKeeper
{
	public class MyAes
	{
		private static readonly byte[] aesKey = {8, 242, 255, 17, 85, 134, 108, 74, 63, 126, 253, 19, 63, 51, 192, 127};
		private static readonly byte[] aesIV = {255, 41, 73, 63, 128, 185, 71, 195, 84, 63, 2, 92, 141, 98, 226, 254};

		public static byte[] EncryptStringToBytes(string toEncrypt)
		{
			byte[] encrypted;

			using (Aes aes = Aes.Create())
			{
				aes.Key = aesKey;
				aes.IV = aesIV;
				ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter sw = new StreamWriter(cs))
						{
							sw.Write(toEncrypt);
						}
						encrypted = ms.ToArray();
					}
				}
			}

			return encrypted;
		}

		public static string EncryptStringToString(string toEncrypt)
		{
			byte[] encryptedBytes;
			string encryptedString = null;

			using (Aes aes = Aes.Create())
			{
				aes.Key = aesKey;
				aes.IV = aesIV;
				ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter sw = new StreamWriter(cs))
						{
							sw.Write(toEncrypt);
						}
						encryptedBytes = ms.ToArray();
					}
				}
			}

			//encryptedString = BitConverter.ToString(encryptedBytes);
			encryptedString = Convert.ToBase64String(encryptedBytes);
			return encryptedString;
		}

		public static string DecryptBytesToString(byte[] toDecrypt)
		{
			string decrypted = null;

			using (Aes aes = Aes.Create())
			{
				aes.Key = aesKey;
				aes.IV = aesIV;
				ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

				using (MemoryStream ms = new MemoryStream(toDecrypt))
				{
					using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader sr = new StreamReader(cs))
						{
							decrypted = sr.ReadToEnd();
						}
					}
				}
			}

			return decrypted;
		}

		public static string DecryptStringToString(string toDecrypt)
		{
			byte[] toDecryptBytes = Convert.FromBase64String(toDecrypt);
			string decrypted = null;

			using (Aes aes = Aes.Create())
			{
				aes.Key = aesKey;
				aes.IV = aesIV;
				ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

				using (MemoryStream ms = new MemoryStream(toDecryptBytes))
				{
					using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader sr = new StreamReader(cs))
						{
							decrypted = sr.ReadToEnd();
						}
					}
				}
			}

			return decrypted;
		}
	}
}
