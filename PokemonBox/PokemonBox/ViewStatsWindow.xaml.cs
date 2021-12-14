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
    /// Interaction logic for ViewStatsWindow.xaml
    /// </summary>
    public partial class ViewStatsWindow : Window
    {
        // constructor
        public ViewStatsWindow(Pokemon viewedPokemon)
        {
            InitializeComponent();
            this.DataContext = viewedPokemon;
        }

        // Exit the page
        private void btnExitPokemonCreation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
