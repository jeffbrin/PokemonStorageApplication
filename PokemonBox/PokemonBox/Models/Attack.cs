using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    class Attack
    {
        public string Category { get; set; } // Physical or special
        public int Power { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PokemonType AttackType { get; set; }
        public int PP { get; set; }
    }
}
