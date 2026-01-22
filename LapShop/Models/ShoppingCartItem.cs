namespace LapShop.Models
{
    /// <summary>
    /// ShoppingCartItem - عنصر واحد في سلة التسوق
    /// يمثل منتج واحد مع كميته وسعره والإجمالي
    /// </summary>
    public class ShoppingCartItem
    {
        /// <summary>
        /// رقم معرّف المنتج
        /// يُستخدم لربط العنصر بالمنتج الأصلي
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// اسم المنتج
        /// يُعرض للمستخدم في السلة
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// اسم صورة المنتج
        /// يُستخدم لعرض صورة المنتج
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// الكمية المطلوبة من المنتج
        /// يمكن تعديلها من قبل المستخدم
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// سعر الوحدة الواحدة للمنتج
        /// سعر البيع وقت الإضافة للسلة
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// الإجمالي لهذا العنصر
        /// يُحسب بضرب الكمية × السعر
        /// </summary>
        public decimal Total { get; set; }
    }
}
