using Boardgames.Data.Models.Enums;
using Boardgames.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using Boardgames.Validators;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Boardgame")]
    public class ImportBoardgamesDTO
    {
        [Required]
        [MaxLength(DataValidators.MaxlenghtName)]
        [MinLength(DataValidators.BoardgameNameMinLenght)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(DataValidators.MinRating,DataValidators.MaxRating)]
        [XmlElement("Rating")]
        public double Rating { get;set; }

        [Required]
        [Range(DataValidators.MinYear,DataValidators.MaxYear)]
        [XmlElement("YearPublished")]
        public int YearPublished { get;set; }

        [Required]
        [Range(0,4)]
        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }

        [Required]
        [XmlElement("Mechanics")]
        public string Mechanics { get; set; } = null!;
    }
}
//< Boardgames >

//            < Boardgame >

//                < Name > 4 Gods </ Name >

//                < Rating > 7.28 </ Rating >

//                < YearPublished > 2017 </ YearPublished >

//                < CategoryType > 4 </ CategoryType >

//                < Mechanics > Area Majority / Influence, Hand Management, Set Collection, Simultaneous Action Selection, Worker Placement</Mechanics>
