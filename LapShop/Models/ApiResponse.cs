namespace LapShop.Models
{
    /// <summary>
    /// ApiResponse - نموذج للاستجابة الموحدة من الـ API
    /// يتم استخدام هذا النموذج في جميع API endpoints لضمان استجابة موحدة
    /// يحتوي على البيانات والأخطاء وكود الحالة
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// البيانات المرسلة من الـ API
        /// يمكن أن تكون قائمة، كائن واحد، أو null
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// الأخطاء إن وجدت
        /// يمكن أن تحتوي على رسالة خطأ واحدة أو قائمة أخطاء
        /// </summary>
        public object Errors { get; set; }

        /// <summary>
        /// كود حالة الـ API
        /// 200 = نجاح العملية
        /// 400 = خطأ في الطلب
        /// 502 = خطأ في الخادم
        /// </summary>
        public string StatusCode { get; set; }
    }
}

