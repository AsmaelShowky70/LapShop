# LapShop - Quick Reference Guide
## للمطورين / For Developers

---

## 🚀 Quick Start

### Setup & Run
```bash
# Navigate to project
cd c:\Users\Asmael\Desktop\LapShop

# Restore packages
dotnet restore

# Build project
dotnet build

# Run application
dotnet run

# Access the app
https://localhost:7159
http://localhost:5208
```

---

## 📍 File Locations Reference

### Controllers
```
LapShop/
├── Controllers/
│   ├── HomeController.cs          ← Homepage
│   ├── ItemsController.cs         ← Products page
│   ├── OrderController.cs         ← Shopping cart
│   ├── PagesController.cs         ← Static pages
│   └── UsersController.cs         ← Login/Register
├── ApiControllers/
│   ├── ItemsController.cs         ← /api/items
│   ├── SettingController.cs       ← /api/settings
│   └── ValuesController.cs        ← Test endpoint
```

### Models
```
LapShop/Models/
├── ApiResponse.cs                 ← API response template
├── UserModel.cs                   ← Login/Register DTO
└── VmHomePage.cs                  ← Homepage view model
```

### Business Logic
```
Bl/
├── ClsCategories.cs               ← Categories CRUD
├── ClsItems.cs                    ← Products CRUD
├── ClsItemTypes.cs                ← Item types CRUD
├── ClsSalesInvoice.cs             ← Orders CRUD
├── ClsSliders.cs                  ← Promotions CRUD
├── LapShopContext.cs              ← Database context
└── Migrations/                    ← Database changes
```

### Entities
```
Domains/
├── TbItem.cs                      ← Product entity
├── TbCategory.cs                  ← Category entity
├── TbCustomer.cs                  ← Customer entity
├── TbSalesInvoice.cs              ← Order entity
├── VwItem.cs                      ← Product view
└── ApplicationUser.cs             ← User (from Identity)
```

---

## 🔑 Key Routes

### Web Pages
| Route | Controller | Action | Purpose |
|-------|-----------|--------|---------|
| `/` | Home | Index | Homepage |
| `/Users/Login` | Users | Login | Login page |
| `/Users/Register` | Users | Register | Registration page |
| `/Users/AccessDenied` | Users | AccessDenied | Access denied |
| `/Users/LogOut` | Users | LogOut | Logout |
| `/Items` | Items | Index | Products listing |
| `/Order/...` | Order | ... | Shopping cart |
| `/Pages/...` | Pages | ... | Static pages |

### API Endpoints
| Method | Route | Purpose |
|--------|-------|---------|
| GET | `/api/items` | All products |
| GET | `/api/items/{id}` | Get product |
| GET | `/api/items/GetByCategoryId/{catId}` | Products by category |
| POST | `/api/items` | Create/update product |
| POST | `/api/items/Delete` | Delete product |
| GET | `/api/settings` | App settings |

---

## 🛠️ Common Tasks

### Add New Field to User
1. Edit `Domains/ApplicationUser.cs`
2. Create migration: `dotnet ef migrations add AddFieldName`
3. Update DB: `dotnet ef database update`

### Add New Product Type
1. Create entity in `Domains/Tb*.cs`
2. Add DbSet to `Bl/LapShopContext.cs`
3. Create business logic in `Bl/Cls*.cs`
4. Register service in `Program.cs`:
   ```csharp
   builder.Services.AddScoped<IMyService, ClsMyService>();
   ```
5. Create controller in `Controllers/` or `ApiControllers/`

### Update Database
```bash
# Create migration
dotnet ef migrations add MigrationName --project Bl --startup-project LapShop

# Apply migration
dotnet ef database update --project Bl --startup-project LapShop

# Remove last migration
dotnet ef migrations remove --project Bl --startup-project LapShop
```

### Test API Endpoint
```bash
# Using curl
curl https://localhost:7159/api/items

curl https://localhost:7159/api/items/1

curl https://localhost:7159/api/items/GetByCategoryId/5

# Using Postman
1. Import the API
2. Set base URL: https://localhost:7159
3. Test each endpoint
```

---

## 📦 Dependencies

### NuGet Packages (Key)
- **Microsoft.EntityFrameworkCore.SqlServer** - Database provider
- **Microsoft.AspNetCore.Identity** - Authentication
- **Microsoft.AspNetCore.Identity.EntityFrameworkCore** - Identity DB integration
- **Microsoft.EntityFrameworkCore.Tools** - Migration tools

### Project References
- **LapShop** (main app) → **Bl** (business logic) → **Domains** (models)

---

## 🔐 Authentication Details

### Password Policy
```csharp
// From Program.cs
options.Password.RequiredLength = 6;              // Min 6 characters
options.Password.RequireNonAlphanumeric = false;  // No special chars needed
options.Password.RequireUppercase = false;        // No uppercase needed
options.User.RequireUniqueEmail = true;           // Email must be unique
```

### Login Session
```csharp
// Cookie expires in 12 hours
options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
// Remember user even after browser close
options.SlidingExpiration = true;
```

### Redirect After Login
```csharp
// If ReturnUrl is provided, redirect there; otherwise go to home
if (string.IsNullOrEmpty(model.ReturnUrl))
    return Redirect("~/");
else
    return Redirect(model.ReturnUrl);
```

---

## 🐛 Troubleshooting

### Issue: "Database not found"
**Solution:**
```bash
# Ensure LocalDB is running and create the database
dotnet ef database update --project Bl
```

### Issue: "Package not found"
**Solution:**
```bash
# Restore NuGet packages
dotnet restore
```

### Issue: "Role 'Customer' does not exist"
**Solution:** The application gracefully handles this - just create the role:
```csharp
// In a controller or migration
var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
await roleManager.CreateAsync(new IdentityRole("Customer"));
```

### Issue: "CORS policy blocked request"
**Solution:** Already fixed! CORS is configured in `Program.cs`:
```csharp
app.UseCors("AllowAll");  // Allows all origins, methods, and headers
```

### Issue: Build fails
**Solution:**
```bash
# Clean build
dotnet clean
dotnet build

# Or remove bin/obj folders
Remove-Item -Recurse -Force bin, obj
dotnet build
```

---

## 📊 Project Statistics

- **Total Controllers**: 5 (HomeController, ItemsController, OrderController, PagesController, UsersController)
- **Total API Controllers**: 3 (ItemsController, SettingController, ValuesController)
- **Business Logic Classes**: 10+
- **Entity Models**: 15+
- **Database Tables**: 20+
- **Lines of Documentation**: 500+

---

## 💡 Best Practices

### When Writing New Code
1. ✅ Add XML documentation comments
2. ✅ Use `#region` blocks for organization
3. ✅ Follow naming conventions (PascalCase for public, camelCase for private)
4. ✅ Use dependency injection, not `new` keyword
5. ✅ Return `ApiResponse` from API endpoints
6. ✅ Add try-catch for database operations
7. ✅ Use async/await for database queries

### When Making Database Changes
1. ✅ Always create a migration first
2. ✅ Test the migration before pushing
3. ✅ Update related entities and views
4. ✅ Document the changes in code comments

### When Adding New Endpoints
1. ✅ Use appropriate HTTP verbs (GET, POST, PUT, DELETE)
2. ✅ Follow REST conventions
3. ✅ Add error handling with try-catch
4. ✅ Return consistent ApiResponse format
5. ✅ Add [HttpGet], [HttpPost] attributes
6. ✅ Document with XML comments

---

## 📞 Useful Commands

```bash
# Build and run
dotnet build && dotnet run

# Build only
dotnet build

# Run specific project
dotnet run --project LapShop

# Show NuGet updates
dotnet package update --project LapShop

# Entity Framework Commands
dotnet ef migrations list --project Bl
dotnet ef migrations add MigrationName --project Bl --startup-project LapShop
dotnet ef database update --project Bl --startup-project LapShop
dotnet ef dbcontext info --project Bl

# Testing
dotnet test

# Publish
dotnet publish -c Release -o ./publish
```

---

## 🎯 Current Status Checklist

- ✅ Project builds successfully
- ✅ User registration working
- ✅ User login working
- ✅ API endpoints functional
- ✅ CORS properly configured
- ✅ Database migrations applied
- ✅ Code well-documented
- ✅ Code organized with regions

---

## 📚 Additional Resources

### Microsoft Docs
- ASP.NET Core: https://docs.microsoft.com/aspnet/core
- Entity Framework Core: https://docs.microsoft.com/ef/core
- ASP.NET Identity: https://docs.microsoft.com/aspnet/core/security/authentication/identity

### Project Owner
- **Website**: https://itlegend.net/
- **Email**: info@itlegend.net

---

## 🔄 Quick Database Operations

### View Database
```bash
# In SQL Server Management Studio (SSMS)
# Server: (localdb)\MSSQLLocalDB
# Database: LapShop
```

### Reset Database
```bash
# Warning: Deletes all data!
dotnet ef database drop --project Bl
dotnet ef database update --project Bl
```

### Add Test Data
```csharp
// In any controller method
var context = serviceProvider.GetRequiredService<LapShopContext>();
var newItem = new TbItem { /* set properties */ };
context.Items.Add(newItem);
await context.SaveChangesAsync();
```

---

## 🎓 Learning Path

1. **Start with**: `Program.cs` - Understand application startup
2. **Then read**: `Controllers/HomeController.cs` - Simple controller
3. **Then read**: `Controllers/UsersController.cs` - Authentication
4. **Then read**: `ApiControllers/ItemsController.cs` - API pattern
5. **Then read**: `Bl/ClsItems.cs` - Business logic
6. **Finally**: Create your own controller following the pattern

---

**Document Version**: 1.0
**Last Updated**: Current Session
**Status**: ✅ Active Project
