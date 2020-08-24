using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategory : Controller
    {
        private readonly ApplicationDbContext _db;

        public SubCategory(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var allSubCategoryData = await _db.SubCategory.Include(s => s.Category).ToListAsync();
            return View(allSubCategoryData);
        }
    }
}
