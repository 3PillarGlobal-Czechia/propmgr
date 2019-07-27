using propmgr.Core.Commands;
using propmgr.Core.Entities;
using propmgr.Core.xUnit.Tests.Mocks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace propmgr.Core.xUnit.Tests.Commands
{
    public class StripCommentsCommandTests
    {
        [Fact]
        public void NullFile_ShouldDoNothing()
        {
            ICommand cmd = new StripCommentsCommand(null);
            cmd.Execute();
        }

        [Fact]
        public void EmptyFile_ShouldDoNothing()
        {
            IPropertiesFile file = new InMemoryPropertiesFile();
            ICommand cmd = new StripCommentsCommand(file);

            cmd.Execute();

            Assert.Empty(file.GetProperties());
        }

        [Fact]
        public void FileWithoutComments_ShouldDoNothing()
        {
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>
            {
                new PropertyPair { Key = "Key1", Value = "Value1" },
                new PropertyPair { Key = "", Value = "" },
                new PropertyPair { Key = "Key2", Value = "Value2" }
            });
            ICommand cmd = new StripCommentsCommand(file);

            cmd.Execute();

            Assert.Equal(3, file.GetProperties().Count());
        }

        [Theory]
        [InlineData("#")]
        [InlineData("!")]
        public void FileWithComment_ShouldRemoveComment(string commentKey)
        {
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>
            {
                new PropertyPair { Key = commentKey, Value = "Hello, I am a comment." }
            });
            ICommand cmd = new StripCommentsCommand(file);

            cmd.Execute();

            Assert.Empty(file.GetProperties());
        }

        [Fact]
        public void IntegrationTest()
        {
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>
            {
                new PropertyPair { Key = "Key1", Value = "Value1" },
                new PropertyPair { Key = "#", Value = "" },
                new PropertyPair { Key = "#", Value = " A TAB-LIKE COMMENT" },
                new PropertyPair { Key = "#", Value = "" },
                new PropertyPair { Key = "", Value = "" },
                new PropertyPair { Key = "!", Value = "key = value"},
                new PropertyPair { Key = "Key2", Value = "Value2" }
            });
            ICommand cmd = new StripCommentsCommand(file);

            cmd.Execute();

            Assert.Equal(3, file.GetProperties().Count());
            Assert.Equal("Key1", file.GetProperties().First().Key);
            Assert.Equal("Key2", file.GetProperties().Last().Key);
        }
    }
}
