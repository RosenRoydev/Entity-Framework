using Medicines.Data.Models;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("medicine")]
    public class ExportMedecine
    {
        [XmlAttribute("Category")]
        public string Category { get; set; }

        [XmlElement("Price")]
        public string Price { get; set; }

        [XmlElement("Producer")]
        public string Producer { get;set; }

        [XmlElement("BestBefore")]
        public string BestBefore { get; set; }
    }
}
//Medicine Category = "antiseptic" >
//        < Name > Ciprofloxacin </ Name >
//        < Price > 19.20 </ Price >
//        < Producer > ReliefMed Labs </ Producer >
//        < BestBefore > 2025 - 07 - 22 </ BestBefore >
//      </ Medicine >