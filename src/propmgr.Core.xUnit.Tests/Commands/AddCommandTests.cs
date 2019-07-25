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
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>());
            IUserConfirmation uc = new AlwaysAcceptingUserConfirmation();
            ICommand cmd = new AddPropertyCommand(file, null, uc);

            cmd.Execute();

            Assert.Empty(file.GetProperties());
        }

        [Fact]
        public void ShouldAddIntoEmptyCollection()
        {
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>());
            IUserConfirmation uc = new AlwaysAcceptingUserConfirmation();
            var prop = new PropertyPair { Key = "greeting", Value = "Hello, World!" };
            ICommand cmd = new AddPropertyCommand(file, prop, uc);

            cmd.Execute();

            var collection = file.GetProperties();
            Assert.Single(collection);
            Assert.Same(prop, collection.First());
        }

        [Fact]
        public void NullCollection_ShouldNotThrow()
        {
            IUserConfirmation uc = new AlwaysAcceptingUserConfirmation();
            var prop = new PropertyPair { Key = "A", Value = "a" };
            ICommand cmd = new AddPropertyCommand(null, prop, uc);

            cmd.Execute();
        }

        [Fact]
        public void NullProperty_ShouldNotEditFilledCollection()
        {
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>
                {
                    new PropertyPair { Key = "A", Value = "a" },
                    new PropertyPair { Key = "B", Value = "b" }
                });
            IUserConfirmation uc = new AlwaysAcceptingUserConfirmation();
            ICommand cmd = new AddPropertyCommand(file, null, uc);

            cmd.Execute();

            Assert.Equal(2, file.GetProperties().Count());
        }

        [Fact]
        public void ShouldBeAddedAtTheEndOfCollection()
        {
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>
                {
                    new PropertyPair { Key = "A", Value = "a" },
                    new PropertyPair { Key = "B", Value = "b" }
                });
            IUserConfirmation uc = new AlwaysAcceptingUserConfirmation();
            var prop = new PropertyPair { Key = "greeting", Value = "Hello, World!" };
            ICommand cmd = new AddPropertyCommand(file, prop, uc);

            cmd.Execute();

            var collection = file.GetProperties();
            Assert.Equal(3, collection.Count());
            Assert.Same(prop, collection.Last());
        }

        [Fact]
        public void ShouldUpdateExistingIfKeyAlreadyExists()
        {
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>
                {
                    new PropertyPair { Key = "A", Value = "a" },
                    new PropertyPair { Key = "B", Value = "b" }
                });
            IUserConfirmation uc = new AlwaysAcceptingUserConfirmation();
            var prop = new PropertyPair { Key = "B", Value = "Hello, World!" };
            ICommand cmd = new AddPropertyCommand(file, prop, uc);

            cmd.Execute();

            Assert.Equal(2, file.GetProperties().Count());
            Assert.Equal("Hello, World!", file.GetProperties().Last().Value);
        }

        [Fact]
        public void WhenUserDeniesShouldNotAddDuplicateValue()
        {
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>
                {
                    new PropertyPair { Key = "A", Value = "a" },
                    new PropertyPair { Key = "B", Value = "b" }
                });
            IUserConfirmation uc = new AlwaysDenyingUserConfirmation();
            var prop = new PropertyPair { Key = "greeting", Value = "b" };
            ICommand cmd = new AddPropertyCommand(file, prop, uc);

            cmd.Execute();

            Assert.Equal(2, file.GetProperties().Count());
        }
    }
}
