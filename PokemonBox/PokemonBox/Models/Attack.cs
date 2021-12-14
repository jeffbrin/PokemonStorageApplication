using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    public class Attack
    {
        public string Category { get; set; } // Physical or special
        public int Power { get; set; } // The damage that can be done by the attack
        public string Name { get; set; } 
        public string Description { get; set; }
        public PokemonType AttackType { get; set; } // The type of the attack
        public int PP { get; set; } // Amount of times this attack can be used
        public int AttackId { get; set; }

    }
}
