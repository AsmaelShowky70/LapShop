using Microsoft.AspNetCore.Mvc;
using LapShop.Models;
using Microsoft.AspNetCore.Identity;

namespace LapShop.Controllers
{
    /// <summary>
    /// UsersController - يتعامل مع جميع عمليات المستخدمين (التسجيل والدخول والخروج)
    /// يستخدم ASP.NET Core Identity للمصادقة والتفويض
    /// </summary>
    public class UsersController : Controller
    {
        #region Private Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        #endregion

        #region Constructor
        /// <summary>
        /// تهيئة controller مع الـ dependencies المطلوبة
        /// </summary>
        public UsersController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #endregion

        #region Login Methods

        /// <summary>
        /// عرض صفحة تسجيل الدخول
        /// </summary>
        /// <param name="returnUrl">الرابط المراد الذهاب إليه بعد الدخول</param>
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            UserModel model = new UserModel()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        /// <summary>
        /// معالجة عملية تسجيل الدخول
        /// </summary>
        /// <param name="model">بيانات المستخدم (البريد والكلمة المرور)</param>
        [HttpPost]
        public async Task<IActionResult> Login(UserModel model)
        {
            try
            {
                // التحقق من بيانات المستخدم باستخدام ASP.NET Core Identity
                var loginResult = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    isPersistent: true,  // تذكر المستخدم
                    lockoutOnFailure: true // قفل الحساب بعد محاولات فاشلة
                );

                if (loginResult.Succeeded)
                {
                    // تسجيل دخول ناجح - إعادة التوجيه
                    if (string.IsNullOrEmpty(model.ReturnUrl))
                        return Redirect("~/");
                    else
                        return Redirect(model.ReturnUrl);
                }
                else if (loginResult.IsLockedOut)
                {
                    ModelState.AddModelError("", "الحساب مقفول مؤقتاً. يرجى المحاولة لاحقاً");
                }
                else if (loginResult.IsNotAllowed)
                {
                    ModelState.AddModelError("", "الحساب غير مفعل");
                }
                else
                {
                    ModelState.AddModelError("", "فشل الدخول - تحقق من البيانات المدخلة");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"حدث خطأ: {ex.Message}");
            }

            return View(model);
        }

        /// <summary>
        /// تسجيل خروج المستخدم
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> LoginOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        #endregion

        #region Register Methods

        /// <summary>
        /// عرض صفحة إنشاء حساب جديد
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View(new UserModel());
        }

        /// <summary>
        /// معالجة عملية إنشاء حساب جديد
        /// </summary>
        /// <param name="model">بيانات المستخدم الجديد</param>
        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            try
            {
                // إنشاء المستخدم الجديد
                ApplicationUser user = new ApplicationUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email
                };

                // حفظ المستخدم في قاعدة البيانات
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // تسجيل دخول المستخدم تلقائياً بعد الإنشاء
                    var loginResult = await _signInManager.PasswordSignInAsync(
                        user,
                        model.Password,
                        isPersistent: true,
                        lockoutOnFailure: true
                    );

                    // محاولة إضافة المستخدم للـ Role "Customer" (بشكل آمن)
                    var myUser = await _userManager.FindByEmailAsync(user.Email);
                    if (myUser != null)
                    {
                        var roleExists = await _userManager.IsInRoleAsync(myUser, "Customer");
                        if (!roleExists)
                        {
                            try
                            {
                                // إضافة الـ role - لن يفشل إذا كان الـ role غير موجود
                                await _userManager.AddToRoleAsync(myUser, "Customer");
                            }
                            catch
                            {
                                // تجاهل الخطأ إذا كان الـ role غير موجود
                            }
                        }
                    }

                    if (loginResult.Succeeded)
                    {
                        return Redirect("/Order/OrderSuccess");
                    }
                }
                else
                {
                    // عرض أخطاء التحقق من الكلمة المرور أو البريد الإلكتروني
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"حدث خطأ: {ex.Message}");
            }

            return View("Register", model);
        }

        #endregion

        #region Authorization Methods

        /// <summary>
        /// عرض صفحة الوصول مرفوض
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion
    }
}
