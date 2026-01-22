using System.ComponentModel.DataAnnotations;

namespace LapShop.Models
{
    /// <summary>
    /// UserModel - نموذج بيانات المستخدم
    /// يُستخدم لنقل بيانات التسجيل والدخول بين العرض (View) والـ Controller
    /// Data Transfer Object (DTO) للمصادقة والتفويض
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// الاسم الأول للمستخدم
        /// إلزامي - يجب ملأه عند التسجيل
        /// </summary>
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        public string FirstName { get; set; }

        /// <summary>
        /// اسم العائلة للمستخدم
        /// إلزامي - يجب ملأه عند التسجيل
        /// </summary>
        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        public string LastName { get; set; }

        /// <summary>
        /// البريد الإلكتروني للمستخدم
        /// إلزامي - يجب أن يكون صيغة بريد إلكتروني صحيحة
        /// يُستخدم كاسم مستخدم فريد في النظام
        /// </summary>
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; }

        /// <summary>
        /// كلمة المرور
        /// إلزامية - الحد الأدنى 6 أحرف
        /// لا تتطلب أحرف كبيرة أو أحرف خاصة
        /// </summary>
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string Password { get; set; }

        /// <summary>
        /// رابط الإعادة (Return URL)
        /// اختياري - إذا تم تعيينه، سيتم إعادة توجيه المستخدم لهذا الرابط بعد التسجيل/الدخول
        /// null = إعادة توجيه إلى الصفحة الرئيسية
        /// </summary>
        public string? ReturnUrl { get; set; }
    }
}

