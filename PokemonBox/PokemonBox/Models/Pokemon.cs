using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel; // For the interface

namespace PokemonBox.Models
{
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
        // The file extensions of the pokemon cries
        private const string POKEMON_CRY_EXTENSIONS = ".FLAC";
        // The path to the pokemon cries
        private const string POKEMON_CRY_PATH = "Audio/Cries/";

        private bool isShiny;
        private char sex;

        public int PokedexNumber { get; set; }
        public string Species { get; set; }
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

        // Default for object initialization syntax
        public Pokemon()
        {
            // Always at most 4 attacks
            Attacks = new Attack[4];
        }

        // constructor for making a new pokemon created by the player
        public Pokemon(Pokemon generic, bool shiny = false, char sex = 'M', Ability ability = null, string nickname = ""): this()
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
        }

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
                if (IsShiny) return $"{SPRITES_DIRECTORY_PATH}{ANIMATED_SPRITES_DIRECTORY_PATH}Shiny/{PokedexNumber}{ICON_FILE_EXTENSIONS}";
                return $"{SPRITES_DIRECTORY_PATH}{ANIMATED_SPRITES_DIRECTORY_PATH}Regular/{Species}{ICON_FILE_EXTENSIONS}";
                if (IsShiny) return $"{SPRITES_DIRECTORY_PATH}{BOX_SPRITES_DIRECTORY_PATH}Shiny/{PokedexNumber}{ICON_FILE_EXTENSIONS}";
                return $"{SPRITES_DIRECTORY_PATH}{BOX_SPRITES_DIRECTORY_PATH}Regular/{Species}{ICON_FILE_EXTENSIONS}";

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

        // The file path to this pokemon's cry sound effect
        public string Cry
        {
            get { return $"{POKEMON_CRY_PATH}{PokedexNumber}{POKEMON_CRY_EXTENSIONS}"; }
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
