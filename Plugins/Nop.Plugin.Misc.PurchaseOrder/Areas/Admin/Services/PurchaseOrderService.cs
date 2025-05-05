using Nop.Core;
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

        public PurchaseOrderService(IRepository<PurchaseOrderList> purchaseOrderRepository, IRepository<PurchaseOrderProductMapping> purchaseOrderProductMappingRepository, IRepository<ProductSupplierMapping> productSupplierMappingRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderProductMappingRepository = purchaseOrderProductMappingRepository;
            _productSupplierMappingRepository = productSupplierMappingRepository;

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





        public virtual async Task<IPagedList<PurchaseOrderProductMapping>> GetAllPurchasedProductAsync(int productId = 0, int purchaseOrderId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return await _purchaseOrderProductMappingRepository.GetAllPagedAsync(query =>
            {
                if (productId > 0)
                    query = query.Where(p => p.ProductId == productId);

                if (purchaseOrderId > 0)
                    query = query.Where(p => p.PurchaseOrderId == purchaseOrderId);

                return query;
            }, pageIndex, pageSize);
        }



        //pop up

        public virtual async Task<IPagedList<ProductSupplierMapping>> GetAllPopupAsync(int productId = 0, int supplierId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return await _productSupplierMappingRepository.GetAllPagedAsync(query =>
            {
                if (productId > 0)
                    query = query.Where(p => p.ProductId == productId);

                if (supplierId > 0)
                    query = query.Where(p => p.SupplierId == supplierId);

                return query;
            }, pageIndex, pageSize);
        }








        //public void CreatePurchaseOrder(string supplierName, decimal totalAmount)
        //{
        //    var latestId = _purchaseOrderRepository.Table.OrderByDescending(p => p.PurchaseOrderId).Select(p => p.PurchaseOrderId).FirstOrDefault();
        //    var purchaseOrder = new PurchaseOrderList
        //    {
        //        PurchaseOrderId = latestId + 1,
        //        SupplierName = supplierName,
        //        CreationDate = DateTime.UtcNow,
        //        TotalAmount = totalAmount,
        //        CreatedBy = "Admin"
        //    };

        //    _purchaseOrderRepository.Insert(purchaseOrder);


        //}




    }
}
