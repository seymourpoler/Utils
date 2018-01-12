using System.Collections;
using System.Dynamic;
using System.Collections.Generic;

namespace Gambon.Core
{
    public static class DictionaryExtensions
    {
        public static dynamic ToDynamic(IDictionary dictionary)
        {
			IDictionary<string, object> result = new ExpandoObject() as IDictionary<string, object>;
            foreach (KeyValuePair<string, object> item in dictionary)
			{
                result.Add(item);
			}
            return result;
        }
    }
}
