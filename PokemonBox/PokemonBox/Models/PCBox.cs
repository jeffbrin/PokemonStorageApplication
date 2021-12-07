using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    class PCBox
    {
        

        private PokemonType[] GetAllTypes(Dictionary<string, Dictionary<string, string[]>> typeDictionary)
        {
            List<PokemonType> pokemonTypes = new List<PokemonType>();

            foreach (string pokemonTypeName in typeDictionary.Keys)
            {
                Dictionary<string, string[]> typeMatchups = typeDictionary[pokemonTypeName];
                pokemonTypes.Add(new PokemonType(pokemonTypeName, typeMatchups["Weak"], typeMatchups["Effective"], typeMatchups["Immune"]));
            }

            return pokemonTypes.ToArray();
        }

        
    }
}
