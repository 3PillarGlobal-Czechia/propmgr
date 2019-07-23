using propmgr.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace propmgr.Core.Commands
{
    public class AddPropertyCommand : ICommand
    {
        private readonly List<PropertyPair> _properties;
        private readonly PropertyPair _propertyToAdd;

        public AddPropertyCommand(List<PropertyPair> properties, PropertyPair newProperty)
        {
            if (properties is null)
                throw new ArgumentNullException("The properties collection cannot be null.");

            _properties = properties;
            _propertyToAdd = newProperty;
        }

        public void Execute()
        {
            if (_propertyToAdd is null)
                return;

            if (KeyAlreadyExists())
                throw new ArgumentException("A property with this key already exists.");

            _properties.Add(_propertyToAdd);
        }

        private bool KeyAlreadyExists()
            => _properties.Any(p => p.Key == _propertyToAdd.Key);
    }
}
