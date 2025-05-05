using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.PurchaseOrder.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Data
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