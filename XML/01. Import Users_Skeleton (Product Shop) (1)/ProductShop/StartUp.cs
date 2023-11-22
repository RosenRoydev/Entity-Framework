using AutoMapper;
using Microsoft.Extensions.Primitives;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            //1.
            // string insertUsersXml = File.ReadAllText("../../../Datasets/users.xml");
            // Console.WriteLine(ImportUsers(context,insertUsersXml));

            //2.
             string productsXml = File.ReadAllText("../../../Datasets/products.xml");
             Console.WriteLine(ImportProducts(context,productsXml));

            //3.
            //string categoriesXml = File.ReadAllText("../../../Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(context,categoriesXml));

            //4.
           // string categoryProductsXml = File.ReadAllText("../../../Datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(context,categoryProductsXml));
        }

        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(c => c.AddProfile<ProductShopProfile>());
            return new Mapper(config);
        }

        //1.
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportUsersDTO[]), new XmlRootAttribute("Users"));

            using
                var reader = new StringReader(inputXml);
            ImportUsersDTO[] importUsersDTO = (ImportUsersDTO[])xmlSerializer.Deserialize(reader);

            var mapper = GetMapper();
            User[] users = mapper.Map<User[]>(importUsersDTO);

            context.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        //2.
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportProductsDTO[]), new XmlRootAttribute("Products"));

            using var reader = new StringReader(inputXml);
            ImportProductsDTO[] importProductsDTOs = (ImportProductsDTO[])xmlSerializer.Deserialize(reader);

            var mapper = GetMapper();
            Product[] products = mapper.Map<Product[]>(importProductsDTOs);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        //3.
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportcategoriesDTO[]), new XmlRootAttribute("Categories"));
            using var reader = new StringReader(inputXml);
            ImportcategoriesDTO[] importcategoriesDTOs = (ImportcategoriesDTO[])xmlSerializer.Deserialize(reader);

            var mapper = GetMapper();

            Category[] categories = mapper.Map<Category[]>(importcategoriesDTOs);
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        //4.
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoiesProductsDTO[]), new XmlRootAttribute("CategoryProducts"));
        
         using var reader =new StringReader(inputXml);
            ImportCategoiesProductsDTO[] importCategoiesProductsDTOs = (ImportCategoiesProductsDTO[])xmlSerializer.Deserialize(reader);

                var mapper = GetMapper();
            CategoryProduct[] categoryProducts = mapper.Map<CategoryProduct[]>(importCategoiesProductsDTOs);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";


        }
    }
}