using CarDealer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Import
{
    public class ImportSaleDTO
    {
        [JsonProperty("discount")]
        public int Discount { get; set; }

        [JsonProperty("customerId")]
        public int CustomerId { get; set; }
        

        [JsonProperty("carId")]
            public int CarId { get; set; }
        
    }
}
