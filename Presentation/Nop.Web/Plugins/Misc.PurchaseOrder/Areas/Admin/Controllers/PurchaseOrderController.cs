using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;
using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Services;
using Nop.Services.Catalog;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    public class PurchaseOrderController : BasePluginController
    {

        private readonly IPurchaseOrderModelFactory _purchaseOrderModelFactory;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductSupplierMapping> _productSupplierMappingRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly ISupplierProductMappingService _supplierProductMappingService;
        private readonly IProductService _productService;
        private readonly IRepository<SupplierProductMapping> _supplierProductMappingRepository;
        private readonly ISupplierService _supplierService;
        private readonly IPurchaseOrderService _purchaseOrderService;
        

        public PurchaseOrderController(IPurchaseOrderModelFactory purchaseOrderModelFactory, IRepository<Product> productRepository,
                                          IRepository<ProductSupplierMapping> productSupplierMappingRepository,
                                          IRepository<Supplier> supplierRepository, ISupplierProductMappingService supplierProductMappingService, IProductService productService, IRepository<SupplierProductMapping> supplierProductMappingRepository, ISupplierService supplierService, IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderModelFactory = purchaseOrderModelFactory;
            _productRepository = productRepository;
            _productSupplierMappingRepository = productSupplierMappingRepository;
            _supplierRepository = supplierRepository;
            _supplierProductMappingService = supplierProductMappingService;
            _productService = productService;
            _supplierProductMappingRepository = supplierProductMappingRepository;
            _supplierService = supplierService;
            _purchaseOrderService = purchaseOrderService;
        }




        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]

        // GET: Display Search Form
        public virtual async Task<IActionResult> PurchaseList()
        {
            var model = await _purchaseOrderModelFactory.PreparePurchaseOrderSearchModelAsync(new PurchaseOrderSearchModel());
            return View("~/Plugins/Misc.PurchaseOrder/Areas/Admin/Views/PurchaseOrder/PurchaseList.cshtml", model);
        }

        // POST: Handle Search Request (AJAX)
        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        [HttpPost]
        public virtual async Task<IActionResult> PurchaseList(PurchaseOrderSearchModel searchModel)
        {
            var model = await _purchaseOrderModelFactory.PreparePurchaseOrderListModelAsync(searchModel);
            return Json(model);
        }








        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        public virtual IActionResult AddProducts()
        {
            // Fetch all suppliers using the SupplierRepository (no need for ISupplierService if it's causing issues)
            var allSuppliers = _supplierRepository.Table
                .OrderBy(s => s.Name) // Order them by name or any other criteria
                .ToList();

            // Fetch products as per your logic
            var model = GetProductsBySupplier(null);  // Adjust according to your data fetching logic

            // Pass the suppliers list to the view via ViewBag
            ViewBag.AllSuppliers = allSuppliers;

            return View("~/Plugins/Misc.PurchaseOrder/Areas/Admin/Views/PurchaseOrder/AddProducts.cshtml", model);
        }


        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        [HttpGet]
        public virtual JsonResult GetProductsBySupplierId(int? supplierId)
        {
            var products = GetProductsBySupplier(supplierId);
            return Json(products);
        }



        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        private List<SelectedProductModel> GetProductsBySupplier(int? supplierId)
        {
            var supplierproductMappings = _supplierProductMappingRepository.Table.ToList();
            var suppliers = _supplierRepository.Table.ToList();
            var products = _productRepository.Table.ToList();

            var query = from ps in supplierproductMappings
                        join p in products on ps.ProductId equals p.Id
                        join s in suppliers on ps.SupplierId equals s.Id
                        select new { Product = p, Supplier = s, Mapping = ps };

            if (supplierId.HasValue)
            {
                query = query.Where(x => x.Mapping.SupplierId == supplierId.Value);
            }

            var productModels = query.Select(x => new SelectedProductModel
            {
                ProductId = x.Product.Id,
                ProductName = x.Product.Name,
                SKU = x.Product.Sku,
                QuantityToOrder = x.Mapping.QuantityToOrder,
                UnitCost = x.Mapping.UnitCost,
                LineTotal = x.Mapping.LineTotal,
                CurrentStock = x.Product.StockQuantity, // ✅ Added this line
                SupplierId = x.Supplier.Id,
                SupplierName = x.Supplier.Name
            }).ToList();

            return productModels;
        }


        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        [HttpGet]
        public virtual IActionResult ProductListPartial(int? supplierId)
        {
            var products = GetProductsBySupplier(supplierId);
            return PartialView("~/Plugins/Misc.PurchaseOrder/Areas/Admin/Views/PurchaseOrder/_ProductList.cshtml", products);
        }

        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        [HttpGet]
        public async Task<IActionResult> AddProductPopup(int supplierId)
        {
            var supplierProducts = _productSupplierMappingRepository.Table
                .Where(x => x.SupplierId == supplierId)
                .ToList();

            var model = new List<AddProductPopupModel>();

            foreach (var sp in supplierProducts)
            {
                var product = await _productService.GetProductByIdAsync(sp.ProductId);
                if (product != null)
                {
                    model.Add(new AddProductPopupModel
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Published = product.Published
                    });
                }
            }

            ViewBag.SupplierId = supplierId;

            return View("~/Plugins/Misc.PurchaseOrder/Areas/Admin/Views/PurchaseOrder/AddProductPopup.cshtml", model);
        }

        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        [HttpPost]
        public IActionResult SaveSelectedProductsFromPopup([FromBody] SaveProductPopupRequestModel model)
        {
            if (model == null || model.SelectedProductIds == null || !model.SelectedProductIds.Any())
            {
                return Json(new { success = false, message = "No products selected." });
            }

            foreach (var productId in model.SelectedProductIds)
            {
                var mapping = new SupplierProductMapping
                {
                    SupplierId = model.SupplierId,
                    ProductId = productId,
                    QuantityToOrder = 1,
                    UnitCost = 0m,
                    LineTotal = 0m
                };

                _supplierProductMappingService.Insert(mapping);
            }

            // Redirect to AddProductPopup again (you can customize this)
            return Json(new { success = true, redirectUrl = Url.Action("AddProductPopup", "PurchaseOrder", new { supplierId = model.SupplierId }) });
        }








        // DTO for updating product mapping
        public class UpdateProductMappingModel
        {
            public int SupplierId { get; set; }
            public int ProductId { get; set; }
            public int QuantityToOrder { get; set; }
            public decimal UnitCost { get; set; }
            public decimal LineTotal { get; set; }
        }


        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        [HttpPost]
        public IActionResult UpdateProductMapping([FromBody] UpdateProductMappingModel model)
        {
            if (model == null)
                return Json(new { success = false, message = "Invalid data." });

            var mapping = _supplierProductMappingRepository.Table
                .FirstOrDefault(x => x.SupplierId == model.SupplierId && x.ProductId == model.ProductId);

            if (mapping == null)
                return Json(new { success = false, message = "Mapping not found." });

            mapping.QuantityToOrder = model.QuantityToOrder;
            mapping.UnitCost = model.UnitCost;
            mapping.LineTotal = model.QuantityToOrder * model.UnitCost;

            _supplierProductMappingRepository.Update(mapping);

            return Json(new
            {
                success = true,
                message = "Product updated successfully.",
                lineTotal = mapping.LineTotal.ToString("F2")
            });
        }



        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        [HttpPost]
        public IActionResult DeleteProductMapping(int productId, int supplierId)
        {
            var mapping = _supplierProductMappingRepository.Table
                .FirstOrDefault(x => x.ProductId == productId && x.SupplierId == supplierId);

            if (mapping == null)
                return Json(new { success = false, message = "Mapping not found." });

            _supplierProductMappingRepository.Delete(mapping);

            return Json(new { success = true, message = "Product deleted successfully." });
        }



        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        [HttpPost]
        public IActionResult SavePurchaseOrder(string supplierName, decimal lineTotal)
        {
            _purchaseOrderService.CreatePurchaseOrder(supplierName, lineTotal);
            return Json(new { success = true });
        }










    }
}
