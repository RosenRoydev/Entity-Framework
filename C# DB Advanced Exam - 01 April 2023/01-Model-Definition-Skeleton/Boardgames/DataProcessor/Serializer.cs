namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(ExportCreatorsDto[]), new XmlRootAttribute("Creators"));

            var creators = context.Creators.Where(c => c.Boardgames.Any()).
            Select(c => new ExportCreatorsDto
            {
                BoardgamesCount = c.Boardgames.Count(),
                CreatorName = c.FirstName + " " + c.LastName,

                Boardgames = c.Boardgames.Select(bg => new ExportBoardgamesDto
                {
                    BoardgameName = bg.Name,
                    BoardgameYearPublished = bg.YearPublished,
                }).OrderBy(bg => bg.BoardgameName).ToArray(),
                
            }).OrderByDescending(c => c.BoardgamesCount).ThenBy(c=> c.CreatorName).ToArray();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(String.Empty, String.Empty);
            using (StringWriter sw = new StringWriter(sb)) 
            {
                serializer.Serialize(sw, creators,ns);
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers.
                Where(s => s.BoardgamesSellers.Any(bs => bs.Boardgame.YearPublished >= year && bs.Boardgame.Rating <= rating)).
                Select(s => new
                {
                    s.Name,
                    s.Website,
                    Boardgames = s.BoardgamesSellers.
                   Where(bs => bs.Boardgame.YearPublished >= year && bs.Boardgame.Rating <= rating).
                   Select(bs => new
                   {
                       bs.Boardgame.Name,
                       bs.Boardgame.Rating,
                       bs.Boardgame.Mechanics,
                       Category = bs.Boardgame.CategoryType.ToString(),
                   }).OrderByDescending(bs => bs.Rating).ThenBy(bs => bs.Name).ToArray()
                }).OrderByDescending(s => s.Boardgames.Length).ThenBy(s => s.Name).Take(5).ToArray();

            return JsonConvert.SerializeObject(sellers,Formatting.Indented);
        }
    }
}



