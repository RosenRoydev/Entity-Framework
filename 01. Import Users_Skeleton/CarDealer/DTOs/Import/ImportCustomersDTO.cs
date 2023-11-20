using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Import
{
    public class ImportCustomersDTO
    {
        public ImportCustomersDTO()
        {
           
        }
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("birthdate")]
        public DateTime BirhtDate { get; set; }

        [JsonProperty("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
