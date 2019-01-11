using System;

namespace LibBase.Utility {
	/// <summary>
	/// Генератор случайных значений
	/// </summary>
	public static class RandomGenerator {
		/// <summary>
		/// Случайная строка
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static string RandomString(int size = 32) {
			var guid = Guid.NewGuid();
			var guidStr = guid.ToString().Replace("-", "").Trim();
			return size > 32 ? string.Concat(guidStr, RandomString(size - 32)) : guidStr.Substring(0, size);
		}
		public static long RandomInt(int size = 16) {
			var r = string.Empty;
			var rand = new Random();

			if (size <= 0) {
				size = 1;
			}
			if (size > 16) {
				size = 16;
			}

			for (var i = 0; i < size; i++) {
				r += rand.Next(9);
			}

			return Convert.ToInt64(r);
		}
	}
}