using System;
using System.Collections.Generic;
using System.Globalization;

namespace LibBase.Extensions
{
	public static class DateTimeExtensions
	{
		public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
		{
			int diff = dt.DayOfWeek - startOfWeek;
			if (diff < 0)
			{
				diff += 7;
			}
			return dt.AddDays(-1 * diff).Date;
		}
		public static int GetIso8601WeekOfYear(this DateTime time)
		{
			// Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
			// be the same week# as whatever Thursday, Friday or Saturday are,
			// and we always get those right
			var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
			if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
			{
				time = time.AddDays(3);
			}

			// Return the week of our adjusted day
			return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
		}
		/// <summary>
		/// Returns the first day of the week that the specified date 
		/// is in. 
		/// </summary>
		public static DateTime GetFirstDayOfWeek(this DateTime dayInWeek, CultureInfo cultureInfo ) {
			var firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
			var firstDayInWeek = dayInWeek.Date;
			while (firstDayInWeek.DayOfWeek != firstDay)
				firstDayInWeek = firstDayInWeek.AddDays(-1);

			return firstDayInWeek;
		}
		public static int GetWeekNumber(this DateTime date)
		{
			var ciCurr = CultureInfo.CurrentCulture;
			var weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
			return weekNum;
		}
		public static DateTime StartOfWeek(this DateTime dateTime)
		{
			var days = dateTime.DayOfWeek - DayOfWeek.Monday;

			if (days < 0)
				days += 7;

			return dateTime.AddDays(-1 * days).Date;
		}
		public static List<DateTime> DatesBetween(DateTime startDate, DateTime endDate)
		{
			var allDates = new List<DateTime>();
			for (var date = startDate; date <= endDate; date = date.AddDays(1))
				allDates.Add(date);
			return allDates;
		}
		public static List<DateTime> EachDay(DateTime from, DateTime to, int frequencyDays = 1)
		{
			var r = new List<DateTime>();
			for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(frequencyDays))
			{
				r.Add(day);
			}
			return r;
		}
		public static decimal TotalHours(DateTime from, DateTime to)
		{
			var r = (to - from).TotalHours;
			return Convert.ToDecimal(r);
		}
		public static DateTime FirstDayOfMonth(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, dateTime.Month, 1);
		}
	}


}