using NUnit.Framework;
using Stride.Input;
using System.Collections.Generic;

namespace BuildInputConfig.Tests
{
    public class BuildInputConfigTests
    {
        private InputBuilder _builder;

        [SetUp]
        public void SetUp()
        {
            _builder = new InputBuilder();
        }

        [Test]
        public void Build_ReturnsEmptyVirtualButtonConfig()
        {
            var result = _builder.Build();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void FromDictionary_GivenNull_ThrowsArgumentException()
        {
            Assert.That(() => _builder.FromDictionary(null), Throws.ArgumentException.With.Message.EqualTo("A dictionary must be provided."));
        }

        [Test]
        public void FromDictionary_GivenAnEmptyDictionary_ThrowsArgumentException()
        {
            Assert.That(() => _builder.FromDictionary(new Dictionary<string, VirtualButton>()), Throws.ArgumentException.With.Message.EqualTo("A dictionary must not be empty."));
        }
    }
}