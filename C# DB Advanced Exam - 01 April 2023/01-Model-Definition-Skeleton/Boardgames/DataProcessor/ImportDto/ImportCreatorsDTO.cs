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
    [XmlType("Creator")]
    public class ImportCreatorsDTO
    {
        [Required]
        [MinLength(DataValidators.CreatorFirstNameMinlenght)]
        [MaxLength(DataValidators.CreatorFirstNameMaxlenght)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(DataValidators.CreatorLastNameMinlenght)]
        [MaxLength (DataValidators.CreatorLastNameMaxlenght)]
        public string LastName { get; set; } = null!;

        [Required]
        [XmlArray("Boardgames")]
        public ImportBoardgamesDTO[] Boardgames { get; set; } = null!;

    }
}
//<? xml version = '1.0' encoding = 'UTF-8' ?>
//< Creators >

//    < Creator >

//        < FirstName > Debra </ FirstName >

//        < LastName > Edwards </ LastName >

//        < Boardgames >

//            < Boardgame >

//                < Name > 4 Gods </ Name >

//                < Rating > 7.28 </ Rating >

//                < YearPublished > 2017 </ YearPublished >

//                < CategoryType > 4 </ CategoryType >

//                < Mechanics > Area Majority / Influence, Hand Management, Set Collection, Simultaneous Action Selection, Worker Placement</Mechanics>
//			</Boardgame>
//			<Boardgame>
//				<Name>7 Steps</Name>
//				<Rating>7.01</Rating>
//				<YearPublished>2015</YearPublished>
//				<CategoryType>4</CategoryType>
//				<Mechanics>Action Queue, Hand Management, Push Your Luck, Set Collection</Mechanics>
//			</Boardgame>
//	     …
//		</Boardgames>
//	</Creator>
