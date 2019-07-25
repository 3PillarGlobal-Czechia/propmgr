using propmgr.Core.Entities;
using propmgr.Core.UserInteraction;
using System.Linq;
using System.Threading.Tasks;

namespace propmgr.Core.Commands
{
    public class AddPropertyCommand : ICommand
    {
        private readonly IPropertiesFile _file;
        private readonly PropertyPair _propertyToAdd;
        private readonly IUserConfirmation _userConfirmation;

        public AddPropertyCommand(IPropertiesFile file, PropertyPair newProperty, IUserConfirmation userConfirmation)
        {
            _file = file;
            _propertyToAdd = newProperty;
            _userConfirmation = userConfirmation;
        }

        public async void Execute()
        {
            if (await GetShouldAbortAsync())
                return;

            AddOrUpdateProperty();
        }

        private async Task<bool> GetShouldAbortAsync()
            => FileOrPropertyIsNull() || await GetUserWantsToAvoidDuplicateValuesAsync();

        private bool FileOrPropertyIsNull()
            => _file is null || _propertyToAdd is null;

        private async Task<bool> GetUserWantsToAvoidDuplicateValuesAsync()
            => GetValueAlreadyExists() && !await UserConfirmsAddition();

        private async Task<bool> UserConfirmsAddition()
            => await _userConfirmation.Confirm($"Another property with the value: ˙{_propertyToAdd.Value}˙ already exists. Do you want to add it anyways?");

        private bool GetValueAlreadyExists()
            => _file.GetProperties().Any(p => p.Value == _propertyToAdd.Value);

        private void AddOrUpdateProperty()
        {
            if (KeyExists())
            {
                UpdateExistingProperty();
            }
            else
            {
                AddPropertyToCollection();
            }
        }

        private bool KeyExists()
            => _file.GetProperties().Any(p => p.Key == _propertyToAdd.Key);

        private void UpdateExistingProperty()
        {
            var key = _file.GetProperties().FirstOrDefault(p => p.Key == _propertyToAdd.Key);
            if (key is null)
                return;
            key.Value = _propertyToAdd.Value;
        }

        private void AddPropertyToCollection()
        {
            var p = _file.GetProperties().ToList();
            p.Add(_propertyToAdd);
            _file.SetCollection(p);
        }
    }
}
