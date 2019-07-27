using System.Linq;

namespace propmgr.Core.Commands
{
    public class RemovePropertyByKeyCommand : ICommand
    {
        private readonly IPropertiesFile _file;
        private readonly string _targetKey;

        public RemovePropertyByKeyCommand(IPropertiesFile file, string targetKey)
        {
            _file = file;
            _targetKey = targetKey;
        }

        public void Execute()
        {
            if (ShouldAbort())
                return;

            RemoveAllPropertiesByKey();
        }

        private bool ShouldAbort() => 
            HasNullParameter() || TargetKeyIsComment();

        private bool HasNullParameter()
            => _file is null || string.IsNullOrEmpty(_targetKey);

        private bool TargetKeyIsComment()
            => _targetKey == "#" || _targetKey == "!";

        private void RemoveAllPropertiesByKey()
        {
            var props = _file.GetProperties().Where(p => p.Key != _targetKey);
            _file.SetProperties(props);
        }
    }
}
