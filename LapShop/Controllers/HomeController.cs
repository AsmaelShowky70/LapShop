using Microsoft.AspNetCore.Mvc;
using LapShop.Models;
using Microsoft.EntityFrameworkCore;
using LapShop.Bl;

namespace LapShop.Controllers
{
    /// <summary>
    /// HomeController - يتعامل مع الصفحة الرئيسية والتنقل العام
    /// يحضر البيانات المختلفة مثل المنتجات والفئات والمنزلقات (Sliders)
    /// </summary>
    public class HomeController : Controller
    {
        #region Private Fields
        /// <summary>
        /// خدمة إدارة المنتجات
        /// </summary>
        private IItems _itemService;

        /// <summary>
        /// خدمة إدارة المنزلقات (Sliders)
        /// </summary>
        private ISliders _sliderService;

        /// <summary>
        /// خدمة إدارة الفئات
        /// </summary>
        private ICategories _categoryService;
        #endregion

        #region Constructor
        /// <summary>
        /// تهيئة الـ Controller مع الخدمات المطلوبة
        /// </summary>
        /// <param name="itemService">خدمة المنتجات</param>
        /// <param name="sliderService">خدمة المنزلقات</param>
        /// <param name="categoryService">خدمة الفئات</param>
        public HomeController(IItems itemService, ISliders sliderService, ICategories categoryService)
        {
            _itemService = itemService;
            _sliderService = sliderService;
            _categoryService = categoryService;
        }
        #endregion

        #region View Actions - عرض الصفحات

        /// <summary>
        /// عرض الصفحة الرئيسية مع جميع البيانات
        /// تحتوي على:
        /// - جميع المنتجات (مع pagination)
        /// - المنتجات الموصى بها
        /// - المنتجات الجديدة
        /// - المنتجات بالشحن المجاني
        /// - المنزلقات الإعلانية
        /// - أول 4 فئات
        /// </summary>
        /// <returns>View مع نموذج VmHomePage يحتوي على جميع البيانات</returns>
        public IActionResult Index(int? categoryId, int page = 1)
        {
            VmHomePage vm = new VmHomePage();

            var allItems = _itemService.GetAllItemsData(null);
            int pageSize = 12;

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                var filtered = _itemService.GetAllItemsData(categoryId);
                vm.lstAllItems = filtered
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                vm.lstAllItems = allItems
                    .OrderByDescending(a => a.CreatedDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }

            vm.lstRecommendedItems = allItems.Take(10).ToList();
            vm.lstNewItems = allItems.OrderByDescending(a => a.CreatedDate).Take(10).ToList();
            vm.lstFreeDelivry = allItems.Skip(10).Take(10).ToList();

            vm.lstSliders = _sliderService.GetAll();
            vm.lstCategories = _categoryService.GetAll().ToList();

            ViewBag.SelectedCategoryId = categoryId ?? 0;
            ViewBag.CurrentPage = page;

            return View(vm);
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var cookieValue = Microsoft.AspNetCore.Localization.CookieRequestCultureProvider
                    .MakeCookieValue(new Microsoft.AspNetCore.Localization.RequestCulture(culture));

                Response.Cookies.Append(
                    Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.DefaultCookieName,
                    cookieValue,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1)
                    });
            }

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            return LocalRedirect(returnUrl);
        }
        #endregion
    }
}
