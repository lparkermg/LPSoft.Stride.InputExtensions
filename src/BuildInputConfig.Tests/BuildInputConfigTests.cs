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
            Assert.That(() => _builder.FromDictionary(new Dictionary<string, VirtualButton[]>()), Throws.ArgumentException.With.Message.EqualTo("A dictionary must not be empty."));
        }

        [TestCase("")]
        [TestCase("        ")]
        public void FromDictionary_GivenDictionaryWithEmptyOrWhitespaceKeys_ThrowsArgumentException(string keyValue)
        {
            var dictionary = new Dictionary<string, VirtualButton[]>
            {
                { keyValue, new[] { VirtualButton.GamePad.Back } }
            };
            Assert.That(() => _builder.FromDictionary(dictionary), Throws.ArgumentException.With.Message.EqualTo("A dictionary cannot have empty or whitespace keys."));
        }

        [TestCase("")]
        [TestCase("        ")]
        public void FromDictionary_GivenMultipleEntryDictionaryWithEmptyOrWhitespaceKey_ThrowsArgumentException(string keyValue)
        {
            var dictionary = new Dictionary<string, VirtualButton[]>
            {
                { "Test", new[] { VirtualButton.GamePad.B } },
                { keyValue, new[] { VirtualButton.GamePad.Back } }
            };
            Assert.That(() => _builder.FromDictionary(dictionary), Throws.ArgumentException.With.Message.EqualTo("A dictionary cannot have empty or whitespace keys."));
        }

        [Test]
        public void FromDictionary_GivenNullValue_ThrowsArgumentNullException()
        {
            var dictionary = new Dictionary<string, VirtualButton[]>
            {
                { "Test", null }
            };
            Assert.That(() => _builder.FromDictionary(dictionary), Throws.ArgumentNullException);
        }

        [Test]
        public void FromDictionary_GivenEntryWithNull_ThrowsArgumentException()
        {
            var dictionary = new Dictionary<string, VirtualButton[]>
            {
                { "Test", new VirtualButton[] { null } },
            };
            Assert.That(() => _builder.FromDictionary(dictionary), Throws.ArgumentException.With.Message.EqualTo("A dictionary cannot have a null value."));
        }

        [Test]
        public void FromDictionary_GivenMultipleEntryDictionaryWithNullValue_ThrowsArgumentNullException()
        {
            var dictionary = new Dictionary<string, VirtualButton[]>
            {
                { "Test", new[] { VirtualButton.GamePad.B } },
                { "Test 2", null }
            };
            Assert.That(() => _builder.FromDictionary(dictionary), Throws.ArgumentNullException);
        }

        [Test]
        public void FromDictionary_GivenMultpleEntriesWithNull_ThrowsArgumentException()
        {
            var dictionary = new Dictionary<string, VirtualButton[]>
            {
                { "Test", new[] { VirtualButton.GamePad.B } },
                { "Test2", new[] { VirtualButton.GamePad.Back, null, VirtualButton.Keyboard.Back } }
            };
            Assert.That(() => _builder.FromDictionary(dictionary), Throws.ArgumentException.With.Message.EqualTo("A dictionary cannot have a null value."));
        }

        [Test]
        public void Build_GivenFromDictionary_ReturnsPopulatedConfig()
        {
            var dictionary = new Dictionary<string, VirtualButton[]>
            {
                {"TestUp", new[] { VirtualButton.GamePad.PadUp } },
                {"TestDown", new[] { VirtualButton.GamePad.PadDown } }
            };
            _builder.FromDictionary(dictionary);
            var result = _builder.Build();
            Assert.That(result, Has.Exactly(2).Items);

            Assert.Multiple(() =>
            {
                var up = result[0];
                Assert.That(up.Name.ToString(), Is.EqualTo("TestUp"));
                Assert.That(((VirtualButton)up.Button).Name, Is.EqualTo(VirtualButton.GamePad.PadUp.Name));
                
                var down = result[1];
                Assert.That(down.Name.ToString(), Is.EqualTo("TestDown"));
                Assert.That(((VirtualButton)down.Button).Name, Is.EqualTo(VirtualButton.GamePad.PadDown.Name));
            });
        }

        [Test]
        public void Build_GivenFromDictionaryWithMultipleEntriesForAKey_ReturnsPopulatedConfig()
        {
            var dictionary = new Dictionary<string, VirtualButton[]>
            {
                {"TestUp", new[] { VirtualButton.GamePad.PadUp, VirtualButton.Keyboard.W, VirtualButton.Keyboard.Up } },
            };
            _builder.FromDictionary(dictionary);
            var result = _builder.Build();
            Assert.That(result, Has.Exactly(3).Items);

            Assert.Multiple(() =>
            {
                var up = result[0];
                Assert.That(up.Name.ToString(), Is.EqualTo("TestUp"));
                Assert.That(((VirtualButton)up.Button).Name, Is.EqualTo(VirtualButton.GamePad.PadUp.Name));

                var up2 = result[1];
                Assert.That(up2.Name.ToString(), Is.EqualTo("TestUp"));
                Assert.That(((VirtualButton)up2.Button).Name, Is.EqualTo(VirtualButton.Keyboard.W.Name));

                var up3 = result[2];
                Assert.That(up3.Name.ToString(), Is.EqualTo("TestUp"));
                Assert.That(((VirtualButton)up3.Button).Name, Is.EqualTo(VirtualButton.Keyboard.Up.Name));
            });
        }
    }
}