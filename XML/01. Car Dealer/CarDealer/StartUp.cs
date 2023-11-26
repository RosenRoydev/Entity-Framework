﻿using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
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
            string carsXml = File.ReadAllText("../../../Datasets/cars.xml");
            Console.WriteLine(ImportCars(context,carsXml));
        }
        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(c => c.AddProfile<CarDealerProfile>());
            return new Mapper(config);
        }

        //9.
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDTO[]),new XmlRootAttribute("Suppliers"));
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
            var suppliersIds= context.Suppliers.Select(x => x.Id).ToArray();

            var mapper = GetMapper();
            Part[]  parts = mapper.Map<Part[]>(importParts.Where(p =>suppliersIds.Contains(p.SupplierId)));
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
    }
}