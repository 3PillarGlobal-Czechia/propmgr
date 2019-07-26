using System;
using System.Threading.Tasks;
using propmgr.Core.UserInteraction;

namespace propmgr.Core.xUnit.Tests.Mocks.UserConfirmation
{
    class ThrowOnTriggerUserConfirmation : IUserConfirmation
    {
        public Task<bool> Confirm(string message)
            => throw new Exception("UserConfirmation dialog was not supposed to trigger, but it did.");
    }
}
