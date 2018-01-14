using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;

namespace Gambon.SqlServer
{
	public static class SqlDataReaderExensions
	{
		public static dynamic ToDynamic(this SqlDataReader dataReader) {
			dynamic e = new ExpandoObject();
			var result = e as IDictionary<string, object>;
			var values = new object[dataReader.FieldCount];
			dataReader.GetValues(values);
			for(var i = 0; i < values.Length; i++)
			{
				var value = values[i];
				result.Add(dataReader.GetName(i), DBNull.Value.Equals(value) ? null : value);
			}
			return result as dynamic;
		}
	}
}
