using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SurfProjektBlazor.Server.Data;
using System.Net;
using System.Threading.Tasks;

namespace SurfProjektBlazor.Server
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RentLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _limit;
        private readonly ApplicationDbContext _context;

        public RentLimitingMiddleware(RequestDelegate next, ApplicationDbContext context)
        {
            _next = next;
            _limit = 2;
            _context = context;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var ipAddress = httpContext.Connection.RemoteIpAddress;
            return _next(httpContext);
        }

        //public bool HasReachedMaxRentals(IPAddress iPAddress)
        //{
        //    bool result = false;
        //    var leases = _context.Lease;
        //    int leasesCount = 0;
        //    foreach (var lease in leases)
        //    {
        //        if (lease.IPAddress == iPAddress) leasesCount++;
        //    }
        //    if (leasesCount >=_limit) result = true;
        //    return result;
        //}
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RentLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRentLimitingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RentLimitingMiddleware>();
        }
    }
}
