using System;
using Gambon.Core;
using NUnit.Framework;

namespace GambonUnitTest.Core
{
	/// <summary>
	/// Description of ObjectExtensionsTests.
	/// </summary>
	[TestFixture]
	public class ObjectExtensionsTests
	{
		[SetUp]
		public void SetUp()
		{}
		
		[Test]
		public void ReturnsNullWhenIsNull()
		{
			object thing = null;
			
			var result = this.ToDynamic();
			
			Assert.IsNull(thing);
		}
	}
}
