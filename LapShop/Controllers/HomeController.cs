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
        public IActionResult Index()
        {
            VmHomePage vm = new VmHomePage();

            // تحميل المنتجات بطرق مختلفة (Skip/Take للـ pagination المبسطة)
            vm.lstAllItems = _itemService.GetAllItemsData(null).Skip(20).Take(20).ToList();
            vm.lstRecommendedItems = _itemService.GetAllItemsData(null).Skip(60).Take(10).ToList();
            vm.lstNewItems = _itemService.GetAllItemsData(null).Skip(90).Take(10).ToList();
            vm.lstFreeDelivry = _itemService.GetAllItemsData(null).Skip(200).Take(10).ToList();

            // تحميل المنزلقات والفئات
            vm.lstSliders = _sliderService.GetAll();
            vm.lstCategories = _categoryService.GetAll().Take(4).ToList();

            return View(vm);
        }
        #endregion
    }
}
