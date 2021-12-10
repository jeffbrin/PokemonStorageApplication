using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    class Pokemon
    {

        // The folder in which the images are stored
        private const string SPRITES_DIRECTORY_PATH = "PokemonData/PokemonSprites";
        // The file extension of the images
        private const string ICON_FILE_EXTENSIONS = ".gif";

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
        public bool Shiny { get; set; }
        public string Sex { get; set; }
        public Ability Ability { get; set; }

        // Default for object initialization syntax
        public Pokemon()
        {

        }

        // constructor for making a new pokemon created by the player
        public Pokemon(Pokemon generic, List<Attack> attacks, bool shiny, string sex, Ability ability, string nickname)
        {
            PokedexNumber = generic.PokedexNumber;
            Species = generic.Species;
            Nickname = nickname;
            Types = generic.Types;
            BaseHealth = generic.BaseHealth;
            BaseAttack = generic.BaseAttack;
            BaseDefence = generic.BaseDefence;
            BaseSpecialAttack = generic.BaseSpecialAttack;
            BaseSpecialDefence = generic.BaseSpecialDefence;
            BaseSpeed = generic.BaseSpeed;
            Attacks = attacks;
            Shiny = shiny;
            Sex = sex;
            Ability = ability;
        }

        // The images are stored as their species name.png
        public string SpritePath
        {
            get {
                if (Shiny) return $"{SPRITES_DIRECTORY_PATH}Shiny/{PokedexNumber}{ICON_FILE_EXTENSIONS}";
                return $"{SPRITES_DIRECTORY_PATH}{Species}Regular/{ICON_FILE_EXTENSIONS}";
            }
        }

        // The display name of the pokemon is either the species name or the nickname if it exists
        public string DisplayName
        {
            get { return Nickname != string.Empty ? Nickname : Species; }
        }

        // This can get deleted
        public override string ToString()
        {
            return $"{DisplayName}|{PokedexNumber}";
        }

    }
}
