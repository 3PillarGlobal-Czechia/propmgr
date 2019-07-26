using propmgr.Core.Commands;
using propmgr.Core.Entities;
using propmgr.Core.xUnit.Tests.Mocks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace propmgr.Core.xUnit.Tests.Commands
{
    public class RemovePropertyByKeyCommandTests
    {
        [Fact]
        public void NullFile_ShouldDoNothing()
        {
            IPropertiesFile file = null;
            ICommand cmd = new RemovePropertyByKeyCommand(file, "TestKey");

            cmd.Execute();
        }

        [Fact]
        public void NullPropertyKey_ShouldDoNothing()
        {
            IPropertiesFile file = new InMemoryPropertiesFile();
            ICommand cmd = new RemovePropertyByKeyCommand(file, null);

            cmd.Execute();

            Assert.Empty(file.GetProperties());
        }

        [Fact]
        public void EmptyPropertyKey_ShouldDoNothing()
        {
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>
            {
                new PropertyPair { Key = "Key1", Value = "Value1" },
                new PropertyPair { Key = "", Value = "" }
            });
            ICommand cmd = new RemovePropertyByKeyCommand(file, "");

            cmd.Execute();

            Assert.Equal(2, file.GetProperties().Count());
        }

        [Theory]
        [InlineData("#")]
        [InlineData("!")]
        public void CommentKey_ShouldDoNothing(string targetKey)
        {
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>
            {
                new PropertyPair { Key = "#", Value = "I am a comment." },
                new PropertyPair { Key = "!", Value = "I am also a comment." }
            });
            ICommand cmd = new RemovePropertyByKeyCommand(file, targetKey);

            cmd.Execute();

            Assert.Equal(2, file.GetProperties().Count());
        }

        public class FileWithTwoProperties
        {
            private readonly IPropertiesFile _file;

            public FileWithTwoProperties()
            {
                _file = new InMemoryPropertiesFile(new List<PropertyPair>
                {
                    new PropertyPair { Key = "Key1", Value = "Value1" },
                    new PropertyPair { Key = "Key2", Value = "Value2" }
                });
            }

            [Fact]
            public void UnknownPropertyKey_ShouldDoNothing()
            {
                ICommand cmd = new RemovePropertyByKeyCommand(_file, "UnknownKey");
                cmd.Execute();
                Assert.Equal(2, _file.GetProperties().Count());
            }

            [Fact]
            public void ValidKey_ShouldRemoveProperty()
            {
                ICommand cmd = new RemovePropertyByKeyCommand(_file, "Key1");
                cmd.Execute();
                Assert.Single(_file.GetProperties());
                Assert.Equal("Key2", _file.GetProperties().First().Key);
            }
        }
    }
}
