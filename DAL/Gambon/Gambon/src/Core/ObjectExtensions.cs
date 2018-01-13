using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;

namespace Gambon.Core
{
	public static class ObjectExtensions
	{
        public static dynamic ToDynamic(this object thing)
        {
			if (thing is ExpandoObject)
				return thing; //shouldn't have to... but just in case
            var expando = new ExpandoObject();
            var result = expando as IDictionary<string, object>; //work with the Expando as a Dictionary
            if (thing.GetType() == typeof(NameValueCollection) || 
                thing.GetType().IsSubclassOf(typeof(NameValueCollection))) {
                var nameValueCollection = (NameValueCollection)thing;
				nameValueCollection.Cast<string>()
                	.Select(key => new KeyValuePair<string, object>(key, nameValueCollection[key]))
                	.ToList()
                	.ForEach(result.Add);
                return result;
            }
            if (typeof(IDictionary).IsAssignableFrom(thing.GetType()))
			{
                var nameValueCollection = (Dictionary <string, object>)thing;
                nameValueCollection
                    .ToList()
					.ForEach(result.Add);
				return result;
			}
			var properties = thing.GetType().GetProperties();
                foreach (var property in properties) {
                    result.Add(property.Name, property.GetValue(thing, null));
                }
            return result;
        }
		
		public static IDictionary<string, object> ToDictionary(this object thing) {
            if(thing == null){
                return null;
            }
            return (IDictionary<string, object>)thing.ToDynamic();
        }
	}
}
