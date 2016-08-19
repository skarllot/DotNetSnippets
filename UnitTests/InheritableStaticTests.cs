using NUnit.Framework;
using System;
using InheritableStatic;

namespace UnitTests
{
	[TestFixture ()]
	public class InheritableStaticTests
	{
		[Test ()]
		public void BaseObjectNameTest ()
		{
			Assert.AreEqual (null, BaseObject.GetName<BaseObject>());
		}

		[Test ()]
		public void AObjectNameTest ()
		{
			Assert.AreEqual ("A", BaseObject.GetName<AObject>());
		}

		[Test ()]
		public void BObjectNameTest ()
		{
			Assert.AreEqual ("B", BaseObject.GetName<BObject>());
		}
	}
}

