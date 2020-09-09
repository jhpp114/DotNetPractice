using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models.ViewModels;
using Spice.Utility;

namespace Spice.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public OrderDetailsCartViewModel orderDetailsCartViewModel { get; set; }
        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            // retreive user
            var claimIdentity = (ClaimsIdentity)this.User.Identity;
            var currentUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var shoppingCartData = _db.ShoppingCart.Where(s => s.ApplicationUserId == currentUser.Value);
            orderDetailsCartViewModel = new OrderDetailsCartViewModel()
            {
                OrderHeader = new Models.OrderHeader()
            };
            orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal = 0;
            if (shoppingCartData != null)
            {
                orderDetailsCartViewModel.ListShoppingCarts = shoppingCartData.ToList();
            }
            //ConvertToRawHtml versus @Html.Raw(item.Description)
            foreach (var orderItem in orderDetailsCartViewModel.ListShoppingCarts)
            {
                orderItem.MenuItem = await _db.MenuItem.FirstOrDefaultAsync(s => s.Id == orderItem.MenuItemId);
                orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal += (orderItem.MenuItem.Price * orderItem.Count);
                orderItem.MenuItem.Description = StaticDetail.ConvertToRawHtml(orderItem.MenuItem.Description);
                if (orderItem.MenuItem.Description.Length > 100)
                {
                    orderItem.MenuItem.Description = orderItem.MenuItem.Description.Substring(0, 99) + "...";
                }
            }
            orderDetailsCartViewModel.OrderHeader.OrderTotalDiscount = orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal;
            if (HttpContext.Session.GetString(StaticDetail.ssCoupon) != null)
            {
                orderDetailsCartViewModel.OrderHeader.CouponCode = HttpContext.Session.GetString(StaticDetail.ssCoupon);
                var couponFromDb = await _db.Coupon.Where(s => s.Name.ToLower() == orderDetailsCartViewModel.OrderHeader.CouponCode.ToLower())
                                                    .FirstOrDefaultAsync();
                orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal = StaticDetail.DiscountedPrice(couponFromDb, orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal);
            }
            return View(orderDetailsCartViewModel);
        }

        public async Task<IActionResult> AddCoupon()
        {
            if (orderDetailsCartViewModel.OrderHeader.CouponCode == null)
            {
                orderDetailsCartViewModel.OrderHeader.CouponCode = "";
            }
            // set up the session for coupon
            HttpContext.Session.SetString(StaticDetail.ssCoupon, orderDetailsCartViewModel.OrderHeader.CouponCode);
            return RedirectToAction("Index");
        }
    }
}
