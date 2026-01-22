namespace LapShop.Models
{
    /// <summary>
    /// ShoppingCart - نموذج سلة التسوق
    /// يتم تخزينها في Cookies ويحتوي على قائمة المنتجات والإجمالي
    /// </summary>
    public class ShoppingCart
    {
        /// <summary>
        /// تهيئة السلة بقائمة فارغة من العناصر
        /// </summary>
        public ShoppingCart()
        {
            lstItems = new List<ShoppingCartItem>();
        }

        /// <summary>
        /// قائمة عناصر السلة (المنتجات المضافة)
        /// </summary>
        public List<ShoppingCartItem> lstItems { get; set; }

        /// <summary>
        /// إجمالي سعر السلة
        /// يتم حسابه تلقائياً عند إضافة أو حذف منتج
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// كود الخصم (إن وُجد)
        /// يمكن استخدامه لتطبيق خصم على الشراء
        /// </summary>
        public string PromoCode { get; set; }
    }
}
