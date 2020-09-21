using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models.ViewModels;

namespace Spice.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;

        }
        [Authorize]
        public async Task<IActionResult> Confirm(int id)
        {
            var claimIdentity = (ClaimsIdentity)this.User.Identity;
            var claimUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetails orderDetails = new OrderDetails
            {
                OrderHeader = await _db.OrderHeader.Include(s => s.ApplicationUser).FirstOrDefaultAsync(s => s.Id == id && s.UserId == claimUser.Value),
                OrderDetail = await _db.OrderDetail.Where(s => s.OrderHeaderId == id).ToListAsync()
            };
            return View(orderDetails);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
