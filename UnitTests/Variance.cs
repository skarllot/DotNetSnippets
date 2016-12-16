using NUnit.Framework;
using System;
using System.Collections.Generic;
using Variance;

namespace UnitTests
{
    [TestFixture(Category = "Variance")]
    public class Variance
    {
        void PrintAnimals(IEnumerable<Animal> animals, System.IO.TextWriter writer)
        {
            foreach (var animal in animals)
                writer.WriteLine(animal.Name);
        }

        void CompareCats(IComparer<Cat> comparer, System.IO.TextWriter writer)
        {
            var cat1 = new Cat("Otto");
            var cat2 = new Cat("Troublemaker");

            if (comparer.Compare(cat2, cat1) > 0)
                writer.WriteLine("Troublemaker wins!");
        }

        void CallWithPrinter(Action<Action<Animal>> animalGenerator, System.IO.TextWriter writer)
        {
            animalGenerator(animal => writer.WriteLine(animal.Name));
        }

        [Test]
        public void Covariance()
        {
            var writer = new System.IO.StringWriter();
            IEnumerable<Cat> cats = new List<Cat> { new Cat("Troublemaker") };
            PrintAnimals(cats, writer);
            Assert.AreEqual("Troublemaker" + Environment.NewLine, writer.ToString());
        }

        [Test]
        public void Contravariance()
        {
            var writer = new System.IO.StringWriter();
            IComparer<Animal> compareAnimals = new AnimalSizeComparator();
            CompareCats(compareAnimals, writer);
            Assert.AreEqual("Troublemaker wins!" + Environment.NewLine, writer.ToString());
        }

        [Test]
        public void VarianceRevert()
        {
            var writer = new System.IO.StringWriter();
            Action<Action<Cat>> catGenerator = handler => handler(new Cat("Otto"));
            CallWithPrinter(catGenerator, writer);
            Assert.AreEqual("Otto" + Environment.NewLine, writer.ToString());
        }
    }
}
