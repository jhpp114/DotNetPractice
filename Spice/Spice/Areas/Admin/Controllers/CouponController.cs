using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Spice.Data;

namespace Spice.Areas.Admin.Controllers
{
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext _db;
        public IActionResult Index()
        {
            return View();
        }
    }
}
