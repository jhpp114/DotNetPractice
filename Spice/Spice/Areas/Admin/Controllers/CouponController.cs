﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CouponController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var allCouponData = await _db.Coupon.ToListAsync();
            return View(allCouponData);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coupon _coupon)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    // files exist
                    byte[] bytes = null;
                    using (var filestream = files[0].OpenReadStream())
                    {
                        using (var memory = new MemoryStream())
                        {
                            // copy memory
                            filestream.CopyTo(memory);
                            bytes = memory.ToArray();
                        }
                    }
                    _coupon.CouponImage = bytes;

                } else
                {
                    return View(_coupon);
                }
                await _db.Coupon.AddAsync(_coupon);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(_coupon);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var foundEditData = await _db.MenuItem.FindAsync(id);
            if (foundEditData == null)
            {
                return NotFound();
            }
            return View(foundEditData);
        }
    }
}
