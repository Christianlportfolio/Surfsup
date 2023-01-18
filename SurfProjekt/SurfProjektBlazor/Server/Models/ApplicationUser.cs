using Microsoft.AspNetCore.Identity;
using SurfProjektBlazor.Shared;

namespace SurfProjektBlazor.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Boards> OwnedBoards { get; set; }


    }
}
