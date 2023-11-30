using Invoices.Data.Models;
using Invoices.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Invoice")]
    public class ExportInvoicesDTO
    {
        [XmlElement("InvoiceNumber")]
        public int InvoiceNumber { get; set; }

        [XmlElement("InvoiceAmount")]
        public decimal InvoiceAmount { get; set; }

        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlElement("Currency")]
        public string Currency { get; set; }
    }
}
//<Invoice>
//        < InvoiceNumber > 1063259096 </ InvoiceNumber >
//        < InvoiceAmount > 167.22 </ InvoiceAmount >
//        < DueDate > 02 / 19 / 2023 </ DueDate >
//        < Currency > EUR </ Currency >
//      </ Invoice >
//      < Invoice >
//        < InvoiceNumber > 1427940691 </ InvoiceNumber >
//        < InvoiceAmount > 913.13 </ InvoiceAmount >
//        < DueDate > 10 / 28 / 2022 </ DueDate >
//        < Currency > EUR </ Currency >
//      </ Invoice >
//      …
//    </ Invoices >
