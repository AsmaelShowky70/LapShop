using LapShop.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using LapShop.Models;
using System;

namespace LapShop.Areas.admin.Controllers
{
    [Area("admin")]
    public class HomeController : Controller
    {
        private readonly LapShopContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(LapShopContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [Auhorization]
        public IActionResult Index()
        {
            var totalUsers = _userManager.Users.Count();
            var pendingOrders = _context.TbSalesInvoices.Count(o => o.CurrentState == 1);
            var inDeliveryOrders = _context.TbSalesInvoices.Count(o => o.CurrentState == 2);
            var completedOrders = _context.TbSalesInvoices.Count(o => o.CurrentState == 3);

            ViewBag.TotalUsers = totalUsers;
            ViewBag.PendingOrders = pendingOrders;
            ViewBag.InDeliveryOrders = inDeliveryOrders;
            ViewBag.CompletedOrders = completedOrders;

            var fromDate = DateTime.Now.AddMonths(-5);

            var monthlyStats = _context.TbSalesInvoices
                .Where(o => o.InvoiceDate >= fromDate)
                .GroupBy(o => new { o.InvoiceDate.Year, o.InvoiceDate.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Total = g.Count(),
                    Completed = g.Sum(o => o.CurrentState == 3 ? 1 : 0),
                    Pending = g.Sum(o => o.CurrentState == 1 ? 1 : 0)
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToList();

            var visitLabels = monthlyStats.Select(m => $"{m.Month}/{m.Year}").ToArray();
            var visitTotals = monthlyStats.Select(m => m.Total).ToArray();
            var visitCompleted = monthlyStats.Select(m => m.Completed).ToArray();
            var visitPending = monthlyStats.Select(m => m.Pending).ToArray();

            ViewBag.VisitSaleLabels = visitLabels;
            ViewBag.VisitSaleTotals = visitTotals;
            ViewBag.VisitSaleCompleted = visitCompleted;
            ViewBag.VisitSalePending = visitPending;

            ViewBag.TrafficLabels = new[] { "Pending", "In Delivery", "Completed" };
            ViewBag.TrafficData = new[] { pendingOrders, inDeliveryOrders, completedOrders };

            var recentOrders = _context.TbSalesInvoices
                .OrderByDescending(o => o.InvoiceDate)
                .Take(5)
                .Select(o => new
                {
                    o.InvoiceId,
                    o.InvoiceDate,
                    o.CurrentState
                })
                .ToList();

            ViewBag.RecentOrders = recentOrders;

            return View();
        }
    }
}
