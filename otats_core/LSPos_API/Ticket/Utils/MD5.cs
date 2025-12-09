using System;
using System.Security.Cryptography;
using System.Text;

namespace Ticket.Utils
{
	public class MD5
	{
		private static string key = "A13DA!%B1E$DDF2#6EDDB02E33D0!&32";

		public static string Hash(string toEncrypt)
		{
			System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
			return BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(toEncrypt))).Replace("-", string.Empty).ToLower();
		}

		public static string EncodeChecksum(string toEncode)
		{
			try
			{
				string encrypted = Encrypt(toEncode, key, useHashing: true);
				Random r = new Random();
				encrypted = encrypted.Insert(encrypted.Length - 1, char.ConvertFromUtf32(r.Next(26) + 97));
				encrypted = encrypted.Insert(8, char.ConvertFromUtf32(r.Next(26) + 97));
				return encrypted.Insert(3, char.ConvertFromUtf32(r.Next(26) + 97));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static string DecodeChecksum(string toDecode)
		{
			try
			{
				string decrypted = toDecode.Remove(toDecode.Length - 2, 1);
				decrypted = decrypted.Remove(9, 1);
				decrypted = decrypted.Remove(3, 1);
				return Decrypt(decrypted, key, useHashing: true);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static string Encrypt(string toEncrypt, string securityKey, bool useHashing)
		{
			string retVal = string.Empty;
			try
			{
				byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
				ValidateInput(toEncrypt);
				ValidateInput(securityKey);
				byte[] keyArray;
				if (useHashing)
				{
					MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
					keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(securityKey));
					hashmd5.Clear();
				}
				else
				{
					keyArray = Encoding.UTF8.GetBytes(securityKey);
				}
				TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
				tdes.Key = keyArray;
				tdes.Mode = CipherMode.ECB;
				tdes.Padding = PaddingMode.PKCS7;
				ICryptoTransform cTransform = tdes.CreateEncryptor();
				byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
				tdes.Clear();
				return Convert.ToBase64String(resultArray, 0, resultArray.Length);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static string Decrypt(string cipherString, string securityKey, bool useHashing)
		{
			string retVal = string.Empty;
			try
			{
				byte[] toEncryptArray = Convert.FromBase64String(cipherString);
				ValidateInput(cipherString);
				ValidateInput(securityKey);
				byte[] keyArray;
				if (useHashing)
				{
					MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
					keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(securityKey));
					hashmd5.Clear();
				}
				else
				{
					keyArray = Encoding.UTF8.GetBytes(securityKey);
				}
				TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
				tdes.Key = keyArray;
				tdes.Mode = CipherMode.ECB;
				tdes.Padding = PaddingMode.PKCS7;
				ICryptoTransform cTransform = tdes.CreateDecryptor();
				byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
				tdes.Clear();
				return Encoding.UTF8.GetString(resultArray);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static bool ValidateInput(string inputValue)
		{
			bool valid = !string.IsNullOrEmpty(inputValue);
			if (!valid)
			{
				throw new Exception("Input is null or empty.");
			}
			return valid;
		}

		public static string Encrypt(string text)
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;
			ICryptoTransform transform = tdes.CreateEncryptor();
			byte[] textBytes = Encoding.UTF8.GetBytes(text);
			byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
			return Convert.ToBase64String(bytes, 0, bytes.Length);
		}

		public static string Decrypt(string cipher)
		{
			using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
			{
				using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
				{
					tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
					tdes.Mode = CipherMode.ECB;
					tdes.Padding = PaddingMode.PKCS7;
					using (ICryptoTransform transform = tdes.CreateDecryptor())
					{
						byte[] cipherBytes = Convert.FromBase64String(cipher);
						byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
						return Encoding.UTF8.GetString(bytes);
					}
				}
			}
		}
	}
}
