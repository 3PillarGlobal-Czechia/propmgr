using propmgr.Core.Entities;
using System.Collections.Generic;

namespace propmgr.Core
{
    public interface IPropertiesFile
    {
        IEnumerable<PropertyPair> GetProperties();
        void SetCollection(IEnumerable<PropertyPair> properties);
    }
}
