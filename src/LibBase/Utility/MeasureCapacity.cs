using System;

namespace LibBase.Utility {
	/// <summary>
	/// Мера объема
	/// </summary>
	public static class MeasureCapacity {

		private const long OneKb = 1024;
		private const long OneMb = OneKb * 1024;
		private const long OneGb = OneMb * 1024;
		private const long OneTb = OneGb * 1024;

		public static string ToPrettySize(this int value, int decimalPlaces = 0) {
			return ((long)value).ToPrettySize(decimalPlaces);
		}
		private static double TryConvert(long value, long parameter, int decimalPlaces = 0) {
			return Math.Round((double)value / parameter, decimalPlaces);
		}
		public static string ToPrettySize(this long value, int decimalPlaces = 0) {
			var asTb = TryConvert(value, OneTb, decimalPlaces);

			var asGb = TryConvert(value, OneGb, decimalPlaces);
			var asMb = TryConvert(value, OneMb, decimalPlaces);
			var asKb = TryConvert(value, OneKb, decimalPlaces);

			var chosenValue = asTb > 1
				? $"{asTb} Tb"
				: asGb > 1
					? $"{asGb} Gb"
					: asMb > 1
						? $"{asMb} Mb"
						: asKb > 1
							? $"{asKb} Kb"
							: $"{Math.Round((double)value, decimalPlaces)} B";
			return chosenValue;
		}

		public static double ConvertKilobytesToMegabytes(int kilobytes) {
			return kilobytes / 1024f;
		}
		public static double ConvertBytesToKilobytes(int bytes) {
			return bytes / 1024f;
		}
		public static double ConvertBytesToMegabytes(long bytes) {
			return (bytes / 1024f) / 1024f;
		}
		public static double ConvertKilobytesToMegabytes(long kilobytes) {
			return kilobytes / 1024f;
		}
	}
}