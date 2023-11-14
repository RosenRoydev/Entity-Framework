using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            //01.Insert users
            //string jsonUsers = File.ReadAllText("../../../Datasets/users.json");

            //02.  Insert products
            //string jsonProdusts = File.ReadAllText("../../../Datasets/products.json");

            //03.Insert Categories
            string jsonCategories = File.ReadAllText("../../../Datasets/categories.json");



            Console.WriteLine(ImportCategories(context,jsonCategories));


        }
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);
            context.Products.AddRange(products);    
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var allCategories = JsonConvert.DeserializeObject<Category[]>(inputJson);
            var validCategories = allCategories?
                .Where(c => c.Name is not null)
                .ToArray();

            if (validCategories is not null)
            {
                context.Categories.AddRange(validCategories);
                context.SaveChanges();
                return $"Successfully imported {validCategories.Length}";
            }
            return $"Successfully imported 0";
        }
    }
}