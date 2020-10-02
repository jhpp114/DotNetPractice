using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;
using Spice.Utility;

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

        [Authorize]
        public async Task<IActionResult> OrderHistory()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claimUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            List<OrderDetails> listOrderDetails = new List<OrderDetails>();
            List<OrderHeader> orderHeaderList = await _db.OrderHeader.Include(s => s.ApplicationUser).Where(s => s.UserId == claimUser.Value).ToListAsync();
            foreach (OrderHeader item in orderHeaderList)
            {
                OrderDetails singleOrderDetail = new OrderDetails
                {
                    OrderHeader = item
                ,
                    OrderDetail = await _db.OrderDetail.Where(s => s.OrderHeaderId == item.Id).ToListAsync()
                };
                listOrderDetails.Add(singleOrderDetail);
            }
            return View(listOrderDetails);
        }

        [Authorize(Roles = StaticDetail.KITCHEN_USER + "," + StaticDetail.MANAGER_USER)]
        public async Task<IActionResult> ManageOrder() 
        {
            List<OrderDetails> orderDetailViews = new List<OrderDetails>();
            List<OrderHeader> listsOfCookOrder = await _db.OrderHeader.Where(s => s.Status == StaticDetail.OrderProcess || s.Status == StaticDetail.OrderSubmitted)
                                                                        .OrderByDescending(s=> s.PickupTime)
                                                                        .ToListAsync();

            foreach (var item in listsOfCookOrder) 
            {
                OrderDetails individualOrderDetail = new OrderDetails
                {
                    OrderHeader = item
                ,
                    OrderDetail = await _db.OrderDetail.Where(s => s.OrderHeaderId == item.Id).ToListAsync()
                };
                orderDetailViews.Add(individualOrderDetail);
            }
            return View(orderDetailViews.OrderBy(s => s.OrderHeader.PickupTime).ToList());
        }

        [Authorize(Roles = StaticDetail.MANAGER_USER + "," + StaticDetail.KITCHEN_USER)]
        public async Task<IActionResult> OrderPrepare(int OrderId) 
        {
            OrderHeader targetOrderHeader = await _db.OrderHeader.FindAsync(OrderId);
            if (targetOrderHeader == null)
            {
                return NotFound();
            }
            targetOrderHeader.Status = StaticDetail.OrderProcess;
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageOrder", "Order");
        }

        [Authorize(Roles = StaticDetail.MANAGER_USER + "," + StaticDetail.KITCHEN_USER)]
        public async Task<IActionResult> OrderReady(int OrderId) 
        {
            OrderHeader targetOrderHeader = await _db.OrderHeader.FindAsync(OrderId);
            if (targetOrderHeader == null)
            {
                return NotFound();
            }
            targetOrderHeader.Status = StaticDetail.ReadyForPickUp;
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageOrder", "Order");
        }

        [Authorize(Roles =StaticDetail.MANAGER_USER + "," + StaticDetail.KITCHEN_USER)]
        public async Task<IActionResult> OrderCancel(int OrderId)
        {
            OrderHeader targetOrderHeader = await _db.OrderHeader.FindAsync(OrderId);
            if (targetOrderHeader == null)
            {
                return NotFound();
            }
            targetOrderHeader.Status = StaticDetail.OrderCancel;
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageOrder", "Order");
            
        }
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            OrderDetails orderDetailsCartVM = new OrderDetails()
            {
                OrderHeader = await _db.OrderHeader.Where(s => s.Id == id).FirstOrDefaultAsync()
             ,  OrderDetail = await _db.OrderDetail.Where(s => s.Id == id).ToListAsync()
            };
            orderDetailsCartVM.OrderHeader.ApplicationUser = await _db.ApplicationUser.FirstOrDefaultAsync(s => s.Id == orderDetailsCartVM.OrderHeader.UserId);
            return PartialView("_IndividualOrderDetail", orderDetailsCartVM);
        }

        public async Task<IActionResult> GetOrderStatus(int id)
        {
            OrderHeader orderHeader = await _db.OrderHeader.FindAsync(id);
            if (orderHeader == null)
            {
                return NotFound();
            }
            return PartialView("_OrderPartial", orderHeader);
        }
    }
}
