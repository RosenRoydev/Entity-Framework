namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCreatorsDTO[]), new XmlRootAttribute("Creators"));
            using StringReader reader = new StringReader(xmlString);
            ImportCreatorsDTO[] importCreators = (ImportCreatorsDTO[])serializer.Deserialize(reader);
                List<Creator> creators = new List<Creator>();

            foreach (var creator in importCreators)
            {
                if (!IsValid(creator))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Creator creator1 = new Creator()
                {
                    FirstName = creator.FirstName,
                    LastName = creator.LastName,
                };
                List<Boardgame> boardgames = new List<Boardgame>();
                foreach (var boardgame in creator.Boardgames)
                {
                    if (!IsValid(boardgame))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame1 = new Boardgame()
                    {
                        Name = boardgame.Name,
                        Rating = boardgame.Rating,
                        YearPublished = boardgame.YearPublished,
                        CategoryType = (CategoryType)boardgame.CategoryType,
                        Mechanics = boardgame.Mechanics,
                    };
                   
                    creator1.Boardgames.Add(boardgame1);
                }
                creators.Add(creator1);
                sb.AppendLine(String.Format(SuccessfullyImportedCreator,creator1.FirstName,creator1.LastName,creator1.Boardgames.Count));

            }
            context.Creators.AddRange(creators); 
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportSellersDTO[] importSellersDTOs = JsonConvert.DeserializeObject<ImportSellersDTO[]>(jsonString);
            int[] BoardgamesIds = context.Boardgames.Select(bg => bg.Id).ToArray();
            List<Seller> sellers = new List<Seller>();

            foreach (var importSeller in importSellersDTOs)
            {
                if (!IsValid(importSeller))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Seller seller = new Seller()
                {
                    Name = importSeller.Name,
                    Address = importSeller.Address,
                    Country = importSeller.Country,
                    Website = importSeller.Website,

                };
                
                foreach (var id in importSeller.Boardgames.Distinct())
                {
                    if (!BoardgamesIds.Contains(id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                   
                    seller.BoardgamesSellers.Add(new BoardgameSeller()
                    {
                        BoardgameId = id,
                        SellerId = seller.Id,
                        
                    });   
                }
                sellers.Add(seller);
                sb.AppendLine(String.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
            }
            context.Sellers.AddRange(sellers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
                
            
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
