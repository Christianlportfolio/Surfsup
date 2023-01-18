using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SurfProjekt;
using SurfProjekt.Data;
using SurfProjekt.Models;
using SurfProjekt.Models.ViewModels;
using static System.Net.WebRequestMethods;

namespace SurfProjekt.Controllers
{
    
    //Denne línje angiver at begge roller, admin og user, har adgang til boards-siden.
    //[Authorize(Roles = $"{ConstantsRole.Roles.Admin}, {ConstantsRole.Roles.User}")]
    
    //Denne linje sørger for, at man godt kan se borads-siden uden at oprette en bruger.
    //[AllowAnonymous]
    public class BoardsController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private readonly SurfProjektContext _context;

        public BoardsController(SurfProjektContext context, UserManager<IdentityUser> UserManager)
        {
            _context = context;
            userManager = UserManager;
        }

        // GET: Boards
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Boards != null ? 
        //                  View(await _context.Boards.ToListAsync()) :
        //                  Problem("Entity set 'SurfProjektContext.Boards'  is null.");
        //}

        public async Task<IActionResult> Index(
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            //var boards = from b in _context.Boards.
            //             Include(b => b.leases).
            //             AsNoTracking().
            //             Where(b => b.IsRented != true);
            string URL;
            if (User.Identity.IsAuthenticated)
            {
                URL = "https://localhost:7244/api/Boards?api-version=2.0";
            }
            else
            {
                URL = "https://localhost:7244/api/Boards?api-version=1.0";
            }

            BoardHierarchy boardHierarchy = new();
            HttpClient boardsClient= new HttpClient();
            var boards= await boardsClient.GetFromJsonAsync<IEnumerable<Boards>>(URL);

            //var boards = _context.Boards.Include(b => b.leases).AsNoTracking();

            //foreach(var board in boards)
            //{
            //    foreach(var lease in board.leases)
            //    {
            //        if (lease.Date < DateTime.Now && lease.EndTime > DateTime.Now)
            //        {
            //            board.IsRented = true;
            //        }
            //        else
            //        {
            //            board.IsRented = false;
            //        }
            //        _context.Update(board);

            //    }
            //}
            //await _context.SaveChangesAsync();

            //boards = from b in boards
            //         where !(b.IsRented == true)
            //         select b;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                //Hvorfor er ! bagved name og ikke contains?
                boards = boards.Where(b => b.Name!.Contains(searchString)
                                        || b.Type.Contains(searchString));
            }

            int pageSize = 4;
            boardHierarchy.Boards = await PaginatedList<Boards>.CreateAsync(boards, pageNumber ?? 1, pageSize);

            return View(boardHierarchy);

  
        }

        // GET: Boards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string URL;
            if (User.Identity.IsAuthenticated)
            {
                URL = $"https://localhost:7244/api/Boards/{id}?api-version=2.0";
            }
            else
            {
                URL = $"https://localhost:7244/api/Boards/{id}?api-version=1.0";
            }

            HttpClient boardsClient = new HttpClient();
            var boards = await boardsClient.GetFromJsonAsync<Boards>(URL);

            if (boards == null)
            {
                return NotFound();
            }

            return View(boards);
        }

        // GET: Boards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Boards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Length,Width,Thickness,Volume,Type,Price,Equipment,Image")] Boards boards)
        {
            if (ModelState.IsValid)
            {
                _context.Add(boards);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boards);
        }

        // GET: Boards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Boards == null)
            {
                return NotFound();
            }

            var boards = await _context.Boards.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (boards == null)
            {
                return NotFound();
            }
            return View(boards);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Length,Width,Thickness,Volume,Type,Price,Equipment,Image, RowVersionBoards")] Boards boards)
        {
            if (id != boards.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //_context.Entry(boards).Property("RowVersionBoard").OriginalValue = rowVersionBoard;
                try
                {
                    _context.Update(boards);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Boards)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Boards)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                        {
                            ModelState.AddModelError("Navn", $"Current value: {databaseValues.Name}");
                        }
                        if (databaseValues.Length != clientValues.Length)
                        {
                            ModelState.AddModelError("Længde", $"Current value: {databaseValues.Length}");
                        }
                        if (databaseValues.Width != clientValues.Width)
                        {
                            ModelState.AddModelError("Bredde", $"Current value: {databaseValues.Width}");
                        }
                        if (databaseValues.Thickness != clientValues.Thickness)
                        {
                            ModelState.AddModelError("Tykkelse", $"Current value: {databaseValues.Thickness}");
                        }
                        if (databaseValues.Volume != clientValues.Volume)
                        {
                            ModelState.AddModelError("Volume", $"Current value: {databaseValues.Volume}");
                        }
                        if (databaseValues.Type != clientValues.Type)
                        {
                            ModelState.AddModelError("Type", $"Current value: {databaseValues.Type}");
                        }
                        if (databaseValues.Price != clientValues.Price)
                        {
                            ModelState.AddModelError("Pris", $"Current value: {databaseValues.Price}");
                        }
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you got the original value. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to edit this record, click "
                                + "the Save button again. Otherwise click the Back to List hyperlink.");
                        boards.RowVersionBoards = (byte[])databaseValues.RowVersionBoards;
                        ModelState.Remove("RowVersionBoards");
                    }
                }
            }
            return View(boards);
            
        }

        public async Task<IActionResult> Rent(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            string URL = $"https://localhost:7244/api/Boards/{id}";
            HttpClient boardsClient = new HttpClient();
            var boards = await boardsClient.GetFromJsonAsync<Boards>(URL);

            var boardleasing = new BoardLeasing();
            boardleasing.Board = boards;
            if (boardleasing.Board == null)
            {
                return NotFound();
            }

            return View(boardleasing);
        }

        [HttpPost, ActionName("Rent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RentConfirmed(int id, [Bind("TimeFrame")] Lease lease)
        {
            var UserID = userManager.GetUserId(User);
            string URL = $"https://localhost:7244/api/Boards/Rent";
            HttpClient boardsClient = new HttpClient();

            lease.BoardID = id;
            lease.UserID = UserID;
            lease.Date = DateTime.Now;

            await boardsClient.PostAsJsonAsync<Lease>(URL, lease);

            return RedirectToAction(nameof(Index));
        }

        //[HttpPost, ActionName("Rent")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RentConfirmed(int id, [Bind("TimeFrame")] Lease lease)
        //{
        //    var UserID = userManager.GetUserId(User);
        //    string URL = $"https://localhost:7244/api/Boards/Rent/{id}/{UserID}/{lease.TimeFrame}";
        //    HttpClient boardsClient = new HttpClient();

        //    await boardsClient.GetAsync(URL);

        //    return RedirectToAction(nameof(Index));
        //}



        // GET: Boards/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null || _context.Boards == null)
            {
                return NotFound();
            }

            var boards = await _context.Boards
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
          
            if (boards == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewData["ConcurrencyErrorMessage"] = "The record you attempted to edit "
                                + "was modified by another user after you got the original value. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to edit this record, click "
                                + "the Save button again. Otherwise click the Back to List hyperlink.";
            }
            return View(boards);         
        }

        // POST: Boards/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Boards boards)
        {
           

            try
            {
                if (await _context.Boards.AnyAsync(m => m.Id == boards.Id)) 
                {
                    
                    _context.Boards.Remove(boards);
                    await _context.SaveChangesAsync();
                    
                }
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction(nameof(Delete), new { concurrencyError = true, id = boards.Id });
            }
            
        }




        private bool BoardsExists(int id)
        {
          return (_context.Boards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
