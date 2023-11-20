using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<ImportCarDTO, Car>();
            CreateMap<ImportCustomersDTO, Customer>();
            CreateMap<ImportSaleDTO, Sale>();
        }
    }
}
