using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    class PCBox
    {

        private Dictionary<string, PokemonType> pokemonTypeByNameDictionary;
        private Dictionary<string, PokemonType> attackTypeByNameDictionary;

        public PCBox()
        {
            // Get the dictionaries generated from csv files by the DataReader class
            Dictionary<string, Dictionary<string, string[]>> pokemonTypeDictionary = DataReader.GetTypesInformation("PokemonData/defenceMatchups.csv");
            Dictionary<string, Dictionary<string, string[]>> attackTypeDictionary = DataReader.GetTypesInformation("PokemonData/attackMatchups.csv");

            // Get the arrays of pokemon types
            PokemonType[] pokemonTypes = GetAllTypes(pokemonTypeDictionary);
            PokemonType[] attackTypes = GetAllTypes(attackTypeDictionary);

            // Get the typeName: PokemonType dictionaries
            pokemonTypeByNameDictionary = GetPokemonTypesDictionary(pokemonTypes);
            attackTypeByNameDictionary = GetPokemonTypesDictionary(attackTypes);

        }

        /// <summary>
        /// Gets an array of types based on the data passed in as typeDictionary.
        /// </summary>
        /// <param name="typeDictionary">A dictionary accessed with type names, whose values are another dictionary with that type's weaknesses</param>
        /// <returns>A list of PokemonTypes generated from the dictionary passed as an argument</returns>
        private PokemonType[] GetAllTypes(Dictionary<string, Dictionary<string, string[]>> typeDictionary)
        {
            // Make the empty list
            List<PokemonType> pokemonTypes = new List<PokemonType>();

            // Loop through the keys in the passed dictionary. Will be "Normal", "Fire", "Water", ...
            foreach (string pokemonTypeName in typeDictionary.Keys)
            {
                // Get the matchups of the type being iterated over
                Dictionary<string, string[]> typeMatchups = typeDictionary[pokemonTypeName];
                // Instantiate this pokemon type and add it to the list
                pokemonTypes.Add(new PokemonType(pokemonTypeName, typeMatchups["Weak"], typeMatchups["Effective"], typeMatchups["Immune"]));
            }

            // Return the pokemon types list as an array
            return pokemonTypes.ToArray();
        }

        /// <summary>
        /// Gets a dictionary of name: PokemonType key value pairs to access PokemonTypes by name
        /// </summary>
        /// <param name="pokemonTypes">A list of PokemonTypes generated from a csv file</param>
        /// <returns>A dictionary of string keys with PokemonType values to access the PokemonTypes by name</returns>
        private Dictionary<string, PokemonType> GetPokemonTypesDictionary(PokemonType[] pokemonTypes)
        {
            // Declare the new empty dictionary 
            Dictionary<string, PokemonType> pokemonTypeDictionary = new Dictionary<string, PokemonType>();

            // Loop through all the created types
            foreach (PokemonType type in pokemonTypes)
            {
                // Add the type being iterated over to the dictionary
                pokemonTypeDictionary.Add(type.Name, type);
            }

            return pokemonTypeDictionary;
        }
        
        public PokemonType GetDefensiveType(string typeName)
        {
            if (pokemonTypeByNameDictionary.ContainsKey(typeName))
                return pokemonTypeByNameDictionary[typeName];
            return null;
        }

        public PokemonType GetAttackType(string typeName)
        {
            if (attackTypeByNameDictionary.ContainsKey(typeName))
                return attackTypeByNameDictionary[typeName];
            return null;
        }

    }
}
