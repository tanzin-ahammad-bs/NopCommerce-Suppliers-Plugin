using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Misc.Suppliers.Areas.Admin;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Infrastructure
{
    public class PurchaseOrderAutoMapperProfile : Profile, IOrderedMapperProfile
    {
        public int Order => 0;

        public PurchaseOrderAutoMapperProfile()
        {
            // Map from entity to model and vice versa
           
            CreateMap<PurchaseOrderList, PurchaseOrderModel>();
            CreateMap<PurchaseOrderModel, PurchaseOrderList>();



           

            CreateMap<SupplierProductMapping, AddProductPopupModel>();
            CreateMap<AddProductPopupModel, SupplierProductMapping>();

            CreateMap<SupplierProductMapping, SelectedProductModel>();
            CreateMap<SelectedProductModel, SupplierProductMapping>();





        }
    }
}
