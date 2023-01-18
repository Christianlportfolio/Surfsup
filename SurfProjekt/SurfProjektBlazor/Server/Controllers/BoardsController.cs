using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfProjektBlazor.Server.Data;
using SurfProjektBlazor.Shared;
using System.Security.Claims;

namespace SurfProjektBlazor.Server.Controllers
{
    [Route("api/boards")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        //private readonly UserManager<IdentityUser> userManager;

        public BoardsController(ApplicationDbContext context)
        {
            _context = context;
            //userManager = UserManager;
        }

        [HttpGet("getboards")]
        public async Task<ActionResult<List<Boards>>> GetBoards()
        {
            var user = await _context.AspNetUsers.Include(u => u.OwnedBoards).FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null)
                return NotFound();
            return Ok(user.OwnedBoards);
        }

    }
}
