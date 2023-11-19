using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            //1.
            //string suppliersJson = File.ReadAllText("../../../Datasets/suppliers.json");
            // Console.WriteLine(ImportSuppliers(context,suppliersJson));

            //2.
            string jsonParts = File.ReadAllText("../../../Datasets/parts.json");
            
            Console.WriteLine(ImportParts(context,jsonParts));

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
    }
}