using Cake.Example.Animals;
using Xunit;

namespace Cake.Example.Tests.AnimalsTests
{
    public sealed class CatTest
    {
        [Fact]
        public void should_return_喵喵()
        {
            IAnimal animal = new Cat();

            var result = animal.Talk();

            Assert.Equal("喵喵", result);
        }
    }
}