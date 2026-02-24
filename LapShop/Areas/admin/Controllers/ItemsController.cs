using Microsoft.AspNetCore.Mvc;
using LapShop.Bl;
using LapShop.Models;
using LapShop.Utlities;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace LapShop.Areas.admin.Controllers
{
    [Authorize(Roles = "Admin,data entry")]
    [Area("admin")]
    public class ItemsController : Controller
    {
        public ItemsController(IItems item, ICategories category,
            IItemTypes itemTypes, IOs os)
        {
            oClsItems = item;
            oClsCategories = category;
            oClsItemTypes = itemTypes;
            oClsOs = os;
        }
        IItems oClsItems;
        ICategories oClsCategories;
        IItemTypes oClsItemTypes;
        IOs oClsOs;

        public IActionResult List()
        {

            ViewBag.lstCategories = oClsCategories.GetAll();
            var items = oClsItems.GetAllItemsData(null);
            return View(items);
        }

        public IActionResult Search(int id)
        {
            ViewBag.lstCategories = oClsCategories.GetAll();
            var items = oClsItems.GetAllItemsData(id);
            return View("List", items);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? itemId)
        {
            var item = new Models.TbItem();
            PopulateDropdowns();

            if (itemId != null)
            {
                item = oClsItems.GetById(Convert.ToInt32(itemId));
            }
            else
            {
                // for new items, pre-select first available ItemType and OS so dropdowns are not empty
                var lstItemTypes = (List<TbItemType>)ViewBag.lstItemTypes;
                var lstOs = (List<TbO>)ViewBag.lstOs;

                var firstType = lstItemTypes.FirstOrDefault();
                if (firstType != null)
                    item.ItemTypeId = firstType.ItemTypeId;

                var firstOs = lstOs.FirstOrDefault();
                if (firstOs != null)
                    item.OsId = firstOs.OsId;
            }
            return View(item);
        }

        private void PopulateDropdowns()
        {
            ViewBag.lstCategories = oClsCategories.GetAll();

            var lstItemTypes = oClsItemTypes.GetAll();
            if (lstItemTypes == null || !lstItemTypes.Any())
            {
                lstItemTypes = new List<TbItemType>
                {
                    new TbItemType { ItemTypeId = 1, ItemTypeName = "Laptop" },
                    new TbItemType { ItemTypeId = 2, ItemTypeName = "Notebook" },
                    new TbItemType { ItemTypeId = 3, ItemTypeName = "Ultrabook" },
                    new TbItemType { ItemTypeId = 4, ItemTypeName = "Gaming" },
                    new TbItemType { ItemTypeId = 5, ItemTypeName = "Convertible (2-in-1)" }
                };
            }
            ViewBag.lstItemTypes = lstItemTypes;

            var lstOs = oClsOs.GetAll();
            if (lstOs == null || !lstOs.Any())
            {
                lstOs = new List<TbO>
                {
                    new TbO { OsId = 1, OsName = "Windows 11" },
                    new TbO { OsId = 2, OsName = "Windows 10" },
                    new TbO { OsId = 3, OsName = "Linux (Ubuntu)" },
                    new TbO { OsId = 4, OsName = "macOS Ventura" }
                };
            }
            ViewBag.lstOs = lstOs;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(TbItem item, List<IFormFile> Files)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View("Edit", item);
            }

            if (Files != null && Files.Any())
            {
                item.ImageName = await Helper.UploadImage(Files, "Items");
            }

            var success = oClsItems.Save(item);
            if (!success)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء حفظ المنتج في قاعدة البيانات.");
                PopulateDropdowns();
                return View("Edit", item);
            }

            return RedirectToAction("List");
        }

        public IActionResult Delete(int itemId)
        {
            oClsItems.Dekete(itemId);
            return RedirectToAction("List");
        }
    }
}
