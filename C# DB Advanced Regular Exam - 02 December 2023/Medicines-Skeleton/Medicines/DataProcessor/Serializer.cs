namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            StringBuilder sb = new();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportPatiensDTO[]),new XmlRootAttribute("Patients"));

            var patientsWithMedicaments = context.Patients.
                Where(p => p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate.ToString("yyyy-mm-dd") != date)).
                Select(p => new ExportPatiensDTO
                {
                    Gender = p.Gender.ToString(),
                    Name = p.FullName,
                    AgeGroup = p.AgeGroup.ToString(),
                    Medecines = p.PatientsMedicines.
                    Select(pm => new ExportMedecine
                    {
                        Category = pm.Medicine.Category.ToString(),
                        Price = pm.Medicine.Price.ToString("f2"),
                        Producer = pm.Medicine.Producer,
                        BestBefore = pm.Medicine.ExpiryDate.ToString("yyyy-mm-dd")
                    }).OrderByDescending(pm=> pm.BestBefore).ThenBy(pm => pm.Price).ToArray()

                    //            Medicine Category = "antiseptic" >
                    //< Name > Ciprofloxacin </ Name >
                    //< Price > 19.20 </ Price >
                    //< Producer > ReliefMed Labs </ Producer >
                    //< BestBefore > 2025 - 07 - 22 </ BestBefore

                }).OrderByDescending(p=> p.Medecines.Count()).ThenBy(p => p.Name)
                .ToArray();

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, String.Empty);

            using (StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, patientsWithMedicaments, ns);
            }


            return sb.ToString().TrimEnd();

        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicinesNonStop = context.Medicines.
                Where(m => m.Category == (Category)medicineCategory).Where(m => m.Pharmacy.IsNonStop == true).
                Select(m => new
                {
                    m.Name,
                    Price = m.Price.ToString("f2"),
                    Pharmacy = new
                    {
                        m.Pharmacy.Name,
                        m.Pharmacy.PhoneNumber
                    }

                }).OrderBy(m => m.Price).ThenBy(m => m.Name).
                ToArray();
            return  JsonConvert.SerializeObject(medicinesNonStop,Formatting.Indented);

            
        }
    }
}
//"Name": "Clindamycin",
//  "Price": "15.30",
//  "Pharmacy": {
//    "Name": "Revive",
//    "PhoneNumber": "(654) 987-0123"
//  }
//},
//{
//    "Name": "Erythromycin",
//  "Price": "16.85",
//  "Pharmacy": {
//        "Name": "Serenity",
//    "PhoneNumber": "(890) 123-4567"
//  }
//},
//{
