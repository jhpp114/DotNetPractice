﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spice.Data;
using Spice.Models.ViewModels;
using Spice.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Spice.Utility;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles =StaticDetail.MANAGER_USER)]
    [Area("Admin")]
    public class SubCategory : Controller
    {
        private readonly ApplicationDbContext _db;
        [TempData]
        public string StatusMessage { get; set; }
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
                    StatusMessage = "Error: This Subcategory is already exist in " + doesExistAlready.First().Category.Name;
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
            ,   StatusMessage = StatusMessage
            };
            return View(errorSubCategory);
        }
        [ActionName("GetSubcategory")]
        public async Task<IActionResult> GetSubcategory(int id)
        {
            var subcategoryItems = await (from sub in _db.SubCategory 
                                      where id == sub.CategoryId 
                                      select sub).ToListAsync();
            return Json(new SelectList(subcategoryItems, "Id", "Name"));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var editSubcategory = await _db.SubCategory.FindAsync(id);
            if (editSubcategory == null)
            {
                return NotFound();
            }
            CategoryAndSubCategoryViewModel subcategoryObj = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync()
              ,
                SubCategory = editSubcategory
              ,
                SubCategoryList = await _db.SubCategory.OrderBy(s => s.Name).Select(s => s.Name).ToListAsync()
            };
            return View(subcategoryObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryAndSubCategoryViewModel _editData)
        {
            if (ModelState.IsValid)
            {
                var isDuplicate = await _db.SubCategory.Include(s => s.Category)
                                                        .Where(s => s.Category.Id == _editData.SubCategory.CategoryId && s.Name == _editData.SubCategory.Name)
                                                        .ToListAsync();
                if (isDuplicate.Count() > 0)
                {
                    StatusMessage = StatusMessage = "Error: This Subcategory is already exist in " + isDuplicate.First().Category.Name;
                }
                else
                {
                    try
                    {
                        var foundData = await _db.SubCategory.FindAsync(id);
                        foundData.Name = _editData.SubCategory.Name;
                        await _db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    } catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return RedirectToAction("Index");
                    }
                   
                }
            }
            var errorSubCategory = new CategoryAndSubCategoryViewModel()
            {
                CategoryList =  await _db.Category.ToListAsync()
            ,   SubCategory = _editData.SubCategory
            ,   SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync()
            ,   StatusMessage = StatusMessage
            };
            return View(errorSubCategory);
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CategoryAndSubCategoryViewModel detailItem = new CategoryAndSubCategoryViewModel();
            detailItem.SubCategory = await _db.SubCategory.FindAsync(id);
            detailItem.CategoryList = await _db.Category.ToListAsync();
            if (detailItem == null)
            {
                return NotFound();
            }
            return View(detailItem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CategoryAndSubCategoryViewModel deleteItem = new CategoryAndSubCategoryViewModel();
            deleteItem.SubCategory = await _db.SubCategory.FindAsync(id);
            deleteItem.CategoryList = await _db.Category.ToListAsync();
            return View(deleteItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var deleteItem = await _db.SubCategory.FindAsync(id);
            if (deleteItem == null)
            {
                return NotFound();
            }
            _db.SubCategory.Remove(deleteItem);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
