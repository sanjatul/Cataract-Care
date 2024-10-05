using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cataract_Care.Data;
using Cataract_Care.Models;
using Microsoft.AspNetCore.Hosting;

namespace Cataract_Care.Controllers
{
    public class PackagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PackagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Subscriptions
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Packages == null)
            {
                return NotFound();
            }

            var subscription = await _context.Packages
                .FirstOrDefaultAsync(m => m.SubscriptionId == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubscriptionId,PackageName,Price,ValidityPeriod,MaxPhotoLimit,Description")] Package subscription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscription);
                await _context.SaveChangesAsync();

                TempData["subScriptionPackage"] = "New Package Created";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index)); ;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubscriptionId,PackageName,Price,ValidityPeriod,MaxPhotoLimit,Description")] Package subscription)
        {
            if (id != subscription.SubscriptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(subscription.SubscriptionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }


        private bool SubscriptionExists(int id)
        {
          return (_context.Packages?.Any(e => e.SubscriptionId == id)).GetValueOrDefault();
        }



        #region API CALLS
        public async Task<IActionResult> GetAllPlans()
        {
            List<Package> subModels = await _context.Packages
                .OrderByDescending(s => s.SubscriptionId) 
                .ToListAsync();

            return Json(new { data = subModels });
        }

        [HttpPut]
        public async Task<IActionResult> Disable(int? id)
        {
            if (id == null)
            {
 
                return Json(new { success = false, message = "Invalid ID" });
            }
            var subscriptionToDelete = await _context.Packages.FirstOrDefaultAsync(x => x.SubscriptionId == id);
            if (subscriptionToDelete == null)
            {
                return Json(new { success = false, message = "Error while disabling" });
            }

            subscriptionToDelete.IsActive = false;
            _context.Packages.Update(subscriptionToDelete);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Package disabled" });
        }
        [HttpPut]
        public async Task<IActionResult> Activate(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Invalid ID" });
            }
            var subscriptionToDelete = await _context.Packages.FirstOrDefaultAsync(x => x.SubscriptionId == id);
            if (subscriptionToDelete == null)
            {
                return Json(new { success = false, message = "Error while activateing" });
            }

            subscriptionToDelete.IsActive = true;
            _context.Packages.Update(subscriptionToDelete);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Package activated" });
        }
        #endregion



    }
}
