using System.Collections.Generic;
using propmgr.Core.Entities;

namespace propmgr.Core.xUnit.Tests.Mocks
{
    class InMemoryPropertiesFile : IPropertiesFile
    {
        private IEnumerable<PropertyPair> _properties;

        public InMemoryPropertiesFile(IEnumerable<PropertyPair> properties)
            => _properties = properties;

        public IEnumerable<PropertyPair> GetProperties()
            => _properties;

        public void SetCollection(IEnumerable<PropertyPair> properties)
            => _properties = properties;
    }
}
