using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    abstract class ElementType
    {
        private const string ELEMENT_ICON_DIRECTORY = "ElementIcons/";
        private const string ICON_FILE_EXTENSIONS = ".png";

        public string Name { get; set; }
        public string ColorTheme { get; set; }
        public string ImagePath
        {
            get { return ELEMENT_ICON_DIRECTORY + Name + ICON_FILE_EXTENSIONS; }
        }

        public abstract string[] GetWeaknessTypes();
        public abstract string[] GetStrengthTypes();
        public abstract string[] GetImmunityTypes();

    }
}
