using Medicines.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicines.Data.Models
{
    public class PatientMedicine
    {
        [Required]
        [ForeignKey(nameof(PatientId))]
        public int PatientId { get;set; }
        public Patient Patient { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(MedicineId))]
        public int MedicineId { get;set; }
        public Medicine Medicine { get; set; } = null!;
    }
}

//•	PatientId – integer, Primary Key, foreign key (required)
//•	Patient – Patient
//•	MedicineId – integer, Primary Key, foreign key (required)
//•	Medicine – Medicine
