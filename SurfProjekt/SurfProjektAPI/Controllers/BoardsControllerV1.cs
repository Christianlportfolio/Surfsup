using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SurfProjekt.Models;
using SurfProjektAPI.Data;

namespace SurfProjektAPI.Controllers
{
    [Route("api/Boards")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BoardsControllerV1 : ControllerBase
    {
        private readonly SurfProjektContext _context;
        //private readonly UserManager<IdentityUser> userManager;

        public BoardsControllerV1(SurfProjektContext context)
        {
            _context = context;
            //userManager = UserManager;
        }

        // GET: api/Boards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Boards>>> GetBoards()
        {
            var boards = _context.Boards.Include(b => b.leases).AsNoTracking().Where(b => b.IsPremium == false);

            foreach (var board in boards)
            {
                foreach(var lease in board.leases)
                {
                    if (lease.Date < DateTime.Now && lease.EndTime > DateTime.Now)
                    {
                        board.IsRented = true;
                    }
                    else
                    {
                        board.IsRented = false;
                    }
                    _context.Update(board);
                }
            }

            await _context.SaveChangesAsync();

            boards = from b in boards
                     where !(b.IsRented == true)
                     select b;

            return await boards.ToListAsync<Boards>();
        }

        // GET: api/Boards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Boards>> GetBoards(int id)
        {
            var boards = await _context.Boards.FindAsync(id);

            if (boards == null)
            {
                return NotFound();
            }

            if (boards.IsPremium)
            {
                return Unauthorized("Du har ikke tilladelse til at se dette surfboard. Log ind for at se den fulde oversigt.");
            }

            return boards;
        }

        [HttpPost("Rent")]
        public async Task<ActionResult<Boards>> Rent([FromBody] Lease lease)
        {
            if (_context.Boards == null)
            {
                return Problem("Entity set 'SurfProjektContext.Boards'  is null.");
            }
            var boards = await _context.Boards.FindAsync(lease.BoardID);
            //var user = await userManager.GetUserAsync(User);
            if (boards == null)
            {
                return NotFound();
            }

            if (boards.IsPremium)
            {
                return Unauthorized("Du har ikke tilladelse til at leje dette surfboard. Log ind for at få adgang til alle boards.");
            }

            boards.leases = new List<Lease>();

            boards.leases.Add(lease);

            //boards.IsRented = true;

            await _context.SaveChangesAsync();
            return Ok();
        }


        // PUT: api/Boards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBoards(int id, Boards boards)
        //{
        //    if (id != boards.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(boards).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BoardsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Boards
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Boards>> PostBoards(Boards boards)
        //{
        //    _context.Boards.Add(boards);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetBoards", new { id = boards.Id }, boards);
        //}

        //// DELETE: api/Boards/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBoards(int id)
        //{
        //    var boards = await _context.Boards.FindAsync(id);
        //    if (boards == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Boards.Remove(boards);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool BoardsExists(int id)
        {
            return _context.Boards.Any(e => e.Id == id);
        }

        [HttpPost("createboard")]
        public async Task<IActionResult> CreateBoard([FromBody] Boards boards)
        {
            try
            {
                boards.leases = null;
                _context.Boards.Add(boards);
                await _context.SaveChangesAsync();
            }

            catch (Exception e)
            {
                return Conflict(e.Message);
            }
            return Ok(boards);
        }

        [HttpPost("updateboard/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Boards boards)
        {
            try
            {
                _context.Update(boards);
                await _context.SaveChangesAsync();
                return Ok(boards);
            }

            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPost("deleteboard")]
        public async Task<IActionResult> Delete(Boards boards)
        {
            try
            {
                if (await _context.Boards.AnyAsync(m => m.Id == boards.Id))
                {
                    _context.Boards.Remove(boards);
                    await _context.SaveChangesAsync();
                }
                return Ok(boards);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }

        }
        [HttpGet("GetUser/{ApplicationUserId}")]


        public async Task<ActionResult<List<IdentityUser>>> getUser(string ApplicationUserId)
        {
            var user = _context.Users.Where(a => a.Id == ApplicationUserId);
            return Ok(user);
        }



    }
}
