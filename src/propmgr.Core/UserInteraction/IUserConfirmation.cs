using System.Threading.Tasks;

namespace propmgr.Core.UserInteraction
{
    public interface IUserConfirmation
    {
        Task<bool> Confirm(string message);
    }
}
