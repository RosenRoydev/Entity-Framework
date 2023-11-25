using AutoMapper;
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
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //9.
            string suppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            Console.WriteLine(ImportSuppliers(context,suppliersXml));
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
    }
}