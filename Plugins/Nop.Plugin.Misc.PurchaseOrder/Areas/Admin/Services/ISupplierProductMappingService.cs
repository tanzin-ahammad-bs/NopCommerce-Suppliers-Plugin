using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Services
{
    public interface ISupplierProductMappingService
    {
        void Insert(SupplierProductMapping entity);
    }
}
