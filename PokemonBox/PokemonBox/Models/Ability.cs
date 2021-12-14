using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBox.Models
{
    /// <summary>
    /// A pokemon's ability. Gives it a special power in or out of combat.
    /// </summary>
    public class Ability
    {
        // The Ability's name
        public string Name { get; set; }
        // What the ability does
        public string Description { get; set; }
        // The ability's id (its position in a list of all abilities when sorted alphabetically)
        public int AbilityId { get; set; }

    }
}
