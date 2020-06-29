using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using FilmApi.DAL;
using FilmApi.Models;
using FilmApi.AuthorityProviders;
using System.Threading.Tasks;

namespace FIlmApi.DAL
{
    public class DBInitializer
    {
        public static void Initialize(Context context) 
        {
            context.Database.EnsureCreated();
        }
    }
}
