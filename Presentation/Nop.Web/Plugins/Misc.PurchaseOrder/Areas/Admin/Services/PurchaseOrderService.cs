using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Services;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IRepository<PurchaseOrderList> _purchaseOrderRepository;
        private readonly IRepository<PurchaseOrderProductMapping> _purchaseOrderProductMappingRepository;
        private readonly IRepository<ProductSupplierMapping> _productSupplierMappingRepository;
        private readonly IRepository<Product> _productRepository;

        public PurchaseOrderService(IRepository<PurchaseOrderList> purchaseOrderRepository, IRepository<PurchaseOrderProductMapping> purchaseOrderProductMappingRepository,
            IRepository<ProductSupplierMapping> productSupplierMappingRepository, IRepository<Product> productRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderProductMappingRepository = purchaseOrderProductMappingRepository;
            _productSupplierMappingRepository = productSupplierMappingRepository;
            _productRepository = productRepository;

        }

        public virtual async Task<IPagedList<PurchaseOrderList>> GetAllPurchaseOrdersAsync(string supplierName = "", DateTime? startDate = null, DateTime? endDate = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return await _purchaseOrderRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrWhiteSpace(supplierName))
                    query = query.Where(p => p.SupplierName.Contains(supplierName));

                if (startDate.HasValue)
                    query = query.Where(p => p.CreationDate >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(p => p.CreationDate <= endDate.Value);

                query = query.OrderByDescending(p => p.CreationDate);

                return query;
            }, pageIndex, pageSize);
        }

        public virtual async Task<IPagedList<PurchaseOrderProductMapping>> GetAllPurchasedProductAsync(string productName = null, int purchaseOrderId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return await _purchaseOrderProductMappingRepository.GetAllPagedAsync(query =>
            {
                // Join with Product table
                query = from mapping in query
                        join product in _productRepository.Table on mapping.ProductId equals product.Id
                        select new PurchaseOrderProductMapping
                        {
                            Id = mapping.Id,
                            PurchaseOrderId = mapping.PurchaseOrderId,
                            ProductId = mapping.ProductId,
                            // Include other fields from mapping if needed
                        };

                // Filter by ProductName if provided
                if (!string.IsNullOrEmpty(productName))
                {
                    query = from mapping in query
                            join product in _productRepository.Table on mapping.ProductId equals product.Id
                            where product.Name.Contains(productName)
                            select mapping;
                }

                // Filter by PurchaseOrderId
                if (purchaseOrderId > 0)
                    query = query.Where(p => p.PurchaseOrderId == purchaseOrderId);

                return query;
            }, pageIndex, pageSize);
        }

        //Pop up

        public virtual async Task<IPagedList<ProductSupplierMapping>> GetAllPopupAsync(string productName = null, int supplierId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return await _productSupplierMappingRepository.GetAllPagedAsync(query =>
            {
                // Join with Product table
                query = from mapping in query
                        join product in _productRepository.Table on mapping.ProductId equals product.Id
                        select new ProductSupplierMapping
                        {
                            Id = mapping.Id,
                            SupplierId = mapping.SupplierId,
                            ProductId = mapping.ProductId,
                            // Include other fields from mapping if needed
                        };

                // Filter by ProductName if provided
                if (!string.IsNullOrEmpty(productName))
                {
                    query = from mapping in query
                            join product in _productRepository.Table on mapping.ProductId equals product.Id
                            where product.Name.Contains(productName)
                            select mapping;
                }

                // Filter by SupplierId
                if (supplierId > 0)
                    query = query.Where(p => p.SupplierId == supplierId);

                return query;
            }, pageIndex, pageSize);
        }


    }
}
