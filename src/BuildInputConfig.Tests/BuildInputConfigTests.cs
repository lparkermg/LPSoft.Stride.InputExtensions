using NUnit.Framework;

namespace BuildInputConfig.Tests
{
    public class BuildInputConfigTests
    {
        [Test]
        public void Build_ReturnsEmptyVirtualButtonConfig()
        {
            var builder = new InputBuilder();
            var result = builder.Build();
            Assert.That(result, Is.Empty);
        }
    }
}