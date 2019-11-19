using System;
using System.Collections.Generic;
using System.Globalization;


namespace LibBase.Utility
{
	public static class DateUtils
	{
		public static int GetIso8601WeekOfYear(DateTime time)
		{
			// Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
			// be the same week# as whatever Thursday, Friday or Saturday are,
			// and we always get those right
			var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
			if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) time = time.AddDays(3);

			// Return the week of our adjusted day
			return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
		}

		/// <summary>
		/// Returns the first day of the week that the specified date 
		/// is in. 
		/// </summary>
		public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
		{
			var firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
			var firstDayInWeek = dayInWeek.Date;
			while (firstDayInWeek.DayOfWeek != firstDay)
				firstDayInWeek = firstDayInWeek.AddDays(-1);

			return firstDayInWeek;
		}
		public static DateTime FirstDateOfWeekIso8601(int year, int weekOfYear)
		{
			var jan1 = new DateTime(year, 1, 1);
			var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

			var firstThursday = jan1.AddDays(daysOffset);
			var cal = CultureInfo.CurrentCulture.Calendar;
			var firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

			var weekNum = weekOfYear;
			if (firstWeek <= 1) weekNum -= 1;
			var result = firstThursday.AddDays(weekNum * 7);
			return result.AddDays(-3);
		}
		public static int GetWeek(DateTime date)
		{
			var ciCurr = CultureInfo.CurrentCulture;
			var weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
			return weekNum;
		}
		public static int TotalWeeksInYear => WeeksInYear();
		private static int WeeksInYear(int? year = null)
		{
			if (year == null)
			{
				year = DateTime.Now.Year;
			}

			var currentDate = new DateTime(Convert.ToInt32(year), 12, 31);
			var ciCurr = CultureInfo.CurrentCulture;
			var weekNum = ciCurr.Calendar.GetWeekOfYear(currentDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
			return weekNum;
		}
		public static int WeekNumber(this DateTime? dateTime)
		{
			return GetWeek(Convert.ToDateTime(dateTime));
		}
		
		public static DateTime FirstDateOfWeek(int year, int weekOfYear)
		{
			var jan1 = new DateTime(year, 1, 1);
			var daysOffset = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
			var firstMonday = jan1.AddDays(daysOffset);
			var firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

			if (firstWeek <= 1)
			{
				weekOfYear -= 1;
			}

			return firstMonday.AddDays(weekOfYear * 7);
		}
		public static DateTime EndOfWeek(DateTime dateTime)
		{
			var start = StartOfWeek(dateTime);
			return start.AddDays(6);
		}
		public static DateTime StartOfWeek(DateTime dateTime)
		{
			var days = dateTime.DayOfWeek - DayOfWeek.Monday;

			if (days < 0)
				days += 7;

			return dateTime.AddDays(-1 * days).Date;
		}
		public static List<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
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
		public static string MonthName(int number)
		{
			return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(number);
		}
		/// <summary>
		/// Последний день месяца
		/// </summary>
		/// <returns></returns>
		public static DateTime LastMonthDay()
		{
			var d = DateTime.Now;
			var daysInMonth = DateTime.DaysInMonth(d.Year, d.Month);
			return new DateTime(d.Year, d.Month, daysInMonth);
		}
		public static List<DateTime> MonthsList(int year)
		{
			var res = new List<DateTime>();
			for (var i = 1; i < 13; i++)
			{
				res.Add(new DateTime(year, i, 1));
			}

			return res;
		}
	}
}