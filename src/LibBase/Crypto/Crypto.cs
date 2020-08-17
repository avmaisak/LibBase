using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibBase.Crypto
{
	public static class Crypto
	{
		public static async Task<string> EncryptAsync(this string plainText, string key)
		{
			var iv = new byte[16];

			using var aes = Aes.Create();
			if (aes == null) throw new Exception("Aes not created");

			aes.Key = Encoding.UTF8.GetBytes(key);
			aes.IV = iv;

			var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

			await using var memoryStream = new MemoryStream();
			await using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
			await using (var streamWriter = new StreamWriter(cryptoStream)) await streamWriter.WriteAsync(plainText);

			var array = memoryStream.ToArray();

			return Convert.ToBase64String(array);
		}
		public static async Task<string> DecryptAsync(this string cipherText, string key)
		{
			var iv = new byte[16];
			var buffer = Convert.FromBase64String(cipherText);

			using var aes = Aes.Create();
			if (aes == null) throw new Exception("Aes not created");
			aes.Key = Encoding.UTF8.GetBytes(key);
			aes.IV = iv;
			var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

			await using var memoryStream = new MemoryStream(buffer);
			await using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			using var streamReader = new StreamReader(cryptoStream);

			return await streamReader.ReadToEndAsync();
		}
	}
}
