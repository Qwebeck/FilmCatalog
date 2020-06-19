﻿using Microsoft.Owin.Hosting;
using Serilog;
using System;

namespace IdentityServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .LiterateConsole(outputTemplate: "{Timestamp:HH:mm} [{Level}] ({Name:l}){NewLine} {Message}{NewLine}{Exception}")
                .CreateLogger();

            // hosting identityserver
            using (WebApp.Start<Startup>("http://localhost:5000"))
            {
                Console.WriteLine("server running...");
                Console.ReadLine();
            }


            Console.WriteLine("Hello World!");
        }
    }
}