using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spice.Data;
using Spice.Models.ViewModels;
using Spice.Models;

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
        public async Task<IActionResult> Create()
        {
            CategoryAndSubCategoryViewModel categoryAndSubViewModel = new CategoryAndSubCategoryViewModel();
            categoryAndSubViewModel.CategoryList = await _db.Category.ToListAsync();
            categoryAndSubViewModel.SubCategory = new Models.SubCategory();
            categoryAndSubViewModel.SubCategoryList = await _db.SubCategory
                                                            .OrderBy(s => s.Name)
                                                            .Select(s => s.Name)
                                                            .Distinct()
                                                            .ToListAsync();
            return View(categoryAndSubViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryAndSubCategoryViewModel _subcategoryData)
        {
            var doesExistAlready = _db.SubCategory.Include(s => s.Category)
                                                  .Where(s => s.Name == _subcategoryData.SubCategory.Name && s.Category.Id == _subcategoryData.SubCategory.CategoryId);
            if (ModelState.IsValid)
            {
                if (doesExistAlready.Count() > 0)
                {
                    // error
                }
                else
                {
                    await _db.SubCategory.AddAsync(_subcategoryData.SubCategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            var errorSubCategory = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync()
            ,   SubCategory = _subcategoryData.SubCategory
            ,   SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync()
            };
            return View(errorSubCategory);
        }
    }
}
