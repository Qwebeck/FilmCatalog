using System.Security.Claims;
using System.Threading.Tasks;
using FilmApi.AuthorityProviders;

namespace FilmApi.Utils
{
    public interface IHaveUserID 
    {
        public string? UserID { get; set; }
    }
    public static class UserExtensions
    {
        public async static Task<bool> IsAuthorizedForAction<T>(this ClaimsPrincipal user, T instance) where T : IHaveUserID
        {
            string userID = user.FindFirstValue("uid");
            var provider = new OktaMiddleware();
            {
                return (instance.UserID ?? "") == userID || await provider.CheckIfAdministrator(userID);
            }
        }
    }
}
