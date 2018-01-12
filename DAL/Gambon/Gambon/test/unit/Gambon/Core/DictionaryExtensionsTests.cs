using System.Collections.Generic;
using NUnit.Framework;
using Gambon.Core;

namespace GambonUnitTest.Core
{
    [TestFixture]
    public class DictionaryExtensionsTests
    {
		[Test]
		public void ReturnsDynamicWhereIsDictionary()
		{
			var values = new Dictionary<string, object>();
			values.Add("keyOne", "valueOne");
			values.Add("keyTwo", "valueTwo");
			values.Add("keyThree", "valueThree");
			values.Add("keyFour", "valueFour");

			var result = values.ToDynamic();

			Assert.AreEqual("valueTwo", result.keyTwo);
		} 
    }
}
