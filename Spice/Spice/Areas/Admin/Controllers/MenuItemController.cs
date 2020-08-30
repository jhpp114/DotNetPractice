using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webhost;
        
        [BindProperty]
        public MenuitemViewModel _MenuitemViewModel { get; set; }
        public MenuItemController(ApplicationDbContext db, IWebHostEnvironment webhost)
        {
            _db = db;
            _webhost = webhost;
            _MenuitemViewModel = new MenuitemViewModel()
            {
                Category = _db.Category,
                MenuItem = new MenuItem()
            };
        }
        public async Task<IActionResult> Index()
        {
            var allMenuItems = await _db.MenuItem.Include(s => s.Category).Include(s => s.SubCategory).ToListAsync();
            return View(allMenuItems);
        }
        public async Task<IActionResult> Create()
        {
            return View(_MenuitemViewModel);
        }

        [ActionName("GetSubcategoryItems")]
        public async Task<IActionResult> GetSubcategoryItems(int id)
        {
            var subcategoryData = await _db.SubCategory.Where(m => m.CategoryId == id).ToListAsync();
            return Json(new SelectList(subcategoryData, "Id", "Name"));
        }
    }
}
