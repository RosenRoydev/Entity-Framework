namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Xml.Serialization;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
        
            StringBuilder stringBuilder = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportClientsDTO[]), new XmlRootAttribute("Clients"));

            using StringReader reader = new StringReader(xmlString);
            ImportClientsDTO[] importClientsDTOs = (ImportClientsDTO[])xmlSerializer.Deserialize(reader);
            List<Client> clients = new List<Client>();

            foreach (var importClientDto in importClientsDTOs)
            {
                if (!IsValid(importClientDto))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }
                Client client = new Client()
                {
                    Name = importClientDto.Name,
                    NumberVat = importClientDto.NumberVat,
                    Addresses = new List<Address>()
                };

                foreach (var addressDTO in importClientDto.Addresses)
                {
                    if (!IsValid(addressDTO))
                    {
                        stringBuilder.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address address = new Address()

                    {
                        StreetName = addressDTO.StreetName,
                        StreetNumber = addressDTO.StreetNumber,
                        PostCode = addressDTO.PostCode,
                        City = addressDTO.City,
                        Country = addressDTO.Country

                    };
                    client.Addresses.Add(address);
                }
                clients.Add(client);
                stringBuilder.AppendLine(String.Format(SuccessfullyImportedClients,client.Name));
            }
            context.AddRange(clients);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
            
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            ImportInvoicesDTO[] importInvoicesDTOs = JsonConvert.DeserializeObject<ImportInvoicesDTO[]>(jsonString);
            int[] invoicesIds=context.Clients.Select(c => c.Id).ToArray();
            List<Invoice> invoices = new List<Invoice>();
            foreach (var invoiceDto in importInvoicesDTOs)
            {
                if (!IsValid(invoiceDto) || !invoicesIds.Contains(invoiceDto.ClientId))
                {
                    stringBuilder.AppendLine(ErrorMessage); 
                    continue;
                }

                DateTime issueDate = DateTime.ParseExact(invoiceDto.IssueDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                DateTime dueDate = DateTime.ParseExact(invoiceDto.DueDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

                if (issueDate > dueDate)
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }
                Invoice invoice = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = issueDate,
                    DueDate = dueDate,
                    Amount = invoiceDto.Amount,
                    CurrencyType = (CurrencyType)invoiceDto.CurrencyType,
                    ClientId = invoiceDto.ClientId,

                };
                invoices.Add(invoice);
                stringBuilder.AppendLine(String.Format(SuccessfullyImportedInvoices, invoiceDto.Number));
            }
            context.Invoices.AddRange(invoices);
            context.SaveChanges();
            return stringBuilder.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportProductsDTO[] importProductsDTOs = JsonConvert.DeserializeObject<ImportProductsDTO[]>(jsonString);
            int[] clientIds = context.Clients.Select(c => c.Id).ToArray();
            List<Product> products = new List<Product>();

            foreach (var importproductDto in importProductsDTOs) 
            {
                if (!IsValid(importproductDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Product product = new Product()
                {
                    Name = importproductDto.Name,
                    Price = importproductDto.Price,
                    CategoryType = (CategoryType)importproductDto.CategoryType
                };

                foreach (var clinetId in importproductDto.Clients.Distinct())
                {
                    if (!clientIds.Contains(clinetId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    product.ProductsClients.Add(new ProductClient()
                    {
                        ClientId = clinetId
                    });
                }
                products.Add(product);
                sb.AppendLine(String.Format(SuccessfullyImportedProducts,product.Name,product.ProductsClients.Count()));
            }
            context.Products.AddRange(products);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
