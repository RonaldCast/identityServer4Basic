using System.Collections;
using System.Collections.Generic;
using IdentityServer4.Models;

namespace Identity_server.Data
{
    public static class Config
    {

        public static List<Client> Clients = new List<Client>
        {
                new Client
                {
                    ClientId =  "the-big-client",
                    AllowedGrantTypes =  new List<string>()
                    {
                        GrantType.ClientCredentials
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                        
                    },
                    AllowedScopes =  {"my-api", "write", "read"},
                    Claims =  new List<ClientClaim>
                    {
                        new ClientClaim("companyName", "John Doe LTD")
                    }, 
                    AllowedCorsOrigins = new List<string>
                    {
                        "https://localhost:5001",
                    },
                    AccessTokenLifetime = 86400
                    
                }
        };
        
        public static  List<ApiResource> ApiResources = new List<ApiResource>
        {
            new ApiResource()
            {
                Name = "my-api",
                DisplayName =  "My Fancy Secured API",
                Scopes = new List<string>()
                {
                    "write",
                    "read"
                }
            }    
        };
        
        public static IEnumerable<ApiScope> ApiScopes = new List<ApiScope>
        {
            new ApiScope("read"),
            new ApiScope("write")
        };
        
        
    }
}