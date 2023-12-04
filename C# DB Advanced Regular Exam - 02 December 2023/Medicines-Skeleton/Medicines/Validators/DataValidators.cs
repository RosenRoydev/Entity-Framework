using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Validators
{
    public static class DataValidators
    {
        public const int PharmacieNameMinLenght = 2;
        public const int PharmacieNameMaxLenght = 50;

        public const int PhoneNumberLenght = 14;

        public const int MedicineNameLenghtMin = 3;
        public const int MedicineNameLenghtMax = 150;

        public const double MinPrice = 0.01;
        public const double MaxPrice = 1000.00;

        public const int ProducerMaxLenght = 100;
        public const int ProducerMinLenght = 3;

        public const int PatientMinLenghtName = 5;
        public const int PatientMaxLenghtName = 100;

    }
}
