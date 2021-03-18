
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Identity_server.Data.DomainModel;
using Identity_server.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace Identity_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager; 

        public IdentityController(UserManager<User> userManager, 
            IMapper mapper, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

      //  [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("signup")]
        public async Task<ActionResult> SignUp([FromBody]UserSignUp model)
        {
            var user = _mapper.Map<UserSignUp, User>(model);

            var userCreate = await _userManager.CreateAsync(user, model.Password);
            
            return userCreate.Succeeded ? Created(string.Empty, string.Empty) : 
                Problem(userCreate.Errors.First().Description, null, 500);
        }
      // [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("role")]
        public async Task<ActionResult> CreateRole([FromBody] CreateRole model)
        {
            var roleExist = await _roleManager.RoleExistsAsync(model.RoleName);

            if (roleExist) return BadRequest();
            var newRole = new Role
            {
                Name = model.RoleName
            };

            var result = await _roleManager.CreateAsync(newRole);

            if (result.Succeeded)
            {
                return Ok(new {message = "se a creado correctamente"});
            }
            return BadRequest();
        }

      //  [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("roleUser")]
        public async Task<ActionResult> AssignRoleToUser([FromBody] AssignRole model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return NotFound();
            }
            
            var currentRoles = await _userManager.GetRolesAsync(user);

            var rolesNotExist = model.Roles.Except(_roleManager.Roles.Select(x => x.Name)).ToArray();
            
            if(rolesNotExist.Any())
            {
                return this.BadRequest();
            }
            
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles.ToArray());


            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest();
            }
            
            var result = await _userManager.AddToRolesAsync(user, model.Roles);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }
        [Authorize(LocalApi.PolicyName, Roles = "Admin")]
        [HttpPut("Hello")]
        public ActionResult d( Proof model )
        {
           return Ok("helloo");
        }
        
        
    }
}