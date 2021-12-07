# Programming_III_Final



Note: I think the best way to implement the PokemonTypes and AttackTypes class is to treat it similarly to the pokemon we're making as options. In the main PokemonBox, we can make a bunch of instances of AttackType and store them in a dictionary so when we get the name from the pokemon, we can get the type directly from that dictionary.



For example:

we have a list of Pokémon types [Poison, Fire, Ice, Ground, ... ]

we loop through each type



```c#
// Create an empty dictionary
Dictionary<String, PokemonType> typesDict = new Dictionary<String, PokemonType>();
// Loop through each PokemonType after making them all from a file
foreach (PokemonType type in typesList){
    // Add a key value pair of PokemonTypeString, PokemonType to the dictionary
    typesDict.Add(type.Name, type);
}

```



Then, any time we want to assign a type to a Pokémon from a string we can just say

``` c#
Pokemon pokemon = new Pokemon(){
    Name = ...,
    Species = ...,
    Moves = ...,
    Types = GetTypes(pokemonTypeStrings),
    ...
}

static List<PokemonType> GetTypes(string[] pokemonTypeStrings){
    List<PokemonType> types = new List<PokemonType>();
    
    // Loop through the types passed
    foreach (string typeName in pokemonTypeStrings){
        // Check if the type is null (second should only ever be null)
        // Not sure if this is needed since I don't think it'll loop over null but idk
        if (typeName != null)
            // Get the PokemonType from the string through the dictionary made above
            types.Add(typesDict["typeName"]);
    }
    
    return types;
}
```

