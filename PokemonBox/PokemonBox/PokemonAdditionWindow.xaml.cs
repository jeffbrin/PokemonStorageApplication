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
        public PokemonAdditionWindow(List<Pokemon> storedPokemon)
        {
            InitializeComponent();
            cmbSpeciesPicker.ItemsSource = PCBox.PokemonOptions;

        }

        private void cmbSpeciesPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            spSelectedSpecies.DataContext = cmbSpeciesPicker.SelectedItem;
        }

    }
}
