namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Newtonsoft.Json;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            StringBuilder sb = new();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportPatiensDTO[]),new XmlRootAttribute("Patients"));
            DateTime dateForCompare = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var patientsWithMedicaments = context.Patients.AsEnumerable().
                Where( p => p.PatientsMedicines.Any(pm =>  pm.Medicine.ProductionDate > dateForCompare)).
                Select(p => new ExportPatiensDTO
                {
                    Gender = p.Gender.ToString().ToLower(),
                    FullName = p.FullName,
                    AgeGroup = p.AgeGroup.ToString(),
                    Medecines = p.PatientsMedicines.Where(pm => pm.Medicine.ProductionDate > dateForCompare).
                    OrderByDescending(pm => pm.Medicine.ExpiryDate).ThenBy(pm=> pm.Medicine.Price).
                    Select(pm => new ExportMedecine
                    {
                        Category = pm.Medicine.Category.ToString().ToLower(),
                        Name = pm.Medicine.Name,
                        Price = pm.Medicine.Price.ToString("f2"),
                        Producer = pm.Medicine.Producer,
                        BestBefore = pm.Medicine.ExpiryDate.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture)
                    }).ToArray()

                    

                }).OrderByDescending(p=> p.Medecines.Count()).ThenBy(p => p.FullName)
                .ToArray();
            //var patients = context.Patients
            //       .AsEnumerable()
            //       .Where(p => p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate > productionDate))
            //       .Select(p => new ExportPatientDto()
            //       {
            //           Gender = p.Gender.ToString().ToLower(),
            //           FullName = p.FullName,
            //           AgeGroup = p.AgeGroup.ToString(),
            //           Medicines = p.PatientsMedicines
            //           .Where(pm => pm.Medicine.ProductionDate > productionDate)
            //           .OrderByDescending(pm => pm.Medicine.ExpiryDate)
            //           .ThenBy(pm => pm.Medicine.Price)
            //           .Select(pm => new ExportMedicineDto()
            //           {
            //               Category = pm.Medicine.Category.ToString().ToLower(),
            //               Name = pm.Medicine.Name,
            //               Price = $"{pm.Medicine.Price:f2}",
            //               Producer = pm.Medicine.Producer,
            //               BestBefore = pm.Medicine.ExpiryDate.ToString("yyyy-MM-dd")
            //           })
            //           .ToArray()
            //       })
            //       .OrderByDescending(p => p.Medicines.Count())
            //       .ThenBy(p => p.FullName)
            //       .ToArray();

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
            var medicinesNonStop = context.Medicines.AsEnumerable().
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
