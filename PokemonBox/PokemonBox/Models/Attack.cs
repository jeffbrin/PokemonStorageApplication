using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    class Attack
    {
        public int Damage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PokemonType Type { get; set; }
    }
}
