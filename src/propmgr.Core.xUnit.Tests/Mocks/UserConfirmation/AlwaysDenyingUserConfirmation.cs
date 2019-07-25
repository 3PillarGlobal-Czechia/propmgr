using System.Threading.Tasks;
using propmgr.Core.UserInteraction;

namespace propmgr.Core.xUnit.Tests.Mocks.UserConfirmation
{
    class AlwaysDenyingUserConfirmation : IUserConfirmation
    {
        public Task<bool> Confirm(string message) => Task.FromResult(false);
    }
}
