using Medicines.Data.Models.Enums;
using Medicines.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Patient")]
    public class ExportPatiensDTO
    {
        [XmlAttribute("Gender")]
        public string Gender { get; set; } = null!;

        [XmlElement("Name")]
        public string FullName { get; set; } = null!;

        [XmlElement("AgeGroup")]
        public string AgeGroup { get; set; } = null!;

        [XmlArray("Medicines")]
        public ExportMedecine[] Medecines { get; set; }

    }

   
}
//<? xml version = "1.0" encoding = "utf-16" ?>
//< Patients >
//  < Patient Gender = "male" >
//    < Name > Stanimir Pavlov </ Name >
//    < AgeGroup > Adult </ AgeGroup >
//    < Medicines >
//      < Medicine Category = "antibiotic" >
//        < Name > Aleve(Naproxen) </ Name >
//        < Price > 10.50 </ Price >
//        < Producer > HealthCare Pharma </ Producer >
//        < BestBefore > 2025 - 09 - 01 </ BestBefore >
//      </ Medicine >
//      < Medicine Category = "antiseptic" >
//        < Name > Ciprofloxacin </ Name >
//        < Price > 19.20 </ Price >
//        < Producer > ReliefMed Labs </ Producer >
//        < BestBefore > 2025 - 07 - 22 </ BestBefore >
//      </ Medicine >
