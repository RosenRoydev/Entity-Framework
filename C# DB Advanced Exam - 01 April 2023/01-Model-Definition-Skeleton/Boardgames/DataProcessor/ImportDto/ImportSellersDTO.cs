using Boardgames.Data.Models;
using Boardgames.Validators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellersDTO
    {
        [JsonProperty("Name")]
        [Required]
        [MaxLength(DataValidators.SellerNameMaxLenght)]
        [MinLength(DataValidators.SellerNameMinLenght)]
        public string Name { get; set; }

        [JsonProperty("Address")]
        [Required]
        [MaxLength(DataValidators.AdressMaxLenght)]
        [MinLength(DataValidators.SellerNameMinLenght)]
        public string Address { get; set; } = null!;

        [JsonProperty("Country")]
        [Required]
        public string Country { get; set; } = null!;

        [JsonProperty("Website")]
        [Required]
        [RegularExpression("^www.[A-Z,a-z,-]+.com")]
        public string Website { get; set; } = null!;

        [JsonProperty("Boardgames")]
       
        public List<int> Boardgames { get; set; }

    }
}
