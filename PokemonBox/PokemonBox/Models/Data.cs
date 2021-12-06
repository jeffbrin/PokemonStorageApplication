using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PokemonBox.Models
{
    static class Data
    {
        static public Pokemon[] GetPokemonInformation(string path)
        {
            try
            {
                if (File.Exists(path))
                {
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
                            PrimaryType = new PokemonType(pokemonData[TYPE_ONE_COLUMN]);
                        }
                    }
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
