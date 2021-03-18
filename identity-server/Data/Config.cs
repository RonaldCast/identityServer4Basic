using System.Collections;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Identity_server.Data
{
    public static class Config
    {

        public static IEnumerable<IdentityResource> IdentityResources =
            new List<IdentityResource>
            {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email()
            };
        
        public static  readonly List<Client> Clients = new List<Client>
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
                    AllowedScopes =
                    {
                        "my-api", "write", "read", IdentityServerConstants.LocalApi.ScopeName 
                    },
                    Claims =  new List<ClientClaim>
                    {
                        new ClientClaim("companyName", "John Doe LTD")
                    }, 
                    AllowedCorsOrigins = new List<string>
                    {
                        "https://localhost:5001",
                    },
                    AccessTokenLifetime = 10
                    
                },
                
                new Client
                {
                    ClientId =  "the-client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secretId".Sha256())
                        
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.LocalApi.ScopeName,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "myapi.write",
                        "myapi.read"
                    },
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AccessTokenLifetime = 10
                    
                    
                }
        };

        public static readonly  List<ApiResource> ApiResources = new List<ApiResource>
        {
            new ApiResource()
            {
                Name = "my-api",
                DisplayName =  "My Fancy Secured API",
                Scopes = new List<string>()
                {
                    "myapi.write",
                    "myapi.read", IdentityServerConstants.LocalApi.ScopeName
                }
            } 
        };
        
        public static IEnumerable<ApiScope> ApiScopes = new List<ApiScope>
        {
              new ApiScope("myapi.write"),
              new ApiScope("myapi.read"), 
              new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };
    }
}