using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.AuthorityProviders
{
    public class NoIdForCreatedUserException: Exception 
    {
        public NoIdForCreatedUserException ( ) { }
        public NoIdForCreatedUserException ( string msg ) : base (msg) { }
        public NoIdForCreatedUserException ( string msg, Exception inner ) : base( msg, inner ) { }
    }
    
}
