using Nop.Data;
using Nop.Plugin.Misc.PurchaseOrder.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Services
{
    public class SupplierProductMappingService : ISupplierProductMappingService
    {
        private readonly IRepository<SupplierProductMapping> _repository;

        public SupplierProductMappingService(IRepository<SupplierProductMapping> repository)
        {
            _repository = repository;
        }

        public void Insert(SupplierProductMapping entity)
        {
            _repository.Insert(entity);
        }

        public IQueryable<SupplierProductMapping> GetAll()
        {
            return _repository.Table;
        }
    }
}
