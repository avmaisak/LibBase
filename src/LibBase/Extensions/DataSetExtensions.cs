using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LibBase.Extensions
{
	public static class DataSetExtensions
	{
		public static List<List<string>> ToStringArray(this DataSet ds) {

			return (
					from DataRow dataRow in ds.Tables[0].Rows 
					select dataRow.ItemArray 
					into columnArray 
						select 
							columnArray.Select(
									t => t.ToString().Trim().ToLower()
							).ToList()
				).ToList();
		}
	}
}
