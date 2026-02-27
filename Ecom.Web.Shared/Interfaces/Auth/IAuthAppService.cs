using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Auth.Models;

namespace Ecom.Web.Shared.Interfaces.Auth
{
    public interface IAuthAppService
    {
        public Task<Result<TokenResponseDto>> RefreshTokenAsync(string refreshToken);
    }
}
