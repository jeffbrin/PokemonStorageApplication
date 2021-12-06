using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    class PokemonType : ElementType
    {
        private PokemonType[] weaknesses; // Not sure what the best type for these are
        private PokemonType[] resistances; // Could be PokemonTypes or maybe strings 
        private string typeName;

        static Dictionary<string, string[]> typeResistances =
             new Dictionary<string, string[]>();

        static Dictionary<string, string[]> typeWeaknesses =
             new Dictionary<string, string[]>();

        static Dictionary<string, string[]> typeImmunities =
             new Dictionary<string, string[]>();


        public PokemonType(string typeName)
        {

        }

        static PokemonType()
        {
            typeResistances.Add("Normal", null);
            typeWeaknesses.Add("Normal", new string[] {"Fighting"});
            typeImmunities.Add("Normal", new string[] { "Ghost" });

            typeResistances.Add("Fire", new string[] { "Fire", "Grass", "Bug"});
            typeWeaknesses.Add("Fire", new string[] { "Water" });
            typeImmunities.Add("Normal", new string[] { "Ghost" });
        }
    }
}
