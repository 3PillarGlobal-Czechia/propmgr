using propmgr.Core.Commands;
using propmgr.Core.Entities;
using propmgr.Core.xUnit.Tests.Mocks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace propmgr.Core.xUnit.Tests.Commands
{
    public class StripEmptyLinesCommandTests
    {
        [Fact]
        public void NullFile_ShouldDoNothing()
        {
            ICommand cmd = new StripEmptyLinesCommand(null);

            cmd.Execute();
        }


        [Fact]
        public void EmptyProperties_ShouldDoNothing()
        {
            IPropertiesFile file = new InMemoryPropertiesFile();
            ICommand cmd = new StripEmptyLinesCommand(file);

            cmd.Execute();

            Assert.Empty(file.GetProperties());
        }


        [Fact]
        public void PropertiesWithoutEmptyLine_ShouldStaySame()
        {
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>
            {
                new PropertyPair { Key = "Key1", Value = "Value1" },
                new PropertyPair { Key = "#", Value = "Comment 1" },
                new PropertyPair { Key = "!", Value = "Comment 2" }
            });
            ICommand cmd = new StripEmptyLinesCommand(file);

            cmd.Execute();

            Assert.Equal(3, file.GetProperties().Count());
            Assert.Equal("Comment 2", file.GetProperties().Last().Value);
            Assert.Equal("Value1", file.GetProperties().First().Value);
        }


        [Fact]
        public void OnlyEmptyLines_ShouldRemoveEmptyLines()
        {
            IPropertiesFile file = new InMemoryPropertiesFile(new List<PropertyPair>
            {
                new PropertyPair { Key = "", Value = "" },
                new PropertyPair { Key = "", Value = "" }
            });
            ICommand cmd = new StripEmptyLinesCommand(file);

            cmd.Execute();

            Assert.Empty(file.GetProperties());
        }
    }
}
