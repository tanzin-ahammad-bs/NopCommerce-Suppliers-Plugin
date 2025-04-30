using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Data
{
    [NopMigration("2025/04/14 10:36:59", "Misc.PurchaseOrder base schema", MigrationProcessType.Installation)]
    public class SchemaMigratio : AutoReversingMigration
    {
        public override void Up()
        {

            Create.TableFor<PurchaseOrderList>();
            Create.TableFor<SupplierProductMapping>();
        }
    }
}