using System.Linq;

namespace propmgr.Core.Commands
{
    public class StripCommentsCommand : ICommand
    {
        private readonly IPropertiesFile _file;

        public StripCommentsCommand(IPropertiesFile file)
        {
            _file = file;
        }

        public void Execute()
        {
            if (_file is null)
                return;

            RemoveComments();
        }

        private void RemoveComments()
        {
            var p = _file.GetProperties().Where(prop => !prop.IsComment());
            _file.SetProperties(p);
        }
    }
}
