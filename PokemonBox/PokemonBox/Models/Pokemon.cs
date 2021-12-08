using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    class Pokemon
    {

        private const string SPRITES_DIRECTORY_PATH = "PokemonData/";

        public int PokedexNumber { get; set; }
        public string Species { get; set; }
        public string Nickname { get; set; }
        public PokemonType[] Types {get; set;}
        public int BaseHealth { get; set; }
        public int BaseAttack { get; set; }
        public int BaseDefence { get; set; }
        public int BaseSpecialAttack { get; set; }
        public int BaseSpecialDefence { get; set; }
        public int BaseSpeed { get; set; }
        public List<Attack> Attacks { get; set; }

        public string SpritePath
        {
            get { return $"{SPRITES_DIRECTORY_PATH}{PokedexNumber}"; }
        }

        public string DisplayName
        {
            get { return (Nickname != string.Empty ? Nickname : Species); }
        }

        public override string ToString()
        {
            return $"{DisplayName}|{PokedexNumber}";
        }

    }
}
