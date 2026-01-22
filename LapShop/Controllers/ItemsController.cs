using Microsoft.AspNetCore.Mvc;
using LapShop.Bl;
using LapShop.Models;

namespace LapShop.Controllers
{
    /// <summary>
    /// ItemsController - يتعامل مع عرض المنتجات والتفاصيل
    /// يعرض قائمة المنتجات وتفاصيل كل منتج مع الصور والمنتجات الموصى بها
    /// </summary>
    public class ItemsController : Controller
    {
        #region Private Fields
        /// <summary>
        /// خدمة إدارة المنتجات
        /// </summary>
        private IItems _itemService;

        /// <summary>
        /// خدمة إدارة صور المنتجات
        /// </summary>
        private IItemImages _itemImagesService;
        #endregion

        #region Constructor
        /// <summary>
        /// تهيئة الـ Controller مع الخدمات المطلوبة
        /// </summary>
        public ItemsController(IItems itemService, IItemImages itemImagesService)
        {
            _itemService = itemService;
            _itemImagesService = itemImagesService;
        }
        #endregion

        #region View Actions - عرض الصفحات

        /// <summary>
        /// عرض تفاصيل منتج واحد مع الصور والمنتجات الموصى بها
        /// </summary>
        /// <param name="id">رقم معرّف المنتج</param>
        /// <returns>View مع تفاصيل المنتج</returns>
        public IActionResult ItemDetails(int id)
        {
            // الحصول على بيانات المنتج
            var item = _itemService.GetItemId(id);

            // إنشاء نموذج العرض مع البيانات
            VmItemDetails vm = new VmItemDetails();
            vm.Item = item;
            vm.lstRecommendedItems = _itemService.GetRecommendedItems(id).Take(20).ToList();
            vm.lstItemImages = _itemImagesService.GetByItemId(id);

            return View(vm);
        }

        /// <summary>
        /// عرض قائمة جميع المنتجات
        /// </summary>
        /// <returns>View بقائمة المنتجات</returns>
        public IActionResult ItemList()
        {
            return View();
        }
        #endregion
    }
}
