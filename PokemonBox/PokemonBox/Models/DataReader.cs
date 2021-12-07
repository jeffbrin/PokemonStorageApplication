using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PokemonBox.Models
{
    static class DataReader
    {
        static public Pokemon[] GetPokemonInformation(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    // TODO: CHANGE TO AN ENUM
                    const int POKEDEX_NUMBER_COLUMN = 0, NAME_COLUMN = 1, TYPE_ONE_COLUMN = 2, TYPE_TWO_COLUMN = 3, HEALTH_COLUMN = 4;
                    const int ATTACK_COLUMN = 5, DEFENCE_COLUMN = 6, SPECIAL_ATTACK_COLUMN = 7, SPECIAL_DEFENCE_COLUMN = 8, SPEED_COLUMN = 9;
                    string[] pokemonLines = File.ReadAllLines(path);
                    Pokemon[] pokemon = new Pokemon[pokemonLines.Length];

                    for (int i = 0; i < pokemonLines.Length; i++)
                    {
                        string[] pokemonData = pokemonLines[i].Split(',');
                        pokemon[i] = new Pokemon()
                        {
                            PokedexNumber = int.Parse(pokemonData[POKEDEX_NUMBER_COLUMN]),
                            Name = pokemonData[NAME_COLUMN],
                            Types = new PokemonType[2] { new PokemonType(pokemonData[TYPE_ONE_COLUMN]), string.IsNullOrEmpty(pokemonData[TYPE_TWO_COLUMN]) ? null : new PokemonType(pokemonData[TYPE_TWO_COLUMN]) }
                        };
                    }

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
    }
}
