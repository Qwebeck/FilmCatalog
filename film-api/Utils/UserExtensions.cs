using FilmApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using FilmApi.AuthorityProviders;

namespace FilmApi.Utils
{
    public interface IHaveUserID 
    {
        public string? UserID { get; set; }
    }
    public static class UserExtensions
    {
        public async static Task<bool> IsAuthorizedForAction<T>(this ClaimsPrincipal user, T instance ) where T: IHaveUserID
        {
            string userID = user.FindFirstValue("uid");
            if (((instance.UserID ?? "") == userID))
            {
                return true;
            }
            var provider = new OktaMiddleware();
            return await provider.CheckIfAdministrator(userID);
        }
    }
}
