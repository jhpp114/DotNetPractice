using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;

namespace Spice.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel indexViewModel = new IndexViewModel()
            {
                MenuItem = await _db.MenuItem.Include(s => s.Category).Include(s => s.SubCategory).ToListAsync()
            ,   Category = await _db.Category.ToListAsync()
            ,   Coupon = await _db.Coupon.Where(s => s.isCouponActive == true).ToListAsync()
            };

            var claimIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var cnt = _db.ShoppingCart.Where(s => s.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32("ssCartCount", cnt);
            }
            return View(indexViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var shoppingData = await _db.MenuItem.Include(s => s.Category)
                                                .Include(s => s.SubCategory)
                                                .Where(s => s.Id == id)
                                                .SingleOrDefaultAsync();
            if (shoppingData == null)
            {
                return NotFound();
            }
            ShoppingCart shoppingCartObj = new ShoppingCart()
            {
                MenuItem = shoppingData
            ,   MenuItemId = shoppingData.Id
            };
            return View(shoppingCartObj);
        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;
            if (ModelState.IsValid)
            {
                var claimUser = (ClaimsIdentity)this.User.Identity;
                var claim = claimUser.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCart.ApplicationUserId = claim.Value;
                ShoppingCart shoppingCartDb = await _db.ShoppingCart.Where(s => s.ApplicationUserId == shoppingCart.ApplicationUserId
                                        && s.MenuItemId == shoppingCart.MenuItemId).FirstOrDefaultAsync();
                if (shoppingCartDb == null)
                {
                    await _db.ShoppingCart.AddAsync(shoppingCart);
                }
                else
                {
                    shoppingCartDb.Count += shoppingCart.Count;
                }
                await _db.SaveChangesAsync();
                var count = _db.ShoppingCart.Where(s => s.ApplicationUserId == shoppingCart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32("ssCartCount", count);
                return RedirectToAction("Index");
            }
            else
            {
                var shoppingData = await _db.MenuItem.Include(s => s.Category)
                                            .Include(s => s.SubCategory)
                                            .Where(s => s.Id == shoppingCart.MenuItemId)
                                            .FirstOrDefaultAsync();
                if (shoppingData == null)
                {
                    return NotFound();
                }
                ShoppingCart shoppingCartObj = new ShoppingCart()
                {
                    MenuItem = shoppingData
                ,
                    MenuItemId = shoppingData.Id
                };
                return View(shoppingCartObj);
            }
            
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
