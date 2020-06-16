using FilmApi.DAL;
using FilmApi.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIlmApi.DAL
{
    public class DBInitializer
    {
        public static void Initialize(Context context) 
        {
            context.Database.EnsureCreated();
            if (context.Users.Any())
                return;
            var users = new User[]
            {
                new User("John","john.doe@gmail.com", "123456",Roles.Administrator),
                new User("Frank","fr.doe@gmail.com", "123456",Roles.User),
                new User("Fred","fredp@gmail.com", "123456",Roles.User),
                new User("Bob","bb.doe@gmail.com", "123456",Roles.User),
                new User("Adam","ad.doe@gmail.com", "123456",Roles.User),
                new User("Andrew","and.doe@gmail.com", "123456",Roles.User),
                new User("Anthony","f.doe@gmail.com", "123456",Roles.User),
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            if (context.Films.Any())
                return;
            var films = new Film[]
            {
                new Film( "Terminator","Very good film", 1, "Action"),
                new Film("Deep in the dark","Very good film", 2, "Horror"),
                new Film("Once in Hollywood","I think it is great", 1, "Comedy"),
                new Film("Django","Very good film", 1, "Western"),

            };
            context.Films.AddRange(films);
            context.SaveChanges();

            if (context.Comments.Any())
                return;
            var comments = new Comment[]
            {
                new Comment ( "My first comment", 1, 1),
                new Comment ( "Sub sample comment", 1, 1)
            };
            context.Comments.AddRange(comments);
            context.SaveChanges();
            Console.WriteLine("Done");
        
        }
    }
}
