using LapShop.Bl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LapShop.Controllers
{
    /// <summary>
    /// PagesController - يتعامل مع الصفحات الثابتة والمحتوى الديناميكي
    /// يعرض صفحات مثل "حول"، "الشروط والأحكام"، وغيرها
    /// </summary>
    public class PagesController : Controller
    {
        #region Private Fields
        /// <summary>
        /// خدمة إدارة الصفحات
        /// </summary>
        private IPages _pageService;
        #endregion

        #region Constructor
        /// <summary>
        /// تهيئة الـ Controller مع خدمة الصفحات
        /// </summary>
        public PagesController(IPages pageService)
        {
            _pageService = pageService;
        }
        #endregion

        #region View Actions - عرض الصفحات

        /// <summary>
        /// عرض صفحة محددة برقم معرّفها
        /// </summary>
        /// <param name="pageId">رقم معرّف الصفحة</param>
        /// <returns>View مع محتوى الصفحة</returns>
        public ActionResult Index(int pageId)
        {
            // الحصول على بيانات الصفحة من قاعدة البيانات
            var page = _pageService.GetById(pageId);
            return View(page);
        }
        #endregion
    }
}
