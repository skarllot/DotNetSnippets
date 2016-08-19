using System;
using System.Linq;

namespace InheritableStatic
{
	public class ObjectAttribute : Attribute
	{
		public string Name { get; set; }

		public ObjectAttribute (string name)
		{
			Name = name;
		}
	}

	public abstract class BaseObject
	{
		public static string GetName<T> () where T : BaseObject
		{
			return (from objAttr in typeof(T).GetCustomAttributes (typeof(ObjectAttribute), false)
			       let attr = (ObjectAttribute)objAttr
			       select attr.Name).FirstOrDefault ();
		}
	}

	[ObjectAttribute ("A")]
	public class AObject : BaseObject
	{
		public string Code { get; set; }

		public int Height { get; set; }
	}

	[ObjectAttribute ("B")]
	public class BObject : BaseObject
	{
		public string Code { get; set; }

		public int Weight { get; set; }
	}
}
