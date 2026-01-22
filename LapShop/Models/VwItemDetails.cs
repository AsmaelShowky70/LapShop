namespace LapShop.Models
{
    /// <summary>
    /// VmItemDetails - نموذج عرض تفاصيل المنتج
    /// يحتوي على المنتج وصوره والمنتجات الموصى بها
    /// </summary>
    public class VmItemDetails
    {
        /// <summary>
        /// تهيئة النموذج بقوائم فارغة
        /// </summary>
        public VmItemDetails()
        {
            Item = new VwItem();
            lstItemImages = new List<TbItemImage>();
            lstRecommendedItems = new List<VwItem>();
        }

        /// <summary>
        /// بيانات المنتج الرئيسي (الاسم، السعر، الوصف، إلخ)
        /// </summary>
        public VwItem Item { get; set; }

        /// <summary>
        /// قائمة صور المنتج
        /// يمكن عرض صور متعددة للمنتج الواحد
        /// </summary>
        public List<TbItemImage> lstItemImages { get; set; }

        /// <summary>
        /// قائمة المنتجات الموصى بها
        /// تُعرض للمستخدم كاقتراحات إضافية
        /// </summary>
        public List<VwItem> lstRecommendedItems { get; set; }
    }
}
