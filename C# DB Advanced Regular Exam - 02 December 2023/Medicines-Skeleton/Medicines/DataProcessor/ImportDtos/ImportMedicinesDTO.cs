using Medicines.Data.Models.Enums;
using Medicines.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Medicines.Validators;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Medicine")]
     public class ImportMedicinesDTO
    {
        [Required]
        [Range(0, 4)]
        [XmlAttribute("category")]
        public int Category { get; set; }

        [Required]
        [MaxLength(DataValidators.MedicineNameLenghtMax)]
        [MinLength(DataValidators.MedicineNameLenghtMin)]
        [XmlElement("Name")]
        
        public string Name { get; set; } = null!;

        [Range(DataValidators.MinPrice,DataValidators.MaxPrice)]
        [XmlElement("Price")]
        public decimal Price { get; set; }


        [XmlElement("ProductionDate")]
        [Required]
        public string ProductionDate { get; set; } = null!;

        [Required]
        [XmlElement("ExpiryDate")]
        public string ExpiryDate { get; set; } = null!;

        [Required]
        [MinLength(DataValidators.ProducerMinLenght)]
        [MaxLength(DataValidators.ProducerMaxLenght)]
        public string Producer { get; set; } = null!;
       
    }
}
//< Pharmacies >

//    < Pharmacy non - stop = "true" >

//        < Name > Vitality </ Name >

//        < PhoneNumber > (123) 456 - 7890 </ PhoneNumber >

//        < Medicines >

//            < Medicine category = "1" >

//                < Name > Ibuprofen </ Name >

//                < Price > 8.50 </ Price >

//                < ProductionDate > 2022 - 02 - 10 </ ProductionDate >

//                < ExpiryDate > 2025 - 02 - 10 </ ExpiryDate >

//                < Producer > ReliefMed Labs </ Producer >

//            </ Medicine >
