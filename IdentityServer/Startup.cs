using System;
using System.Collections.Generic;
using System.Text;
using Owin;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.InMemory;
using IdentityServer.AuthorizationServer;
namespace IdentityServer
{
    class Startup
    {
        public void AuthConfiguration(IAppBuilder app)
        {
            var options = new IdentityServerOptions
            {
                Factory = new IdentityServerServiceFactory()
                                .UseInMemoryClients(Clients.Get())
                                .UseInMemoryScopes(Scopes.Get())
                                .UseInMemoryUsers(new List<InMemoryUser>()),
                RequireSsl = false
            };
            app.UseIdentityServer(options);
           }
        }
}
