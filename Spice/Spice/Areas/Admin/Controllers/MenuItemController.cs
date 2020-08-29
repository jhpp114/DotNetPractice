using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webhost;
        
        public MenuItemController(ApplicationDbContext db, IWebHostEnvironment webhost)
        {
            _db = db;
            _webhost = webhost;
        }
        public async Task<IActionResult> Index()
        {
            var allMenuItems = await _db.MenuItem.Include(s => s.Category).Include(s => s.SubCategory).ToListAsync();
            return View(allMenuItems);
        }
    }
}
