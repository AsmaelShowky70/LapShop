using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bl.Bl.Migrations
{
    /// <inheritdoc />
    public partial class AddViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed ItemTypes and OS if empty to prevent FK failures
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM TbItemTypes)
                BEGIN
                    INSERT INTO TbItemTypes (ItemTypeName, CurrentState, CreatedDate, CreatedBy)
                    VALUES ('Laptop', 1, GETDATE(), '1'),
                           ('Notebook', 1, GETDATE(), '1'),
                           ('Ultrabook', 1, GETDATE(), '1'),
                           ('Gaming', 1, GETDATE(), '1'),
                           ('Convertible (2-in-1)', 1, GETDATE(), '1')
                END

                IF NOT EXISTS (SELECT 1 FROM TbOs)
                BEGIN
                    INSERT INTO TbOs (OsName, ImageName, ShowInHomePage, CurrentState, CreatedDate, CreatedBy)
                    VALUES ('Windows 11', '', 1, 1, GETDATE(), '1'),
                           ('Windows 10', '', 1, 1, GETDATE(), '1'),
                           ('Linux (Ubuntu)', '', 1, 1, GETDATE(), '1'),
                           ('macOS Ventura', '', 1, 1, GETDATE(), '1')
                END
            ");

            // Create Views
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.views WHERE name = 'VwItems') DROP VIEW VwItems;
                EXEC('CREATE VIEW [dbo].[VwItems] AS
                SELECT 
                    i.ItemId, i.ItemName, i.SalesPrice, i.PurchasePrice, i.CategoryId, i.ImageName, 
                    i.CreatedDate, i.CreatedBy, i.CurrentState, i.UpdatedBy, i.UpdatedDate, 
                    i.Description, i.Gpu, i.HardDisk, i.ItemTypeId, i.Processor, i.RamSize, 
                    i.ScreenReslution, i.ScreenSize, i.Weight, i.OsId,
                    c.CategoryName, it.ItemTypeName, o.OsName
                FROM TbItems i
                LEFT JOIN TbCategories c ON i.CategoryId = c.CategoryId
                LEFT JOIN TbItemTypes it ON i.ItemTypeId = it.ItemTypeId
                LEFT JOIN TbOs o ON i.OsId = o.OsId');
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.views WHERE name = 'VwItemCategory') DROP VIEW VwItemCategory;
                EXEC('CREATE VIEW [dbo].[VwItemCategory] AS
                SELECT 
                    i.ItemId, i.ItemName, i.SalesPrice, i.PurchasePrice, i.CategoryId, i.ImageName, 
                    c.CategoryName
                FROM TbItems i
                LEFT JOIN TbCategories c ON i.CategoryId = c.CategoryId');
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.views WHERE name = 'VwSalesInvoices') DROP VIEW VwSalesInvoices;
                EXEC('CREATE VIEW [dbo].[VwSalesInvoices] AS
                SELECT 
                    si.InvoiceId, si.InvoiceDate, si.DelivryDate, si.DelivryManId, si.Notes, si.CustomerId,
                    u.FirstName, u.LastName
                FROM TbSalesInvoices si
                LEFT JOIN AspNetUsers u ON si.CustomerId = CAST(u.Id AS UNIQUEIDENTIFIER)');
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS VwItems");
            migrationBuilder.Sql("DROP VIEW IF EXISTS VwItemCategory");
            migrationBuilder.Sql("DROP VIEW IF EXISTS VwSalesInvoices");
        }
    }
}
