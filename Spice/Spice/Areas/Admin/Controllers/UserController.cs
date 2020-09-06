using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Utility;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles =StaticDetail.MANAGER_USER)]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var currentUserIdentity = (ClaimsIdentity)this.User.Identity;
            var currentUser = currentUserIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(await _db.ApplicationUser.Where(s => s.Id != currentUser.Value).ToListAsync());
        }

        public async Task<IActionResult> Lock(string id) 
        {
            if (id == null)
            {
                return NotFound();
            }
            var userData = await _db.ApplicationUser.FindAsync(id);
            if (userData == null)
            {
                return NotFound();
            }
            userData.LockoutEnd = DateTime.Now.AddYears(100);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Unlock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userData = await _db.ApplicationUser.FindAsync(id);
            if (userData == null)
            {
                return NotFound();
            }
            userData.LockoutEnd = DateTime.Now;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
