using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    public class PCBox
    {
        // These are static since every PCBox would share the same options
        // They still need to be set in the constructor
        static private Pokemon[] pokemonOptions;
        static private Attack[] attackOptions;
        static private Ability[] abilityOptions;

        private List<Pokemon> storedPokemon;

        public PCBox(int maxStorageSlots)
        {

            MaxStorageSlots = maxStorageSlots;

            // Initialize stored pokemon
            storedPokemon = new List<Pokemon>();

            // Get the dictionaries generated from csv files by the DataReader class
            Dictionary<string, Dictionary<string, string[]>> pokemonTypeDictionary = DataReaderWriter.GetTypesInformation("PokemonData/defenceMatchups.csv");
            Dictionary<string, Dictionary<string, string[]>> attackTypeDictionary = DataReaderWriter.GetTypesInformation("PokemonData/attackMatchups.csv");

            // Get the arrays of pokemon types
            PokemonType[] pokemonTypes = GetAllTypes(pokemonTypeDictionary);
            PokemonType[] attackTypes = GetAllTypes(attackTypeDictionary);

            // Get the typeName: PokemonType dictionaries
            Dictionary<string, PokemonType> pokemonTypeByNameDictionary = GetPokemonTypesDictionary(pokemonTypes);
            Dictionary<string, PokemonType> attackTypeByNameDictionary = GetPokemonTypesDictionary(attackTypes);

            // Get all the pokemon options from the backend
            pokemonOptions = DataReaderWriter.GetPokemonOptions("PokemonData/pokemonData.csv", pokemonTypeByNameDictionary);
            attackOptions = DataReaderWriter.GetAttackOptions("PokemonData/attackOptions.csv", attackTypeByNameDictionary);
            abilityOptions = DataReaderWriter.GetAbilityOptions("PokemonData/abilityOptions.csv");
        }

        // The size of the box
        public int MaxStorageSlots { get; set; }

        // Indicated whether the box full
        public bool IsFull
        {
            get { return StoredPokemon.Count == MaxStorageSlots; }
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
                pokemonTypes.Add(new PokemonType(pokemonTypeName,
                                    typeMatchups.ContainsKey("Weak") ? typeMatchups["Weak"] : null,
                                    typeMatchups.ContainsKey("Effective") ? typeMatchups["Effective"] : null,
                                    typeMatchups.ContainsKey("Immune") ? typeMatchups["Immune"] : null));
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

        // All the species of pokemon that the user can choose from
        static public Pokemon[] PokemonOptions
        {
            get { return pokemonOptions; }
        }

        // All the attacks that the user can choose from
        static public Attack[] AttackOptions
        {
            get { return attackOptions; }
        }


        // All the attacks that the user can choose from
        static public Ability[] AbilityOptions
        {
            get { return abilityOptions; }
        }

        // All the pokemon currently stored in the box
        public List<Pokemon> StoredPokemon
        {
            get { return storedPokemon; }
        }

        /// <summary>
        /// Add a pokemon to the box.
        /// </summary>
        /// <param name="pokemon">The pokemon to add.</param>
        /// <returns>Whether the pokemon was successfully added.</returns>
        public bool AddPokemon(Pokemon pokemon)
        {
            if (storedPokemon.Count < MaxStorageSlots)
            {
                storedPokemon.Add(pokemon);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove a pokemon from the box.
        /// </summary>
        /// <param name="pokemon">The pokemon to remove.</param>
        public void RemovePokemon(Pokemon pokemon)
        {
            storedPokemon.Remove(pokemon);
        }
    }
}
