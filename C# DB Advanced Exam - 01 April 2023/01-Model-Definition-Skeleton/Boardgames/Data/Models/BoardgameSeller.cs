using Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boardgames.Data.Models
{
    public class BoardgameSeller
    {
        [ForeignKey(nameof(BoardgameId))]
        [Required]
        public int BoardgameId { get; set; }
        public Boardgame Boardgame { get; set; }

        [ForeignKey(nameof(SellerId))]
        [Required]
        public int SellerId { get; set; } 
        public Seller Seller { get; set; }

    }
}

//BoardgameSeller
//•	BoardgameId – integer, Primary Key, foreign key (required)
//•	Boardgame – Boardgame
//•	SellerId – integer, Primary Key, foreign key (required)
//•	Seller – Seller
