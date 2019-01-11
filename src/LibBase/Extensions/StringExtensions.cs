using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LibBase.Extensions
{
	/// <summary>
	/// String Extensions class.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Replace a string char at index with another char
		/// </summary>
		/// <param name="text">string to be replaced</param>
		/// <param name="index">position of the char to be replaced</param>
		/// <param name="c">replacement char</param>
		public static string ReplaceAtIndex(this string text, int index, char c)
		{
			var stringBuilder = new StringBuilder(text) { [index] = c };
			return stringBuilder.ToString();
		}

		/// <summary>
		/// ToKeyValuePair.
		/// </summary>
		public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePair(this string raw, string frameSeparator = " ")
		{
			//сегмент кадра. разделитель пробел
			var segments = raw.Split(Convert.ToChar(frameSeparator));
			//Перебор сегментов
			return (
				from segment in segments
				let segmentLength = segment.Length
				let segmentKey = segment.Substring(0, 1)
				let segmentValue = segment.Substring(1, segmentLength - 1)
				select new KeyValuePair<string, string>(
					segmentKey, segmentValue)
			).ToList();
		}

		/// <summary>
		/// Remove All Spaces.
		/// </summary>
		/// <returns></returns>
		public static string RemoveAllSpaces(this string text)
		{
			return text.Replace(" ", string.Empty);
		}

		/// <summary>
		/// ToSha512.
		/// </summary>
		/// <param name="inData"></param>
		/// <returns></returns>
		public static string ToSha512(this string inData)
		{
			var bytes = Encoding.UTF8.GetBytes(inData);
			using (var hash = SHA512.Create())
			{
				var hashedInputBytes = hash.ComputeHash(bytes);
				// StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
				var hashedInputStringBuilder = new StringBuilder(128);
				foreach (var b in hashedInputBytes) hashedInputStringBuilder.Append(b.ToString("X2"));
				return hashedInputStringBuilder.ToString();
			}
		}

		/// <summary>
		/// Uppercase First Letter.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string UppercaseFirst(this string s)
		{
			// Check for empty string.
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}

			s = s.Trim();
			// Return char and concat substring.
			return char.ToUpper(s[0]) + s.Substring(1);
		}

		/// <summary>
		/// UpperCaseEachWord.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string UpperCaseEachWord(this string s)
		{
			var res = string.Empty;
			if (!string.IsNullOrWhiteSpace(s))
			{
				if (s.Contains(" "))
				{
					var wordsList = s.Split(' ');
					res = wordsList.Select(w => w.UppercaseFirst()).Aggregate(res, (current, upperW) => current + $"{upperW} ");
				}
			}
			return res.Trim();
		}

		/// <summary>
		/// ToIntForce.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static int ToIntForce(this string s)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				return 0;
			}

			try
			{
				return Convert.ToInt32(s.Trim());
			}
			catch
			{
				return 0;
			}
		}

		/// <summary>
		/// IsEmail.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsEmail(this string value)
		{
			const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
			return Regex.Match(value, pattern: pattern).Success;
		}

		/// <summary>
		/// Retrieves a substring from this instance. If start index has negative value it will be replaced
		/// to 0. If substring exceed length of target string the end of string will be returned. <c>null</c> will
		/// be converted to empty string.
		/// </summary>
		/// <param name="target">Target string.</param>
		/// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
		/// <param name="length">The number of characters in the substring.</param>
		/// <returns>Substring.</returns>
		public static string SafeSubstring(string target, int startIndex, int length = 0)
		{
			if (target == null)
			{
				return string.Empty;
			}

			if (startIndex < 0)
			{
				startIndex = 0;
			}
			else if (startIndex > target.Length)
			{
				return string.Empty;
			}
			if (length == 0)
			{
				length = target.Length;
			}
			else if (startIndex + length > target.Length)
			{
				length = target.Length - startIndex;
			}
			return target.Substring(startIndex, length);
		}

		/// <summary>
		/// Returns empty string if target string is null or string itself.
		/// </summary>
		/// <param name="target">Target string.</param>
		/// <returns>Empty string if null or target string.</returns>
		public static string NullSafe(string target)
		{
			return target ?? string.Empty;
		}


		/// <summary>
		/// Tries to convert target string to Boolean. If fails returns default value.
		/// </summary>
		public static Boolean ParseOrDefault(string target, Boolean defaultValue)
		{
			Boolean result;
			var success = Boolean.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		private static readonly string[] TrueValues = { "yes", "y", "t", "1" };
		private static readonly string[] FalseValues = { "no", "n", "f", "0" };

		/// <summary>
		/// Tries to convert target string to Boolean. If fails returns default value.
		/// SetPart extended parameter to true to be able to parse from values "0", "1", "Yes", "No".
		/// </summary>
		public static Boolean ParseOrDefault(string target, Boolean defaultValue, Boolean extended)
		{
			Boolean result;
			var success = Boolean.TryParse(target, out result);
			if (extended && !success)
			{
				var trimmedTarget = target.ToLowerInvariant().Trim();
				if (Array.IndexOf(TrueValues, trimmedTarget) > -1)
				{
					success = true;
					result = true;
				}
				else if (Array.IndexOf(FalseValues, trimmedTarget) > -1)
				{
					success = true;
					result = false;
				}
			}
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Byte. If fails returns default value.
		/// </summary>
		public static Byte ParseOrDefault(string target, Byte defaultValue)
		{
			Byte result;
			var success = Byte.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Byte. If fails returns default value.
		/// </summary>
		public static Byte ParseOrDefault(string target, NumberStyles style, IFormatProvider provider, Byte defaultValue)
		{
			Byte result;
			var success = Byte.TryParse(target, style, provider, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Char. If fails returns default value.
		/// </summary>
		public static Char ParseOrDefault(string target, Char defaultValue)
		{
			Char result;
			var success = Char.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to DateTime. If fails returns default value.
		/// </summary>
		public static DateTime ParseOrDefault(string target, DateTime defaultValue)
		{
			DateTime result;
			var success = DateTime.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to DateTime. If fails returns default value.
		/// </summary>
		public static DateTime ParseOrDefault(string target, IFormatProvider provider, DateTimeStyles styles, DateTime defaultValue)
		{
			DateTime result;
			var success = DateTime.TryParse(target, provider, styles, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Decimal. If fails returns default value.
		/// </summary>
		public static Decimal ParseOrDefault(string target, Decimal defaultValue)
		{
			Decimal result;
			var success = Decimal.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Decimal. If fails returns default value.
		/// </summary>
		public static Decimal ParseOrDefault(string target, NumberStyles style, IFormatProvider provider, Decimal defaultValue)
		{
			Decimal result;
			var success = Decimal.TryParse(target, style, provider, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Double. If fails returns default value.
		/// </summary>
		public static Double ParseOrDefault(string target, Double defaultValue)
		{
			Double result;
			var success = Double.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Double. If fails returns default value.
		/// </summary>
		public static Double ParseOrDefault(string target, NumberStyles style, IFormatProvider provider, Double defaultValue)
		{
			Double result;
			var success = Double.TryParse(target, style, provider, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Int16. If fails returns default value.
		/// </summary>
		public static Int16 ParseOrDefault(string target, Int16 defaultValue)
		{
			Int16 result;
			var success = Int16.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Int16. If fails returns default value.
		/// </summary>
		public static Int16 ParseOrDefault(string target, NumberStyles style, IFormatProvider provider, Int16 defaultValue)
		{
			Int16 result;
			var success = Int16.TryParse(target, style, provider, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to int. If fails returns default value.
		/// </summary>
		public static Int32 ParseOrDefault(string target, Int32 defaultValue)
		{
			Int32 result;
			var success = Int32.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to int. If fails returns default value.
		/// </summary>
		public static Int32 ParseOrDefault(string target, NumberStyles style, IFormatProvider provider, Int32 defaultValue)
		{
			Int32 result;
			var success = Int32.TryParse(target, style, provider, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Int64. If fails returns default value.
		/// </summary>
		public static Int64 ParseOrDefault(string target, Int64 defaultValue)
		{
			Int64 result;
			var success = Int64.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Int64. If fails returns default value.
		/// </summary>
		public static Int64 ParseOrDefault(string target, NumberStyles style, IFormatProvider provider, Int64 defaultValue)
		{
			Int64 result;
			var success = Int64.TryParse(target, style, provider, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to SByte. If fails returns default value.
		/// </summary>
		public static SByte ParseOrDefault(string target, SByte defaultValue)
		{
			SByte result;
			var success = SByte.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to SByte. If fails returns default value.
		/// </summary>
		public static SByte ParseOrDefault(string target, NumberStyles style, IFormatProvider provider, SByte defaultValue)
		{
			SByte result;
			var success = SByte.TryParse(target, style, provider, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Single. If fails returns default value.
		/// </summary>
		public static Single ParseOrDefault(string target, Single defaultValue)
		{
			Single result;
			var success = Single.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Single. If fails returns default value.
		/// </summary>
		public static Double ParseOrDefault(string target, NumberStyles style, IFormatProvider provider, Single defaultValue)
		{
			Single result;
			var success = Single.TryParse(target, style, provider, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to UInt16. If fails returns default value.
		/// </summary>
		public static UInt16 ParseOrDefault(string target, UInt16 defaultValue)
		{
			UInt16 result;
			var success = UInt16.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to UInt16. If fails returns default value.
		/// </summary>
		public static UInt16 ParseOrDefault(string target, NumberStyles style, IFormatProvider provider, UInt16 defaultValue)
		{
			UInt16 result;
			var success = UInt16.TryParse(target, style, provider, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Uint. If fails returns default value.
		/// </summary>
		public static UInt32 ParseOrDefault(string target, UInt32 defaultValue)
		{
			UInt32 result;
			var success = UInt32.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Uint. If fails returns default value.
		/// </summary>
		public static UInt32 ParseOrDefault(string target, NumberStyles style, IFormatProvider provider, UInt32 defaultValue)
		{
			UInt32 result;
			var success = UInt32.TryParse(target, style, provider, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to UInt64. If fails returns default value.
		/// </summary>
		public static UInt64 ParseOrDefault(string target, UInt64 defaultValue)
		{
			UInt64 result;
			var success = UInt64.TryParse(target, out result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to UInt64. If fails returns default value.
		/// </summary>
		public static ulong ParseOrDefault(string target, NumberStyles style, IFormatProvider provider, ulong defaultValue)
		{
			var success = ulong.TryParse(target, style, provider, out var result);
			return success ? result : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Enum. If fails returns default value.
		/// </summary>
		public static T ParseOrDefault<T>(string target, T defaultValue) where T : struct
		{
			return Enum.IsDefined(typeof(T), target) ? (T)Enum.Parse(typeof(T), target, true) : defaultValue;
		}

		/// <summary>
		/// Tries to convert target string to Enum. If fails returns default value.
		/// </summary>
		public static T ParseOrDefault<T>(string target, bool ignoreCase, T defaultValue) where T : struct
		{
			return Enum.IsDefined(typeof(T), target) ? (T)Enum.Parse(typeof(T), target, ignoreCase) : defaultValue;
		}
		public static DateTime? ToDateTimeForce(this string dateTime)
		{

			try
			{
				if (string.IsNullOrWhiteSpace(dateTime))
				{
					return null;
				}

				return Convert.ToDateTime(dateTime);
			}
			catch
			{

				return null;
			}
		}
		public static string[] Split(this string str, string splitter) {
			return str.Split(new[] { splitter }, StringSplitOptions.None);
		}
		public static string TrimString(this string str) {
			var res = string.Empty;

			if (str == null) {
				return string.Empty;
			}

			if (!string.IsNullOrWhiteSpace(str)) {
				res = str.Trim();
			}

			return res;
		}
	}
}