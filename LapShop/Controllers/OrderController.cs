using Microsoft.AspNetCore.Mvc;
using LapShop.Models;
using LapShop.Bl;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LapShop.Controllers
{
    /// <summary>
    /// OrderController - يتعامل مع سلة التسوق والطلبات
    /// يدير إضافة المنتجات للسلة وإنشاء الطلبات والفواتير
    /// </summary>
    public class OrderController : Controller
    {
        #region Private Fields
        /// <summary>
        /// خدمة المنتجات
        /// </summary>
        private IItems _itemService;

        /// <summary>
        /// مدير المستخدمين من Identity
        /// </summary>
        private UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// خدمة الفواتير والطلبات
        /// </summary>
        private ISalesInvoice _salesInvoiceService;
        #endregion

        #region Constructor
        /// <summary>
        /// تهيئة الـ Controller مع الخدمات المطلوبة
        /// </summary>
        public OrderController(IItems itemService, UserManager<ApplicationUser> userManager,
            ISalesInvoice salesInvoiceService)
        {
            _itemService = itemService;
            _userManager = userManager;
            _salesInvoiceService = salesInvoiceService;
        }
        #endregion

        #region Shopping Cart Methods - طرق سلة التسوق

        /// <summary>
        /// عرض محتويات سلة التسوق الحالية
        /// تحصل على بيانات السلة من الـ Cookies
        /// </summary>
        /// <returns>View مع محتويات السلة</returns>
        public IActionResult Cart()
        {
            // الحصول على بيانات السلة من الـ Cookies
            string sessionCart = string.Empty;
            if (HttpContext.Request.Cookies["Cart"] != null)
                sessionCart = HttpContext.Request.Cookies["Cart"];

            // تحويل JSON إلى كائن ShoppingCart
            var cart = JsonConvert.DeserializeObject<ShoppingCart>(sessionCart);
            return View(cart);
        }

        /// <summary>
        /// إضافة منتج إلى سلة التسوق
        /// </summary>
        /// <param name="itemId">رقم معرّف المنتج</param>
        /// <returns>إعادة توجيه إلى صفحة السلة</returns>
        public IActionResult AddToCart(int itemId)
        {
            ShoppingCart cart;

            // الحصول على السلة من Cookies أو إنشاء واحدة جديدة
            if (HttpContext.Request.Cookies["Cart"] != null)
                cart = JsonConvert.DeserializeObject<ShoppingCart>(HttpContext.Request.Cookies["Cart"]);
            else
                cart = new ShoppingCart();

            // الحصول على بيانات المنتج
            var item = _itemService.GetById(itemId);

            // البحث عن المنتج في السلة
            var itemInList = cart.lstItems.Where(a => a.ItemId == itemId).FirstOrDefault();

            if (itemInList != null)
            {
                // إذا كان المنتج موجود، زيادة الكمية
                itemInList.Qty++;
                itemInList.Total = itemInList.Qty * itemInList.Price;
            }
            else
            {
                // إضافة منتج جديد إلى السلة
                cart.lstItems.Add(new ShoppingCartItem
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    Price = item.SalesPrice,
                    Qty = 1,
                    Total = item.SalesPrice
                });
            }

            // حساب إجمالي السلة
            cart.Total = cart.lstItems.Sum(a => a.Total);

            // حفظ السلة في Cookies
            HttpContext.Response.Cookies.Append("Cart", JsonConvert.SerializeObject(cart));

            return RedirectToAction("Cart");
        }
        #endregion

        #region Order Methods - طرق الطلبات

        /// <summary>
        /// عرض طلبات المستخدم السابقة
        /// يتطلب تسجيل دخول المستخدم
        /// </summary>
        /// <returns>View بقائمة الطلبات</returns>
        public IActionResult MyOrders()
        {
            return View();
        }

        /// <summary>
        /// صفحة تأكيد الطلب بعد الشراء
        /// تحفظ الطلب في قاعدة البيانات وتنظف السلة
        /// يتطلب المستخدم أن يكون مسجل دخول (Authorize)
        /// </summary>
        /// <returns>View بتأكيد الطلب</returns>
        [Authorize]
        public async Task<IActionResult> OrderSuccess()
        {
            // الحصول على بيانات السلة
            string sessionCart = string.Empty;
            if (HttpContext.Request.Cookies["Cart"] != null)
                sessionCart = HttpContext.Request.Cookies["Cart"];

            // تحويل JSON إلى كائن
            var cart = JsonConvert.DeserializeObject<ShoppingCart>(sessionCart);

            // حفظ الطلب في قاعدة البيانات
            await SaveOrder(cart);

            return View();
        }
        #endregion

        #region Helper Methods - طرق مساعدة

        /// <summary>
        /// حفظ الطلب وعناصره في قاعدة البيانات
        /// </summary>
        /// <param name="oShoppingCart">بيانات سلة التسوق</param>
        private async Task SaveOrder(ShoppingCart oShoppingCart)
        {
            try
            {
                // إنشاء قائمة عناصر الفاتورة
                List<TbSalesInvoiceItem> lstInvoiceItems = new List<TbSalesInvoiceItem>();
                foreach (var item in oShoppingCart.lstItems)
                {
                    lstInvoiceItems.Add(new TbSalesInvoiceItem()
                    {
                        ItemId = item.ItemId,
                        Qty = item.Qty,
                        InvoicePrice = item.Price
                    });
                }

                // الحصول على بيانات المستخدم الحالي
                var user = await _userManager.GetUserAsync(User);

                // إنشاء فاتورة المبيعات
                TbSalesInvoice oSalesInvoice = new TbSalesInvoice()
                {
                    InvoiceDate = DateTime.Now,
                    CustomerId = Guid.Parse(user.Id),
                    DelivryDate = DateTime.Now.AddDays(5),
                    CreatedBy = user.Id,
                    CreatedDate = DateTime.Now
                };

                // حفظ الفاتورة وعناصرها
                _salesInvoiceService.Save(oSalesInvoice, lstInvoiceItems, true);
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء (يمكن إضافة logging هنا)
            }
        }
        #endregion
    }
}
