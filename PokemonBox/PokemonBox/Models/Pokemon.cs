using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel; // For the interface

namespace PokemonBox.Models
{
    // Implements INotifyPropertyChanged for the calculated properties
    public class Pokemon : INotifyPropertyChanged
    {

        // The folder in which the images are stored
        private const string SPRITES_DIRECTORY_PATH = "Images/";
        // The folder in which the non animated sprites are stored
        private const string BOX_SPRITES_DIRECTORY_PATH = "BoxSprites/";
        // The folder in which the animated sprites are stored
        private const string ANIMATED_SPRITES_DIRECTORY_PATH = "AnimatedSprites/";
        // The file extension of the images
        private const string ICON_FILE_EXTENSIONS = ".gif";
        // The file extension for the Box Pokemon Images
        private const string BOX_FILE_EXTENSIONS = ".png";
        // The file extensions of the pokemon cries
        private const string POKEMON_CRY_EXTENSION = ".flac";
        // The path to the pokemon cries
        private const string POKEMON_CRY_PATH = "Audio/Cries/";

        private bool isShiny;
        private char sex;
        private string species;

        public int PokedexNumber { get; set; }
        public string Nickname { get; set; }
        public PokemonType[] Types { get; set; }
        public int BaseHealth { get; set; }
        public int BaseAttack { get; set; }
        public int BaseDefence { get; set; }
        public int BaseSpecialAttack { get; set; }
        public int BaseSpecialDefence { get; set; }
        public int BaseSpeed { get; set; }
        public Attack[] Attacks { get; set; }
        public Ability Ability { get; set; }

        private enum CSVColumns
        {
            PokedexNumber,
            Species,
            Nickname,
            TypeOne,
            TypeTwo,
            BaseHealth,
            BaseDefence,
            BaseAttack,
            BaseSpecialAttack,
            BaseSpecialDefence,
            BaseSpeed,
            AttackOne,
            AttackTwo,
            AttackThree,
            AttackFour,
            Ability,
            Shiny,
            Sex
        }

        /// <summary>
        /// Sets the property values from csv data, excluding the attacks, ability and type.
        /// </summary>
        public string CSVData
        {
            get
            {
                // Write each property as csv data
                string typeTwoName = Types[1] == null ? string.Empty : Types[1].Name;
                string[] attackIds = new string[Attacks.Length];
                for (int i = 0; i < Attacks.Length; i++)
                    attackIds[i] = Attacks[i] == null ? string.Empty : Attacks[i].AttackId.ToString();

                return $"{PokedexNumber},{Species},{Nickname},{Types[0].Name},{typeTwoName}," +
                        $"{BaseHealth},{BaseAttack},{BaseDefence},{BaseSpecialAttack},{BaseSpecialDefence},{BaseSpeed}," +
                        $"{attackIds[0]},{attackIds[1]},{attackIds[2]},{attackIds[3]},{Ability.AbilityId},{IsShiny},{Sex}";
            }
            set
            {
                // split the passed value
                string[] pokemonData = value.Split(',');

                // Assign each field its corresponding property
                PokedexNumber = int.Parse(pokemonData[(int)CSVColumns.PokedexNumber]);
                Species = pokemonData[(int)CSVColumns.Species];
                Nickname = pokemonData[(int)CSVColumns.Nickname];
                BaseHealth = int.Parse(pokemonData[(int)CSVColumns.BaseHealth]);
                BaseAttack = int.Parse(pokemonData[(int)CSVColumns.BaseAttack]);
                BaseDefence = int.Parse(pokemonData[(int)CSVColumns.BaseDefence]);
                BaseSpecialAttack = int.Parse(pokemonData[(int)CSVColumns.BaseSpecialDefence]);
                BaseSpecialDefence = int.Parse(pokemonData[(int)CSVColumns.BaseSpecialDefence]);
                BaseSpeed = int.Parse(pokemonData[(int)CSVColumns.BaseSpeed]);
                IsShiny = bool.Parse(pokemonData[(int)CSVColumns.Shiny]);
                Sex = char.Parse(pokemonData[(int)CSVColumns.Sex]);
            }
        }

        // The path to this pokemon's cry sound effect file
        public string CrySoundPath
        {
            get
            {
                // Format pokedex number to have leading zeros to match the path's naming conventions
                string pokedexNumberWithLeadingZeros = new string('0', 3 - PokedexNumber.ToString().Length) + PokedexNumber.ToString();
                return POKEMON_CRY_PATH + pokedexNumberWithLeadingZeros + "_" + Species.ToLower() + POKEMON_CRY_EXTENSION;
            }
        }

        // Default for object initialization syntax
        public Pokemon()
        {
            // Always at most 4 attacks
            Attacks = new Attack[4];
        }

        // constructor for making a new pokemon created by the player
        // used to make a copy of another pokemon
        public Pokemon(Pokemon generic, Attack[] attacks, bool shiny = false, char sex = 'M', Ability ability = null, string nickname = "")
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
            IsShiny = shiny;
            Sex = sex;
            Ability = ability;
            Attacks = attacks;
        }

        // The sex of the pokemon can only be M or F
        // Not auto implemented since it needs validation
        public char Sex
        {
            get { return sex; }
            set
            {
                char tempSex = char.ToUpper(value);
                if (tempSex != 'M' && tempSex != 'F')
                    throw new ArgumentException("Sex", "Sex must be M or F for a pokemon.");
                sex = tempSex;
            }
        }

        // The path to this pokemon's animated sprites
        public string AnimatedSpritePath
        {
            get
            {
                if (IsShiny) return $"{SPRITES_DIRECTORY_PATH}{ANIMATED_SPRITES_DIRECTORY_PATH}Shiny/{PokedexNumber}{ICON_FILE_EXTENSIONS}";
                return $"{SPRITES_DIRECTORY_PATH}{ANIMATED_SPRITES_DIRECTORY_PATH}Regular/{Species}{ICON_FILE_EXTENSIONS}";
            }
        }

        // The path to this pokemon's non animated sprites
        public string BoxSpritePath
        {
            get
            {
                if (IsShiny) return $"{SPRITES_DIRECTORY_PATH}{BOX_SPRITES_DIRECTORY_PATH}Shiny/{PokedexNumber}{BOX_FILE_EXTENSIONS}";
                return $"{SPRITES_DIRECTORY_PATH}{BOX_SPRITES_DIRECTORY_PATH}Regular/{PokedexNumber}{BOX_FILE_EXTENSIONS}";

            }
        }

        // The display name of the pokemon is either the species name or the nickname if it exists
        public string DisplayName
        {
            get { return string.IsNullOrWhiteSpace(Nickname) ? Species : Nickname; }
        }

        // Whether of not this pokemon is shiny. Custom setter is needed for PropertyChanged events on the images
        public bool IsShiny
        {
            get { return isShiny; }
            set
            {
                isShiny = value;
                NotifyPropertyChanged("AnimatedSpritePath"); // Notify that the animated sprite was changed
                NotifyPropertyChanged("BoxSpritePath"); // Notify that the box sprite was changed
            }
        }

        // Needed to notify that the image has changed, used in edit window
        public string Species
        {
            get { return species; }
            set
            {
                species = value;
                NotifyPropertyChanged("AnimatedSpritePath"); // Notify that the animated sprite was changed
                NotifyPropertyChanged("BoxSpritePath"); // Notify that the box sprite was changed
            }
        }


        #region INotify
        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Invoke a property changed event on the property
            
            // This function checks if the PropertyChanged event handler is null
            // If it exists, then it invokes the event, with this object and the name of the property that has been changed
            // This is necessary to change the image sources since they are calculated properties and this event
            // won't be called automatically when the setter is used
        }

        // The event to invoke when a property has been changed, (built in)
        public event PropertyChangedEventHandler PropertyChanged;


        #endregion
    }
}
