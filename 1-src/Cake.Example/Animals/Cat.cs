namespace Cake.Example.Animals
{
    /// <summary>
    /// 🐱 
    /// </summary>
    public sealed class Cat : IAnimal
    {
        /// <summary>
        /// 喵喵
        /// </summary>
        /// <returns></returns>
        public string Talk()
        {
            return "喵喵";
        }
    }
}