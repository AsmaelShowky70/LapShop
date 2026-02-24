using LapShop.Bl;
using LapShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// تكوين الخدمات الأساسية (Services Configuration)
// ============================================================================

builder.Services.AddLocalization();
builder.Services.AddControllersWithViews();

// تفعيل CORS للسماح بالطلبات من أي مصدر (للـ API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

#region Entity Framework - قاعدة البيانات والـ Identity
builder.Services.AddDbContext<LapShopContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        }));

// تكوين ASP.NET Core Identity (المصادقة والتفويض)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<LapShopContext>();
#endregion

#region Custom Services - الخدمات المخصصة (Business Logic)
// تسجيل الخدمات المخصصة للـ dependency injection
builder.Services.AddScoped<ICategories, ClsCategories>();
builder.Services.AddScoped<IItems, ClsItems>();
builder.Services.AddScoped<IItemTypes, ClsItemTypes>();
builder.Services.AddScoped<IOs, ClsOs>();
builder.Services.AddScoped<IItemImages, ClsItemImages>();
builder.Services.AddScoped<ISalesInvoice, ClsSalesInvoice>();
builder.Services.AddScoped<ISalesInvoiceItems, ClsSalesInvoiceItems>();
builder.Services.AddScoped<ISliders, ClsSliders>();
builder.Services.AddScoped<ISettings, ClsSettings>();
builder.Services.AddScoped<IPages, ClsPages>();
#endregion

#region Session and Cookies - جلسة المستخدم والـ Cookies
// تفعيل الجلسات والـ Cookies
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

// تكوين خيارات الـ Cookie للمصادقة
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Users/AccessDenied";
    options.Cookie.Name = "LapShopCookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(720);  // 12 ساعة
    options.LoginPath = "/Users/Login";
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});
#endregion

#region Swagger Configuration - توثيق الـ API (معطّل حالياً)
// يمكن تفعيل Swagger لتوثيق الـ API التلقائي
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Version = "v1",
//        Title = "Lao Shop Api",
//        Description = "api to access items and related categories",
//        TermsOfService = new Uri("https://itlegend.net/"),
//        Contact = new OpenApiContact
//        {
//            Email = "info@itlegend.net",
//            Name = "Ali Shahin",
//            Url = new Uri("https://itlegend.net/")
//        },
//        License = new OpenApiLicense
//        {
//            Name = "It Legend Licence",
//            Url = new Uri("https://itlegend.net/")
//        }
//    });

//    var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var fullPath = Path.Combine(AppContext.BaseDirectory, xmlComments);
//    options.IncludeXmlComments(fullPath);
//}); 
#endregion

#region Application Builder - بناء التطبيق وتكوين الـ Pipeline
var app = builder.Build();

// التعامل مع الأخطاء والأمان
if (!app.Environment.IsDevelopment())
{
    // معالج الأخطاء - يحول الأخطاء إلى صفحة خطأ آمنة
    app.UseExceptionHandler("/Home/Error");
    // HSTS (HTTP Strict Transport Security) - يفرض HTTPS على جميع الاتصالات
    // المدة الافتراضية: 30 يوم - يمكن تعديلها حسب احتياجاتك
    app.UseHsts();
}
#endregion

#region Swagger UI - توثيق الـ API (معطّل حالياً)
// يمكن تفعيل Swagger UI لعرض توثيق الـ API التفاعلي
//app.UseSwagger();
//app.UseSwaggerUI(options =>
//{
//    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
//    options.RoutePrefix = string.Empty;
//}); 
#endregion

#region Middleware Pipeline - خط أنابيب الطلبات والاستجابات
// إعادة التوجيه من HTTP إلى HTTPS - تحسين الأمان
app.UseHttpsRedirection();

// تقديم الملفات الثابتة (CSS, JS, Images, إلخ)
app.UseStaticFiles();

// تفعيل نظام التوجيه (Routing)
app.UseRouting();

var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("ar")
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

app.UseRequestLocalization(localizationOptions);

// تفعيل CORS - السماح بالطلبات من جميع المصادر
app.UseCors("AllowAll");

// المصادقة والتحقق من الهوية
app.UseAuthentication();
app.UseAuthorization();

// الجلسات - تخزين بيانات المستخدم
app.UseSession();
#endregion

#region Routing Configuration - تكوين المسارات والـ Endpoints
app.UseEndpoints(endpoints =>
{
    // مسار المناطق (Areas) - للإدارة والوحدات المنفصلة
    endpoints.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}");

    // مسار الصفحات - للمجالات الأفقية والصفحات المخصصة
    endpoints.MapControllerRoute(
    name: "LandingPages",
    pattern: "{area:exists}/{controller=Home}/{action=Index}");

    // المسار الافتراضي - يتم استخدامه عند عدم تطابق أي مسار آخر
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    // مسار مخصص - للاختبار والتطوير
    endpoints.MapControllerRoute(
    name: "ali",
    pattern: "ali/{controller=Home}/{action=Index}/{id?}");
});
#endregion

#region Application Startup - بدء التطبيق
// تشغيل التطبيق
app.Run();
#endregion
