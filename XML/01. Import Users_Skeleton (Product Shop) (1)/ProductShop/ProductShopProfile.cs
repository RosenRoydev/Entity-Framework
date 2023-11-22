using AutoMapper;
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

        }
    }
}
