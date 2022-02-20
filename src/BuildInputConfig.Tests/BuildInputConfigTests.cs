using NUnit.Framework;
using Stride.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BuildInputConfig.Tests
{
    public class BuildInputConfigTests
    {
        private InputBuilder _builder;
        private string _invalidJsonFilePath;
        private string _invalidJson = "This is not a json file";
        private string _jsonFilePath;

        private string CreateJsonFile(string fileName, string fileData)
        {
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            using (var fs = File.Create(filePath))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(fileData);
                }
                fs.Close();
            }

            return filePath;
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _invalidJsonFilePath = CreateJsonFile("invalid-file.json", _invalidJson);
        }

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

        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public void FromJson_GivenNullEmptyOrWhiteSpaceFileName_ThrowsArgumentException(string fileName)
        {
            Assert.That(() => _builder.FromJson(fileName), Throws.ArgumentException.With.Message.EqualTo("A filepath cannot be null, empty of whitespace."));
        }

        [Test]
        public void FromJson_GivenInvalidJsonFile_ThrowsJsonException()
        {
            Assert.That(() => _builder.FromJson(_invalidJsonFilePath), Throws.TypeOf<JsonException>());
        }

        [TestCase("")]
        [TestCase("      ")]
        [TestCase(null)]
        public void FromJson_GivenKeyThatIsNullEmptyOrWhitespace_ThrowsJsonException(string keyData)
        {
            _jsonFilePath = CreateJsonFile("file.json", "{"+$"{keyData}"+":[\"Test Data\"]}");
            Assert.That(() => _builder.FromJson(_jsonFilePath), Throws.TypeOf<JsonException>());
        }

        [Test]
        public void FromJson_GivenValueThatIsNull_ThrowsFormatException()
        {
            _jsonFilePath = CreateJsonFile("file.json", "{" + $"\"Test Key\":null" + "}");
            Assert.That(() => _builder.FromJson(_jsonFilePath), Throws.TypeOf<FormatException>().With.Message.EqualTo("Value of 'Test Key' cannot be null."));
        }

        [TestCase("\"\"")]
        [TestCase("\"      \"")]
        public void FromJson_GivenArrayValueThatIsEmptyOrWhiteSpace_ThrowsFormatException(string value)
        {
            _jsonFilePath = CreateJsonFile("file.json", "{" + $"\"Test Key\":[{value}]" + "}");
            Assert.That(() => _builder.FromJson(_jsonFilePath), Throws.TypeOf<FormatException>().With.Message.EqualTo("Any value of 'Test Key' cannot be empty or whitespace."));
        }

        [Test]
        public void FromJson_GivenEmptyArray_ThrowsFormatException()
        {
            _jsonFilePath = CreateJsonFile("file.json", "{" + $"\"Test Key\":[]" + "}");
            Assert.That(() => _builder.FromJson(_jsonFilePath), Throws.TypeOf<FormatException>().With.Message.EqualTo("'Test Key' cannot be empty."));
        }

        [TearDown]
        public void TearDown()
        {
            if (!string.IsNullOrWhiteSpace(_jsonFilePath))
            {
                File.Delete(_jsonFilePath);
                _jsonFilePath = null;
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            File.Delete(_invalidJsonFilePath);
        }
    }
}