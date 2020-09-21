using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;
using Spice.Utility;
using Stripe;

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

        public IActionResult RemoveCoupon()
        {
            HttpContext.Session.SetString(StaticDetail.ssCoupon, string.Empty);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var cartData = await _db.ShoppingCart.Where(s => s.Id == cartId).FirstOrDefaultAsync();
            if (cartData == null)
            {
                return NotFound();
            }
            cartData.Count += 1;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cartData = await _db.ShoppingCart.Where(s => s.Id == cartId).FirstOrDefaultAsync();
            if (cartData == null)
            {
                return NotFound();
            }
            if (cartData.Count == 1)
            {
                _db.ShoppingCart.Remove(cartData);
                await _db.SaveChangesAsync();
                var cnt = _db.ShoppingCart.Where(s => s.ApplicationUserId == cartData.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32("ssCartCount", cnt);
            }
            else
            {
                cartData.Count -= 1;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cartData = await _db.ShoppingCart.Where(s => s.Id == cartId).FirstOrDefaultAsync();
            if (cartData == null)
            {
                return NotFound();
            }
            // need to decrease the shopping cart number for this user
            _db.ShoppingCart.Remove(cartData);
            await _db.SaveChangesAsync();
            var cnt = _db.ShoppingCart.Where(s => s.ApplicationUserId == cartData.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32("ssCartCount", cnt);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Summary()
        {
            // get currentuser
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claimUser = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            // getting user_name
            ApplicationUser user = await _db.ApplicationUser.Where(s => s.Id == claimUser.Value).FirstOrDefaultAsync();

            var shoppingCartData = _db.ShoppingCart.Where(s => s.ApplicationUserId == claimUser.Value);
            orderDetailsCartViewModel = new OrderDetailsCartViewModel()
            {
                OrderHeader = new Models.OrderHeader()
            };
            orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal = 0;
            if (shoppingCartData != null)
            {
                orderDetailsCartViewModel.ListShoppingCarts = shoppingCartData.ToList();
            }
            foreach(var shoppingItem in orderDetailsCartViewModel.ListShoppingCarts)
            {
                shoppingItem.MenuItem = await _db.MenuItem.FirstOrDefaultAsync(s => s.Id == shoppingItem.MenuItemId);
                // update the total price
                orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal += shoppingItem.MenuItem.Price * shoppingItem.Count;
            }
            orderDetailsCartViewModel.OrderHeader.OrderTotalDiscount = orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal;
            // use user name
            orderDetailsCartViewModel.OrderHeader.PickupName = user.Name;
            orderDetailsCartViewModel.OrderHeader.Phonenumber = user.PhoneNumber;
            orderDetailsCartViewModel.OrderHeader.PickupTime = DateTime.Now;
           
            if (HttpContext.Session.GetString(StaticDetail.ssCoupon) != null)
            {
                orderDetailsCartViewModel.OrderHeader.CouponCode = HttpContext.Session.GetString(StaticDetail.ssCoupon);
                var couponFromDb = await _db.Coupon.Where(s => s.Name.ToLower() == orderDetailsCartViewModel.OrderHeader.CouponCode.ToLower())
                                                    .FirstOrDefaultAsync();
                orderDetailsCartViewModel.OrderHeader.OrderTotalDiscount = StaticDetail.DiscountedPrice(couponFromDb, orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal);
            }
            return View(orderDetailsCartViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(string stripeToken)
        {
            // claim the current user
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claimUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            orderDetailsCartViewModel.ListShoppingCarts = await _db.ShoppingCart.Where(s => s.ApplicationUserId == claimUser.Value).ToListAsync();
            // update orderheader 
            orderDetailsCartViewModel.OrderHeader.UserId = claimUser.Value;
            orderDetailsCartViewModel.OrderHeader.OrderDate = DateTime.Now;
            orderDetailsCartViewModel.OrderHeader.PaymentStatus = StaticDetail.PaymentStatusPending;
            orderDetailsCartViewModel.OrderHeader.Status = StaticDetail.PaymentStatusPending;
            orderDetailsCartViewModel.OrderHeader.PickupTime = Convert.ToDateTime(orderDetailsCartViewModel.OrderHeader.PickupDate.ToShortDateString() + " " + orderDetailsCartViewModel.OrderHeader.PickupTime.ToShortTimeString());
            await _db.OrderHeader.AddAsync(orderDetailsCartViewModel.OrderHeader);
            await _db.SaveChangesAsync();
            orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal = 0;

            List<OrderDetail> orderDetailsList = new List<OrderDetail>();
            foreach (var shoppingItem in orderDetailsCartViewModel.ListShoppingCarts)
            {
                shoppingItem.MenuItem = await _db.MenuItem.FirstOrDefaultAsync(s => s.Id == shoppingItem.MenuItemId);
                OrderDetail orderdetail = new OrderDetail
                {
                    MenuitemId = shoppingItem.MenuItemId,
                    OrderHeaderId = orderDetailsCartViewModel.OrderHeader.Id,
                    Description = shoppingItem.MenuItem.Description,
                    Name = shoppingItem.MenuItem.Name,
                    Price = shoppingItem.MenuItem.Price,
                    Count = shoppingItem.Count
                };
                orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal += orderdetail.Price * orderdetail.Count;
                await _db.OrderDetail.AddAsync(orderdetail);
            }
            

            if (HttpContext.Session.GetString(StaticDetail.ssCoupon) != null)
            {
                orderDetailsCartViewModel.OrderHeader.CouponCode = HttpContext.Session.GetString(StaticDetail.ssCoupon);
                var couponFromDb = await _db.Coupon.Where(s => s.Name.ToLower() == orderDetailsCartViewModel.OrderHeader.CouponCode.ToLower())
                                                    .FirstOrDefaultAsync();
                orderDetailsCartViewModel.OrderHeader.OrderTotalDiscount = StaticDetail.DiscountedPrice(couponFromDb, orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal);
            }
            else
            {
                orderDetailsCartViewModel.OrderHeader.OrderTotalDiscount = orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal;
            }
            orderDetailsCartViewModel.OrderHeader.CouponDiscount = orderDetailsCartViewModel.OrderHeader.OrderTotalOriginal - orderDetailsCartViewModel.OrderHeader.OrderTotalDiscount;
            await _db.SaveChangesAsync();
            // remove from shoppingcart since we are done with shopping
            _db.ShoppingCart.RemoveRange(orderDetailsCartViewModel.ListShoppingCarts);
            HttpContext.Session.SetInt32("ssCartCount", 0);
            await _db.SaveChangesAsync();


            // make trasnaction using stripe
            var options = new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(orderDetailsCartViewModel.OrderHeader.OrderTotalDiscount * 100),
                Currency = "usd",
                Description = "Order ID: " + orderDetailsCartViewModel.OrderHeader.Id,
                Source= stripeToken
            };
            var service = new ChargeService();
            // create service with options above
            Charge charge = service.Create(options);
            if (charge.BalanceTransactionId == null)
            {
                orderDetailsCartViewModel.OrderHeader.PaymentStatus = StaticDetail.PaymentStatusRejected;
            } 
            else
            {
                orderDetailsCartViewModel.OrderHeader.TransactionId = charge.BalanceTransactionId;
            }

            if (charge.Status.ToLower() == "succeeded")
            {
                orderDetailsCartViewModel.OrderHeader.PaymentStatus = StaticDetail.PaymentStatusApproved;
                orderDetailsCartViewModel.OrderHeader.Status = StaticDetail.OrderSubmitted;
            }
            else
            {
                orderDetailsCartViewModel.OrderHeader.PaymentStatus = StaticDetail.PaymentStatusRejected;
            }
            await _db.SaveChangesAsync();
            // return RedirectToAction("Index", "Home");
            // actioname, controller, pass param
            return RedirectToAction("Confirm", "Order", new { id = orderDetailsCartViewModel.OrderHeader.Id });
        }

    }
}
