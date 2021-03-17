using Microsoft.AspNetCore.Identity;

namespace Identity_server.Data.DomainModel
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
    }
}