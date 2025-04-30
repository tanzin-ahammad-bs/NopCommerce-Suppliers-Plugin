using System.Linq;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services
{
    public interface ISupplierProductMappingService
    {
        void Insert(SupplierProductMapping entity);

        IQueryable<SupplierProductMapping> GetAll(); // Required for filtering duplicates
    }
}
