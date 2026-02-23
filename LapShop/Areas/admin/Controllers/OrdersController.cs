using LapShop.Bl;
using LapShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LapShop.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly LapShopContext _context;

        public OrdersController(LapShopContext context)
        {
            _context = context;
        }

        public IActionResult List()
        {
            var orders = _context.TbSalesInvoices
                .OrderByDescending(o => o.InvoiceDate)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkPending(int id)
        {
            UpdateOrderState(id, 1);
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkInDelivery(int id)
        {
            UpdateOrderState(id, 2);
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkCompleted(int id)
        {
            UpdateOrderState(id, 3);
            return RedirectToAction(nameof(List));
        }

        private void UpdateOrderState(int id, int state)
        {
            var order = _context.TbSalesInvoices.FirstOrDefault(o => o.InvoiceId == id);
            if (order != null)
            {
                order.CurrentState = state;
                _context.SaveChanges();
            }
        }
    }
}
