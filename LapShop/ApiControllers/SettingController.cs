using LapShop.Bl;
using LapShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace LapShop.ApiControllers
{
    /// <summary>
    /// SettingController - API Endpoint لإعدادات التطبيق
    /// يوفر واجهة برمجية للوصول إلى إعدادات التطبيق الأساسية
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        #region Private Fields
        /// <summary>
        /// خدمة إدارة الإعدادات
        /// </summary>
        private ISettings _settingsService;
        #endregion

        #region Constructor
        /// <summary>
        /// تهيئة الـ API Controller مع خدمة الإعدادات
        /// </summary>
        public SettingController(ISettings settingsService)
        {
            _settingsService = settingsService;
        }
        #endregion

        #region GET Methods - الحصول على البيانات

        /// <summary>
        /// الحصول على جميع إعدادات التطبيق
        /// </summary>
        /// <returns>كائن TbSettings يحتوي على الإعدادات</returns>
        [HttpGet]
        public TbSettings Get()
        {
            var settings = _settingsService.GetAll();
            return settings;
        }

        /// <summary>
        /// الحصول على إعداد محدد برقم معرّفه
        /// </summary>
        /// <param name="id">رقم معرّف الإعداد</param>
        /// <returns>قيمة الإعداد</returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        #endregion

        #region POST Methods - إنشاء البيانات

        /// <summary>
        /// إضافة إعداد جديد
        /// </summary>
        /// <param name="value">قيمة الإعداد</param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        #endregion

        #region PUT Methods - تحديث البيانات

        /// <summary>
        /// تحديث إعداد محدد
        /// </summary>
        /// <param name="id">رقم معرّف الإعداد</param>
        /// <param name="value">القيمة الجديدة</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        #endregion

        #region DELETE Methods - حذف البيانات

        /// <summary>
        /// حذف إعداد محدد
        /// </summary>
        /// <param name="id">رقم معرّف الإعداد</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        #endregion
    }
}
