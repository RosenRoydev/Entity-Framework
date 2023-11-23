using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            //1.
            //string insertUsersXml = File.ReadAllText("../../../Datasets/users.xml");

            //Console.WriteLine(ImportUsers(context, insertUsersXml)); 

            //2.
            //string productsXml = File.ReadAllText("../../../Datasets/products.xml");
            //Console.WriteLine(ImportProducts(context,productsXml));

            //3.
            // string categoriesXml = File.ReadAllText("../../../Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(context,categoriesXml));

            //4.
            // string categoryProductsXml = File.ReadAllText("../../../Datasets/categories-products.xml");
            // Console.WriteLine(ImportCategoryProducts(context,categoryProductsXml));

            //5.
            // Console.WriteLine(GetProductsInRange(context));

            //6.
            // Console.WriteLine(GetSoldProducts(context));

            //7.
            Console.WriteLine(GetCategoriesByProductsCount(context));

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

        //5.
        public static string GetProductsInRange(ProductShopContext context)
        {
            
            Mapper mapper = GetMapper();

            var productsInRange = context.Products.Where(p => p.Price >= 500 && p.Price <=1000).OrderBy(p=>p.Price).
                ProjectTo<ExportProductsDTO>(mapper.ConfigurationProvider).
                Take(10).ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportProductsDTO[]), new XmlRootAttribute("Products"));

            var xsn = new XmlSerializerNamespaces();
            xsn.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using(StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, productsInRange,xsn);
            }
            return sb.ToString().TrimEnd();
        }

        //6.
        public static string GetSoldProducts(ProductShopContext context)
        {
            var soldProducts = context.Users.Where(u => u.ProductsSold.Any(ps => ps.BuyerId != null)).
                OrderBy(u => u.LastName).ThenBy(u => u.LastName).Take(5).
                Select(u => new ExportSoldProductsDTO()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(sp => new ProductDto()
                    {
                        Name = sp.Name,
                        Price = sp.Price,
                    }).ToArray()

                }).ToArray();

            XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
            xsn.Add(string.Empty,string.Empty);
            XmlSerializer serializer = new (typeof(ExportSoldProductsDTO[]), new XmlRootAttribute("Users"));

           StringBuilder sb = new StringBuilder();

            using StringWriter sw = new StringWriter(sb);
             serializer.Serialize(sw, soldProducts,xsn);

            return sb.ToString().TrimEnd();
        }

        //7.
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories.Select(c => new ExportCategoriesDTO
            {
                Name = c.Name,
                Count = c.CategoryProducts.Count(),
                AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
            }).OrderByDescending(c=>c.Count).ThenBy(c => c.TotalRevenue).ToArray();

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty,string.Empty);
            XmlSerializer serializer = new XmlSerializer(typeof(ExportCategoriesDTO[]), new XmlRootAttribute("Categories"));

            StringBuilder sb = new StringBuilder();
            using StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, categories, ns);
            return sb.ToString().TrimEnd();
        }


    }   
}