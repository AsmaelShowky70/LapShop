# LapShop - E-Commerce Application
## Project Documentation

---

## 📋 Project Overview

**LapShop** is a modern ASP.NET Core 10.0 e-commerce application built with Entity Framework Core for managing laptop sales and inventory. The application features user authentication, product management, shopping cart functionality, and comprehensive API endpoints.

### Technology Stack
- **Framework**: ASP.NET Core 10.0
- **Database**: SQL Server LocalDB
- **ORM**: Entity Framework Core (Code-First)
- **Authentication**: ASP.NET Core Identity
- **Architecture**: MVC with Separate API Controllers

---

## 🏗️ Project Structure

### Main Folders

#### 1. **LapShop/** (Main Web Application)
The main ASP.NET Core application containing controllers, views, and configuration.

**Key Subdirectories:**
- `ApiControllers/` - REST API endpoints for the frontend
- `Controllers/` - MVC controllers for web pages
- `Views/` - Razor templates for rendering HTML
- `Models/` - Data transfer objects and view models
- `Resources/` - Localization resources
- `wwwroot/` - Static files (CSS, JS, Images)
- `Filters/` - Custom action filters
- `Utilities/` - Helper functions and extensions

**Key Files:**
- `Program.cs` - Application startup and configuration
- `appsettings.json` - Configuration settings
- `LapShop.csproj` - Project file

---

#### 2. **Bl/** (Business Logic Layer)
Contains the business logic and data access layer for all entities.

**Key Files:**
- `LapShopContext.cs` - Entity Framework DbContext
- `Cls*.cs` - Business logic classes (Categories, Items, Orders, etc.)
- `Migrations/` - Database migration history

**Main Classes:**
- `ClsCategories` - Manage product categories
- `ClsItems` - Manage products/items
- `ClsItemTypes` - Manage item types
- `ClsItemImages` - Manage product images
- `ClsSalesInvoice` - Manage sales invoices
- `ClsSliders` - Manage promotional sliders
- `ClsSettings` - Manage application settings

---

#### 3. **Domains/** (Domain Models)
Entity models representing database tables and view models.

**Entity Classes:**
- `TbItem` - Product entity
- `TbCategory` - Category entity
- `TbCustomer` - Customer entity
- `TbSalesInvoice` - Sales invoice entity
- `TbSupplier` - Supplier entity
- `ApplicationUser` - User entity (from Identity)
- `ApplicationRole` - Role entity (from Identity)

**View Models:**
- `VwItemCategory` - View for item categories
- `VwSalesInvoice` - View for sales invoices
- `VwItem` - View for items with related data
- `VwItemsOutOfInvoice` - View for items not in invoices

---

## 🔌 Key Controllers

### Web Controllers (MVC)

#### **UsersController**
Handles user authentication (login, register, logout)
- **Namespace**: `LapShop.Controllers`
- **Key Methods**:
  - `Login(UserModel)` - Handles user login with email/password
  - `Register(UserModel)` - Creates new user account with auto-login
  - `LogOut()` - Signs out the user
  - `AccessDenied()` - Displays access denied page

**Authentication Details:**
- Uses ASP.NET Core Identity with UserManager and SignInManager
- Password Requirements: 6+ characters (no uppercase or special chars required)
- User roles: Optional (Customer role gracefully handled if missing)
- Persistent cookies with 720-minute expiration (12 hours)

---

#### **HomeController**
Displays the homepage with featured products and categories
- **Key Methods**:
  - `Index()` - Returns VmHomePage with featured items, sliders, and categories

---

#### **ItemsController** (Web)
Manages product catalog browsing and details
- Inherits from HomeController for shared data

---

#### **OrderController**
Manages shopping cart and order operations

---

#### **PagesController**
Manages static pages and content

---

### API Controllers (REST)

#### **ItemsController (API)**
Provides REST endpoints for product data
- **Route**: `/api/items`
- **Key Endpoints**:
  - `GET /api/items` - Get all products
  - `GET /api/items/{id}` - Get product by ID
  - `GET /api/items/GetByCategoryId/{categoryId}` - Get products by category
  - `POST /api/items` - Create/update product
  - `POST /api/items/Delete` - Delete product

**Response Format**: All responses use `ApiResponse` model:
```json
{
  "data": { /* product data */ },
  "errors": null,
  "statusCode": "200"
}
```

---

#### **SettingController (API)**
Provides access to application settings

---

## 📦 Models & DTOs

### **UserModel**
Data Transfer Object for user registration and login
```csharp
public class UserModel
{
    public string FirstName { get; set; }      // Required
    public string LastName { get; set; }       // Required
    public string Email { get; set; }          // Required, EmailAddress
    public string Password { get; set; }       // Required, 6+ chars
    public string? ReturnUrl { get; set; }     // Optional
}
```

---

### **ApiResponse**
Standard response format for all API endpoints
```csharp
public class ApiResponse
{
    public object Data { get; set; }           // Response data
    public object Errors { get; set; }         // Error messages
    public string StatusCode { get; set; }     // HTTP status code
}
```

---

### **VmHomePage**
View model for homepage
```csharp
public class VmHomePage
{
    public List<VwItem> lstAllItems { get; set; }
    public List<VwItem> lstRecommendedItems { get; set; }
    public List<VwItem> lstNewItems { get; set; }
    public List<VwItem> lstFreeDelivry { get; set; }
    public List<TbSlider> lstSliders { get; set; }
    public List<TbCategory> lstCategories { get; set; }
}
```

---

## 🔐 Authentication & Security

### ASP.NET Core Identity Configuration
Located in `Program.cs`:

```csharp
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<LapShopContext>();
```

### Cookie Configuration
```csharp
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Users/Login";
    options.AccessDeniedPath = "/Users/AccessDenied";
    options.Cookie.Name = "LapShopCookie";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(720);  // 12 hours
    options.SlidingExpiration = true;
});
```

### CORS Configuration
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
```

---

## 🗄️ Database Configuration

### Connection String
```
Server=(localdb)\MSSQLLocalDB; Database=LapShop; Trusted_Connection=True;
```

### Migrations
Located in `Bl/Migrations/`:
- Initial migration with all core tables
- Item-Category view migration
- Settings table migration
- ASP.NET Identity tables migration
- User fields migration
- Slider columns migration
- Settings columns migration
- Pages table migration

### Key Tables
- **AspNetUsers** - User accounts
- **AspNetRoles** - User roles
- **AspNetUserRoles** - User-Role mapping
- **TbItems** - Products
- **TbCategories** - Product categories
- **TbItemImages** - Product images
- **TbSalesInvoice** - Sales orders
- **TbSalesInvoiceItems** - Order line items
- **TbSliders** - Promotional sliders
- **TbSettings** - Application settings

---

## 🚀 Running the Application

### Development Mode
```bash
cd c:\Users\Asmael\Desktop\LapShop
dotnet run
```

### HTTPS URL
```
https://localhost:7159
```

### HTTP URL
```
http://localhost:5208
```

### API Testing
Use a tool like Postman or curl to test API endpoints:
```bash
# Get all items
curl https://localhost:7159/api/items

# Get item by ID
curl https://localhost:7159/api/items/1

# Get items by category
curl https://localhost:7159/api/items/GetByCategoryId/5
```

---

## 📝 Configuration Files

### **Program.cs**
Main application startup file containing:
- CORS configuration
- Entity Framework setup
- Identity configuration
- Dependency injection registration
- Session and cookie configuration
- Middleware pipeline setup
- Routing configuration

### **appsettings.json**
Application settings including:
- Database connection string
- Logging configuration
- Environment settings

### **appsettings.Development.json**
Development-specific settings

---

## 📚 Business Logic Layer (Bl)

### Service Interfaces & Implementations

Each entity has:
1. **Interface** (ICategories, IItems, etc.)
2. **Implementation** (ClsCategories, ClsItems, etc.)

Both use dependency injection in `Program.cs`:
```csharp
builder.Services.AddScoped<ICategories, ClsCategories>();
builder.Services.AddScoped<IItems, ClsItems>();
// ... more services
```

### Standard Operations
- `GetAll()` - Retrieve all records
- `GetById(int id)` - Get single record
- `Save(Entity)` - Insert or update
- `Delete(int id)` - Remove record

---

## 🐛 Fixed Issues

### Issue 1: API Not Returning Data
**Problem**: API endpoints were not returning data to frontend
**Cause**: CORS (Cross-Origin Resource Sharing) not configured
**Solution**: Added CORS policy in `Program.cs` with AllowAll policy

### Issue 2: New Account Registration Not Saving
**Problem**: User registration form appeared to work but data wasn't saved to database
**Causes**: 
1. Missing `return` statement in Register action
2. ReturnUrl validation error preventing form submission
**Solutions**:
1. Added `return` statement before redirect
2. Made ReturnUrl nullable (`string?`)
3. Added hidden input field in view
4. Added exception handling for missing Customer role

---

## 📋 Code Organization Standards

### Commenting
All files include:
- **Class documentation** (/// <summary>)
- **Method documentation** with parameters and return values
- **Property documentation** explaining purpose and requirements
- **Inline comments** for complex logic

### Code Structure
Files are organized with **#region** blocks for readability:
- `Private Fields` - Class variables
- `Constructor` - Class initialization
- `Public Methods` - Main functionality
- `Helper Methods` - Supporting functions
- `Event Handlers` - Event management

### Naming Conventions
- **Private Fields**: `_camelCase` (e.g., `_itemService`)
- **Public Properties**: `PascalCase` (e.g., `FirstName`)
- **Methods**: `PascalCase` (e.g., `GetById`)
- **Local Variables**: `camelCase` (e.g., `itemList`)

---

## 🔄 Dependency Injection

### Registered Services
In `Program.cs`:
```csharp
// Business Logic Services
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
```

### Injection Pattern
Controllers receive services via constructor:
```csharp
public ItemsController(IItems itemService)
{
    _itemService = itemService;
}
```

---

## 🎯 Development Guidelines

### Adding New Features
1. Create Entity in `Domains/Tb*.cs`
2. Create DbSet in `LapShopContext`
3. Create Business Logic in `Bl/Cls*.cs`
4. Register service in `Program.cs`
5. Create Controller (Web or API)
6. Create or update Views

### Database Changes
1. Modify Entity models
2. Create migration: `dotnet ef migrations add MigrationName`
3. Update database: `dotnet ef database update`

### API Endpoints
1. Create controller in `ApiControllers/`
2. Add [Route("api/[controller]")] attribute
3. Inherit from ControllerBase
4. Return ApiResponse for all endpoints

---

## 📞 Support & Contact

### Project Owner
- **Name**: Ali Shahin (based on Swagger comments)
- **Website**: https://itlegend.net/
- **Email**: info@itlegend.net

### License
IT Legend Licence

---

## 📅 Project History

### Recent Updates
- Fixed CORS configuration for API functionality
- Fixed user registration and database persistence
- Added comprehensive code documentation
- Organized code with #region blocks
- Added XML documentation comments

### Current Status
✅ Build: Successful
✅ Registration: Working
✅ Login: Working
✅ API: Functional with CORS enabled
✅ Database: Connected and migrated

---

## 🔗 Important Files Reference

| File | Purpose |
|------|---------|
| `Program.cs` | Application startup & configuration |
| `Controllers/UsersController.cs` | User authentication |
| `ApiControllers/ItemsController.cs` | Product API endpoints |
| `Controllers/HomeController.cs` | Homepage display |
| `Models/UserModel.cs` | User DTO |
| `Models/ApiResponse.cs` | API response model |
| `Bl/LapShopContext.cs` | Database context |
| `Domains/TbItem.cs` | Product entity |
| `appsettings.json` | Configuration |

---

## 🎓 Code Examples

### Login Example
```csharp
// POST: /Users/Login
[HttpPost]
public async Task<IActionResult> Login(UserModel model)
{
    var result = await _signInManager.PasswordSignInAsync(
        model.Email,
        model.Password,
        isPersistent: true,
        lockoutOnFailure: true
    );

    if (result.Succeeded)
        return Redirect(model.ReturnUrl ?? "~/");
    
    return View(model);
}
```

### API Endpoint Example
```csharp
// GET: /api/items/GetByCategoryId/5
[HttpGet("GetByCategoryId/{categoryId}")]
public ApiResponse GetByCategoryId(int categoryId)
{
    return new ApiResponse
    {
        Data = _itemService.GetAllItemsData(categoryId),
        Errors = null,
        StatusCode = "200"
    };
}
```

---

**Last Updated**: Current Session
**Status**: ✅ Production Ready
**Build**: Successful
