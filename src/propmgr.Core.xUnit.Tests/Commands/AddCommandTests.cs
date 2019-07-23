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

        public class EmptyPropertyList
        {
            private readonly List<PropertyPair> _properties;

            public EmptyPropertyList()
            {
                _properties = new List<PropertyPair>();
            }

            [Fact]
            public void NullProperty_ShouldNotEditEmptyCollection()
            {
                ICommand cmd = new AddPropertyCommand(_properties, null);

                cmd.Execute();

                Assert.Empty(_properties);
            }

            [Fact]
            public void ShouldAddIntoEmptyCollection()
            {
                var prop = new PropertyPair { Key = "greeting", Value = "Hello, World!" };
                ICommand cmd = new AddPropertyCommand(_properties, prop);

                cmd.Execute();

                Assert.Single(_properties);
                Assert.Same(prop, _properties.First());
            }
        }

        public class PropertyListWithTwoElements
        {
            private readonly List<PropertyPair> _properties;

            public PropertyListWithTwoElements()
            {
                _properties = new List<PropertyPair>
                {
                    new PropertyPair { Key = "A", Value = "a" },
                    new PropertyPair { Key = "B", Value = "b" }
                };
            }

            [Fact]
            public void NullProperty_ShouldNotEditFilledCollection()
            {
                ICommand cmd = new AddPropertyCommand(_properties, null);

                cmd.Execute();

                Assert.Equal(2, _properties.Count);
            }

            [Fact]
            public void ShouldBeAddedAtTheEndOfCollection()
            {
                var prop = new PropertyPair { Key = "greeting", Value = "Hello, World!" };
                ICommand cmd = new AddPropertyCommand(_properties, prop);

                cmd.Execute();

                Assert.Equal(3, _properties.Count);
                Assert.Same(prop, _properties.Last());
            }

            [Fact]
            public void ShouldThrowIfDuplicateKey()
            {
                var prop = new PropertyPair { Key = "B", Value = "Hello, World!" };
                ICommand cmd = new AddPropertyCommand(_properties, prop);

                Assert.Throws<ArgumentException>(() => cmd.Execute());
            }
        }
    }
}
