using System.Threading.Tasks;
using propmgr.Core.UserInteraction;

namespace propmgr.Core.xUnit.Tests.Mocks.UserConfirmation
{
    class AlwaysAcceptingUserConfirmation : IUserConfirmation
    {
        public Task<bool> Confirm(string message) => Task.FromResult(true);
    }
}
