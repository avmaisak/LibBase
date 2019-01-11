using System;

namespace LibBase.Utility
{
	public class NumericUtils
	{
		/// <summary>
		/// Получить процент от большего к меньшему числу
		/// </summary>
		/// <param name="total"></param>
		/// <param name="need"></param>
		/// <param name="roundLimit"></param>
		/// <returns></returns>
		public static decimal GetPercentage(int total, int need, int roundLimit = 2) {
			var defaultReturnValue = Math.Round(Convert.ToDecimal(0), roundLimit);

			if (total > 0 || (total < need)) {
				return Math.Round(
					total > 0 ?
						Convert.ToDecimal(Convert.ToDecimal(need) /
						                  Convert.ToDecimal(total) * Convert.ToDecimal(100.00)
						) : Convert.ToDecimal(0), 2);
			}

			return defaultReturnValue;
		}
		public static decimal GetPercentage(decimal total, decimal need, int roundLimit = 2) {
			var defaultReturnValue = Math.Round(Convert.ToDecimal(0), roundLimit);

			if (total > 0 || (total < need)) {
				return Math.Round(
					total > 0 ?
						Convert.ToDecimal(Convert.ToDecimal(need) /
						                  Convert.ToDecimal(total) * Convert.ToDecimal(100.00)
						) : Convert.ToDecimal(0), roundLimit);
			}

			return defaultReturnValue;
		}
	}
}
