using System.Linq;

namespace propmgr.Core.Commands
{
    public class StripEmptyLinesCommand : ICommand
    {
        private readonly IPropertiesFile _file;

        public StripEmptyLinesCommand(IPropertiesFile file)
        {
            _file = file;
        }

        public void Execute()
        {
            if (_file is null)
                return;

            RemoveEmptyLines();
        }

        private void RemoveEmptyLines()
        {
            var p = _file.GetProperties().Where(prop => !prop.IsEmptyLine());
            _file.SetProperties(p);
        }
    }
}
