using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PokemonBox.Models;

namespace PokemonBox
{
    /// <summary>
    /// Interaction logic for PokemonAdditionWindow.xaml
    /// </summary>
    public partial class PokemonAdditionWindow : Window
    {

        private Pokemon tempPokemon;

        public PokemonAdditionWindow(List<Pokemon> storedPokemon)
        {
            InitializeComponent();
            cmbSpeciesPicker.ItemsSource = PCBox.PokemonOptions; // Bind the combo box with the pokemon options
            cmbSpeciesPicker.SelectedItem = PCBox.PokemonOptions[0];
            tempPokemon = new Pokemon(cmbSpeciesPicker.SelectedItem as Pokemon); // Set the default pokemon for the selection
            spSelectedSpecies.DataContext = tempPokemon; // sync the selected species stack grid with the temp pokemon
            
        }

        private void cmbSpeciesPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tempPokemon = new Pokemon(cmbSpeciesPicker.SelectedItem as Pokemon); // Get the selected pokemon and copy it to the temp pokemon
            spSelectedSpecies.DataContext = tempPokemon; // resync the DataContext
        }

    }
}
