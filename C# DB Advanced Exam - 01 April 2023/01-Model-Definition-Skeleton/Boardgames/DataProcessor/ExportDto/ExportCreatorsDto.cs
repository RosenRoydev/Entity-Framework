using Boardgames.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class ExportCreatorsDto
    {
        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }

        [XmlElement("CreatorName")]
        public string CreatorName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public ExportBoardgamesDto[] Boardgames { get; set; }   
    }
}
//< Creators >
//  < Creator BoardgamesCount = "6" >
//    < CreatorName > Cade O'Neill</CreatorName>
//    <Boardgames>
//      <Boardgame>
//        <BoardgameName>Bohnanza: The Duel</BoardgameName>
//        <BoardgameYearPublished>2019</BoardgameYearPublished>
//      </Boardgame>
//      <Boardgame>
//        <BoardgameName>Great Western Trail</BoardgameName>
//        <BoardgameYearPublished>2018</BoardgameYearPublished>
//      </Boardgame>
//      <Boardgame>
