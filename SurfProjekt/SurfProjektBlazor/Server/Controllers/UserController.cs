using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfProjektBlazor.Server.Data;
using SurfProjektBlazor.Server.Models;
using SurfProjektBlazor.Shared;
using System.Drawing;
using System.Security.Claims;

namespace SurfProjektBlazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private SignInManager<IdentityUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public UserController(SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<bool> GetUserAuthentication()
        {
            return _signInManager.IsSignedIn(User);
        }

        [HttpGet("{role}")]
        public async Task<bool> AddRole(string role)
        {
            var newRole = new IdentityRole(role);
            var result = await _roleManager.CreateAsync(newRole);
            return result.Succeeded;
        }

        [HttpGet("role")]
        public async Task<string> GetRole()
        {
            return (await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User)))[0];
        }

        [HttpGet("roles")]
        public async Task<List<IdentityRole>> GetRoles()
        {
            return await _context.Roles.ToListAsync<IdentityRole>();
        }

        [HttpGet("UserId")]
        public async Task<string> GetUserId()
        {
            return (await _userManager.GetUserAsync(User)).Id;
        }

        [HttpGet("users")]
        public async Task<List<IdentityUser>> getUsers()
        {
            var response = await _context.Users.Where(user => user.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)).ToListAsync();
            return response;
        }

        //[HttpGet("GetUser/{ApplicationUserId}")]
        //public async Task<string> getUser(string ApplicationUserId)
        //{
        //    var user = _context.AspNetUsers.Where(a => a.Id == ApplicationUserId).Select(a => a.UserName).ToString();
        //    return user;
        //}

    }
}
