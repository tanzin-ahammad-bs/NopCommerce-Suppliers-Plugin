using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Suppliers.Models;
public partial record SuppliersListModel : BasePagedListModel<SupplierModel>
{
}