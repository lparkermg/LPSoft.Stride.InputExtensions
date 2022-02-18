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

        [TestCase("")]
        [TestCase("        ")]
        public void FromDictionary_GivenDictionaryWithEmptyOrWhitespaceKeys_ThrowsArgumentException(string keyValue)
        {
            var dictionary = new Dictionary<string, VirtualButton>
            {
                { keyValue, VirtualButton.GamePad.Back }
            };
            Assert.That(() => _builder.FromDictionary(dictionary), Throws.ArgumentException.With.Message.EqualTo("A dictionary cannot have empty or whitespace keys."));
        }

        [TestCase("")]
        [TestCase("        ")]
        public void FromDictionary_GivenMultipleEntryDictionaryWithEmptyOrWhitespaceKey_ThrowsArgumentException(string keyValue)
        {
            var dictionary = new Dictionary<string, VirtualButton>
            {
                { "Test", VirtualButton.GamePad.B },
                { keyValue, VirtualButton.GamePad.Back }
            };
            Assert.That(() => _builder.FromDictionary(dictionary), Throws.ArgumentException.With.Message.EqualTo("A dictionary cannot have empty or whitespace keys."));
        }

        [Test]
        public void FromDictionary_GivenNullValue_ThrowsArgumentException()
        {
            var dictionary = new Dictionary<string, VirtualButton>
            {
                { "Test", null }
            };
            Assert.That(() => _builder.FromDictionary(dictionary), Throws.ArgumentException.With.Message.EqualTo("A dictionary cannot have a null value."));
        }
    }
}