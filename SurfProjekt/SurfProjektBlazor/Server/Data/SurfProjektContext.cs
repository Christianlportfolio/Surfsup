using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SurfProjektBlazor.Server.Models;
using SurfProjektBlazor.Shared;

namespace SurfProjektBlazor.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<IdentityUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
     
        public DbSet<ApplicationUser> AspNetUsers { get; set; } = default!;


        public DbSet<Boards> Boards { get; set; } = default!;
        public DbSet<Lease> Lease { get; set; } = default!;

        public DbSet<Equipment> Equipment { get; set; } = default!;
        public DbSet<SUPboard> SUPboards { get; set; } = default!;
    }
}