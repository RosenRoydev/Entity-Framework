using Medicines.Data.Models.Enums;
using Medicines.Validators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ImportDtos
{
    
    public class ImportPatientsDTO
    {
        [Required]
        [MaxLength(DataValidators.PatientMaxLenghtName)]
        [MinLength(DataValidators.PatientMinLenghtName)]
        [JsonProperty("FullName")]
        public string FullName { get; set; } = null!;

        [Required]
        [Range(0,2)]
        [JsonProperty("AgeGroup")]
        public int AgeGroup { get; set; }

        [Required]
        [Range(0,1)]
        [JsonProperty("Gender")]
        public int Gender { get; set; }

        [JsonProperty("Medicines")]  
        public int[] Medicines { get; set; }

    }
}
//"FullName": "Ivan Petrov",
//  "AgeGroup": "1",
//  "Gender": "0",
//  "Medicines": [
//    15,
//    23
//  ]
