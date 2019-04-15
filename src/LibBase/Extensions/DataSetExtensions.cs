using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LibBase.Extensions
{
	public static class DataSetExtensions
	{
		public static List<List<string>> ToStringArray(this DataSet ds, int tableIndex = 0) {

			return (
				from DataRow dataRow in ds.Tables[tableIndex].Rows 
				select dataRow.ItemArray 
				into columnArray 
				select columnArray.Select(t => t?.ToString().Trim()).ToList()
			).ToList();
		}
	}
}
