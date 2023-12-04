using Medicines.Data.Models;
using Medicines.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Pharmacy")]
    public class ImportPharmacieDTO
    {
        [Required]
        [XmlAttribute("non-stop")]
        public string IsNonStop { get; set; } = null!;


        [Required]
        [MaxLength(DataValidators.PharmacieNameMaxLenght)]
        [MinLength(DataValidators.PharmacieNameMinLenght)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression("^\\(\\d{3}\\) \\d{3}-\\d{4}$")]
        [MaxLength(DataValidators.PhoneNumberLenght)]
        [MinLength(DataValidators.PhoneNumberLenght)]
        [XmlElement("PhoneNumber")]
        
        public string PhoneNumber { get; set; } = null!;

        [XmlArray("Medicines")]
        public ImportMedicinesDTO[] Medicines { get; set; }
       
    }
}
//< Pharmacies >

//    < Pharmacy non - stop = "true" >

//        < Name > Vitality </ Name >

//        < PhoneNumber > (123) 456 - 7890 </ PhoneNumber >

//        < Medicines >
