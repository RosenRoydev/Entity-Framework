using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();


            //1.
            //string suppliersJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(context,suppliersJson));

            //2.
            // string jsonParts = File.ReadAllText("../../../Datasets/parts.json");
            //Console.WriteLine(ImportParts(context,jsonParts));

            //3.
            // string jsonCars = File.ReadAllText("../../../Datasets/cars.json");
            // Console.WriteLine(ImportCars(context,jsonCars));

            //4.
            //string jsonCustomers = File.ReadAllText("../../../Datasets/customers.json");
            //Console.WriteLine(ImportCustomers(context,jsonCustomers));

            //5.
            //string jsonSales = File.ReadAllText("../../../Datasets/sales.json");
            //Console.WriteLine(ImportSales(context,jsonSales));

            //6.
            // Console.WriteLine(GetOrderedCustomers(context));

            //7.
            //Console.WriteLine(GetCarsFromMakeToyota(context));

            //8.
            //Console.WriteLine(GetLocalSuppliers(context));

            //9.
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

            //10.
            // Console.WriteLine(GetTotalSalesByCustomer(context));

            //11.
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }
        public static IMapper CreateMapper()
        {
            MapperConfiguration configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<CarDealerProfile>();
            });

            IMapper mapper = configuration.CreateMapper();

            return mapper;
        }
        //1.
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);
            context.AddRange(suppliers);
            context.SaveChanges();

          return  $"Successfully imported {suppliers.Count}.";
        }

        //2.
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            var validParts = new List<Part>();

            foreach (var part in parts)
            {
                var existingSupplier = context.Suppliers.Find(part.SupplierId);

                if (existingSupplier != null)
                {
                    validParts.Add(part);
                }
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}.";
        }

        //3.
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            //Turning the json file to a DTO
            ImportCarDTO[] importCarDtos = JsonConvert.DeserializeObject<ImportCarDTO[]>(inputJson);

            //Mapping the Cars from their DTOs
            ICollection<Car> carsToAdd = new HashSet<Car>();

            foreach (var carDto in importCarDtos)
            {
                Car currentCar = mapper.Map<Car>(carDto);

                foreach (var id in carDto.PartsIds)
                {
                    if (context.Parts.Any(p => p.Id == id))
                    {
                        currentCar.PartsCars.Add(new PartCar
                        {
                            PartId = id,
                        });
                    }
                }

                carsToAdd.Add(currentCar);
            }

            //Adding the Cars
            context.Cars.AddRange(carsToAdd);
            context.SaveChanges();

            //Output
            return $"Successfully imported {carsToAdd.Count}.";
        }
        //4.
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var mapper = CreateMapper();
            ImportCustomersDTO[] importedCustomers = JsonConvert.DeserializeObject<ImportCustomersDTO[]>(inputJson);
            Customer[] customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);


            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}.";



        }
        //5.
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var mapper = CreateMapper();
            ImportSaleDTO[] importSales = JsonConvert.DeserializeObject<ImportSaleDTO[]>(inputJson);
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count()}.";
        }

        //6.
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
            .AsNoTracking()
            .OrderBy(c => c.BirthDate)
            .ThenBy(c => c.IsYoungDriver)
            .Select(c => new
            {
                c.Name,
                BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                c.IsYoungDriver
            })
            .ToArray();

            string jsonOrderedCustomers= JsonConvert.SerializeObject(customers);
            return jsonOrderedCustomers;
        }

        //7.
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var carsToyota = context.Cars.
             
                Where(c => c.Make == "Toyota").
                Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TraveledDistance

                }). OrderBy(c => c.Model).
                ThenByDescending(c => c.TraveledDistance).ToArray();
            string jsonToyotaCars = JsonConvert.SerializeObject(carsToyota);
            return jsonToyotaCars;
        }

        //8.
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers.
                Where(s => s.IsImporter == false).
                Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count(),
                }).ToArray();
            
            string jsonSuppliers = JsonConvert.SerializeObject(suppliers);
            return jsonSuppliers;
        }

        //9.
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithParts = context.Cars.Select(c => new
            {
                car = new
                {
                    c.Make,
                    c.Model,
                    c.TraveledDistance,
                },


                parts = c.PartsCars.Select(c => new
                {
                    c.Part.Name,
                    Price = c.Part.Price.ToString("f2")
                })
            }).ToList();

            string jsonCarsWithParts = JsonConvert.SerializeObject(carsWithParts);
            return jsonCarsWithParts;
           
        }

        //10.
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customersWithCars = context.Customers.Where(c => c.Sales.Count() > 0).
                Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count(),
                    spentMoney = c.Sales.Sum(s => s.Car.PartsCars.Sum(pc => pc.Part.Price))


                }


                ).OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToArray();

            string jsoncustumersWithCars = JsonConvert.SerializeObject(customersWithCars,Formatting.Indented);
            return jsoncustumersWithCars;
        }

        //11.
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var information = context.Sales.Take(10).Select(s => new
            {
                ccar = new
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TraveledDistance = s.Car.TraveledDistance
                },
                customerName = s.Customer.Name,
                discount = s.Discount.ToString("f2"),
                price = s.Car.PartsCars.Sum(pc => pc.Part.Price).ToString("f2"),
                priceWithDiscount = ((s.Car.PartsCars.Sum(pc => pc.Part.Price) * (1 - s.Discount / 100))).ToString("f2")
             }).ToArray();

            string jsonCarInfo = JsonConvert.SerializeObject (information,Formatting.Indented);
            return jsonCarInfo;
        }
    }
}