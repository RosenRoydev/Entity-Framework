﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportProductsDTO
    {
        [JsonProperty("Name")]
        [Required]
        [MinLength(9)]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        [JsonProperty("Price")]
        [Required]
        [Range(5.00, 1000.00)]
        public decimal Price { get; set; }

        [JsonProperty("CategoryType")]
        [Required]
        [Range(0,4)]
        public int CategoryType { get;set; }

        [Required]
        [JsonProperty("Clients")]
        public List<int> Clients { get; set; } = new List<int>();
    }
}

//"Name": "ADR plate",
//    "Price": 14.97,
//    "CategoryType": 1,
//    "Clients": [
//      1,
//      105,
//      1,
//      5,
//      15
//    ]
//  },
