﻿using Gambon.Core;
using NUnit.Framework;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace GambonUnitTest.Core
{
    [TestFixture]
	public class ObjectExtensionsTests
	{
		[Test]
		public void ReturnsNullWhenIsNull()
		{
			object thing = null;
			
			var result = this.ToDynamic();
			
			Assert.IsNull(thing);
		}

        [Test]
        public void ReturnsDynamicWhereIsNameValueCollection()
        {
            var values = new NameValueCollection();
            values.Add("keyOne", "valueOne");
            values.Add("keyTwo", "valueTwo");
            values.Add("keyThree", "valueThree");
            values.Add("keyFour", "valueFour");

            var result = values.ToDynamic();

            Assert.AreEqual("valueThree", result.keyThree);
        }

		[Test]
		public void ReturnsDynamicWhereIsDictionary()
		{
			var values = new Dictionary<string, string>();
			values.Add("keyOne", "valueOne");
			values.Add("keyTwo", "valueTwo");
			values.Add("keyThree", "valueThree");
			values.Add("keyFour", "valueFour");

			var result = values.ToDynamic();

			Assert.AreEqual("valueTwo", result.keyTwo);
		}
    }
}