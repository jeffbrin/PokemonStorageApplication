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

        public PokemonType(string name, string[] weaknesses, string[] strengths, string[] immunities)
        {
            Name = name;
            this.weaknesses = weaknesses;
            this.strengths = strengths;
            this.immunities = immunities;
        }

        public PokemonType(string name, string[] weaknesses, string[] strengths, string[] immunities, string colorTheme): this(name, weaknesses, strengths, immunities)
        {
            ColorTheme = colorTheme;
        }

        public string Name { get; set; }
        public string ColorTheme { get; set; }
        public string ImagePath
        {
            get { return ELEMENT_ICON_DIRECTORY + Name + ICON_FILE_EXTENSIONS; }
        }

        public string[] GetWeaknessTypes
        {
            get { return weaknesses; }
        }
        public string[] GetStrengthTypes
        {
            get { return strengths; }
        }
        public string[] GetImmunityTypes
        {
            get { return immunities; }
        }

    }
}
