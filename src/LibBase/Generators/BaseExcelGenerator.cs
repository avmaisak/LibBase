using System;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace LibBase.Generators
{
	/// <summary>
	/// https://github.com/JanKallman/EPPlus
	/// </summary>
	public class BaseExcelGenerator: IDisposable
	{
		/// <summary>
		/// Книга Excel.
		/// </summary>
		protected ExcelPackage Excel;
		/// <summary>
		/// Страница Excel.
		/// </summary>
		private ExcelWorksheet _sheet;
		/// <summary>
		/// Первая значимая строка, обычно с нее начинается таблица.
		/// Используется для нанесения сетки.
		/// </summary>
		private int _firstRowNumber = 1;
		/// <summary>
		/// Количество столбцов в таблице.
		/// </summary>
		private int _columnsTotal;
		public BaseExcelGenerator() => CreateWorkBook();
		/// <summary>
		/// Content type for excel files.
		/// </summary>
		public string ContentType { get; } = "application/vnd.ms-excel";
		/// <summary>
		/// Имя файла по умолчанию.
		/// </summary>
		public string ExcelFileName { get; set; } = "report.xlsx";
		/// <summary>
		/// Страница Excel. Возвращает текущую страницу.
		/// </summary>
		public ExcelWorksheet Sheet => _sheet ??= Excel.Workbook.Worksheets.Add("Таблица");
		/// <summary>
		/// Текущая позиция курсора - строка.
		/// </summary>
		public int Row { get; set; } = 1;
		/// <summary>
		/// Текущая позиция курсора - столбец.
		/// </summary>
		public int Col { get; set; } = 1;
		/// <summary>
		/// Добавить страницу в рабочую книгу. Устанавливает ее в качестве текущей.
		/// </summary>
		/// <param name="name">Имя страницы.</param>
		/// <returns>Объект страницы.</returns>
		public ExcelWorksheet AddWorksheet(string name)
		{
			Row = 1;
			Col = 1;
			_sheet = Excel.Workbook.Worksheets.Add(name);
			return _sheet;
		}
		/// <summary>
		/// Добавить заголовок столбца в таблицу.
		/// </summary>
		/// <param name="name">Названия столбца.</param>
		/// <param name="width">Ширина столбца.</param>
		public void AddColumnTitle(string name, int width)
		{
			_firstRowNumber = Math.Max(_firstRowNumber, Row);
			_columnsTotal = Math.Max(_columnsTotal, Col);

			Sheet.Cells[Row, Col].Value = name;
			Sheet.Column(Col).Width = width;
			Sheet.Cells[Row, Col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
			Sheet.Cells[Row, Col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
			Sheet.Cells[Row, Col].Style.Font.Bold = true;
			Sheet.Cells[Row, Col].Style.Fill.PatternType = ExcelFillStyle.Solid;
			Sheet.Cells[Row, Col].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
			Sheet.Cells[Row, Col].Style.WrapText = true;
			Col++;
		}
		/// <summary>
		/// Нанести сетку на таблицу с данными.
		/// </summary>
		/// <param name="r1">От строки.</param>
		/// <param name="c1">От столбца.</param>
		/// <param name="r2">До строки.</param>
		/// <param name="c2">До столбца.</param>
		public void ApplyCellBorders(int r1 = 0, int c1 = 0, int r2 = 0, int c2 = 0)
		{
			if (r1 == 0) r1 = Math.Max(1, _firstRowNumber);
			if (c1 == 0) c1 = 1;
			if (r2 == 0) r2 = Math.Max(1, Row - 1);
			if (c2 == 0) c2 = Math.Max(1, _columnsTotal);
			var table = Sheet.Cells[r1, c1, r2, c2];
			table.Style.Border.Top.Style = ExcelBorderStyle.Thin;
			table.Style.Border.Left.Style = ExcelBorderStyle.Thin;
			table.Style.Border.Right.Style = ExcelBorderStyle.Thin;
			table.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
		}
		/// <summary>
		/// Инициализация новой книги.
		/// </summary>
		public void CreateWorkBook()
		{
			Excel = new ExcelPackage();
			Row = 1;
			Col = 1;
		}
		/// <summary>
		/// Получить сгенерированный Excel файл в виде потока.
		/// </summary>
		/// <returns>Поток Stream.</returns>
		public Stream GetFileStream()
		{
			var fileStream = new MemoryStream();
			Excel.SaveAs(fileStream);
			fileStream.Position = 0;
			return fileStream;
		}
		#region IDisposable implementation

		/// <inheritdoc />
		/// <summary>
		/// Implementation of Dispose pattern.
		/// </summary>
		public void Dispose() => Dispose(true);

		/// <summary>
		/// Implementation of Dispose pattern.
		/// </summary>
		/// <param name="disposing">Признак уничтожения.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing) return;
			Excel?.Dispose();
			_sheet?.Dispose();
		}

		#endregion
	}
}
