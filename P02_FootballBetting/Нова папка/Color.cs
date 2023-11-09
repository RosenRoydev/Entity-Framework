﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Color
    {
        public int ColorId { get; set; }
        [Required]
        public string Name { get; set; }

        [InverseProperty(nameof(Team.PrimaryKitColor))]
        public virtual ICollection<Team> PrimaryKitColors { get; set; }

        [InverseProperty(nameof (Team.SecondaryKitColor))]
        public virtual ICollection<Team> SecondaryKitColors { get; set; }

    }
}
