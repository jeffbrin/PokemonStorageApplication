using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    class PokemonType
    {
        private const string ELEMENT_ICON_DIRECTORY = "ElementIcons/";
        private const string ICON_FILE_EXTENSIONS = ".png";
        private string[] weaknesses;
        private string[] strengths;
        private string[] immunities;

        // Creates a pokemon type without the color set yet
        public PokemonType(string name, string[] weaknesses, string[] strengths, string[] immunities)
        {
            Name = name;
            this.weaknesses = weaknesses;
            this.strengths = strengths;
            this.immunities = immunities;
        }

        // Creates a pokemon with the color set
        public PokemonType(string name, string[] weaknesses, string[] strengths, string[] immunities, string colorTheme): this(name, weaknesses, strengths, immunities)
        {
            ColorTheme = colorTheme;
        }

        public string Name { get; set; }
        public string ColorTheme { get; set; }

        // Returns the path to the type's image icon
        public string ImagePath
        {
            get { return ELEMENT_ICON_DIRECTORY + Name + ICON_FILE_EXTENSIONS; }
        }

        // Returns a string array or all this type's weaknesses' names
        public string[] GetWeaknessTypes
        {
            get { return weaknesses; }
        }

        // Returns a string array or all this type's strengths' names
        public string[] GetStrengthTypes
        {
            get { return strengths; }
        }

        // Returns a string array or all this type's immunities' names
        public string[] GetImmunityTypes
        {
            get { return immunities; }
        }

    }
}
