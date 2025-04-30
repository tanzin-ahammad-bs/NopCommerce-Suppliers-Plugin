using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Services
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
    }

}
