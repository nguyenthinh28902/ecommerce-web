using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.User;

namespace Ecom.Web.Shared.Interfaces.User
{
    public interface IUserInformation
    {
        public Task<Result<UserInforDto>> GetUserInfoAsync(string AccessToken);
    }
}
