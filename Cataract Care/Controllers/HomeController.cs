using Cataract_Care.Data;
using Cataract_Care.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Cataract_Care.Controllers
{
   
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        

        public HomeController(UserManager<ApplicationUser> userManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            string userRole = string.Empty;
            string isSubscribed=string.Empty;
            if (user != null)
            {
                // Get the role of the user
                var roles = await _userManager.GetRolesAsync(user);
                userRole = roles.FirstOrDefault();
                var package = _context.UserSubscriptions.FirstOrDefault(x => x.UserId == user.Id);

                if (package != null)
                {
                    isSubscribed = "true";
                }
            }
            ViewData["isSubscribed"] = isSubscribed;
            ViewData["UserRole"] = userRole;

            // Start with all active packages
            var subscriptionPackages = _context.Packages.Where(x => x.IsActive).AsQueryable();

            if (userRole == "User")
            {
                // Fetch the list of subscribed package IDs for the current user
                var subscribedPackageIds = await _context.UserSubscriptions
                    .Where(us => us.UserId == user.Id)
                    .Select(us => us.SubscriptionId)
                    .ToListAsync();

                // Exclude packages that the user has already subscribed to
                subscriptionPackages = subscriptionPackages
                    .Where(p => !subscribedPackageIds.Contains(p.SubscriptionId));
            }

            

            // Execute the query and get the list of packages
            var activePackages = await subscriptionPackages.ToListAsync();

            return View(activePackages);
        }



    }
}
