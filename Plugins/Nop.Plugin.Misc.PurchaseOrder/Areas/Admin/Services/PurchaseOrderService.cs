using System;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Services;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IRepository<PurchaseOrderList> _purchaseOrderRepository;

        public PurchaseOrderService(IRepository<PurchaseOrderList> purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
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



        public void CreatePurchaseOrder(string supplierName, decimal totalAmount)
        {
            var latestId = _purchaseOrderRepository.Table.OrderByDescending(p => p.PurchaseOrderId).Select(p => p.PurchaseOrderId).FirstOrDefault();
            var purchaseOrder = new PurchaseOrderList
            {
                PurchaseOrderId = latestId + 1,
                SupplierName = supplierName,
                CreationDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                CreatedBy = "Admin"
            };

            _purchaseOrderRepository.Insert(purchaseOrder);
        }



    }
}
