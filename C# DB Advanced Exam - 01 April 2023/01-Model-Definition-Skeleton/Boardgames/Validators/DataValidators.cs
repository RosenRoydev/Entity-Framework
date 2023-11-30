using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Validators
{
    public static class DataValidators
    {
        public const int MaxlenghtName = 20;

        public const double MinRating = 1.00;
        public const double Maxraying = 10.00;

        public const int MinYear = 2018;
        public const int MaxYear = 2023;

        public const int SellerNameMaxLenght = 20;
        public const int SellerNameMinLenght = 5;

        public const int AdressMinLenght = 2;
        public const int AdressMaxLenght = 30;

        public const int CreatorFirstNameMaxlenght = 7;
        public const int CreatorFirstNameMinlenght = 2;

        public const int CreatorLastNameMaxlenght = 7;
        public const int CreatorLastNameMinlenght = 2;

    }
}
