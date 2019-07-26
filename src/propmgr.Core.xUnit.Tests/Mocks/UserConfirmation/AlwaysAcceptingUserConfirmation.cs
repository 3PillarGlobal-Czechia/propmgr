using System;
using System.Threading.Tasks;
using propmgr.Core.UserInteraction;

namespace propmgr.Core.xUnit.Tests.Mocks.UserConfirmation
{
    class AlwaysAcceptingUserConfirmation : IUserConfirmation
    {
        public Task<bool> Confirm(string message)
        {
            if (string.IsNullOrEmpty(message)) throw new Exception("Confirm must have a message.");
            return Task.FromResult(true);
        }
    }
}
