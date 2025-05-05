using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Data
{
    [NopMigration("2026/05/15 13:39:59", "Misc.PurchaseOrder base schema", MigrationProcessType.Installation)]
    public class SchemaMigratio : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<PurchaseOrderList>();
            Create.TableFor<SupplierProductMapping>();
            Create.TableFor<PurchaseOrderProductMapping>();
        }
    }
}