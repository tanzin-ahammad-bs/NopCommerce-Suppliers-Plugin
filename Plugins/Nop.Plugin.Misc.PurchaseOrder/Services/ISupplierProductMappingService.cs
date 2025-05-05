using Nop.Plugin.Misc.PurchaseOrder.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Services
{
    public interface ISupplierProductMappingService
    {
        void Insert(SupplierProductMapping entity);

        IQueryable<SupplierProductMapping> GetAll(); // Required for filtering duplicates
    }
}
