namespace Invoices.DataProcessor
{
   
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportClientsWithInvoicesDTO[]), new XmlRootAttribute("Clients"));

            var clientsWithIvoices = context.Clients.Where(c => c.Invoices.Any(ci => ci.IssueDate > date)).
                Select(c => new ExportClientsWithInvoicesDTO
                {
                    InvoicesCount = c.Invoices.Count,
                    ClientName = c.Name,
                    VatNumber = c.NumberVat,
                    Invoices = c.Invoices.OrderBy(i => i.IssueDate).ThenByDescending(i => i.DueDate).
                    Select(i => new ExportInvoicesDTO
                    {
                        InvoiceNumber = i.Number,
                        InvoiceAmount = i.Amount,
                        DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        Currency = i.CurrencyType.ToString(),
                    }).ToArray()
                }).OrderByDescending(i=> i.Invoices.Length).ThenBy(i => i.ClientName).ToArray();
            
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            using (StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, clientsWithIvoices, ns);
            };

            return sb.ToString().TrimEnd();
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {

            var productsWithMostCliens = context.Products.
                Where(p=> p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength )).
                Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.CategoryType.ToString(),
                    Clients = p.ProductsClients.Where( pc=> pc.Client.Name.Length >= nameLength).Select(c => new 
                    {
                        c.Client.Name,
                        c.Client.NumberVat
                    }).OrderBy(c => c.Name).ToArray()


                }).OrderByDescending(p => p.Clients.Length).ThenBy(p=>p.Name).Take(5);

            return JsonConvert.SerializeObject(productsWithMostCliens, Newtonsoft.Json.Formatting.Indented);
        }
    }
}

