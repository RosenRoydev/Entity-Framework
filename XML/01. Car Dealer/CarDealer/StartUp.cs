using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            //9.
            //string suppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context,suppliersXml));

            //10.
            //  string partsXml = File.ReadAllText("../../../Datasets/parts.xml");
            // Console.WriteLine(ImportParts(context,partsXml));

            //11.
            //string carsXml = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context,carsXml));

            //12.
            //var customersXml = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context,customersXml));

            //13.
            // var salesXml = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context,salesXml));

            //14.
            //Console.WriteLine(GetCarsWithDistance(context));

            //15
            //Console.WriteLine(GetCarsFromMakeBmw(context));

            //16.
            // Console.WriteLine(GetLocalSuppliers(context));

            //17.
            // Console.WriteLine(GetCarsWithTheirListOfParts(context));

            //18.
            Console.WriteLine(GetTotalSalesByCustomer(context));

        }
        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(c => c.AddProfile<CarDealerProfile>());
            return new Mapper(config);
        }

        //9.
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDTO[]), new XmlRootAttribute("Suppliers"));
            using var reader = new StringReader(inputXml);
            ImportSupplierDTO[] importSuppliersDTO = (ImportSupplierDTO[])xmlSerializer.Deserialize(reader);
            var mapper = GetMapper();
            Supplier[] suppliers = mapper.Map<Supplier[]>(importSuppliersDTO);

            context.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}";

        }

        //10.
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartsDTO[]), new XmlRootAttribute("Parts"));
            using var reader = new StringReader(inputXml);
            ImportPartsDTO[] importParts = (ImportPartsDTO[])xmlSerializer.Deserialize(reader);
            var suppliersIds = context.Suppliers.Select(x => x.Id).ToArray();

            var mapper = GetMapper();
            Part[] parts = mapper.Map<Part[]>(importParts.Where(p => suppliersIds.Contains(p.SupplierId)));
            context.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count()}";
        }

        //11.
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCarsDTO[]), new XmlRootAttribute("Cars"));
            using var reader = new StringReader(inputXml);
            ImportCarsDTO[] importCarsDTOs = (ImportCarsDTO[])xmlSerializer.Deserialize(reader);

            var mapper = GetMapper();
            List<Car> cars = new List<Car>();

            foreach (var carDTO in importCarsDTOs)
            {
                Car car = mapper.Map<Car>(carDTO);
                var partIds = carDTO.PartIds.Select(c => c.Id).Distinct().ToArray();

                List<PartCar> parts = new List<PartCar>();
                foreach (var id in partIds)
                {
                    parts.Add(new PartCar
                    {
                        Car = car,
                        PartId = id
                    });
                }
                car.PartsCars = parts;
                cars.Add(car);
            }
            context.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count}";
        }

        //12.
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomersDTO[]), new XmlRootAttribute("Customers"));
            using var reader = new StringReader(inputXml);
            ImportCustomersDTO[] importCustomersDTOs = (ImportCustomersDTO[])xmlSerializer.Deserialize(reader);

            var mapper = GetMapper();
            var customers = mapper.Map<Customer[]>(importCustomersDTOs);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}";
        }

        //13.
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSalesDTO[]), new XmlRootAttribute("Sales"));
            using var reader = new StringReader(inputXml);
            ImportSalesDTO[] importSales = (ImportSalesDTO[])xmlSerializer.Deserialize(reader);

            var carIds = context.Cars.Select(c => c.Id).ToList();
            var mapper = GetMapper();

            Sale[] sales = mapper.Map<Sale[]>(importSales.Where(s => carIds.Contains(s.CarId)));
            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}";

        }

        //14.
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarsWithDistanceDTO[]), new XmlRootAttribute("cars"));

            var cars = context.Cars.Where(c => c.TraveledDistance > 2000000).
                OrderBy(c => c.Make).ThenBy(c => c.Model).
                Select(c => new ExportCarsWithDistanceDTO
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                }).Take(10).ToArray();

            StringBuilder sb = new StringBuilder();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            using (StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, cars, ns);
            }


            return sb.ToString().TrimEnd();
        }

        //15.
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportBMWCarsDTO[]), new XmlRootAttribute("cars"));

            var carsBMW = context.Cars.Where(c => c.Make == "BMW").
                OrderBy(c => c.Model).ThenByDescending(c => c.TraveledDistance).
                Select(c => new ExportBMWCarsDTO
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravaledDistance = c.TraveledDistance,
                }).ToArray();

            StringBuilder sb = new StringBuilder();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            using (StringWriter writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, carsBMW, ns);
            }
            return sb.ToString().TrimEnd();
        }

        //16.
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportLocalSuppliersDTO[]), new XmlRootAttribute("suppliers"));

            var localSuppliers = context.Suppliers.Where(s => s.IsImporter == false).
                Select(s => new ExportLocalSuppliersDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count()
                }).ToArray();

            StringBuilder sb = new();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            using (StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, localSuppliers, ns);
            }
            return sb.ToString().TrimEnd();
        }

        //17.
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarsWithPartsDTO[]), new XmlRootAttribute("cars"));

            var carsWithParts = context.Cars.
                OrderByDescending(c => c.TraveledDistance).ThenBy(c => c.Model).
                Take(5).
                Select(c => new ExportCarsWithPartsDTO()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    Parts = c.PartsCars.Select(pc => new PartsDTO()
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price,
                    }).OrderByDescending(pc => pc.Price).
                    ToArray()

                }).ToArray();
            StringBuilder sb = new StringBuilder();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            using (StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, carsWithParts, ns);
            }
            return sb.ToString().TrimEnd();
        }

        //18.
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCustomersDTO[]), new XmlRootAttribute("customers"));

            var salesDTO = context.Customers.Where(c => c.Sales.Any()).
                Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count(),
                    InfoForSales = c.Sales.Select(s => new
                    {
                        Prices = c.IsYoungDriver ?
                        s.Car.PartsCars.Sum(pc => Math.Round((double)pc.Part.Price, 2))
                        : s.Car.PartsCars.Sum(pc => (double)pc.Part.Price)

                    }).ToArray()

                }).ToArray();

            var totalSales = salesDTO.OrderByDescending(t => t.InfoForSales.Sum(s => s.Prices))
                .Select(t => new ExportCustomersDTO()
                {
                    FullName = t.FullName,
                    BoughtCars = t.BoughtCars,
                    SpentMoney = t.InfoForSales.Sum(s => s.Prices).ToString("f2")
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            using(StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, totalSales, ns);
            }

            return sb.ToString().TrimEnd();
        }
    }
}