using AutoMapper;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<ImportUsersDTO, User>();
            CreateMap<ImportProductsDTO, Product>();
            CreateMap<ImportcategoriesDTO, Category>();
            CreateMap<ImportCategoiesProductsDTO,CategoryProduct>();
            CreateMap<Product, ExportProductsDTO>().
                ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => $"{src.Buyer.FirstName} {src.Buyer.LastName}"));
               
   


        }
    }
}
