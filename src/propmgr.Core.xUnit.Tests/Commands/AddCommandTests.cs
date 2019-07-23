using propmgr.Core.Commands;
using propmgr.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace propmgr.Core.xUnit.Tests.Commands
{
    public class AddCommandTests
    {
        [Fact]
        public void NullCollection_ShouldThrowOnConstruction()
        {
            var prop = new PropertyPair { Key = "A", Value = "a" };
            Assert.Throws<ArgumentNullException>(() => { ICommand cmd = new AddPropertyCommand(null, prop); });
        }

        [Fact]
        public void NullProperty_ShouldNotEditEmptyCollection()
        {
            var props = new List<PropertyPair>();
            ICommand cmd = new AddPropertyCommand(props, null);

            cmd.Execute();

            Assert.Empty(props);
        }

        [Fact]
        public void NullProperty_ShouldNotEditFilledCollection()
        {
            var props = new List<PropertyPair>
            {
                new PropertyPair { Key = "A", Value = "a" },
                new PropertyPair { Key = "B", Value = "b" }
            };
            ICommand cmd = new AddPropertyCommand(props, null);

            cmd.Execute();

            Assert.Equal(2, props.Count);
        }

        [Fact]
        public void ShouldAddIntoEmptyCollection()
        {
            var props = new List<PropertyPair>();
            var prop = new PropertyPair { Key = "greeting", Value = "Hello, World!" };
            ICommand cmd = new AddPropertyCommand(props, prop);

            cmd.Execute();

            Assert.Single(props);
            Assert.Same(prop, props.First());
        }
    }
}
