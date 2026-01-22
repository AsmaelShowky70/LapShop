using LapShop.Bl;
using LapShop.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LapShop.ApiControllers
{
    /// <summary>
    /// ItemsController - API Controller
    /// يوفر endpoints لإدارة المنتجات (الحصول على المنتجات، البحث، الإضافة، الحذف)
    /// يُستخدم من قبل التطبيق الأمامي (Frontend) للحصول على بيانات المنتجات
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        #region Private Fields
        /// <summary>
        /// خدمة إدارة المنتجات - Business Logic Layer
        /// </summary>
        private IItems _itemService;
        #endregion

        #region Constructor
        /// <summary>
        /// تهيئة الـ API Controller مع خدمة المنتجات
        /// </summary>
        /// <param name="itemService">خدمة إدارة المنتجات من الـ DI Container</param>
        public ItemsController(IItems itemService)
        {
            _itemService = itemService;
        }
        #endregion

        #region GET Methods - الحصول على البيانات

        /// <summary>
        /// الحصول على جميع المنتجات من قاعدة البيانات
        /// </summary>
        /// <returns>ApiResponse يحتوي على قائمة جميع المنتجات</returns>
        [HttpGet]
        public ApiResponse Get()
        {
            ApiResponse oApiResponse = new ApiResponse();
            oApiResponse.Data = _itemService.GetAll();
            oApiResponse.Errors = null;
            oApiResponse.StatusCode = "200";
            return oApiResponse;
        }

        /// <summary>
        /// الحصول على منتج واحد برقم معرّفه
        /// </summary>
        /// <param name="id">رقم معرّف المنتج</param>
        /// <returns>ApiResponse يحتوي على بيانات المنتج</returns>
        [HttpGet("{id}")]
        public ApiResponse Get(int id)
        {
            ApiResponse oApiResponse = new ApiResponse();
            oApiResponse.Data = _itemService.GetById(id);
            oApiResponse.Errors = null;
            oApiResponse.StatusCode = "200";
            return oApiResponse;
        }

        /// <summary>
        /// الحصول على جميع المنتجات في فئة معينة
        /// </summary>
        /// <param name="categoryId">رقم معرّف الفئة</param>
        /// <returns>ApiResponse يحتوي على المنتجات التابعة للفئة</returns>
        [HttpGet("GetByCategoryId/{categoryId}")]
        public ApiResponse GetByCategoryId(int categoryId)
        {
            ApiResponse oApiResponse = new ApiResponse();
            oApiResponse.Data = _itemService.GetAllItemsData(categoryId);
            oApiResponse.Errors = null;
            oApiResponse.StatusCode = "200";
            return oApiResponse;
        }
        #endregion

        #region POST Methods - إنشاء وتعديل البيانات

        /// <summary>
        /// إضافة أو تعديل منتج
        /// </summary>
        /// <param name="item">بيانات المنتج</param>
        /// <returns>ApiResponse يؤكد إتمام العملية</returns>
        [HttpPost]
        public ApiResponse Post([FromBody] TbItem item)
        {
            try
            {
                _itemService.Save(item);
                ApiResponse oApiResponse = new ApiResponse();
                oApiResponse.Data = "done";
                oApiResponse.Errors = null;
                oApiResponse.StatusCode = "200";
                return oApiResponse;
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء وإرسال رسالة خطأ
                ApiResponse oApiResponse = new ApiResponse();
                oApiResponse.Data = null;
                oApiResponse.Errors = ex.Message;
                oApiResponse.StatusCode = "502";
                return oApiResponse;
            }
        }
        #endregion

        #region DELETE Methods - حذف البيانات

        /// <summary>
        /// حذف منتج برقم معرّفه
        /// </summary>
        /// <param name="id">رقم معرّف المنتج المراد حذفه</param>
        [HttpPost]
        [Route("Delete")]
        public void Delete([FromBody] int id)
        {
            _itemService.Dekete(id);
        }
        #endregion
    }
}
