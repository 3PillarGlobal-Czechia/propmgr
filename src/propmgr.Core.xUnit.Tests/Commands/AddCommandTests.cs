using propmgr.Core.Commands;
using propmgr.Core.Entities;
using propmgr.Core.UserInteraction;
using propmgr.Core.xUnit.Tests.Mocks;
using propmgr.Core.xUnit.Tests.Mocks.UserConfirmation;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace propmgr.Core.xUnit.Tests.Commands
{
    public class AddCommandTests
    {
        [Fact]
        public void NullProperty_ShouldNotEditEmptyCollection()
        {
            IPropertiesFile file = CreateEmptyPropertiesFile();
            ICommand cmd = CreateCommandWithoutUserConfirmation(file, null);

            cmd.Execute();

            Assert.Empty(file.GetProperties());
        }

        [Fact]
        public void NullProperty_ShouldNotEditFilledCollection()
        {
            IPropertiesFile file = CreateFileWithTwoProperties();
            ICommand cmd = CreateCommandWithoutUserConfirmation(file, null);

            cmd.Execute();

            Assert.Equal(2, file.GetProperties().Count());
        }

        [Fact]
        public void NullCollection_ShouldNotThrow()
        {
            var prop = CreateProperty("Key", "Value");
            ICommand cmd = CreateCommandWithoutUserConfirmation(null, prop);

            cmd.Execute();
        }

        [Fact]
        public void ShouldAddIntoEmptyCollection()
        {
            IPropertiesFile file = CreateEmptyPropertiesFile();
            var prop = CreateProperty("Key", "Value");
            ICommand cmd = CreateCommandWithoutUserConfirmation(file, prop);

            cmd.Execute();

            var collection = file.GetProperties();
            Assert.Single(collection);
            Assert.Same(prop, collection.First());
        }

        [Fact]
        public void ShouldBeAddedAtTheEndOfCollection()
        {
            IPropertiesFile file = CreateFileWithTwoProperties();
            var prop = CreateProperty("Key", "Value");
            ICommand cmd = CreateCommandWithoutUserConfirmation(file, prop);

            cmd.Execute();

            var collection = file.GetProperties();
            Assert.Equal(3, collection.Count());
            Assert.Same(prop, collection.Last());
        }

        [Fact]
        public void ShouldUpdateExistingIfKeyAlreadyExists()
        {
            const string newValue = "NewValue";
            IPropertiesFile file = CreateFileWithTwoProperties();
            var duplicateKey = file.GetProperties().Last().Key;
            var prop = CreateProperty(duplicateKey, newValue);
            prop.Value = newValue;
            ICommand cmd = CreateCommandWithoutUserConfirmation(file, prop);

            cmd.Execute();

            Assert.Equal(2, file.GetProperties().Count());
            Assert.Equal(newValue, file.GetProperties().Last().Value);
        }

        [Fact]
        public void ShouldNotAddDuplicateValueWhenUserDenies()
        {
            IPropertiesFile file = CreateFileWithTwoProperties();
            IUserConfirmation uc = new AlwaysDenyingUserConfirmation();
            var duplicateValue = file.GetProperties().Last().Value;
            var prop = CreateProperty("Key1", duplicateValue);
            ICommand cmd = new AddPropertyCommand(file, prop, uc);

            cmd.Execute();

            Assert.Equal(2, file.GetProperties().Count());
        }

        private static IPropertiesFile CreateFileWithTwoProperties()
            => new InMemoryPropertiesFile(new List<PropertyPair>
            {
                new PropertyPair { Key = "ExistingKey1", Value = "ExistingValue1" },
                new PropertyPair { Key = "ExistingKey2", Value = "ExistingValue2" }
            });

        private static IPropertiesFile CreateEmptyPropertiesFile()
            => new InMemoryPropertiesFile();

        private static ICommand CreateCommandWithoutUserConfirmation(IPropertiesFile file, PropertyPair property)
            => new AddPropertyCommand(file, property, new ThrowOnTriggerUserConfirmation());

        private static PropertyPair CreateProperty(string key, string value)
            => new PropertyPair { Key = key, Value = value };
    }
}
