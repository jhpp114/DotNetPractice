using System;
using System.Collections.Generic;
using System.IO;
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
using Spice.Utility;

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

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            if (!ModelState.IsValid)
            {
                return View(_MenuitemViewModel);
            }
            
            await _db.MenuItem.AddAsync(_MenuitemViewModel.MenuItem);
            await _db.SaveChangesAsync();
            // work on the image
            var webRootPath = _webhost.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var menuItemFromDb = await _db.MenuItem.FindAsync(_MenuitemViewModel.MenuItem.Id);
            if (files.Count > 0)
            {
                var path = Path.Combine(webRootPath, "Images");
                var extension = Path.GetExtension(files[0].FileName);
                var fullPath = Path.Combine(path, _MenuitemViewModel.MenuItem.Id + extension);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                menuItemFromDb.Image = @"\Images\" + _MenuitemViewModel.MenuItem.Id + extension;
            }
            else
            {
                var uploadPath = Path.Combine(webRootPath, @"Images\" + StaticDetail.DefaultImage);
                // copy this file from folder
                System.IO.File.Copy(uploadPath, webRootPath+@"\Images\"+_MenuitemViewModel.MenuItem.Id+".png");
                menuItemFromDb.Image = @"\Images\" + _MenuitemViewModel.MenuItem.Id + ".png";
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _MenuitemViewModel.MenuItem = await _db.MenuItem.Include(s => s.Category).Include(s => s.SubCategory).SingleOrDefaultAsync(s => s.Id == id);
            if (_MenuitemViewModel.MenuItem == null)
            {
                return NotFound();
            }
            return View(_MenuitemViewModel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(_MenuitemViewModel);
            }
            var webRootPath = _webhost.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var menuitemDataFromDb = await _db.MenuItem.FindAsync(id);
            if (files.Count() > 0)
            {
                // user upload new photo
                var path = Path.Combine(webRootPath, "Images");
                var extension = Path.GetExtension(files[0].FileName);
                var fullPath = Path.Combine(path, _MenuitemViewModel.MenuItem.Id + extension);

                // delete the original file
                var imagePath = Path.Combine(webRootPath, menuitemDataFromDb.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                menuitemDataFromDb.Image = @"\Images\" + _MenuitemViewModel.MenuItem.Id + extension;
            }
            menuitemDataFromDb.Name = _MenuitemViewModel.MenuItem.Name;
            menuitemDataFromDb.Description = _MenuitemViewModel.MenuItem.Description;
            menuitemDataFromDb.Price = _MenuitemViewModel.MenuItem.Price;
            menuitemDataFromDb.CategoryId = _MenuitemViewModel.MenuItem.CategoryId;
            menuitemDataFromDb.SubCategoryId = _MenuitemViewModel.MenuItem.SubCategoryId;
            menuitemDataFromDb.Spicyness = _MenuitemViewModel.MenuItem.Spicyness;
            // user did not upload new photo no need to do anything
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _MenuitemViewModel.MenuItem = await _db.MenuItem.Include(s => s.Category).Include(s => s.SubCategory).SingleOrDefaultAsync(s => s.Id == id);
            if (_MenuitemViewModel == null)
            {
                return NotFound();
            }
            return View(_MenuitemViewModel);
        }
    }
}
