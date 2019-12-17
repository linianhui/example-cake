using Cake.Example.Animals;
using Xunit;

namespace Cake.Example.Tests.AnimalsTests
{
    public sealed class DotTest
    {
        [Fact]
        public void should_return_汪汪()
        {
            IAnimal animal = new Dog();

            var result = animal.Talk();

            Assert.Equal("汪汪", result);
        }
    }
}
