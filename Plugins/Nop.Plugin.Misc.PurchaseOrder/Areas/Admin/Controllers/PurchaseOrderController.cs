using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework;
using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Services;
using Nop.Services.Catalog;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.PurchasedProduct;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services;
using Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Services;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;
using Nop.Core.Events;
using Nop.Plugin.Misc.PurchaseOrder.Events;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models.AddProductPopup;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Controllers
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
        private readonly ISuppliersModelFactory _suppliersModelFactory;
        private readonly IRepository<PurchaseOrderList> _purchaseOrderRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<PurchaseOrderProductMapping> _purchaseOrderProductMappingRepository;


        public PurchaseOrderController(IPurchaseOrderModelFactory purchaseOrderModelFactory, IRepository<Product> productRepository,
                                          IRepository<ProductSupplierMapping> productSupplierMappingRepository,
                                          IRepository<Supplier> supplierRepository, 
                                          ISupplierProductMappingService supplierProductMappingService, IProductService productService, 
                                          IRepository<SupplierProductMapping> supplierProductMappingRepository, ISupplierService supplierService, 
                                          IPurchaseOrderService purchaseOrderService, ISuppliersModelFactory suppliersModelFactory, 
                                          IRepository<PurchaseOrderList> purchaseOrderRepository, IEventPublisher eventPublisher,
                                          IRepository<PurchaseOrderProductMapping> purchaseOrderProductMappingRepository)
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
            _suppliersModelFactory = suppliersModelFactory;
            _purchaseOrderRepository = purchaseOrderRepository;
            _eventPublisher = eventPublisher;
            _purchaseOrderProductMappingRepository = purchaseOrderProductMappingRepository;
        }
        

        // GET: Display Search Form
        // First Page of Purchase Order Plugin (Showing Purchase List)
        public virtual async Task<IActionResult> PurchaseList()
        {
            var model = await _purchaseOrderModelFactory.PreparePurchaseOrderSearchModelAsync(new PurchaseOrderSearchModel());
            return View("~/Plugins/Misc.PurchaseOrder/Areas/Admin/Views/PurchaseOrder/PurchaseList.cshtml", model);
        }

        // POST: Handle Search Request (AJAX)
        
        [HttpPost]
        public virtual async Task<IActionResult> PurchaseList(PurchaseOrderSearchModel searchModel)
        {
            var model = await _purchaseOrderModelFactory.PreparePurchaseOrderListModelAsync(searchModel);
            return Json(model);
        }

        // Add Products Page
        
        public virtual IActionResult AddProducts(int id)
        {
            // Fetch all suppliers for dropdown
            var allSuppliers = _supplierRepository.Table
                .OrderBy(s => s.Name)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name,
                    Selected = s.Id == id // mark selected supplier
                }).ToList();

            ViewBag.AllSuppliers = allSuppliers;        // send to view
            ViewBag.SupplierId = id;                    // send to view
            ViewBag.SupplierName = allSuppliers.FirstOrDefault(x => x.Value == id.ToString())?.Text ?? "";

            // Load default products for selected supplier
            var model = GetProductsBySupplier(id);

            return View("~/Plugins/Misc.PurchaseOrder/Areas/Admin/Views/PurchaseOrder/AddProducts.cshtml", model);
        }
        
        [HttpGet]
        public virtual JsonResult GetProductsBySupplierId(int? supplierId)
        {
            var products = GetProductsBySupplier(supplierId);
            return Json(products);
        }
        
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
                CurrentStock = x.Product.StockQuantity,
                SupplierId = x.Supplier.Id,
                SupplierName = x.Supplier.Name
            }).ToList();

            return productModels;
        }
      
        [HttpGet]
        public virtual IActionResult ProductListPartial(int? supplierId)
        {
            var products = GetProductsBySupplier(supplierId);
            return PartialView("~/Plugins/Misc.PurchaseOrder/Areas/Admin/Views/PurchaseOrder/_ProductList.cshtml", products);
        }

        [HttpPost]
        public IActionResult SaveSelectedProductsFromPopup([FromBody] SaveProductPopupRequestModel model)
        {
            if (model == null || model.SelectedProductIds == null || !model.SelectedProductIds.Any())
            {
                return Json(new { success = false, message = "No products selected." });
            }

            var existingMappings = _supplierProductMappingService
                .GetAll()
                .Where(spm => spm.SupplierId == model.SupplierId)
                .Select(spm => spm.ProductId)
                .ToHashSet();

            var newProductIds = model.SelectedProductIds
                .Where(id => !existingMappings.Contains(id))
                .ToList();

            if (!newProductIds.Any())
            {
                return Json(new { success = false, message = "All selected products are already added." });
            }

            foreach (var productId in newProductIds)
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

            return Json(new { success = true, refreshPage = true });
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

        [HttpPost]
        public async Task<IActionResult> SavePurchaseOrder([FromBody] SavePurchaseOrderRequest model)
        {
            if (model == null || model.Products == null || !model.Products.Any())
                return Json(new { success = false, message = "Invalid purchase order data." });

            // Get latest PurchaseOrderId and create a new one
            var latestId = _purchaseOrderRepository.Table
                .OrderByDescending(p => p.PurchaseOrderId)
                .Select(p => p.PurchaseOrderId)
                .FirstOrDefault();

            var purchaseOrder = new PurchaseOrderList
            {
                PurchaseOrderId = latestId + 1,
                SupplierName = model.SupplierName,
                CreationDate = DateTime.UtcNow,
                TotalAmount = model.TotalAmount,
                CreatedBy = "Admin"
            };

            await _purchaseOrderRepository.InsertAsync(purchaseOrder);

            // Insert each product mapping with quantity and cost
            foreach (var product in model.Products)
            {
                var mapping = new PurchaseOrderProductMapping
                {
                    PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                    ProductId = product.ProductId,
                    QuantityToOrder = product.QuantityToOrder,
                    UnitCost = product.UnitCost
                };

                await _purchaseOrderProductMappingRepository.InsertAsync(mapping);
            }

            // Fire event to update stock
            var selectedProducts = model.Products.Select(p => new SelectedProductModel
            {
                ProductId = p.ProductId,
                QuantityToOrder = p.QuantityToOrder
            }).ToList();

            if (selectedProducts.Any())
                await _eventPublisher.PublishAsync(new PurchaseOrderSavedEvent(selectedProducts));

            return Json(new { success = true });
        }

        // Show purchase list view items in data table
        //Purchased Product

        // GET: Display Search Form
        public virtual async Task<IActionResult> PurchasedProduct(int purchaseOrderId)
        {
            var model = await _purchaseOrderModelFactory.PreparePurchasedProductSearchModelAsync(new PurchasedProductSearchModel
            {
                PurchaseOrderId = purchaseOrderId
            });

            return View("~/Plugins/Misc.PurchaseOrder/Areas/Admin/Views/PurchaseOrder/PurchasedProduct.cshtml", model);
        }

        // POST: Handle AJAX Data Request
        [HttpPost]
        public virtual async Task<IActionResult> PurchasedProduct(PurchasedProductSearchModel searchModel)
        {
            var model = await _purchaseOrderModelFactory.PreparePurchasedProductListModelAsync(searchModel);
            return Json(model);
        }

        //Popup

        // GET: Display Search Form
        public virtual async Task<IActionResult> AddProductPopup(int supplierId)
        {
            var model = await _purchaseOrderModelFactory.PreparePopupSearchModelAsync(new AddProductPopupSearchModel
            {
                SupplierId = supplierId
            });

            return View("~/Plugins/Misc.PurchaseOrder/Areas/Admin/Views/PurchaseOrder/AddProductPopup.cshtml", model);
        }

        // POST: Handle AJAX Data Request
        [HttpPost]
        public virtual async Task<IActionResult> AddProductPopup(AddProductPopupSearchModel searchModel)
        {
            var model = await _purchaseOrderModelFactory.PreparePopupListModelAsync(searchModel);
            return Json(model);
        }


    }
}
