namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportPatientsDTO[] importPatientsDTOs = JsonConvert.DeserializeObject<ImportPatientsDTO[]>(jsonString);

            List<Patient> patients = new List<Patient>();
            int[] medicinesIds = context.Medicines.Select(m => m.Id).ToArray();

            foreach (var importPatient in importPatientsDTOs)
            {
                if (!IsValid(importPatient))
                {
                    sb.AppendLine(ErrorMessage); continue;
                }
                Patient patient = new Patient()
                {
                    FullName = importPatient.FullName,
                    AgeGroup = (AgeGroup)importPatient.AgeGroup,
                    Gender = (Gender)importPatient.Gender,

                };
                foreach (var mId in importPatient.Medicines.Distinct())
                {
                    if (!IsValid(mId) || !medicinesIds.Contains(mId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    patient.PatientsMedicines.Add(new PatientMedicine
                    {
                        MedicineId = mId,
                    });
                }
                patients.Add(patient);
                sb.AppendLine(String.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count));

            }
            context.Patients.AddRange(patients);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPharmacieDTO[]), new XmlRootAttribute("Pharmacies"));
            using StringReader reader = new StringReader(xmlString);
            ImportPharmacieDTO[] importPharmacieDTOs = (ImportPharmacieDTO[])xmlSerializer.Deserialize(reader);
            List<Pharmacy> pharmacies = new List<Pharmacy>();

            foreach (var importPharmacie in importPharmacieDTOs)
            {
                if (!IsValid(importPharmacie))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (importPharmacie.IsNonStop != "true" && importPharmacie.IsNonStop != "false")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy pharmacy = new Pharmacy()
                {
                    IsNonStop = bool.Parse(importPharmacie.IsNonStop.ToString().ToLower()),
                    Name = importPharmacie.Name,
                    PhoneNumber = importPharmacie.PhoneNumber

                };





                foreach (var importMedicine in importPharmacie.Medicines)
                {
                    if (!IsValid(importMedicine))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    


                    DateTime productionDate = DateTime.ParseExact(importMedicine.ProductionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime expiryDate = DateTime.ParseExact(importMedicine.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (expiryDate <= productionDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    

                    if (importMedicine.Producer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (pharmacy.Medicines.Any(m => m.Name == importMedicine.Name && m.Producer == importMedicine.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine medicine = new Medicine()
                    {
                        Name = importMedicine.Name,
                        Category = (Category)importMedicine.Category,
                        Price = importMedicine.Price,
                        ProductionDate = productionDate,
                        ExpiryDate = expiryDate,
                        Producer = importMedicine.Producer
                    };
                    
                    pharmacy.Medicines.Add(medicine);
                }
                pharmacies.Add(pharmacy);
                sb.AppendLine(String.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count));


            }
            context.Pharmacies.AddRange(pharmacies);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
