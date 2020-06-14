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
                new User(1, "John","john.doe@gmail.com", "123456",Roles.Administrator),
                new User(2, "Frank","fr.doe@gmail.com", "123456",Roles.User),
                new User(3, "Fred","fredp@gmail.com", "123456",Roles.User),
                new User(4, "Bob","bb.doe@gmail.com", "123456",Roles.User),
                new User(5, "Adam","ad.doe@gmail.com", "123456",Roles.User),
                new User(6, "Andrew","and.doe@gmail.com", "123456",Roles.User),
                new User(7, "Anthony","f.doe@gmail.com", "123456",Roles.User),
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            if (context.Films.Any())
                return;
            var films = new Film[]
            {
                new Film(1, "Great Film","Very good film", 1),
                new Film(2, "Great Film","Very good film", 2),
                new Film(3, "Great Film","I think it is great", 1),
                new Film(4, "Great Film","Very good film", 1),

            };
            context.Films.AddRange(films);
            context.SaveChanges();

            if (context.Comments.Any())
                return;
            var comments = new Comment[]
            {
                new Comment (1, DateTime.Now, "My first comment", 1, 1),
                new Comment (2, DateTime.Now, "Sub sample comment", 1, 1, 1)
            };
            context.Comments.AddRange(comments);
            context.SaveChanges();
            Console.WriteLine("Done");
        
        }
    }
}
