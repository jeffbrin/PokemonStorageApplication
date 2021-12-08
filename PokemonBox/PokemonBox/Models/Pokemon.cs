using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    class Pokemon
    {

        // The folder in which the images are stored
        private const string SPRITES_DIRECTORY_PATH = "PokemonData/";
        // The file extension of the images
        private const string ICON_FILE_EXTENSIONS = ".png";

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

        // The images are stored as their pokedex number.png
        public string SpritePath
        {
            get { return $"{SPRITES_DIRECTORY_PATH}{PokedexNumber}{ICON_FILE_EXTENSIONS}"; }
        }

        // The display name of the pokemon is either the species name or the nickname if it exists
        public string DisplayName
        {
            get { return (Nickname != string.Empty ? Nickname : Species); }
        }

        // This can get deleted
        public override string ToString()
        {
            return $"{DisplayName}|{PokedexNumber}";
        }

    }
}
