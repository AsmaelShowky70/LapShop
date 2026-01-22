namespace LapShop.Models
{
    /// <summary>
    /// VmHomePage - نموذج عرض الصفحة الرئيسية
    /// يحتوي على جميع البيانات المطلوبة لعرض الصفحة الرئيسية
    /// بما فيها المنتجات والفئات والمنزلقات والإعدادات
    /// </summary>
    public class VmHomePage
    {
        /// <summary>
        /// تهيئة نموذج الصفحة الرئيسية بقوائم فارغة
        /// </summary>
        public VmHomePage()
        {
            lstAllItems = new List<VwItem>();
            lstRecommendedItems = new List<VwItem>();
            lstNewItems = new List<VwItem>();
            lstFreeDelivry = new List<VwItem>();
            lstCategories = new List<TbCategory>();
        }

        /// <summary>
        /// قائمة جميع المنتجات المتاحة
        /// تُعرض في قسم "جميع المنتجات"
        /// </summary>
        public List<VwItem> lstAllItems { get; set; }

        /// <summary>
        /// قائمة المنتجات الموصى بها
        /// تُعرض في قسم "منتجات موصى بها"
        /// </summary>
        public List<VwItem> lstRecommendedItems { get; set; }

        /// <summary>
        /// قائمة المنتجات الجديدة
        /// تُعرض في قسم "المنتجات الجديدة"
        /// </summary>
        public List<VwItem> lstNewItems { get; set; }

        /// <summary>
        /// قائمة المنتجات بالشحن المجاني
        /// تُعرض في قسم "شحن مجاني"
        /// </summary>
        public List<VwItem> lstFreeDelivry { get; set; }

        /// <summary>
        /// قائمة الفئات المعروضة بالصفحة الرئيسية
        /// عادة أول 4 فئات
        /// </summary>
        public List<TbCategory> lstCategories { get; set; }

        /// <summary>
        /// قائمة المنزلقات الإعلانية (Sliders)
        /// تُعرض في الجزء العلوي من الصفحة
        /// </summary>
        public List<TbSlider> lstSliders { get; set; }

        /// <summary>
        /// إعدادات التطبيق (الشركة، معلومات التواصل، إلخ)
        /// تُستخدم في تخطيط الصفحة
        /// </summary>
        public TbSettings Settings { get; set; }
    }
}
