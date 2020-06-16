using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace IdentityServer.AuthorizationServer
{
    public class Scopes
    {
        public static List<Scope> Get() 
        {
            return new List<Scope>()
            {
                new Scope { Name="FilmApi" }
            };
        }
        
    }
}
