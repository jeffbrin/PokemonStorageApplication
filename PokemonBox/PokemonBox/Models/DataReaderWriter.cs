﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PokemonBox.Models
{
    static class DataReaderWriter
    {
        // Used to read the lines in the GetPokemonOptions function
        private enum PokemonDatasheetColumns
        {
            PokedexNumber,
            Species,
            TypeOne,
            TypeTwo,
            Health,
            Attack,
            Defence,
            SpecialAttack,
            SpecialDefence,
            Speed
        }

        // Used to read the lines in the GetPokemonOptions function
        private enum AttackDatasheetColumns
        {
            Name,
            Type,
            Category,
            Description,
            Power,
            Accuracy,
            PP
        }

        /// <summary>
        /// Gets an array of pokemon generated from the csv file passed in through the path
        /// </summary>
        /// <param name="path">A csv file with pokemon data</param>
        /// <param name="pokemonTypeDictionary">A dictionary that returns a pokemon type when passed a string</param>
        /// <returns></returns>
        static public Pokemon[] GetPokemonOptions(string path, Dictionary<string, PokemonType> pokemonTypeDictionary)
        {

            try
            {
                // Check if the csv file exists
                if (File.Exists(path))
                {
                    
                    // Read all the lines from the file and instantiate the empty pokemon array
                    string[] pokemonLines = File.ReadAllLines(path);
                    Pokemon[] pokemon = new Pokemon[pokemonLines.Length];

                    // Loop through each line in the file
                    for (int i = 0; i < pokemonLines.Length; i++)
                    {
                        // Split the pokemon data on the comma
                        string[] pokemonData = pokemonLines[i].Split(',');
                        // Generate the new pokemon from this line of data
                        pokemon[i] = new Pokemon()
                        {
                            PokedexNumber = int.Parse(pokemonData[(int)PokemonDatasheetColumns.PokedexNumber]), // Get the pokedex number
                            Species = pokemonData[(int)PokemonDatasheetColumns.Species], //Get the pokemon's species
                            Types = new PokemonType[] {  // Add the pokemon's types. Make the second type null if it isn't a dual type
                                pokemonTypeDictionary[pokemonData[(int)PokemonDatasheetColumns.TypeOne]], 
                                string.IsNullOrEmpty(pokemonData[(int)PokemonDatasheetColumns.TypeTwo]) ? null : pokemonTypeDictionary[pokemonData[(int)PokemonDatasheetColumns.TypeTwo]] 
                            },
                            BaseAttack = int.Parse(pokemonData[(int)PokemonDatasheetColumns.Attack]), // Get the base attack from the line of the file
                            BaseDefence = int.Parse(pokemonData[(int)PokemonDatasheetColumns.Defence]), // Get the base defence from the line of the file
                            BaseHealth = int.Parse(pokemonData[(int)PokemonDatasheetColumns.Health]), // Get the base health from the line of the file
                            BaseSpecialAttack = int.Parse(pokemonData[(int)PokemonDatasheetColumns.SpecialAttack]), // Get the base special defence from the line of the file
                            BaseSpecialDefence = int.Parse(pokemonData[(int)PokemonDatasheetColumns.SpecialDefence]), // Get the base Special defence from the line of the file
                            BaseSpeed = int.Parse(pokemonData[(int)PokemonDatasheetColumns.Speed]), // Get the speed from the line of the file
                            Nickname = string.Empty // Set the nickname to empty to be set later
                        };
                    }

                    return pokemon; // return all the pokemon array

                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }

        /// <summary>
        /// Reads the type matchups file and returns a dictionary used to find the strengths and weaknesses of different attack types.
        /// The dictionary is nested and is used to access the info with the syntax:
        /// dictionary[TYPE_NAME][TYPE_RELATION] with TYPE_RELATION = "Weak", "Effective", or "Immune"
        /// </summary>
        /// <param name="path">The path to the attack matchups file</param>
        /// <returns>A nested dictionary of super effective types, weak types, and immunities for each pokemon type.</returns>
        static public Dictionary<string, Dictionary<string, string[]>> GetTypesInformation(string path)
        {
            try
            {
                if (File.Exists(path))
                {

                    // Read all lines
                    string[] lines = File.ReadAllLines(path);
                    // Scan each line
                    bool nextLineIsMainTypeName = false; // Indicated whether the next line will be a type of interest
                    string typeSeparator = null; // The separator in the csv file between types
                    string typeOfInterest = null; // The type that is having a dictionary made for it currently
                    string typeRelation = null; // The relation the type of interest has with the types below it I.E. Type of relation is strong, weak or has no effect against them
                    Dictionary<string, string[]> relationshipDictionary = new Dictionary<string, string[]>();
                    Dictionary<string, Dictionary<string, string[]>> attackTypesAndRelationsDictionary = new Dictionary<string, Dictionary<string, string[]>>();
                    foreach (string typeInfoLine in lines)
                    { 

                        // Set the typeSeparator to the first line of the file, which will contain the separator
                        if (typeSeparator == null)
                            typeSeparator = typeInfoLine;

                        // If the current line is the separator, the next line is a new type of interest
                        if (nextLineIsMainTypeName)
                        {
                            typeOfInterest = typeInfoLine;
                            nextLineIsMainTypeName = false;
                        }
                        else if (typeInfoLine == typeSeparator)
                        {
                            nextLineIsMainTypeName = true;

                            // Check if types of interest is null,
                            // this will be true for the first type
                            if (typeOfInterest != null)
                            {
                                attackTypesAndRelationsDictionary.Add(typeOfInterest, relationshipDictionary);
                                relationshipDictionary = new Dictionary<string, string[]>();
                            }
                        }
                        else if (typeRelation == null)
                            typeRelation = typeInfoLine;
                        else
                        {
                            string[] typesInRelation = null;

                            // If the line has content add it to the typesInRelation
                            if (!string.IsNullOrEmpty(typeInfoLine))
                                typesInRelation = typeInfoLine.Split(',');

                            relationshipDictionary.Add(typeRelation, typesInRelation);

                            // The type relation has been used and it's time for another to be read
                            typeRelation = null;
                        }

                    }

                    // Return the info dictionary
                    return attackTypesAndRelationsDictionary;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }
    
        /// <summary>
        /// Reads the attack data file and returns an array of all the attacks the user can give their pokemon.
        /// </summary>
        /// <param name="path">The path to the attack options csv file</param>
        /// <param name="stringToTypeDictionary">A dictionary to get a pokemon type from a string</param>
        /// <returns></returns>
        static public Attack[] GetAttackOptions(string path, Dictionary<string, PokemonType> stringToTypeDictionary)
        {
            try
            {
                // Check if the csv file exists
                if (File.Exists(path))
                {

                    // Read all the lines from the file and instantiate the empty pokemon array
                    string[] attackLines = File.ReadAllLines(path);
                    Attack[] attacks = new Attack[attackLines.Length];

                    // Loop through each line in the file
                    for (int i = 0; i < attackLines.Length; i++)
                    {
                        // Split the pokemon data on the comma
                        string[] attackData = attackLines[i].Split(',');
                        // Generate the new pokemon from this line of data
                        attacks[i] = new Attack()
                        {
                            Name = attackData[(int)AttackDatasheetColumns.Name], // Get the attack name
                            Category = attackData[(int)AttackDatasheetColumns.Category], // Get the attack category (Physical or special)
                            Description = attackData[(int)AttackDatasheetColumns.Description], // Get the attack description
                            Power = int.Parse(attackData[(int)AttackDatasheetColumns.Power]), // Get the attack power
                            PP = int.Parse(attackData[(int)AttackDatasheetColumns.PP]), // Get the attack pp
                            AttackType = stringToTypeDictionary[attackData[(int)AttackDatasheetColumns.Type]], // Get the PokemonType related to that attack
                            AttackId = i
                        };
                    }

                    return attacks; // return all the pokemon array

                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }

        /// <summary>
        /// Reads the attack data file and returns an array of all the attacks the user can give their pokemon.
        /// </summary>
        /// <param name="path">The path to the attack options csv file</param>
        /// <param name="stringToTypeDictionary">A dictionary to get a pokemon type from a string</param>
        /// <returns></returns>
        static public Ability[] GetAbilityOptions(string path)
        {

            {
                try
                {
                    // Check if the csv file exists
                    if (File.Exists(path))
                    {

                        // Read all the lines from the file and instantiate the empty pokemon array
                        string[] abilityLines = File.ReadAllLines(path);
                        Ability[] abilities = new Ability[abilityLines.Length];

                        // Loop through each line in the file
                        for (int i = 0; i < abilityLines.Length; i++)
                        {
                            // Split the pokemon data on the comma
                            string[] abilityData = abilityLines[i].Split(',');
                            // Generate the new pokemon from this line of data
                            abilities[i] = new Ability()
                            {
                                Name = abilityData[0],
                                Description = abilityData[1],
                                AbilityId = i
                            };
                        }

                        return abilities; // return all the pokemon array

                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return null;
            }
        }


        private enum LoadingCSVColumns
        {
            TypeOne = 3,
            TypeTwo = 4,
            AttackOneID = 11,
            AttackTwoID = 12,
            AttackThreeID = 13,
            AttackFourID = 14,
            AbilityID = 15
        }

        /// <summary>
        /// Gets a list of pokemon read from a file.
        /// </summary>
        /// <param name="path">The file path of the file holding the pokemon info</param>
        /// <param name="nameToTypeDictionary">A dictionary that returns a PokemonType object when given a string.</param>
        /// <returns></returns>
        static public List<Pokemon> LoadBoxFromFile(string path, Dictionary<string, PokemonType> nameToTypeDictionary)
        {
            try
            {
                // Read from the file
                if (File.Exists(path))
                {
                    string[] pokemonLines = File.ReadAllLines(path);
                    List<Pokemon> boxedPokemon = new List<Pokemon>();

                    // Get all the pokemon in the box, let the user decide how many to keep
                    foreach (string line in pokemonLines)
                    {
                        string[] values = line.Split(',');

                        // Create a new pokemon and add to the list
                        boxedPokemon.Add(new Pokemon()
                        {
                            CSVData = line,

                            // Add the types, null if there's no secondary type
                            Types = new PokemonType[] {
                                nameToTypeDictionary[values[(int)LoadingCSVColumns.TypeOne]],
                                values[(int)LoadingCSVColumns.TypeTwo] == string.Empty ? null : nameToTypeDictionary[values[(int)LoadingCSVColumns.TypeTwo]]
                            },
                            // Add the attacks, null if there's no attack
                            Attacks = new Attack[] {
                                values[(int)LoadingCSVColumns.AttackOneID] == "" ? null : PCBox.AttackOptions[int.Parse(values[(int)LoadingCSVColumns.AttackOneID])],
                                values[(int)LoadingCSVColumns.AttackTwoID] == "" ? null : PCBox.AttackOptions[int.Parse(values[(int)LoadingCSVColumns.AttackTwoID])],
                                values[(int)LoadingCSVColumns.AttackThreeID] == "" ? null : PCBox.AttackOptions[int.Parse(values[(int)LoadingCSVColumns.AttackThreeID])],
                                values[(int)LoadingCSVColumns.AttackFourID] == "" ? null : PCBox.AttackOptions[int.Parse(values[(int)LoadingCSVColumns.AttackFourID])]
                            },

                            // This should never be null
                            Ability = PCBox.AbilityOptions[int.Parse(values[(int)LoadingCSVColumns.AbilityID])]
                        });
                    }

                    return boxedPokemon;
                }

            }
            catch (Exception)
            {

                return null;
            }

            return null;
        }

        // Saves the box to the file located at the path
        static public bool SaveBox(string path, Pokemon[] boxedPokemon)
        {
            try
            {

                StringBuilder sb = new StringBuilder();

                // Append a pokemon to each line
                foreach (Pokemon pokemon in boxedPokemon)
                {
                    sb.AppendLine(pokemon.CSVData);
                }

                // Write to the file
                File.WriteAllText(path, sb.ToString());

            }
            catch (Exception)
            {
                // Unsuccessful write
                return false;
                throw;
            }

            // Successful write
            return true;
        }
    }
}
