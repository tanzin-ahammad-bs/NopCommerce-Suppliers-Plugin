using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Infrastructure
{
    public class SuppliersAutoMapperProfile : Profile, IOrderedMapperProfile
    {
        public int Order => 0;

        public SuppliersAutoMapperProfile()
        {
            // Map from entity to model and vice versa
            CreateMap<Supplier, SupplierModel>();
            CreateMap<SupplierModel, Supplier>();
            CreateMap<ProductSupplierMapping, SupplierProductModel>();
            CreateMap<SupplierProductModel, ProductSupplierMapping>();
            
        }
    }
}
