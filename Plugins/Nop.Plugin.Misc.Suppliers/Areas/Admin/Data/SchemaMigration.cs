using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;


namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Data
{
    [NopMigration("2025/04/14 11:05:30", "Misc.Suppliers base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<Supplier>();
            Create.TableFor<ProductSupplierMapping>();
        }
    }
}

