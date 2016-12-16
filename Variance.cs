using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variance
{
    public class Animal
    {
        public string Name { get; set; }
    }

    public class Cat : Animal
    {
        public Cat(string name)
        {
            Name = name;
        }
    }

    public class Fish : Animal
    { }

    public class Random
    { }

    public class AnimalSizeComparator : IComparer<Animal>
    {
        public int Compare(Animal x, Animal y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
