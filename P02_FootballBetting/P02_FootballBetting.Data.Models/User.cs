using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(150)]
        public string Username { get; set; } = null!;
        [Required]
        [StringLength(150)]
        public string Password { get; set; } = null!;
        [StringLength(150)]
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = null!;
        public decimal Balance { get; set; }
        
        public virtual ICollection<Bet> Bets { get; set; }

    }
}
