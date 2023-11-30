using Invoices.Data.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Client")]
    public class ExportClientsWithInvoicesDTO
    {
        [XmlAttribute("InvoicesCount")]
        public int InvoicesCount { get;set; }

        [XmlElement("ClientName")]
        public string ClientName { get; set; }

        [XmlElement("VatNumber")]
        public string VatNumber { get; set; }

        [XmlArray("Invoices")]
        public ExportInvoicesDTO[] Invoices { get; set; }
    }
}
//< Clients >
//  < Client InvoicesCount = "9" >
//    < ClientName > SPEDOX,SRO </ ClientName >
//    < VatNumber > SK2023911087 </ VatNumber >
//    < Invoices >
//      < Invoice >
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
